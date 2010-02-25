// ColorSpace.cpp: implementation of the CColorSpace class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "ColorSpace.h"
//#include "WorkThreadPool.h"


#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif


#define asm __asm


//颜色饱和函数
__forceinline long border_color(long color)
{
	if (color>255)
		return 255;
	else if (color<0)
		return 0;
	else
		return color;
}


const int csY_coeff_16 = 1.164383*(1<<16);
const int csU_blue_16  = 2.017232*(1<<16);
const int csU_green_16 = (-0.391762)*(1<<16); 
const int csV_green_16 = (-0.812968)*(1<<16);
const int csV_red_16   = 1.596027*(1<<16);


__forceinline void YUVToRGB32_Two(TARGB32* pDst, const TUInt8 Y0, const TUInt8 Y1, const TUInt8 U, const TUInt8 V)
{
    int Ye0	= csY_coeff_16 * (Y0 - 16); 
    int Ye1	= csY_coeff_16 * (Y1 - 16);
    int Ue	= (U-128);
    int Ve	= (V-128);

	int Ue_blue		= csU_blue_16 * Ue;
    int Ue_green	= csU_green_16 * Ue;
	int Ve_green	= csV_green_16 * Ve;
    int Ve_red		= csV_red_16 * Ve;
    int UeVe_green	= Ue_green + Ve_green;
    
    pDst->b	= border_color( ( Ye0 + Ue_blue ) >>16 );
    pDst->g	= border_color( ( Ye0 + UeVe_green ) >>16 );
    pDst->r	= border_color( ( Ye0 + Ve_red ) >>16 );
    pDst->a	= 255;

	++pDst;

	pDst->b	= border_color( ( Ye1 + Ue_blue ) >>16 );
    pDst->g	= border_color( ( Ye1 + UeVe_green ) >>16 );
    pDst->r	= border_color( ( Ye1 + Ve_red ) >>16 );
    pDst->a	= 255;
}


void DECODE_UYVY_Common_line(TARGB32 *pDstLine, const TUInt8 *pUYVY, long width)
{
    for (long x = 0 ; x < width ; x += 2)
    {
        YUVToRGB32_Two(&pDstLine[x], pUYVY[1], pUYVY[3], pUYVY[0], pUYVY[2]);
        pUYVY+=4;
    }
}


const  UInt64   csMMX_16_b      = 0x1010101010101010; // byte{16,16,16,16,16,16,16,16}
const  UInt64   csMMX_128_w     = 0x0080008000800080; //short{  128,  128,  128,  128}
const  UInt64   csMMX_0x00FF_w  = 0x00FF00FF00FF00FF; //掩码
const  UInt64   csMMX_Y_coeff_w = 0x2543254325432543; //short{ 9539, 9539, 9539, 9539} =1.164383*(1<<13)
const  UInt64   csMMX_U_blue_w  = 0x408D408D408D408D; //short{16525,16525,16525,16525} =2.017232*(1<<13)
const  UInt64   csMMX_U_green_w = 0xF377F377F377F377; //short{-3209,-3209,-3209,-3209} =(-0.391762)*(1<<13)
const  UInt64   csMMX_V_green_w = 0xE5FCE5FCE5FCE5FC; //short{-6660,-6660,-6660,-6660} =(-0.812968)*(1<<13)
const  UInt64   csMMX_V_red_w   = 0x3313331333133313; //short{13075,13075,13075,13075} =1.596027*(1<<13)


//一次处理8个颜色输出
#define YUV422ToRGB32_MMX(out_RGB_reg,WriteCode)                                                 \
   /*input :  mm0 = Y7 Y6 Y5 Y4 Y3 Y2 Y1 Y0    */                                                \
   /*         mm1 = 00 u3 00 u2 00 u1 00 u0    */                                                \
   /*         mm2 = 00 v3 00 v2 00 v1 00 v0    */                                                \
   /*output : [out_RGB_reg -- out_RGB_reg+8*4]                 */                                \
                                                                                                 \
          asm   psubusb     mm0,csMMX_16_b        /* mm0 : Y -= 16                       */      \
          asm   psubsw      mm1,csMMX_128_w       /* mm1 : u -= 128                      */      \
          asm   movq        mm7,mm0                                                              \
          asm   psubsw      mm2,csMMX_128_w       /* mm2 : v -= 128                      */      \
          asm   pand        mm0,csMMX_0x00FF_w    /* mm0 = 00 Y6 00 Y4 00 Y2 00 Y0       */      \
          asm   psllw       mm1,3                 /* mm1 : u *= 8                        */      \
          asm   psllw       mm2,3                 /* mm2 : v *= 8                        */      \
          asm   psrlw       mm7,8                 /* mm7 = 00 Y7 00 Y5 00 Y3 00 Y1       */      \
          asm   movq        mm3,mm1                                                              \
          asm   movq        mm4,mm2                                                              \
                                                                                                 \
          asm   pmulhw      mm1,csMMX_U_green_w   /* mm1 = u * U_green                   */      \
          asm   psllw       mm0,3                 /* y*=8                                */      \
          asm   pmulhw      mm2,csMMX_V_green_w   /* mm2 = v * V_green                   */      \
          asm   psllw       mm7,3                 /* y*=8                                */      \
          asm   pmulhw      mm3,csMMX_U_blue_w                                                   \
          asm   paddsw      mm1,mm2                                                              \
          asm   pmulhw      mm4,csMMX_V_red_w                                                    \
          asm   movq        mm2,mm3                                                              \
          asm   pmulhw      mm0,csMMX_Y_coeff_w                                                  \
          asm   movq        mm6,mm4                                                              \
          asm   pmulhw      mm7,csMMX_Y_coeff_w                                                  \
          asm   movq        mm5,mm1                                                              \
          asm   paddsw      mm3,mm0               /* mm3 = B6 B4 B2 B0       */                  \
          asm   paddsw      mm2,mm7               /* mm2 = B7 B5 B3 B1       */                  \
          asm   paddsw      mm4,mm0               /* mm4 = R6 R4 R2 R0       */                  \
          asm   paddsw      mm6,mm7               /* mm6 = R7 R5 R3 R1       */                  \
          asm   paddsw      mm1,mm0               /* mm1 = G6 G4 G2 G0       */                  \
          asm   paddsw      mm5,mm7               /* mm5 = G7 G5 G3 G1       */                  \
                                                                                                 \
          asm   packuswb    mm3,mm4               /* mm3 = R6 R4 R2 R0 B6 B4 B2 B0 to [0-255] */ \
          asm   packuswb    mm2,mm6               /* mm2 = R7 R5 R3 R1 B7 B5 B3 B1 to [0-255] */ \
          asm   packuswb    mm5,mm1               /* mm5 = G6 G4 G2 G0 G7 G5 G3 G1 to [0-255] */ \
          asm   movq        mm4,mm3                                                              \
          asm   punpcklbw   mm3,mm2               /* mm3 = B7 B6 B5 B4 B3 B2 B1 B0     */        \
          asm   punpckldq   mm1,mm5               /* mm1 = G7 G5 G3 G1 xx xx xx xx     */        \
          asm   punpckhbw   mm4,mm2               /* mm4 = R7 R6 R5 R4 R3 R2 R1 R0     */        \
          asm   punpckhbw   mm5,mm1               /* mm5 = G7 G6 G5 G4 G3 G2 G1 G0     */        \
                                                                                                 \
                /*out*/                                                                          \
          asm   pcmpeqb     mm2,mm2               /* mm2 = FF FF FF FF FF FF FF FF     */        \
                                                                                                 \
          asm   movq        mm0,mm3                                                              \
          asm   movq        mm7,mm4                                                              \
          asm   punpcklbw   mm0,mm5             /* mm0 = G3 B3 G2 B2 G1 B1 G0 B0       */        \
          asm   punpcklbw   mm7,mm2             /* mm7 = FF R3 FF R2 FF R1 FF R0       */        \
          asm   movq        mm1,mm0                                                              \
          asm   movq        mm6,mm3                                                              \
          asm   punpcklwd   mm0,mm7             /* mm0 = FF R1 G1 B1 FF R0 G0 B0       */        \
          asm   punpckhwd   mm1,mm7             /* mm1 = FF R3 G3 B3 FF R2 G2 B2       */        \
          asm   WriteCode   [out_RGB_reg],mm0                                                    \
          asm   movq        mm7,mm4                                                              \
          asm   punpckhbw   mm6,mm5             /* mm6 = G7 B7 G6 B6 G5 B5 G4 B4       */        \
          asm   WriteCode   [out_RGB_reg+8],mm1                                                  \
          asm   punpckhbw   mm7,mm2             /* mm7 = FF R7 FF R6 FF R5 FF R4      */         \
          asm   movq        mm0,mm6                                                              \
          asm   punpcklwd   mm6,mm7             /* mm6 = FF R5 G5 B5 FF R4 G4 B4      */         \
          asm   punpckhwd   mm0,mm7             /* mm0 = FF R7 G7 B7 FF R6 G6 B6      */         \
          asm   WriteCode  [out_RGB_reg+8*2],mm6                                                 \
          asm   WriteCode  [out_RGB_reg+8*3],mm0                       


#define UYVY_Loader_MMX(in_yuv_reg)																\
          asm   movq        mm0,[in_yuv_reg  ] /*mm0=Y3 V1 Y2 U1 Y1 V0 Y0 U0  */                \
          asm   movq        mm4,[in_yuv_reg+8] /*mm4=Y7 V3 Y6 U3 Y5 V2 Y4 U2  */                \
          asm   movq        mm1,mm0                                                             \
          asm   movq        mm5,mm4                                                             \
          asm   psrlw       mm0,8              /*mm0=00 Y3 00 Y2 00 Y1 00 Y0  */                \
          asm   psrlw       mm4,8              /*mm4=00 Y7 00 Y6 00 Y5 00 Y4  */                \
          asm   pand        mm1,csMMX_0x00FF_w /*mm1=00 V1 00 U1 00 V0 00 U0  */                \
          asm   pand        mm5,csMMX_0x00FF_w /*mm5=00 V3 00 U3 00 V2 00 U2  */                \
          asm   packuswb    mm1,mm5            /*mm1=V3 U3 V2 U2 V1 U1 V0 U0  */                \
          asm   movq        mm2,mm1                                                             \
          asm   packuswb    mm0,mm4            /*mm0=Y7 Y6 Y5 Y4 Y3 Y2 Y1 Y0  */                \
          asm   psllw       mm1,8              /*mm1=U3 00 U2 00 U1 00 U0 00  */                \
          asm   psrlw       mm2,8              /*mm2=00 V3 00 V2 00 V1 00 V0  */                \
          asm   psrlw       mm1,8              /*mm1=00 U3 00 U2 00 U1 00 U0  */                


void DECODE_UYVY_MMX_line(TARGB32* pDstLine,const TUInt8* pUYVY,long width)
{
	long expand8_width=(width>>3)<<3;
	
	if (expand8_width>0)
	{

		asm
		{
			mov     ecx,expand8_width
            mov     eax,pUYVY
            mov     edx,pDstLine
            lea     eax,[eax+ecx*2]
            lea     edx,[edx+ecx*4]
            neg     ecx
                
loop_beign:
			UYVY_Loader_MMX(eax+ecx*2)
            YUV422ToRGB32_MMX(edx+ecx*4,movq)
			
            add     ecx,8
            jnz     loop_beign
			
            mov     pUYVY,eax
            mov     pDstLine,edx
		}
	}
	
	//处理边界
	DECODE_UYVY_Common_line(pDstLine, pUYVY, width-expand8_width);
}


//在x86CPU上可以使用CPUID指令来得到各种关于当前CPU的特性，
//包括制造商、CPU家族号、缓存信息、是否支持MMX\SSE\SSE2指令集 等等；
//要使用CPUID指令，首先应该判断CPU是否支持该指令；方法是判断EFLAGS寄存器的第21位
//是否可以改写；如果可以改写，那么说明这块CPU支持CPUID指令
bool _CPUSupportCPUID()
{
	long int CPUIDInfOld = 0;
	long int CPUIDInfNew = 0;
	
	try
	{
        asm
        {
            pushfd						// 保存原 EFLAGS
			pop     eax
			mov     edx, eax
			mov     CPUIDInfOld, eax	//
			
			xor     eax, 00200000h		// 改写 第21位
			push    eax
			popfd						// 改写 EFLAGS
			
			pushfd						// 保存新 EFLAGS
			pop     eax              
			mov     CPUIDInfNew, eax
			
			push    edx					// 恢复原 EFLAGS
			popfd
        }
        return (CPUIDInfOld != CPUIDInfNew);	// EFLAGS 第21位 可以改写
	}
	catch(...)
	{
		return false;
	}
}


//那么判断CPU是否支持MMX指令的函数如下:
bool _CPUSupportMMX()  //判断CPU是否支持MMX指令
{
	
	if (!_CPUSupportCPUID())
		return false;
	
	long int MMXInf=0;
	
	try
	{
		asm
		{
			push  ebx
			mov   eax,1
			cpuid
			mov   MMXInf,edx
			pop   ebx
		}
		MMXInf=MMXInf & (1 << 23);  //检测edx第23位
		return (MMXInf == (1 << 23));
	}
	catch(...)
	{
		return false;
	}
}


const bool _IS_MMX_ACTIVE = _CPUSupportMMX();


typedef void (*TDECODE_UYVY_line_proc)(TARGB32 *pDstLine, const TUInt8 *pUYVY, long width);


const TDECODE_UYVY_line_proc DECODE_UYVY_Auto_line = ( _IS_MMX_ACTIVE ? DECODE_UYVY_MMX_line : DECODE_UYVY_Common_line );


__forceinline void DECODE_filish()
{
    if (_IS_MMX_ACTIVE)
    {
        asm emms
    }
}


void DECODE_UYVY_Auto(const TUInt8 *pUYVY, const TPicRegion &DstPic)
{
	long YUV_byte_width	= (DstPic.width >> 1) << 2;
    TARGB32 *pDstLine	= DstPic.pdata; 

    for (long y = 0 ; y < DstPic.height ; ++y)
    {
        DECODE_UYVY_Auto_line(pDstLine, pUYVY, DstPic.width);

        pUYVY += YUV_byte_width;

        ((TUInt8*&)pDstLine) += DstPic.byte_width;
    } 

    DECODE_filish();
}


struct TDECODE_UYVY_Parallel_WorkData
{
    const TUInt8* pUYVY;
    TPicRegion    DstPic;
};


void DECODE_UYVY_Parallel_callback(void* wd)
{
    TDECODE_UYVY_Parallel_WorkData* WorkData = (TDECODE_UYVY_Parallel_WorkData*)wd;
    DECODE_UYVY_Auto(WorkData->pUYVY, WorkData->DstPic);
}


bool DECODE_UYVY_TO_RGB32(const TUInt8 *pUYVY, const TPicRegion &DstPic)
{
	if (NULL == pUYVY)
	{
		TRACE ("DECODE_UYVY_TO_RGB32() error, NULL == pUYVY\n");
		return false;
	}

	if (NULL == DstPic.pdata)
	{
		TRACE ("DECODE_UYVY_TO_RGB32() error, NULL == DstPic.pdata\n");
		return false;
	}

	//long work_count = CWorkThreadPool::best_work_count(); 将这句话注释掉，改为下面的语句即可
	long work_count = 1;


	if (work_count > 1)
	{
		std::vector<TDECODE_UYVY_Parallel_WorkData>   work_list(work_count);
		std::vector<TDECODE_UYVY_Parallel_WorkData*>  pwork_list(work_count);
		long cheight = DstPic.height / work_count; 
		for (long i = 0 ; i < work_count ; ++i)
		{
			work_list[i].pUYVY = pUYVY + i * cheight * (DstPic.width << 1);
			work_list[i].DstPic.pdata		= DstPic.pixel_pos(0, cheight*i);
			work_list[i].DstPic.byte_width	= DstPic.byte_width;
			work_list[i].DstPic.width		= DstPic.width;
			work_list[i].DstPic.height		= cheight;
			pwork_list[i] = &work_list[i];
		}
		work_list[work_count-1].DstPic.height = DstPic.height-cheight*(work_count-1);
	//	CWorkThreadPool::work_execute(DECODE_UYVY_Parallel_callback, (void**)&pwork_list[0],work_count);
	}
	else
	{
		DECODE_UYVY_Auto(pUYVY, DstPic);
	}

	return true;
}

bool DECODE_RGB_TO_BGRA(const unsigned char *pRGB, const TPicRegion &DstPic)
{

 	DWORD RGBcount = DstPic.width * DstPic.height;
	for (register int i=0 ; i<RGBcount ; i++)
	{
		DstPic.pdata[i].r = pRGB[i*3];
		DstPic.pdata[i].g = pRGB[i*3+1];
		DstPic.pdata[i].b = pRGB[i*3+2];
		DstPic.pdata[i].a = 255;
	}
	return true;
}

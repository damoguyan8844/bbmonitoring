// ColorSpace.h: interface for the CColorSpace class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_COLORSPACE_H__11F9CC28_7D9D_48EE_9CDC_F5EFC4B7B51A__INCLUDED_)
#define AFX_COLORSPACE_H__11F9CC28_7D9D_48EE_9CDC_F5EFC4B7B51A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


typedef unsigned char		TUInt8;		// [0..255]
typedef unsigned long		TUInt32;
typedef unsigned __int64	UInt64;

typedef struct tagARGB32	//32 bit color
{
    TUInt8  b,g,r,a;		//a is alpha
}
TARGB32;

typedef struct tagPicRegion		//һ����ɫ�����������������ڲ�������
{
    TARGB32*    pdata;			//��ɫ�����׵�ַ
    long        byte_width;		//һ�����ݵ�������(�ֽڿ��)��
								//abs(byte_width)�п��ܴ��ڵ���width*sizeof(TARGB32);
    long        width;			//���ؿ��
    long        height;			//���ظ߶�

	TARGB32 *pixel_pos(const long x, const long y) const
	{
		return (TARGB32 *)((TUInt8*)pdata + byte_width*y + x * sizeof(TARGB32));
	}
}
TPicRegion;


bool DECODE_UYVY_TO_RGB32(const TUInt8 *pUYVY, const TPicRegion &DstPic);

bool DECODE_RGB_TO_BGRA(const TUInt8 *pRGB, const TPicRegion &DstPic);
bool DECODE_RGB_TO_BGRA_2(const int width , const int height,const unsigned char *pRGB, unsigned char * pBGR);
bool DECODE_RGB_TO_BGR(const int width , const int height,const unsigned char *pRGB, unsigned char * pBGR);


#endif // !defined(AFX_COLORSPACE_H__11F9CC28_7D9D_48EE_9CDC_F5EFC4B7B51A__INCLUDED_)

// TC1000API.h : Defines the interface for the DLL.
//

// *****************************************************************************
//  rgbcacc   VERSION:  1.0   DATE: 2008-10-17
//  ----------------------------------------------------------------------------
//		FileName: 	rgbcacc.h
//		Created:	2008-10-17   16:46
//		Author:		孟娟
//		Purpose:	RGB采集卡类接口的声明	
//		Version:	
//					【V1.1 2008-10-17 孟娟】
//						Initial
//		Remark:	
//				使用时只需包含TC1000API.h头文件即可。
//  ----------------------------------------------------------------------------
//  Copyright (C) 2008 
//	Nanjing Talent Electronics and Technology Co., LTD.
//  All Rights Reserved
// *****************************************************************************



#ifndef _RGBPROSDK_API_H_
#define _RGBPROSDK_API_H_
 
#ifdef RGBPROSDK_EXPORTS
	#define RGBPROSDKAPI __declspec(dllexport)
#else
	#define RGBPROSDKAPI __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C"
{
#endif


#ifndef RGBCAPI
#define RGBCAPI __stdcall
#endif

#include "RGBDEFS.H"

typedef  void (CALLBACK RGBNOTIFYFN)(LPVOID NotifyArg, 
                                    UINT   uMsg, 
                                    WPARAM wParam,
                                    LPARAM lParam);

typedef unsigned long   HRGBCAPTURE, *PHRGBCAPTURE, FAR *LPHRGBCAPTURE;
typedef unsigned long   HFRAMESTORE, *PHFRAMESTORE; // No need for legacy LPHFRAMESTORE.

typedef struct
{
   LPVOID         LpVoidBuffer;  /* Pointer application uses to access data. */
   unsigned long  Index;         /* Used to identify buffer to RGB device. */
   BOOL bufferflags;
}  RGBCAPTUREBUFFER, *PRGBCAPTUREBUFFER, FAR *LPRGBCAPTUREBUFFER;


typedef struct tagVDIF
{
   unsigned long  HorFrequency;     /* Line rate in Hz. */
   unsigned long  VerFrequency;     /* Refresh rate in Hz*1000. */
   unsigned long  PixelClock;       /* Dot clock in Hz. */

   unsigned short Flags;            /* Bitwise OR of VDIF_FLAG_.*. */

   /* The following values are in pixels. */
   unsigned long  HorAddrTime;      /* Amount of active video (resolution). */
   unsigned long  HorRightBorder;
   unsigned long  HorFrontPorch;
   unsigned long  HorSyncTime;
   unsigned long  HorBackPorch;
   unsigned long  HorLeftBorder;

   /* The following values are in lines. */
   unsigned long  VerAddrTime;      /* Amount of active video (resolution). */
   unsigned long  VerBottomBorder;
   unsigned long  VerFrontPorch;
   unsigned long  VerSyncTime;
   unsigned long  VerBackPorch;
   unsigned long  VerTopBorder;

   /* TODO: Could make this [1] or a pointer. */
   char          Description[128];
}  VDIF, *PVDIF, *LPVDIF;


typedef struct
{
   unsigned long  Size;          /* Size in bytes of this structure. */
   unsigned long  Flags;         /* Indicates which fields are relevant. */
   unsigned long  Input;         /* Which input connector to capture from. */
   unsigned long  Format;        /* Format of RGB signal to be captured. */
   VDIF          VideoTimings;  /* The characteristics of the signal. */
   unsigned long  Phase;         /* Aligns samples with pixels in signal. */
   unsigned long  Brightness;    /* Modifies DC offset. */
   unsigned long  Contrast;      /* Input voltage/gain. */
   unsigned long  BlackLevel;    /* When to sample black level (number of
                                  * pixels from the reference point). */
   unsigned long  SampleRate;    /* Frames to drop between samples. */
   unsigned long  PixelFormat;   /* Pixel format for frame buffer reads. */
   unsigned long  SyncEdge;      /* The HSYNC edge to use as reference. */
   signed long    HorScale;      /* . */
   signed long    HorOffset;     /* . */
   signed long    VerOffset;     /* . */
   HWND           HWnd;          /* Window to receive messages from ISR. */
   unsigned long  HScaled;       /* horizontal size after scaling */
   unsigned long  VScaled;       /* vertical size after scaling */
   RGBNOTIFYFN   *NotifyFn;      /* Pointer to the notification function */
   PVOID          NotifyArg;     /* Pointer to a user supplied argument for the
                                  * notification function. */
   unsigned long  NotifyFlags;   /* Indicates the required notification 
                                  * messages. */
} RGBCAPTUREPARMS, *PRGBCAPTUREPARMS, FAR *LPRGBCAPTUREPARMS;




/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureInitialise (
   unsigned long FAR *lpNumberOfDevices );



//函数名：RGBCaptureInitialise
//功能描述：初始化图象采集卡
//输入参数：lpNumberOfDevices(存放采集卡数量的指针)   
//输出参数：成功:RGBERROR_NO_ERROR
//          失败:RGBERROR_HARDWARE_NOT_FOUND  // 没有找到VGA视频采集卡    

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureOpen (
   unsigned long deviceIndex,
   LPHRGBCAPTURE  lpHRGBCapture );

//函数名：RGBCaptureOpen
//功能描述：初始化指定的VGA视频采集卡，返回设备句柄
//输入参数：	deviceIndex		输入需要初始化的设备号，从0开始
//输出参数：	lpHRGBCapture	返回设备句柄
//返回值：	成功:RGBERROR_NO_ERROR
//          失败:   RGBERROR_INVALID_POINTER  //没有存放打开设备句柄的指针
// 					RGBERROR_HARDWARE_NOT_FOUND  //没有找到VGA视频采集卡
// 					RGBERROR_INVALID_INDEX  //不存在的设备号
// 					RGBERROR_DEVICE_IN_USE  //设备已被占用
// 					RGBERROR_UNABLE_TO_LOAD_DRIVER  //分配内存资源失败
// 					RGBERROR_INVALID_DEVICE  //打开设备失败
/******************************************************************************/

RGBPROSDKAPI void RGBCAPI
RGBCaptureClose (
   HRGBCAPTURE hRGBCapture );

//函数名：	RGBCaptureClose
//功能描述：	关闭指定的VGA视频采集卡
//输入参数：	hRGBCapture 	设备句柄
//输出参数：	无
//返回值：	无

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureEnable (
   HRGBCAPTURE    hRGBCapture,
   unsigned long bEnable );

//函数名：	RGBCaptureEnable
//功能描述：启动VGA视频采集
//输入参数：hRGBCapture	设备句柄
//			bEnable		是否启动
//输出参数：	无
//返回值：	成功:RGBERROR_NO_ERROR
//          失败:	RGBERROR_INVALID_HRGBCAPTURE  //设备未被使用
// 					RGBERROR_CAPTURE_OUTSTANDING  //设备未被打开
// 					RGBERROR_THREAD_FAILURE  //创建信号灯失败

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureGetParameters (
   HRGBCAPTURE       hRGBCapture,
   LPRGBCAPTUREPARMS lpRGBCaptureParms,
   unsigned long     ulFlag );

//函数名：	RGBCaptureGetParameters
//功能描述：获取VGA视频采集卡配置参数
//输入参数：hRGBCapture	设备句柄
//			ulFlag		参数类型标识
//输出参数：lpRGBCaptureParms		返回配置参数
//返回值：	成功:RGBERROR_NO_ERROR
//          失败:   RGBERROR_INVALID_HRGBCAPTURE  //设备未被使用
// 					RGBERROR_INVALID_FLAGS  //不支持的 ulFlag
// 					RGBERROR_INVALID_POINTER  //没有存放打开设备句柄的指针
// 					RGBERROR_INVALID_SIZE  //错误的配置参数大小（lpRGBCaptureParms->Size）
/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureSetParameters (
   HRGBCAPTURE       hRGBCapture,
   LPRGBCAPTUREPARMS lpRGBCaptureParms,
   unsigned long     ulFlag );

//函数名：	RGBCaptureSetParameters
//功能描述：设置VGA视频采集卡配置参数
//输入参数：hRGBCapture	设备句柄
//			lpRGBCaptureParms		指定的配置参数
//			ulFlag		配置参数类型标识
//输出参数：无
//返回值：	成功:RGBERROR_NO_ERROR
// 			失败:   RGBERROR_INVALID_HRGBCAPTURE  //设备未被使用
// 					RGBERROR_INVALID_POINTER  //没有存放打开设备句柄的指针
// 					RGBERROR_INVALID_SIZE  //错误的配置参数大小（lpRGBCaptureParms->Size）
// 					RGBERROR_INVALID_FLAGS  //不支持的 ulFlag
// 					RGBERROR_INVALID_PIXEL_FORMAT  //不支持的显示模式 lpRGBCaptureParms->PixelFormat
// 					RGBERROR_INVALID_HWND  //不支持的消息接受窗口 lpRGBCaptureParms->HWnd
// 					RGBERROR_INVALID_FORMAT  //不支持的输入信号格式 lpRGBCaptureParms->Format
// 					RGBERROR_INVALID_INPUT  //不支持或设置失败输入端口 lpRGBCaptureParms->Input
// 					RGBERROR_INVALID_PHASE  //不支持或设置失败相位 lpRGBCaptureParms-> Phase
// 					RGBERROR_INVALID_BRIGHTNESS  //不支持或设置失败亮度 lpRGBCaptureParms->Brightness
// 					RGBERROR_INVALID_CONTRAST  //不支持或设置失败对比度lpRGBCaptureParms->Contrast
// 					RGBERROR_INVALID_BLACKLEVEL  //不支持的黑色度 lpRGBCaptureParms->BlackLevel
// 					RGBERROR_INVALID_SYNCEDGE  //不支持的同步沿 lpRGBCaptureParms->SyncEdge
// 					RGBERROR_VSCALED_OUT_OF_RANGE  //不支持的水平缩放比例 lpRGBCaptureParms->VScaled
// 					RGBERROR_HSCALED_OUT_OF_RANGE  //不支持的垂直缩放比例 lpRGBCaptureParms->HScaled
// 					RGBERROR_SCALING_NOT_SUPPORTED  //不支持或设置水平偏移失败lpRGBCaptureParms->HorOffset

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureFrameBufferReady (
   HRGBCAPTURE    hRGBCapture,
   unsigned long  applicationBuffer,
   unsigned long  ulNumberBytes );

//函数名：	RGBCaptureFrameBufferReady
//功能描述：通知SDK采集缓冲区已就绪
//输入参数：hRGBCapture	设备句柄
//			applicationBuffer	采集缓冲区索引
//			ulNumberBytes		采集缓冲区大小
//输出参数：无
//返回值：	成功:RGBERROR_NO_ERROR
//          失败:RGBERROR_INVALID_HRGBCAPTURE  //设备未被使用
// 				 RGBERROR_INVALID_INDEX   //无效的采集缓存索引
// 				 RGBERROR_BUFFER_TOO_SMALL  //采集缓存太小

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureDetectVideoMode (
   HRGBCAPTURE hRGBCapture,
   int ratioN,
   int RatioD,
   VDIF * pResultVdif,
   BOOL bForceInterlaced);

//函数名：	RGBCaptureDetectVideoMode
//功能描述：检测当前输入的VGA视频信号分辨率
//输入参数：hRGBCapture	设备句柄
//			ratioN, RatioD, bForceInterlaced	无效参数
//输出参数：pResultVdif	返回获取的输出信号模式
//返回值：	成功:RGBERROR_NO_ERROR
//          失败:RGBERROR_INVALID_HRGBCAPTURE  //设备未被使用
//         		 RGBERROR_INVALID_POINTER  //没有存放当前视频时钟的指针VideoTimings

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureUseMemoryBuffer (
   HRGBCAPTURE hRGBCaptureBogus, 
   LPVOID pMemory, 
   DWORD dwMemorySize,
   DWORD *pdwIndex);

//函数名：	RGBCaptureUseMemoryBuffer
//功能描述：注册用户分配的采集缓冲区
//输入参数：hRGBCaptureBogus	设备句柄
//			pMemory			用户分配的采集缓冲区指针
//			dwMemorySize		用户分配的采集缓冲区大小
//输出参数：pdwIndex			返回采集缓冲区索引
//返回值：	成功:RGBERROR_NO_ERROR
// 			失败:RGBERROR_INVALID_HRGBCAPTURE  //设备未被使用
// 		         RGBERROR_INVALID_POINTER  //存放当前采集缓存的指针错误

/* Allows captures to use buffers which originate in the application, or from
*  libraries other than the RGBCapture SDK library.
*/

RGBPROSDKAPI RGBERROR RGBCAPI
 RGBCaptureReleaseBuffer (
   HRGBCAPTURE hRGBCapture, 
   DWORD dwIndex);
//函数名：	RGBCaptureReleaseBuffer
//功能描述：注销用户分配的采集缓冲区
//输入参数：hRGBCapture	设备句柄
//			dwIndex		采集缓冲区索引
//输出参数：无
//返回值：	成功:RGBERROR_NO_ERROR
//          失败:RGBERROR_INVALID_HRGBCAPTURE  //设备未被使用
// 		         RGBERROR_INVALID_INDEX  //无效的采集缓存索引

RGBPROSDKAPI BOOL RGBCAPI
 RGBCaptureGetDataMode (
   HRGBCAPTURE hRGBCapture, 
   unsigned long * pdatamode);
//函数名：	RGBCaptureGetDataMode
//功能描述：获取输入数据的模式(YUV/RGB)
//输入参数：hRGBCapture	设备句柄	
//输出参数：pdatamode	存放输入数据模式值（0－YUV;1－RGB）
//返回值：	成功:true
//          失败:false

RGBPROSDKAPI BOOL RGBCAPI
 RGBCaptureSetClampPlacement (
   HRGBCAPTURE hRGBCapture
   , unsigned long ulClampPlacement);

//函数名：	RGBCaptureSetClampPlacement
//功能描述：设置VGA视频采集卡fpga配置参数(箝位位置),调整图像抖动现象
//输入参数：hRGBCapture	设备句柄
//			ulClampPlacement		指定的配置参数
//输出参数：无
//返回值：	成功:true
//          失败:false

RGBPROSDKAPI BOOL RGBCAPI
 RGBCaptureSetClampDuration (
   HRGBCAPTURE hRGBCapture
   , unsigned long ulClampDuration);

//函数名：	RGBCaptureSetClampPlacement
//功能描述：设置VGA视频采集卡fpga配置参数(箝位延迟),调整图像抖动现象
//输入参数：hRGBCapture	设备句柄
//			ulClampDuration		指定的配置参数
//输出参数：无
//返回值：	成功:true
//          失败:false
	

#ifdef __cplusplus
}
#endif

 #endif//_RGBPROSDK_API_H_

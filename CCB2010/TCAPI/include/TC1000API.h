// TC1000API.h : Defines the interface for the DLL.
//

// *****************************************************************************
//  rgbcacc   VERSION:  1.0   DATE: 2008-10-17
//  ----------------------------------------------------------------------------
//		FileName: 	rgbcacc.h
//		Created:	2008-10-17   16:46
//		Author:		�Ͼ�
//		Purpose:	RGB�ɼ�����ӿڵ�����	
//		Version:	
//					��V1.1 2008-10-17 �Ͼ꡿
//						Initial
//		Remark:	
//				ʹ��ʱֻ�����TC1000API.hͷ�ļ����ɡ�
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



//��������RGBCaptureInitialise
//������������ʼ��ͼ��ɼ���
//���������lpNumberOfDevices(��Ųɼ���������ָ��)   
//����������ɹ�:RGBERROR_NO_ERROR
//          ʧ��:RGBERROR_HARDWARE_NOT_FOUND  // û���ҵ�VGA��Ƶ�ɼ���    

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureOpen (
   unsigned long deviceIndex,
   LPHRGBCAPTURE  lpHRGBCapture );

//��������RGBCaptureOpen
//������������ʼ��ָ����VGA��Ƶ�ɼ����������豸���
//���������	deviceIndex		������Ҫ��ʼ�����豸�ţ���0��ʼ
//���������	lpHRGBCapture	�����豸���
//����ֵ��	�ɹ�:RGBERROR_NO_ERROR
//          ʧ��:   RGBERROR_INVALID_POINTER  //û�д�Ŵ��豸�����ָ��
// 					RGBERROR_HARDWARE_NOT_FOUND  //û���ҵ�VGA��Ƶ�ɼ���
// 					RGBERROR_INVALID_INDEX  //�����ڵ��豸��
// 					RGBERROR_DEVICE_IN_USE  //�豸�ѱ�ռ��
// 					RGBERROR_UNABLE_TO_LOAD_DRIVER  //�����ڴ���Դʧ��
// 					RGBERROR_INVALID_DEVICE  //���豸ʧ��
/******************************************************************************/

RGBPROSDKAPI void RGBCAPI
RGBCaptureClose (
   HRGBCAPTURE hRGBCapture );

//��������	RGBCaptureClose
//����������	�ر�ָ����VGA��Ƶ�ɼ���
//���������	hRGBCapture 	�豸���
//���������	��
//����ֵ��	��

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureEnable (
   HRGBCAPTURE    hRGBCapture,
   unsigned long bEnable );

//��������	RGBCaptureEnable
//��������������VGA��Ƶ�ɼ�
//���������hRGBCapture	�豸���
//			bEnable		�Ƿ�����
//���������	��
//����ֵ��	�ɹ�:RGBERROR_NO_ERROR
//          ʧ��:	RGBERROR_INVALID_HRGBCAPTURE  //�豸δ��ʹ��
// 					RGBERROR_CAPTURE_OUTSTANDING  //�豸δ����
// 					RGBERROR_THREAD_FAILURE  //�����źŵ�ʧ��

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureGetParameters (
   HRGBCAPTURE       hRGBCapture,
   LPRGBCAPTUREPARMS lpRGBCaptureParms,
   unsigned long     ulFlag );

//��������	RGBCaptureGetParameters
//������������ȡVGA��Ƶ�ɼ������ò���
//���������hRGBCapture	�豸���
//			ulFlag		�������ͱ�ʶ
//���������lpRGBCaptureParms		�������ò���
//����ֵ��	�ɹ�:RGBERROR_NO_ERROR
//          ʧ��:   RGBERROR_INVALID_HRGBCAPTURE  //�豸δ��ʹ��
// 					RGBERROR_INVALID_FLAGS  //��֧�ֵ� ulFlag
// 					RGBERROR_INVALID_POINTER  //û�д�Ŵ��豸�����ָ��
// 					RGBERROR_INVALID_SIZE  //��������ò�����С��lpRGBCaptureParms->Size��
/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureSetParameters (
   HRGBCAPTURE       hRGBCapture,
   LPRGBCAPTUREPARMS lpRGBCaptureParms,
   unsigned long     ulFlag );

//��������	RGBCaptureSetParameters
//��������������VGA��Ƶ�ɼ������ò���
//���������hRGBCapture	�豸���
//			lpRGBCaptureParms		ָ�������ò���
//			ulFlag		���ò������ͱ�ʶ
//�����������
//����ֵ��	�ɹ�:RGBERROR_NO_ERROR
// 			ʧ��:   RGBERROR_INVALID_HRGBCAPTURE  //�豸δ��ʹ��
// 					RGBERROR_INVALID_POINTER  //û�д�Ŵ��豸�����ָ��
// 					RGBERROR_INVALID_SIZE  //��������ò�����С��lpRGBCaptureParms->Size��
// 					RGBERROR_INVALID_FLAGS  //��֧�ֵ� ulFlag
// 					RGBERROR_INVALID_PIXEL_FORMAT  //��֧�ֵ���ʾģʽ lpRGBCaptureParms->PixelFormat
// 					RGBERROR_INVALID_HWND  //��֧�ֵ���Ϣ���ܴ��� lpRGBCaptureParms->HWnd
// 					RGBERROR_INVALID_FORMAT  //��֧�ֵ������źŸ�ʽ lpRGBCaptureParms->Format
// 					RGBERROR_INVALID_INPUT  //��֧�ֻ�����ʧ������˿� lpRGBCaptureParms->Input
// 					RGBERROR_INVALID_PHASE  //��֧�ֻ�����ʧ����λ lpRGBCaptureParms-> Phase
// 					RGBERROR_INVALID_BRIGHTNESS  //��֧�ֻ�����ʧ������ lpRGBCaptureParms->Brightness
// 					RGBERROR_INVALID_CONTRAST  //��֧�ֻ�����ʧ�ܶԱȶ�lpRGBCaptureParms->Contrast
// 					RGBERROR_INVALID_BLACKLEVEL  //��֧�ֵĺ�ɫ�� lpRGBCaptureParms->BlackLevel
// 					RGBERROR_INVALID_SYNCEDGE  //��֧�ֵ�ͬ���� lpRGBCaptureParms->SyncEdge
// 					RGBERROR_VSCALED_OUT_OF_RANGE  //��֧�ֵ�ˮƽ���ű��� lpRGBCaptureParms->VScaled
// 					RGBERROR_HSCALED_OUT_OF_RANGE  //��֧�ֵĴ�ֱ���ű��� lpRGBCaptureParms->HScaled
// 					RGBERROR_SCALING_NOT_SUPPORTED  //��֧�ֻ�����ˮƽƫ��ʧ��lpRGBCaptureParms->HorOffset

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureFrameBufferReady (
   HRGBCAPTURE    hRGBCapture,
   unsigned long  applicationBuffer,
   unsigned long  ulNumberBytes );

//��������	RGBCaptureFrameBufferReady
//����������֪ͨSDK�ɼ��������Ѿ���
//���������hRGBCapture	�豸���
//			applicationBuffer	�ɼ�����������
//			ulNumberBytes		�ɼ���������С
//�����������
//����ֵ��	�ɹ�:RGBERROR_NO_ERROR
//          ʧ��:RGBERROR_INVALID_HRGBCAPTURE  //�豸δ��ʹ��
// 				 RGBERROR_INVALID_INDEX   //��Ч�Ĳɼ���������
// 				 RGBERROR_BUFFER_TOO_SMALL  //�ɼ�����̫С

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureDetectVideoMode (
   HRGBCAPTURE hRGBCapture,
   int ratioN,
   int RatioD,
   VDIF * pResultVdif,
   BOOL bForceInterlaced);

//��������	RGBCaptureDetectVideoMode
//������������⵱ǰ�����VGA��Ƶ�źŷֱ���
//���������hRGBCapture	�豸���
//			ratioN, RatioD, bForceInterlaced	��Ч����
//���������pResultVdif	���ػ�ȡ������ź�ģʽ
//����ֵ��	�ɹ�:RGBERROR_NO_ERROR
//          ʧ��:RGBERROR_INVALID_HRGBCAPTURE  //�豸δ��ʹ��
//         		 RGBERROR_INVALID_POINTER  //û�д�ŵ�ǰ��Ƶʱ�ӵ�ָ��VideoTimings

/******************************************************************************/

RGBPROSDKAPI RGBERROR RGBCAPI
RGBCaptureUseMemoryBuffer (
   HRGBCAPTURE hRGBCaptureBogus, 
   LPVOID pMemory, 
   DWORD dwMemorySize,
   DWORD *pdwIndex);

//��������	RGBCaptureUseMemoryBuffer
//����������ע���û�����Ĳɼ�������
//���������hRGBCaptureBogus	�豸���
//			pMemory			�û�����Ĳɼ�������ָ��
//			dwMemorySize		�û�����Ĳɼ���������С
//���������pdwIndex			���زɼ�����������
//����ֵ��	�ɹ�:RGBERROR_NO_ERROR
// 			ʧ��:RGBERROR_INVALID_HRGBCAPTURE  //�豸δ��ʹ��
// 		         RGBERROR_INVALID_POINTER  //��ŵ�ǰ�ɼ������ָ�����

/* Allows captures to use buffers which originate in the application, or from
*  libraries other than the RGBCapture SDK library.
*/

RGBPROSDKAPI RGBERROR RGBCAPI
 RGBCaptureReleaseBuffer (
   HRGBCAPTURE hRGBCapture, 
   DWORD dwIndex);
//��������	RGBCaptureReleaseBuffer
//����������ע���û�����Ĳɼ�������
//���������hRGBCapture	�豸���
//			dwIndex		�ɼ�����������
//�����������
//����ֵ��	�ɹ�:RGBERROR_NO_ERROR
//          ʧ��:RGBERROR_INVALID_HRGBCAPTURE  //�豸δ��ʹ��
// 		         RGBERROR_INVALID_INDEX  //��Ч�Ĳɼ���������

RGBPROSDKAPI BOOL RGBCAPI
 RGBCaptureGetDataMode (
   HRGBCAPTURE hRGBCapture, 
   unsigned long * pdatamode);
//��������	RGBCaptureGetDataMode
//������������ȡ�������ݵ�ģʽ(YUV/RGB)
//���������hRGBCapture	�豸���	
//���������pdatamode	�����������ģʽֵ��0��YUV;1��RGB��
//����ֵ��	�ɹ�:true
//          ʧ��:false

RGBPROSDKAPI BOOL RGBCAPI
 RGBCaptureSetClampPlacement (
   HRGBCAPTURE hRGBCapture
   , unsigned long ulClampPlacement);

//��������	RGBCaptureSetClampPlacement
//��������������VGA��Ƶ�ɼ���fpga���ò���(��λλ��),����ͼ�񶶶�����
//���������hRGBCapture	�豸���
//			ulClampPlacement		ָ�������ò���
//�����������
//����ֵ��	�ɹ�:true
//          ʧ��:false

RGBPROSDKAPI BOOL RGBCAPI
 RGBCaptureSetClampDuration (
   HRGBCAPTURE hRGBCapture
   , unsigned long ulClampDuration);

//��������	RGBCaptureSetClampPlacement
//��������������VGA��Ƶ�ɼ���fpga���ò���(��λ�ӳ�),����ͼ�񶶶�����
//���������hRGBCapture	�豸���
//			ulClampDuration		ָ�������ò���
//�����������
//����ֵ��	�ɹ�:true
//          ʧ��:false
	

#ifdef __cplusplus
}
#endif

 #endif//_RGBPROSDK_API_H_

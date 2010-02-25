// RGBSAMPDlg.h : header file
//
// *****************************************************************************
//  RGBSAMPDlg   VERSION:  1.1   DATE: 2008-10-15
//  ----------------------------------------------------------------------------
//		FileName: 	RGBSAMPDlg.h
//		Created:	2008-10-15   16:50
//		Author:		MengJuan
//		Purpose:	�ɼ���ͼ��ɼ�������
//		Version:	
//					��V1.1 2008-10-15 mengjuan��
//						Initial
//					  ������CRGBSAMPDlg
// 	                  ��ʼ��OnInitDialogϵͳ����
//                    OnSysCommandϵͳ����
// 					  ��ͼOnPaintϵͳ����
// 	                  ��������OnQueryDragIcon����
// 	                  �ر�OnClose����
// 					  �ر�OnDestroy����
//					  �ߴ��С�ı�OnSize����
// 					  �Ҽ�����OnRButtonUp����
// 					  ���е�һ���ɼ���OnRunVGA1����
// 					  SDK���زɼ���Ϣ�󴥷�OnMyMessage_Sta1����
// 					  ֹͣ�ɼ�OnStopVGA1����
// 					  ��������OnCreate����
// 					  ������ɫ����OnCtlColor����
// 					  �����ɼ�����OnAdjustVga1����
//					  OnClickTest����
// 					  ģʽ�ı䣬�ֱ��ʸı�ʱ����OnMyMessage_modechange����
// 					  ��ͣOnPauseVga1����
// 					  ����OnGoonVga1����
// 					  ���˫��OnLButtonDblClk����
// 					  ��������OnQualpropVga1����
// 					  ���ź�OnMyMessage_nosignal����		
// 					  ���вɼ���RunVGA����
// 					  ����ͼ��PaintFrame����
// 					  ���·��仺����ReallocateBuffers����
// 					  �ͷŻ�����FreeBuffers����
// 					  ���仺����AllocateBuffers����
// 					  ��������λ��CountBits����
// 					  ��⵱ǰ����ģʽDetectPixelFormat����
// 					  ���òɼ�����SetCapture����
// 					  ���òɼ�������SetVGAParameter����
// 					  ��ʼ���豸InitDevice����
//      Remark:		N/A
//  ----------------------------------------------------------------------------
//  Copyright (C) 2008 
//	Nanjing Talent Electronics and Technology Co., LTD.
//  All Rights Reserved
// *****************************************************************************

#ifndef RGB_SAMPLE_DLG
#define RGB_SAMPLE_DLG

#include "./include/TC1000API.h"
#include "TCAPI.h"
//#include "DDrawRGB32.h"
#include <string>

using namespace std;

#define RGBSAMPWM_MAXSIZE ( RGBWM_BASE + 0x1001 )
#define RGBSAMPWM_QUALPARA ( RGBWM_BASE + 0x1002 )
#define RGBSAMPWM_QUALADJUST ( RGBWM_BASE + 0x1003 )

/*If the biCompression member of the BITMAPINFOHEADER is BI_BITFIELDS, 
*the bmiColors member contains three DWORD color masks that specify 
*the red, green, and blue components, respectively, of each pixel. 
*Each DWORD in the bitmap array represents a single pixel.
*/
//BITMAPINFOHEADER->ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.WIN32COM.v10.en/gdi/bitmaps_1rw2.htm
typedef struct
{
	BITMAPINFOHEADER bmiHeader;	
	DWORD       colorMark[3];	//���ڵ�ǰ֧�ֵ���32λ����ģʽ��������Ҫ3��������DWORD���ֱ��ź죬�̣���������ο�MSDN
}  RGBBITMAP, *PRGBBITMAP;
/////////////////////////////////////////////////////////////////////////////
// CRGBSAMPDlg dialog

struct Lock
{
	Lock(CRITICAL_SECTION * cs) :_cs(cs)
	{	::EnterCriticalSection(_cs);	}
	~Lock()
	{	::LeaveCriticalSection(_cs);	}
	CRITICAL_SECTION * _cs;
}  ;

class CRGBSAMPDlg 
{
// Construction
public:
	
	CRGBSAMPDlg(HWND* pParent = NULL, HWND hWnd=0);	// standard constructor
	~CRGBSAMPDlg();

	string m_WorkFolder;
	fun_NewCapture m_NewCapture;
	int m_maxCachePicNumber;

// Implementation
	public:
// 		/*****************************************************************************
// 			FunctionName:	RGBClose
// 			Purpose:		�رղɼ�����
// 			Parameter:		
// 							hWnd ��ǰ��Ĳɼ��ؼ��Ĵ��ھ��
// 							phRGBCapture ָ��ǰ��Ĳɼ��������ָ��
// 							pCaptureBufferָ����ǰ��¼��ɼ�����Ļ���
// 			Return:			NO
// 
// 			Remark:			N/A
// 		*****************************************************************************/ 
// 		void RGBClose(  HWND hWnd
// 			          , HRGBCAPTURE *phRGBCapture 
// 				      , RGBCAPTUREBUFFER *pCaptureBuffer);

		/*****************************************************************************
			FunctionName:	stopVGA
			Purpose:		ֹͣ�ɼ�����
			Parameter:		
							hWnd ��ǰ��Ĳɼ��ؼ��Ĵ��ھ��
							phRGBCapture ָ��ǰ��Ĳɼ��������ָ��
							pCaptureBufferָ��ǰ��¼��ɼ�����Ļ����ָ��
			Return:			NO

			Remark:			N/A
		*****************************************************************************/ 
		void stopVGA(  HWND HWnd 
					 , HRGBCAPTURE *phRGBCapture 
					 , RGBCAPTUREBUFFER *pCaptureBuffer);

		/*****************************************************************************
			FunctionName:	RunVGA
			Purpose:		���вɼ�����
			Parameter:		
							hWnd ��ǰ��Ĳɼ��ؼ��Ĵ��ھ��
							phRGBCapture ָ��ǰ��Ĳɼ��������ָ��
							pCaptureBufferָ��ǰ��¼��ɼ�����Ļ����ָ��
							pBufferSize ָ����������Ļ����С��ָ��
							pRGBBitmap λͼ��Ϣָ��
							usDevice     ָ����ǰ��Ĳɼ��������
			Return:			NO

			Remark:			N/A
		*****************************************************************************/ 
		void RunVGA(HWND hWnd
					, HRGBCAPTURE *phRGBCapture 
			        , RGBCAPTUREBUFFER *pCaptureBuffer
					, unsigned long    *pBufferSize
					, RGBBITMAP        *pRGBBitmap
					, unsigned long usDevice);

		/*****************************************************************************
			FunctionName:	PaintFrame
			Purpose:		����ͼ��
			Parameter:		
							hWnd ��ǰ��Ĳɼ��ؼ��Ĵ��ھ��
							hDC  ��ǰ��Ĳɼ��ؼ��Ĵ���DC
							pCaptureBufferָ��ǰ��¼��ɼ�����Ļ����ָ��
							pRGBBitmap λͼ��Ϣָ��
			Return:			NO

			Remark:			N/A
		*****************************************************************************/
		void PaintFrame(HWND hWnd
			            , HDC hDC 
			            , RGBCAPTUREBUFFER captureBuffer
						, RGBBITMAP *pRGBBitmap);   

		/*****************************************************************************
			FunctionName:	FreeBuffers
			Purpose:		�ͷŻ���
			Parameter:		
							hWnd ��ǰ��Ĳɼ��ؼ��Ĵ��ھ��
							hRGBCapture ��ǰ��Ĳɼ������
							pCaptureBufferָ��ǰ��¼��ɼ�����Ļ����ָ��
			Return:			
							�ɹ����أ�TRUE                     RGBERROR_NO_ERROR
							ʧ�ܷ��أ�FALSE                    RGBERROR_INVALID_HRGBCAPTURE
							                                   RGBERROR_INVALID_INDEX
			Remark:			N/A
		*****************************************************************************/
		BOOL FreeBuffers( HRGBCAPTURE hRGBCapture
						  , HWND hWnd 
						  , RGBCAPTUREBUFFER *pCaptureBuffer);  //�ͷŻ�����

		/*****************************************************************************
			FunctionName:	AllocateBuffers
			Purpose:		����ָ����С���仺��
			Parameter:		
							hWnd ��ǰ��Ĳɼ��ؼ��Ĵ��ھ��
							hRGBCapture ��ǰ��Ĳɼ������
							pCaptureBufferָ��ǰ��¼��ɼ����軺���ָ��
							BufferSize ��������Ļ����С
			Return:		
			                �ɼ�������ָ��pCaptureBuffer
							�ɹ����أ�TRUE                         RGBERROR_NO_ERROR
							ʧ�ܷ��أ�FALSE                        RGBERROR_INVALID_HRGBCAPTURE
							                                       RGBERROR_INVALID_POINTER
									                               RGBERROR_INVALID_INDEX
			Remark:			N/A
		*****************************************************************************/
		BOOL AllocateBuffers( HRGBCAPTURE hRGBCapture
                              , RGBCAPTUREBUFFER *pCaptureBuffer
							  , HWND hWnd
							  , UINT BufferSize);     //���仺��

		/*****************************************************************************
			FunctionName:	CountBits
			Purpose:		��������λ��
			Parameter:		
							ulValue ��ŵ�ǰ���λͼ���������ݻ���
							start  ��ʼ������ֵ��Ϊ0
							end  ���������ݵ�������
			Return:			��������λ��

			Remark:			N/A
		*****************************************************************************/
	    int CountBits ( unsigned long  ulValue
					    , int            start
						, int            end );    //��������λ��

		/*****************************************************************************
			FunctionName:	DetectPixelFormat
			Purpose:		��⵱ǰ����ģʽ
			Parameter:		
							hWnd ��ǰ��Ĳɼ��ؼ��Ĵ��ھ��
							pColourMaskָ��һ�����������ֵ����飬ÿ���ֱַ�װ���ŵ�ǰͼ��λ�ĺ죬�̣���ɫ������  

			Return:		    
			                ���ص�ǰ����ģʽ:
							RGBPIXELFORMAT_555
                            RGBPIXELFORMAT_565
			                RGBPIXELFORMAT_888
                            RGBPIXELFORMAT_UNKNOWN

			Remark:			N/A
		*****************************************************************************/
		unsigned short DetectPixelFormat ( HWND hWnd 
										   , DWORD *pColourMask );//��⵱ǰ����ģʽ

		/*****************************************************************************
			FunctionName:	SetCapture
			Purpose:		���òɼ�����
			Parameter:		
			                hWnd ��ǰ��Ĳɼ��ؼ��Ĵ��ھ��
			                pCaptureParmsָ��ǰ�ɼ��¼��Ĳ�����ָ��
							phRGBCapture ָ��ǰ��Ĳɼ��������ָ��
							pCaptureBufferָ��ǰ��¼��ɼ����軺���ָ��
							pBufferSize ָ�򻺴��С��ָ��
							pRGBBitmap λͼ��Ϣָ��
			Return:		    
							�ɹ����أ�TRUE                            RGBERROR_NO_ERROR
							ʧ�ܷ��أ�FALSE                           RGBERROR_INVALID_HRGBCAPTURE
																	  RGBERROR_INVALID_INDEX
																	  RGBERROR_BUFFER_TOO_SMALL
																	  RGBERROR_INVALID_POINTER
			Remark:			N/A
		*****************************************************************************/
		BOOL SetCapture( HWND hWnd
						 , RGBCAPTUREPARMS  *pCaptureParms
						 , HRGBCAPTURE      *phRGBCapture
						 , RGBCAPTUREBUFFER *pCaptureBuffer
						 , unsigned long    *pBufferSize
						 , RGBBITMAP        *pRGBBitmap);//���òɼ�����

		/*****************************************************************************
			FunctionName:	SetVGAParameter
			Purpose:		���òɼ�������
			Parameter:		
							hWnd ��ǰ��Ĳɼ��ؼ��Ĵ��ھ��
			                pCaptureParmsָ��ǰ�ɼ��¼��Ĳ�����ָ��
							phRGBCapture ָ��ǰ��Ĳɼ��������ָ��
							pRGBBitmap λͼ��Ϣָ��
							usDevice Ҫ���õĲɼ�������������
			Return:			�ɹ����أ�TRUE
							ʧ�ܷ��أ�FALSE

			Remark:			N/A
		*****************************************************************************/
		BOOL SetVGAParameter( HWND hWnd
							  , RGBCAPTUREPARMS *pCaptureParms
							  , HRGBCAPTURE *phRGBCapture
							  , RGBBITMAP *pRGBBitmap
							  , unsigned long usDevice);//���òɼ�������

		/*****************************************************************************
			FunctionName:	InitDevice
			Purpose:		��ʼ���豸
			Parameter:		NO
			Return:			�ɹ����أ�TRUE
							ʧ�ܷ��أ�FALSE

			Remark:			N/A
		*****************************************************************************/
		BOOL InitDevice();   //��ʼ���豸
public:

//	// Generated message map functions
//	//{{AFX_MSG(CRGBSAMPDlg)
	virtual BOOL OnInitDialog();
	//afx_msg HCURSOR OnQueryDragIcon();
	void OnClose();
	void OnDestroy();
	//afx_msg void OnSize(UINT nType, int cx, int cy);
	// void OnRButtonUp(UINT nFlags, CPoint point);
	 void OnRunVGA1();
	 void OnMyMessage_Sta1(WPARAM wParam, LPARAM lParam);
	 void OnStopVGA1();
	 int OnCreate(LPCREATESTRUCT lpCreateStruct);
	// HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	 void OnAdjustVga1();
	 void OnMyMessage_modechange(WPARAM wParam, LPARAM lParam);
	 void OnPauseVga1();
	 void OnGoonVga1();
	// void OnLButtonDblClk(UINT nFlags, CPoint point);
	 void OnQualpropVga1();
	 void OnMyMessage_nosignal(WPARAM wParam, LPARAM lParam);
	 LRESULT OnMyMessage_datamode(WPARAM wParam, LPARAM lParam);
	 void OnMyMessage_ddrerror(WPARAM wParam, LPARAM lParam);
	 void OnCancel();
	//}}AFX_MSG
//	DECLARE_MESSAGE_MAP()
private:
//	CBrush m_Brush;
	LPVOID pData;
/*
*�ɼ��¼����
*/
//	CDDrawRGB32 ddrawRGB;
	HANDLE	m_hPaintThread;
	DWORD	m_dwThreadID;
public:
	HWND	m_hWnd;
	HANDLE	m_hPThreadEvents[2];
 	HANDLE	m_hCloseEvent1;
	HANDLE  m_hCapEvent;
	bool    m_isInitSucceed;
	RGBCAPTUREBUFFER *pABuffer,*pPBuffer;
/*
*��־�Ƿ�ʼ��¼ͼ��֡��
*/
BOOL startcount1;
/*
*��Ųɼ���ʵ�ʻ�����ͼ�������
*/
int  g_framecount1;  //��ŵ�һ���ɼ����ɼ�ͼ������

/*
*��־��ǰ�Ĵ��ڵ�״̬
*/
int              ActiveStatic;   //��־��ǰ���ڵ�״̬:0-û����󻯣�1����󻯴��ڡ�
/*
*�ɼ��¼���ͣ��־
*/
BOOL            PauseRunSignal1;    //�ɼ��¼���ͣ��־��true��ͣ��false����
/*
*�ɼ��¼�λͼ��Ϣ
*/
RGBBITMAP       RGBBitmap1;     //��һ���ɼ��¼�λͼ��Ϣ


	/*
*�ɼ��¼����
*/
HRGBCAPTURE     hRGBCapture1; //��1���ɼ�����Ӧ�ľ��
/*
*��־��ǰ�����Ƿ��ѱ�ʹ��
*/
bool      bUsingAccBuffer1;  //��־��ǰ�����Ƿ��ѱ�ʹ�ã�true�ǣ�false��
/*
*����ģʽ
*/
unsigned short   pixelFormat;             //����ģʽ
/*
*�ɼ�����Ӧ�Ļ����С
*/
unsigned long    bufferSize1;         //��1���ɼ�����Ӧ�Ļ����С
/*
*�ɼ�����Ӧ�Ļ���
*/
RGBCAPTUREBUFFER *pCaptureBuffer1;//��1���ɼ�����Ӧ�Ļ���
/*
*�źű�־
*/
bool signalflag;
string singnalInfo;
/*
*���ݴ����ʽ��0��yuv��Ĭ�ϣ���1��rgb��
*/
DWORD datamode;
int m_capnum; //�ɼ����ȡֵ��Χ>1
int m_iIndex;
};

#endif 

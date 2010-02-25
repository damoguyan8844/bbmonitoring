// RGBSAMPDlg.h : header file
//
// *****************************************************************************
//  RGBSAMPDlg   VERSION:  1.1   DATE: 2008-10-15
//  ----------------------------------------------------------------------------
//		FileName: 	RGBSAMPDlg.h
//		Created:	2008-10-15   16:50
//		Author:		MengJuan
//		Purpose:	采集卡图象采集及处理
//		Version:	
//					【V1.1 2008-10-15 mengjuan】
//						Initial
//					  创建类CRGBSAMPDlg
// 	                  初始化OnInitDialog系统方法
//                    OnSysCommand系统方法
// 					  画图OnPaint系统方法
// 	                  质量管理OnQueryDragIcon方法
// 	                  关闭OnClose方法
// 					  关闭OnDestroy方法
//					  尺寸大小改变OnSize方法
// 					  右键单击OnRButtonUp方法
// 					  运行第一个采集卡OnRunVGA1方法
// 					  SDK返回采集消息后触发OnMyMessage_Sta1方法
// 					  停止采集OnStopVGA1方法
// 					  创建窗口OnCreate方法
// 					  窗口颜色控制OnCtlColor方法
// 					  调整采集参数OnAdjustVga1方法
//					  OnClickTest方法
// 					  模式改变，分辨率改变时触发OnMyMessage_modechange方法
// 					  暂停OnPauseVga1方法
// 					  继续OnGoonVga1方法
// 					  左键双击OnLButtonDblClk方法
// 					  质量管理OnQualpropVga1方法
// 					  无信号OnMyMessage_nosignal方法		
// 					  运行采集卡RunVGA方法
// 					  画出图象PaintFrame方法
// 					  重新分配缓存区ReallocateBuffers方法
// 					  释放缓冲区FreeBuffers方法
// 					  分配缓冲区AllocateBuffers方法
// 					  数出像素位数CountBits方法
// 					  检测当前像素模式DetectPixelFormat方法
// 					  设置采集参数SetCapture方法
// 					  设置采集卡参数SetVGAParameter方法
// 					  初始化设备InitDevice方法
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
	DWORD       colorMark[3];	//由于当前支持的是32位像素模式，所以需要3个单独的DWORD来分别存放红，绿，蓝，具体参考MSDN
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
// 			Purpose:		关闭采集程序
// 			Parameter:		
// 							hWnd 当前活动的采集控件的窗口句柄
// 							phRGBCapture 指向当前活动的采集卡句柄的指针
// 							pCaptureBuffer指定当前活动事件采集所需的缓存
// 			Return:			NO
// 
// 			Remark:			N/A
// 		*****************************************************************************/ 
// 		void RGBClose(  HWND hWnd
// 			          , HRGBCAPTURE *phRGBCapture 
// 				      , RGBCAPTUREBUFFER *pCaptureBuffer);

		/*****************************************************************************
			FunctionName:	stopVGA
			Purpose:		停止采集程序
			Parameter:		
							hWnd 当前活动的采集控件的窗口句柄
							phRGBCapture 指向当前活动的采集卡句柄的指针
							pCaptureBuffer指向当前活动事件采集所需的缓存的指针
			Return:			NO

			Remark:			N/A
		*****************************************************************************/ 
		void stopVGA(  HWND HWnd 
					 , HRGBCAPTURE *phRGBCapture 
					 , RGBCAPTUREBUFFER *pCaptureBuffer);

		/*****************************************************************************
			FunctionName:	RunVGA
			Purpose:		运行采集程序
			Parameter:		
							hWnd 当前活动的采集控件的窗口句柄
							phRGBCapture 指向当前活动的采集卡句柄的指针
							pCaptureBuffer指向当前活动事件采集所需的缓存的指针
							pBufferSize 指向所需申请的缓存大小的指针
							pRGBBitmap 位图信息指针
							usDevice     指定当前活动的采集卡的序号
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
			Purpose:		画出图象
			Parameter:		
							hWnd 当前活动的采集控件的窗口句柄
							hDC  当前活动的采集控件的窗口DC
							pCaptureBuffer指向当前活动事件采集所需的缓存的指针
							pRGBBitmap 位图信息指针
			Return:			NO

			Remark:			N/A
		*****************************************************************************/
		void PaintFrame(HWND hWnd
			            , HDC hDC 
			            , RGBCAPTUREBUFFER captureBuffer
						, RGBBITMAP *pRGBBitmap);   

		/*****************************************************************************
			FunctionName:	FreeBuffers
			Purpose:		释放缓存
			Parameter:		
							hWnd 当前活动的采集控件的窗口句柄
							hRGBCapture 当前活动的采集卡句柄
							pCaptureBuffer指向当前活动事件采集所需的缓存的指针
			Return:			
							成功返回：TRUE                     RGBERROR_NO_ERROR
							失败返回：FALSE                    RGBERROR_INVALID_HRGBCAPTURE
							                                   RGBERROR_INVALID_INDEX
			Remark:			N/A
		*****************************************************************************/
		BOOL FreeBuffers( HRGBCAPTURE hRGBCapture
						  , HWND hWnd 
						  , RGBCAPTUREBUFFER *pCaptureBuffer);  //释放缓冲区

		/*****************************************************************************
			FunctionName:	AllocateBuffers
			Purpose:		按照指定大小分配缓存
			Parameter:		
							hWnd 当前活动的采集控件的窗口句柄
							hRGBCapture 当前活动的采集卡句柄
							pCaptureBuffer指向当前活动事件采集所需缓存的指针
							BufferSize 所需申请的缓存大小
			Return:		
			                采集缓存区指针pCaptureBuffer
							成功返回：TRUE                         RGBERROR_NO_ERROR
							失败返回：FALSE                        RGBERROR_INVALID_HRGBCAPTURE
							                                       RGBERROR_INVALID_POINTER
									                               RGBERROR_INVALID_INDEX
			Remark:			N/A
		*****************************************************************************/
		BOOL AllocateBuffers( HRGBCAPTURE hRGBCapture
                              , RGBCAPTUREBUFFER *pCaptureBuffer
							  , HWND hWnd
							  , UINT BufferSize);     //分配缓存

		/*****************************************************************************
			FunctionName:	CountBits
			Purpose:		数出像素位数
			Parameter:		
							ulValue 存放当前相关位图二进制数据缓存
							start  开始计数的值，为0
							end  缓存中数据的总数量
			Return:			返回像素位数

			Remark:			N/A
		*****************************************************************************/
	    int CountBits ( unsigned long  ulValue
					    , int            start
						, int            end );    //数出像素位数

		/*****************************************************************************
			FunctionName:	DetectPixelFormat
			Purpose:		检测当前像素模式
			Parameter:		
							hWnd 当前活动的采集控件的窗口句柄
							pColourMask指向一个包含三个字的数组，每个字分辨装载着当前图象位的红，绿，蓝色素数据  

			Return:		    
			                返回当前象素模式:
							RGBPIXELFORMAT_555
                            RGBPIXELFORMAT_565
			                RGBPIXELFORMAT_888
                            RGBPIXELFORMAT_UNKNOWN

			Remark:			N/A
		*****************************************************************************/
		unsigned short DetectPixelFormat ( HWND hWnd 
										   , DWORD *pColourMask );//检测当前像素模式

		/*****************************************************************************
			FunctionName:	SetCapture
			Purpose:		设置采集参数
			Parameter:		
			                hWnd 当前活动的采集控件的窗口句柄
			                pCaptureParms指向当前采集事件的参数的指针
							phRGBCapture 指向当前活动的采集卡句柄的指针
							pCaptureBuffer指向当前活动事件采集所需缓存的指针
							pBufferSize 指向缓存大小的指针
							pRGBBitmap 位图信息指针
			Return:		    
							成功返回：TRUE                            RGBERROR_NO_ERROR
							失败返回：FALSE                           RGBERROR_INVALID_HRGBCAPTURE
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
						 , RGBBITMAP        *pRGBBitmap);//设置采集参数

		/*****************************************************************************
			FunctionName:	SetVGAParameter
			Purpose:		设置采集卡参数
			Parameter:		
							hWnd 当前活动的采集控件的窗口句柄
			                pCaptureParms指向当前采集事件的参数的指针
							phRGBCapture 指向当前活动的采集卡句柄的指针
							pRGBBitmap 位图信息指针
							usDevice 要设置的采集卡的索引参数
			Return:			成功返回：TRUE
							失败返回：FALSE

			Remark:			N/A
		*****************************************************************************/
		BOOL SetVGAParameter( HWND hWnd
							  , RGBCAPTUREPARMS *pCaptureParms
							  , HRGBCAPTURE *phRGBCapture
							  , RGBBITMAP *pRGBBitmap
							  , unsigned long usDevice);//设置采集卡参数

		/*****************************************************************************
			FunctionName:	InitDevice
			Purpose:		初始化设备
			Parameter:		NO
			Return:			成功返回：TRUE
							失败返回：FALSE

			Remark:			N/A
		*****************************************************************************/
		BOOL InitDevice();   //初始化设备
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
*采集事件句柄
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
*标志是否开始记录图象帧数
*/
BOOL startcount1;
/*
*存放采集卡实际画出的图象的数量
*/
int  g_framecount1;  //存放第一个采集卡采集图象数量

/*
*标志当前的窗口的状态
*/
int              ActiveStatic;   //标志当前窗口的状态:0-没有最大化；1－最大化窗口。
/*
*采集事件暂停标志
*/
BOOL            PauseRunSignal1;    //采集事件暂停标志：true暂停，false运行
/*
*采集事件位图信息
*/
RGBBITMAP       RGBBitmap1;     //第一个采集事件位图信息


	/*
*采集事件句柄
*/
HRGBCAPTURE     hRGBCapture1; //第1个采集卡对应的句柄
/*
*标志当前缓存是否已被使用
*/
bool      bUsingAccBuffer1;  //标志当前缓存是否已被使用，true是；false否
/*
*像素模式
*/
unsigned short   pixelFormat;             //像素模式
/*
*采集卡对应的缓存大小
*/
unsigned long    bufferSize1;         //第1个采集卡对应的缓存大小
/*
*采集卡对应的缓存
*/
RGBCAPTUREBUFFER *pCaptureBuffer1;//第1个采集卡对应的缓存
/*
*信号标志
*/
bool signalflag;
string singnalInfo;
/*
*数据传输格式（0：yuv（默认）；1：rgb）
*/
DWORD datamode;
int m_capnum; //采集编号取值范围>1
int m_iIndex;
};

#endif 

// RGBSAMPDlg.cpp : implementation file
// *****************************************************************************
//  RGBSAMPDlg   VERSION:  1.0   DATE: 2008-10-29
//  ----------------------------------------------------------------------------
//		FileName: 	RGBSAMPDlg.cpp
//		Created:	2008-10-29   16:57
//		Author:		孟娟
//		Purpose:	采集图象，显示图像，通过修改图象参数调节显示图象质量
//		Version:	
//					【V1.0 2008-10-29 孟娟】
//						Initial
//		Remark:		N/A
//  ----------------------------------------------------------------------------
//  Copyright (C) 2008 
//	Nanjing Talent Electronics and Technology Co., LTD.
//  All Rights Reserved
// *****************************************************************************

#include "stdafx.h"
#include "RGBSAMP.h"
#include "RGBSAMPDlg.h"
#include "AdjParamDlg.h"
#include "QualPropDlg.h"
#include "ColorSpace.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif
//////////////////////////////////////////////////////////////////////////
// RECT client;
// RECT screen;
/*
*声明存放错误报告消息的函数
*/
char * RGBErrorToString(RGBERROR error);

/* Map Pixel format to number of bytes per pixel */
static int PixelFormatToBytesPerPixel[] =
{
   0  /* RGBPIXELFORMAT_UNKNOWN */,
   4  /* RGBPIXELFORMAT_555 */,
   4  /* RGBPIXELFORMAT_565 */,
   8  /* RGBPIXELFORMAT_888 */,
};
// 林云修改了该数组的值，全部扩大了两倍，因为在部分机器上运行，会内存越界



//RGBCAPTUREBUFFER *pABuffer,*pPBuffer;
DWORD WINAPI  thread_paint(LPVOID lpParameter)
{
	CRGBSAMPDlg *pRGBsamp = (CRGBSAMPDlg *)lpParameter;
//	HDC hDC;
 	HWND hWnd;
	RGBCAPTUREBUFFER *pCaptureBuffer;
	RGBBITMAP        *pRGBBitmap;
	bool		bExitThread	= false;
   	DWORD		dwWaitResult;
	HANDLE threadMarks[2];
	threadMarks[0] = pRGBsamp->m_hPThreadEvents[1];
	threadMarks[1] = pRGBsamp->m_hPThreadEvents[0];

	if (NULL == pRGBsamp->m_hCloseEvent1)
	{
		return FALSE;
	}
	WaitForSingleObject(pRGBsamp->m_hCloseEvent1, INFINITE);
	ResetEvent(pRGBsamp->m_hCloseEvent1);

	while (!bExitThread)
	{
		dwWaitResult = WaitForMultipleObjects(2, threadMarks, FALSE, INFINITE);
		if ((WAIT_OBJECT_0) == dwWaitResult)
		{			
			// End of thread signalled.
			bExitThread = true;
			continue;
		}
		// We got a WAIT_OBJECT_0, so we want another frame.
		ResetEvent(threadMarks[1]);

		//////////////////////////////////////////////////////////////////////////
		/*根据当前消息的lParam参数来判断消息来自哪个采集卡的采集事件*/
		hWnd = pRGBsamp->m_hWnd;  //此处固定了第1个采集卡只能对应第1个窗口控件
		pCaptureBuffer =	pRGBsamp->pCaptureBuffer1;
		pRGBBitmap = &pRGBsamp->RGBBitmap1;

		pRGBsamp->pABuffer->bufferflags = TRUE;
		//pABuffer->bufferflags = TRUE;
		//////////////////////////////////////////////////////////////////////////
		if (!pRGBsamp->PauseRunSignal1)
		{
		//	hDC = ::GetDC(hWnd);
			pRGBsamp->PaintFrame(hWnd, 0, *(pRGBsamp->pPBuffer), pRGBBitmap);
		//	::ReleaseDC(hWnd, hDC);
		}
		pRGBsamp->pPBuffer->bufferflags = FALSE;  		
		SetEvent(pRGBsamp->m_hCapEvent);
		//pPBuffer->bufferflags = FALSE;  		
	}
	
	CloseHandle(pRGBsamp->m_hPThreadEvents[1]);
	pRGBsamp->m_hPThreadEvents[1] = NULL;
	CloseHandle(pRGBsamp->m_hPThreadEvents[0]);
	pRGBsamp->m_hPThreadEvents[0] = NULL;
 	SetEvent(pRGBsamp->m_hCloseEvent1);
	return 1000;
}

/////////////////////////////////////////////////////////////////////////////
// CRGBSAMPDlg dialog

CRGBSAMPDlg::CRGBSAMPDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CRGBSAMPDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CRGBSAMPDlg)
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hCloseEvent1 = NULL;
	m_hPThreadEvents[0] = NULL;
	m_hPThreadEvents[1] = NULL;
	m_capnum = 0;
	m_WorkFolder="c:";
	m_maxCachePicNumber=100;
}

void CRGBSAMPDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CRGBSAMPDlg)
	DDX_Control(pDX, IDC_STATIC2, m_static);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CRGBSAMPDlg, CDialog)
	//{{AFX_MSG_MAP(CRGBSAMPDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_QUERYDRAGICON()
	ON_WM_CLOSE()
	ON_WM_DESTROY()
	ON_WM_SIZE()
	ON_WM_TIMER()
	ON_WM_RBUTTONUP()
	ON_COMMAND(ID_Run_VGA1, OnRunVGA1)
	ON_MESSAGE(RGBWM_FRAMECAPTURED, OnMyMessage_Sta1)
	ON_COMMAND(ID_Stop_VGA1, OnStopVGA1)
	ON_WM_CREATE()
	ON_WM_CTLCOLOR()
	ON_COMMAND(ID_ADJUST_VGA1, OnAdjustVga1)
	ON_MESSAGE(RGBWM_MODECHANGED, OnMyMessage_modechange)
	ON_COMMAND(ID_PAUSE_VGA1, OnPauseVga1)
	ON_COMMAND(ID_GOON_VGA1, OnGoonVga1)
	ON_WM_LBUTTONDBLCLK()
	ON_COMMAND(ID_QUALPROP_VGA1, OnQualpropVga1)
	ON_MESSAGE(RGBWM_NOSIGNAL, OnMyMessage_nosignal)
	ON_MESSAGE(RGBWM_DATAMODE, OnMyMessage_datamode)
	ON_MESSAGE(RGBWM_DDRERROR, OnMyMessage_ddrerror)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRGBSAMPDlg message handlers


/*****************************************************************************/
/*
*创建窗口，初始化采集卡
*/
int CRGBSAMPDlg::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
//	MessageBox("Create Dialog","Info",MB_ICONINFORMATION|MB_OK);
	
	if (CDialog::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	// TODO: Add your specialized creation code here
	if (!InitDevice())	//初始化采集卡
	{
		return -1;
	}
	
	return 0;
}

/*****************************************************************************/
/*
*初始化设备
*/
BOOL CRGBSAMPDlg::InitDevice()
{
	unsigned long ulNumber = 0;  //存放检测到的采集卡总数量
	RGBERROR error;
	error = RGBCaptureInitialise(&ulNumber);

	if (RGBERROR_NO_ERROR != error)
	{
		//初始化失败，返回值仅为：RGBERROR_HARDWARE_NOT_FOUND
		if (RGBERROR_HARDWARE_NOT_FOUND == error)
		{
			//没有找到硬件设备,打印日志
			TRACE("RGBCaptureInitialise() 初始化，没有找到硬件设备\n");
			return(FALSE);
		}
	}
	if (0 == ulNumber)
	{
		MessageBox("没有找到硬件设备","系统提示",MB_ICONWARNING | MB_OK);
		return(FALSE);
	}
	return(TRUE);
}

/*****************************************************************************/
/*
*对话框初始化，设定窗口大小，创建背景画刷
*/
BOOL CRGBSAMPDlg::OnInitDialog()
{
	CDialog::OnInitDialog();	
	// TODO: Add extra initialization here
//  	ModifyStyle(WS_CAPTION,0,0);   //如果不想去掉标题栏，去掉该句。
//     SendMessage(WM_SYSCOMMAND,SC_MAXIMIZE,0); 
 //	ShowWindow(SW_SHOWMAXIMIZED);   
	hRGBCapture1 = 0;
    //////////////////////////////////////////////////////////////////////////
    /*设定窗口大小*/
// 	RECT myRect;
//     this->GetClientRect(&myRect);
// 	MoveWindow(&myRect,TRUE);
    //m_static1.MoveWindow(0, 0, myRect.right, myRect.bottom,TRUE);
	//////////////////////////////////////////////////////////////////////////
	/*??????*/
// 	m_Brush.CreateSolidBrush(RGB(0,0,0));
// 
// 	//////////////////////////////////////////////////////////////////////////
// 	/*????????*/
// 	CFont * cFont=new CFont;
// 	cFont->CreateFont(0,0,0,0,FW_BOLD,TRUE,FALSE,0, 
// 							  ANSI_CHARSET,OUT_DEFAULT_PRECIS,
// 							  CLIP_DEFAULT_PRECIS,DEFAULT_QUALITY,
// 							  DEFAULT_PITCH&FF_SWISS,_T("Arial"));
// 	SetFont(cFont);
// 	delete cFont;
	pData = new LPVOID[1600*1200*4];
	hRGBCapture1 = 0;
	ActiveStatic = 0;   //标志当前窗口的状态:0-没有最大化；1－最大化窗口。
    PauseRunSignal1 = false;    //采集事件暂停标志：true暂停，false运行
	bUsingAccBuffer1=false;  //标志当前缓存是否已被使用，true是；false否
    bufferSize1 = 0L;         //第1个采集卡对应的缓存大小
	startcount1 = FALSE;
	g_framecount1 = 0;  //存放第一个采集卡采集图象数量
	signalflag = FALSE;
	datamode = 1;

//	OnRunVGA1();
//	MessageBox("Init Dialog","Info",MB_ICONINFORMATION|MB_OK);
	return TRUE;  // return TRUE  unless you set the focus to a control
}


// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

/*****************************************************************************/

/*****************************************************************************/
/*
*窗口颜色控制
*/
HBRUSH CRGBSAMPDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
 	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
// 	
// 	// TODO: Change any attributes of the DC here
// 	hbr = m_Brush;  //??????
// 	switch(nCtlColor)
//     {
//        case CTLCOLOR_STATIC: //对所有静态文本控件的设置
//       {
// 			CFont * cFont=new CFont;
// 			cFont->CreateFont(30,0,0,0,FW_SEMIBOLD,FALSE,FALSE,0, 
// 									  ANSI_CHARSET,OUT_DEFAULT_PRECIS,
// 									  CLIP_DEFAULT_PRECIS,DEFAULT_QUALITY,
// 									  DEFAULT_PITCH&FF_SWISS,"Arial");
// 			pDC->SetBkMode(TRANSPARENT); 
// 			//设置背景为透明
// 			pDC->SetTextColor(RGB(255,255,255)); //设置字体颜色
// 			pWnd->SetFont(cFont); //设置字体
// 			delete cFont;
//       }
// 	}

	
	// TODO: Return a different brush if the default is not desired
	return hbr;
}


/*****************************************************************************/
/*
*右击，弹出对话框
*/
void CRGBSAMPDlg::OnRButtonUp(UINT nFlags, CPoint point) 
{	
	RECT clientRect;	//存放显示控件的尺寸
	GetWindowRect(&clientRect);	//获取第一个显示控件的尺寸
	this->GetWindowRect(&clientRect);
	ClientToScreen(&point);
	if (PtInRect(&clientRect, point))
	{
		TRACE("OnRButtonUp(),鼠标在状态控件中\n");	//鼠标在第一个状态控件中
		CMenu menuPopup;
		if (menuPopup.CreatePopupMenu())
		{
			//向菜单 menuPopup 中添加菜单项
			menuPopup.AppendMenu(MF_STRING, ID_Run_VGA1, "开始VGA");
			menuPopup.AppendMenu(MF_STRING, ID_Stop_VGA1, "停止VGA");
			menuPopup.AppendMenu(MF_SEPARATOR);
			menuPopup.AppendMenu(MF_STRING, ID_ADJUST_VGA1, "VGA参数调节");
			menuPopup.AppendMenu(MF_STRING, ID_QUALPROP_VGA1, "质量管理");
			menuPopup.AppendMenu(MF_SEPARATOR);
			menuPopup.AppendMenu(MF_STRING, ID_PAUSE_VGA1, "暂停VGA");
			menuPopup.AppendMenu(MF_STRING, ID_GOON_VGA1, "继续VGA");
			menuPopup.AppendMenu(MF_SEPARATOR);
			
			//显示弹出式菜单,并对用户选择的菜单项作出反应
			
			menuPopup.TrackPopupMenu(TPM_RIGHTBUTTON, point.x, point.y, this, 0);
		}
	}	
	CDialog::OnRButtonUp(nFlags, point); 
}

/*****************************************************************************/
/*
*运行采集卡
*/
void CRGBSAMPDlg::OnRunVGA1() 
{
	// TODO: Add your command handler code here

	if (0 == hRGBCapture1)
	{
		m_hPThreadEvents[0] = CreateEvent(NULL, TRUE, FALSE, NULL);
		m_hPThreadEvents[1] = CreateEvent(NULL, FALSE, FALSE, NULL);
		m_hPaintThread = CreateThread(NULL, 0, thread_paint, this, 0, &m_dwThreadID);

		m_hCapEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
		SetEvent(m_hCapEvent);
		m_hCloseEvent1 = CreateEvent(NULL, TRUE, FALSE, NULL);
		SetEvent(m_hCloseEvent1);
		pCaptureBuffer1 = new RGBCAPTUREBUFFER[2];
		RunVGA(m_hWnd, &hRGBCapture1, pCaptureBuffer1, &bufferSize1, &RGBBitmap1, m_iIndex);

	}
	else
	{
//		::MessageBox(m_hWnd, "VGA1采集卡已运行 !","系统提示",MB_ICONWARNING | MB_OK);
	}
}

/*****************************************************************************/
/*
 *运行采集程序
 */
void CRGBSAMPDlg::RunVGA(  HWND hWnd 
						 , HRGBCAPTURE *phRGBCapture 
						 , RGBCAPTUREBUFFER *pCaptureBuffer 
						 , unsigned long *pBufferSize
						 , RGBBITMAP        *pRGBBitmap
						 , unsigned long usDevice)
{	
	RGBCAPTUREPARMS CaptureParms;  //存放当前采集卡的参数
	//////////////////////////////////////////////////////////////////////////
	/*设置采集卡参数*/
	if (!SetVGAParameter(m_hWnd,&CaptureParms, phRGBCapture, pRGBBitmap, usDevice))   
	{
		TRACE("SetVGAParameter(),设置采集卡参数失败");
		MessageBox("没有找到硬件设备","系统提示",MB_ICONWARNING | MB_OK);
		return;
	}
	Sleep(1);
    //////////////////////////////////////////////////////////////////////////
    /*获取当前像素模式*/
   // pixelFormat = DetectPixelFormat (hWnd,(DWORD *)&pRGBBitmap->bmiColors[0]);
    switch (pixelFormat)
	{

        case RGBPIXELFORMAT_555:
        case RGBPIXELFORMAT_565:
		{
			pRGBBitmap->bmiHeader.biBitCount = 16;
			break;
		}

		case RGBPIXELFORMAT_888: //当前只支持这个模式
		{
			pRGBBitmap->bmiHeader.biBitCount = 32;
			break;
		}

		default:
		{
			break;
		}
	}
	Sleep(1);
	//////////////////////////////////////////////////////////////////////////
    /* 确定缓存的大小值 ，分配缓存*/
    *pBufferSize =1600 * 1200 * PixelFormatToBytesPerPixel[pixelFormat];
	if (!AllocateBuffers(*phRGBCapture, &pCaptureBuffer[0], hWnd, *pBufferSize))
	{
		return ;
	} 
	Sleep(1);
	if (!AllocateBuffers(*phRGBCapture, &pCaptureBuffer[1], hWnd, *pBufferSize))
	{
		return ;
	} 
	Sleep(1);
	//////////////////////////////////////////////////////////////////////////
	/*设置采集参数*/
	if (!SetCapture(hWnd, &CaptureParms, phRGBCapture, pCaptureBuffer, pBufferSize, pRGBBitmap)) 
	{
		TRACE("SetCapture(),设置采集参数失败");
		return;
	}

}

/*****************************************************************************/
/*
*设置采集卡参数
*/
BOOL CRGBSAMPDlg::SetVGAParameter(HWND hWnd
								  , RGBCAPTUREPARMS *pCaptureParms
								  , HRGBCAPTURE *phRGBCapture
								  , RGBBITMAP *pRGBBitmap
								  , unsigned long usDevice)
{
	RGBERROR error;
	//unsigned short pixelFormat;  //存放当前检测到的像素模式
	//////////////////////////////////////////////////////////////////////////
	/* 打开采集卡*/
	error = RGBCaptureOpen(usDevice,phRGBCapture);
	if (RGBERROR_NO_ERROR != error)     //打开设备失败，返回结果如下：
	{	
		switch (error)
		{
	        case RGBERROR_HARDWARE_NOT_FOUND :
			{
				TRACE("RGBCaptureOpen() 打开设备，没有找到硬件设备\n");//没有找到设备
				break;
			}

		    case RGBERROR_INVALID_POINTER :
			{
				TRACE("RGBCaptureOpen() 打开设备，没有找到指向该硬件设备的指针\n");//没有找到设备
				break;
			}

			case RGBERROR_INVALID_INDEX :
			{
				TRACE("RGBCaptureOpen() 打开设备，没有找到硬件设备的索引\n");//没有找到设备
				break;
			}

			case RGBERROR_DEVICE_IN_USE :
			{
				TRACE("RGBCaptureOpen() 打开设备，硬件设备正在被使用\n");//硬件设备正在被使用
				break;
			}

			case RGBERROR_UNABLE_TO_LOAD_DRIVER :
			{
				TRACE("RGBCaptureOpen() 打开设备，不能加载驱动\n");//软硬件不批配
				break;
			}

			case RGBERROR_INVALID_DEVICE :
			{
				TRACE("RGBCaptureOpen() 打开设备，硬件不存在\n");//软硬件不批配
				break;
			}

			default:
			{
				break;
			}
		}
		return FALSE;
	}

    //////////////////////////////////////////////////////////////////////////
	/*设置采集参数*/
	pCaptureParms->Size = sizeof (*pCaptureParms);
    pCaptureParms->Flags = RGBCAPTURE_PARM_PIXELFORMAT
                           | RGBCAPTURE_PARM_HWND
                           | RGBCAPTURE_PARM_NOTIFY_FLAGS
						   | RGBCAPTURE_PARM_VDIF_DESCRIPTION;    //设置的参数有：当前像素模式，主窗口句柄，通知消息模式
    //pixelFormat = DetectPixelFormat(hWnd,(DWORD *)&pRGBBitmap->bmiColors[0]);
	pixelFormat = DetectPixelFormat(hWnd,(DWORD *)&pRGBBitmap->colorMark);
    pCaptureParms->PixelFormat = pixelFormat;
	pCaptureParms->HWnd = m_hWnd;
    pCaptureParms->NotifyFlags = RGBNOTIFY_NO_SIGNAL | RGBNOTIFY_MODE_CHANGE;
	sprintf(pCaptureParms->VideoTimings.Description, "titleTCAPI");
	error = RGBCaptureSetParameters(*phRGBCapture, pCaptureParms,
                                   RGBCAPTURE_FLAG_CURRENT|RGBCAPTURE_FLAG_REAL_WORLD);
    if (RGBERROR_NO_ERROR != error)
	{
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				 TRACE("RGBCaptureSetParameters() 设置采集卡参数，没有找到硬件设备\n");//没有找到硬件设备
				 break;
			}

			case RGBERROR_INVALID_POINTER:
			{  
				TRACE("RGBCaptureSetParameters() 设置采集卡参数，没有找到参数指针\n");//没有找到参数指针
				break;
			}

			case RGBERROR_INVALID_SIZE:
			{
				TRACE("RGBCaptureSetParameters() 设置采集卡参数，参数指针大小错误\n");//参数指针大小错误
				break;
			}

			case RGBERROR_INVALID_FLAGS:
			{
				TRACE("RGBCaptureSetParameters() 设置采集卡参数，不支持的标志位操作\n");//不支持的标志位操作
				break;
			}

			case RGBERROR_INVALID_FORMAT:
			{
				TRACE("RGBCaptureSetParameters() 设置采集卡参数，不支持的输入信号格式\n");//不支持的输入信号格式
				break;
			}

			case RGBERROR_INVALID_BLACKLEVEL:
			{
				TRACE("RGBCaptureSetParameters() 设置采集卡参数，不支持的黑色度\n");//不支持的黑色度
				break;
			}

			case RGBERROR_INVALID_PIXEL_FORMAT:
			{   
				TRACE("RGBCaptureSetParameters() 设置采集卡参数，不支持的像素模式\n");//不支持的像素模式
				break;
			}

			case RGBERROR_INVALID_HWND:
			{
				TRACE("RGBCaptureSetParameters() 设置采集卡参数，不是当前窗口句柄\n");//不是当前窗口句柄
				break;
			}

			case RGBERROR_INVALID_SYNCEDGE:
			{   
				TRACE("RGBCaptureSetParameters() 设置采集卡参数，不支持的同步沿\n");//不支持的同步沿
				break;
			}

			case RGBERROR_HSCALED_OUT_OF_RANGE:
			{  
				TRACE("RGBCaptureSetParameters() 设置采集卡参数，不支持的缩放比例\n");//不支持的缩放比例
				break;
			}

			default:
			{
				break;
			}
		}
		return FALSE;
	}

    //////////////////////////////////////////////////////////////////////////
   /*开始采集事件*/
	error = RGBCaptureEnable(*phRGBCapture, TRUE);
	if (RGBERROR_NO_ERROR != error)     //打开设备失败，返回结果如下：
	{	
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureEnable() 启动采集卡，打开队列中没有找到该硬件设备句柄\n");//打开队列中没有找到该硬件设备句柄
				break;
			}

			case RGBERROR_CAPTURE_OUTSTANDING:
			{
				TRACE("RGBCaptureEnable() 启动采集卡，硬件设备没有打开\n");//硬件设备没有打开
				break;
			}

			case RGBERROR_THREAD_FAILURE:
			{
				TRACE("RGBCaptureEnable() 启动采集卡，启动线程失败\n");//启动线程失败
				break;
			}

			default:
			{
				break;
			}
		}
		return FALSE;
	}
    ////////////////////////////////////////////////////////////////////////
	/*设置窗口参数*/
// 	{
// 	    RECT rect;
// 	    GetWindowRect(&rect);
// 		AdjustWindowRect(&rect, GetWindowLong(m_hWnd, GWL_STYLE), NULL);
//         ::SetWindowPos( m_hWnd, NULL,0, 0,
//                         rect.right - rect.left, 
// 						rect.bottom - rect.top,
//                         SWP_NOMOVE|SWP_NOZORDER);
// 	}
 	return TRUE;
}

/*****************************************************************************/
/*
*设置采集参数
*/
BOOL CRGBSAMPDlg::SetCapture(HWND hWnd
							 , RGBCAPTUREPARMS  *pCaptureParms
							 , HRGBCAPTURE      *phRGBCapture
							 , RGBCAPTUREBUFFER *pCaptureBuffer
							 , unsigned long    *pBufferSize
							 , RGBBITMAP       *pRGBBitmap)
{
	RGBERROR error;
	memset(pCaptureParms, 0, sizeof (*pCaptureParms));

	//////////////////////////////////////////////////////////////////////////
    /*检测当前输入的VGA视频信号分辨率*/
	error = RGBCaptureDetectVideoMode(*phRGBCapture, 0, 0, &pCaptureParms->VideoTimings, FALSE);
	if (RGBERROR_NO_ERROR != error)
	{
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureDetectVideoMode(), 检测当前输入的VGA视频信号分辨率，检测失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
				break;
			}

			case RGBERROR_INVALID_POINTER:
			{
				TRACE("RGBCaptureDetectVideoMode(), 检测当前输入的VGA视频信号分辨率，没有找到参数指针\n");//没有找到参数指针
				break;
			}

			default:
			{
				break;
			}
		}
		return FALSE;
	}
	
	//////////////////////////////////////////////////////////////////////////
	/*设置窗口标题*/
	char szWndTitle[120];
	wsprintf(szWndTitle, "VGA采集卡： ");
    ::SetWindowText(::GetParent(this->m_hWnd), szWndTitle);

	//////////////////////////////////////////////////////////////////////////
    /* 设置位图参数为输出采集数据做准备 */

    pRGBBitmap->bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
    pRGBBitmap->bmiHeader.biPlanes = 1;
    pRGBBitmap->bmiHeader.biCompression = BI_BITFIELDS;
    pRGBBitmap->bmiHeader.biSizeImage = 0;
    pRGBBitmap->bmiHeader.biXPelsPerMeter = 3000;
    pRGBBitmap->bmiHeader.biYPelsPerMeter = 3000;
    pRGBBitmap->bmiHeader.biClrUsed = 0;
    pRGBBitmap->bmiHeader.biClrImportant = 0;
    pRGBBitmap->bmiHeader.biWidth = pCaptureParms->VideoTimings.HorAddrTime;
    pRGBBitmap->bmiHeader.biHeight = - (int)pCaptureParms->VideoTimings.VerAddrTime;
	
// 	if (DD_OK !=ddrawRGB.DDRelease())
// 	{
// 		TRACE("CRGBSAMPDlg::SetCapture DDRelease???!!! \n");
// 		//return FALSE;
// 	}
// 	if (DD_OK !=ddrawRGB.DDInit(hWnd,pCaptureParms->VideoTimings.HorAddrTime,pCaptureParms->VideoTimings.VerAddrTime))
// 	{
// 		TRACE("CRGBSAMPDlg::SetCapture DDInit????????!!! \n");
// 		//return FALSE;
// 	}
	
	pCaptureBuffer[0].bufferflags = FALSE;
	pCaptureBuffer[1].bufferflags = FALSE;
	pABuffer = &pCaptureBuffer[0];
	pPBuffer = &pCaptureBuffer[1];
	//////////////////////////////////////////////////////////////////////////
	/*此处检测缓存是否就绪，成功则继续采集事件*/
	error = RGBCaptureFrameBufferReady (*phRGBCapture,
                                       pCaptureBuffer[0].Index,
                                       *pBufferSize);
	if (error != RGBERROR_NO_ERROR)
	{
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureFrameBufferReady(),  检测当前VGA采集卡缓存是否就绪，检测失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
				break;
			}

			case RGBERROR_INVALID_INDEX:
			{
				TRACE("RGBCaptureFrameBufferReady(),  检测当前VGA采集卡缓存是否就绪，没有找到设备索引\n");//没有找到参数指针
				break;
			}

			case RGBERROR_BUFFER_TOO_SMALL:
			{
				TRACE("RGBCaptureFrameBufferReady(),  检测当前VGA采集卡缓存是否就绪，缓存太小\n");//没有找到参数指针
				break;
			}

			default:
			{
				break;
			}
		}

		char text[80];
		wsprintf(text, "RGBCaptureFrameBufferReady returned: %s", RGBErrorToString(error));
		::MessageBox(NULL, text, NULL, MB_OK | MB_ICONERROR);

		RGBCaptureClose(*phRGBCapture);
		*phRGBCapture = 0;

		if (!FreeBuffers(*phRGBCapture, hWnd, &pCaptureBuffer[0]))
		{
			TRACE("FreeBuffers(),释放缓存失败！");
			return FALSE;
		}

		return FALSE;
	}
	pCaptureBuffer[0].bufferflags = TRUE;
	return TRUE;
}

/*****************************************************************************/ 
/*
*检测当前像素模式
*/
unsigned short CRGBSAMPDlg::DetectPixelFormat( HWND hWnd, DWORD *pColourMask )
/*
 * Summary: Detects the pixel format of the graphics card frame buffer.
 *
 * Purpose: 
 *          pColourMask points to an array of three DWORDs. Each DWORD is
 *             loaded with a bit mask representing the bits for red, green
 *             and blue respectively.
 *
 * Return:  One of the following values:
 *             RGBPIXELFORMAT_555,
 *             RGBPIXELFORMAT_565,
 *             RGBPIXELFORMAT_888,
 *             RGBPIXELFORMAT_UNKNOWN.
 */
{
    HDC            hDC, hDCCompatible;
    HBITMAP        hBitmap, hBitmapOld;
    DWORD          dwPixel, dwMask;
    int            nBytes, end, red, i, green, blue;
    unsigned short format;

    hDC = ::GetDC(hWnd);
    hDCCompatible = CreateCompatibleDC(hDC);

    hBitmap = CreateCompatibleBitmap(hDC, 1, 1);
    hBitmapOld = (HBITMAP)SelectObject(hDCCompatible, hBitmap);

    SetPixel(hDCCompatible, 0, 0, RGB (255, 0, 0));
    nBytes = GetBitmapBits(hBitmap, sizeof (dwPixel), &dwPixel);
    end = (nBytes << 3) - 1;
    red = CountBits(dwPixel, 0, end);

    dwMask = 0;
    for (i = 0; i < nBytes; i++)
	{
		dwMask |= (0xff << ( i << 3 ));
	}
    pColourMask[0] = dwPixel & dwMask;

    SetPixel(hDCCompatible, 0, 0, RGB (0, 255, 0));
    GetBitmapBits(hBitmap, sizeof(dwPixel ), &dwPixel);
    green = CountBits(dwPixel, 0, end);
    pColourMask[1] = dwPixel & dwMask;

    SetPixel(hDCCompatible, 0, 0, RGB (0, 0, 255));
    GetBitmapBits(hBitmap, sizeof (dwPixel), &dwPixel);
    blue = CountBits(dwPixel, 0, end);
    pColourMask[2] = dwPixel & dwMask;

    SelectObject(hDCCompatible, hBitmapOld);
    DeleteDC(hDCCompatible);
    DeleteObject(hBitmap);

    ::ReleaseDC(hWnd, hDC);

    if ((green == 5) && (red == 5) && (blue == 5))
	{
        format = RGBPIXELFORMAT_555;
	}
    else if ((green == 6) && (red == 5) && (blue == 5))
	{
        format = RGBPIXELFORMAT_565;
	}
    else if ((green == 8) && (red == 8) && (blue == 8))
	{
        format = RGBPIXELFORMAT_888;
	}
    else
	{
        format = RGBPIXELFORMAT_UNKNOWN;
	}
	
// 	char temp[128];
// 	sprintf(temp,"Format:%d,%u,%d",format,*pColourMask,*pColourMask);
// 
// 	MessageBox(temp,"Info",MB_ICONQUESTION|MB_OK);

    return format;
}

/*****************************************************************************/ 
/*
 *数出像素位数
 */
int CRGBSAMPDlg::CountBits(unsigned long  ulValue, int start, int end)
{
    int count = 0, i;
    unsigned long ulMask;

    for (i = start, ulMask = 1 << start; i <= end; i++, ulMask <<= 1)
	{
		if (ulMask & ulValue)
		{
			count++;
		}
	}
	return count;
}

/*****************************************************************************/
/*
 *按照指定大小分配缓存
 */
BOOL CRGBSAMPDlg::AllocateBuffers(HRGBCAPTURE hRGBCapture, 
                                      RGBCAPTUREBUFFER *pCaptureBuffer, 
									  HWND hWnd, 
									  UINT BufferSize)
{
	//////////////////////////////////////////////////////////////////////////
   /*分配缓存*/
   pCaptureBuffer->LpVoidBuffer = GlobalAlloc(GMEM_FIXED, BufferSize);
   if (pCaptureBuffer->LpVoidBuffer == NULL)
   {
	   return FALSE;
   }
	TRACE("%d",GlobalSize(pCaptureBuffer->LpVoidBuffer));
   //////////////////////////////////////////////////////////////////////////
   /*注册当前的VGA采集事件*/
   RGBERROR error;
   error = RGBCaptureUseMemoryBuffer(hRGBCapture, pCaptureBuffer->LpVoidBuffer, BufferSize, &pCaptureBuffer->Index);
   if (RGBERROR_NO_ERROR != error)
   {
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  注册当前的VGA采集事件，注册失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
				break;
			}

			case RGBERROR_INVALID_POINTER:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  注册当前的VGA采集事件，没有找到指向当前缓存存储区的指针\n");//没有找到指向当前缓存存储区的指针
				break;
			}

			case RGBERROR_INVALID_INDEX:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  注册当前的VGA采集事件，没有找到对应当前缓存存储区的索引\n");//没有找到对应当前缓存存储区的索引
				break;
			}

			default:
			{
				break; 
			}
		}
		return FALSE;
   }
   if ((bUsingAccBuffer1 == FALSE) && (hRGBCapture == hRGBCapture1))
   {
	  bUsingAccBuffer1 = true;  //修改标志缓存使用的参数为TRUE
   }

   return TRUE;
}

/*****************************************************************************/
/*
*窗口尺寸控制
*/
// void CRGBSAMPDlg::OnSize(UINT nType, int cx, int cy) 
// {
// 	CDialog::OnSize(nType, cx, cy);
// 		// TODO: Add your message handler code here	
// // 	if (NULL != m_static1.m_hWnd /*|| NULL != m_static2.m_hWnd || NULL != m_static3.m_hWnd || NULL != m_static4.m_hWnd*/)
// // 	{
// // 		RECT wdrect;
// // 		this->GetClientRect(&wdrect);
// // 		m_static1.MoveWindow(&wdrect,TRUE);    
// // 	m_static1.GetWindowRect(&wdrect);
// // 	}
// }

// void CRGBSAMPDlg::OnClose() 
// {
// 	// TODO: Add your message handler code here and/or call default
// 	if ( MessageBox("确定要退出系统吗 ?\t","系统提示",MB_ICONQUESTION | MB_OKCANCEL ) != IDOK )
// 	{
// 		return ;
// 	}
// 	CDialog::OnClose();
// }

void CRGBSAMPDlg::OnDestroy() 
{
	PostQuitMessage(1);

	CDialog::OnDestroy();
	
	// TODO: Add your message handler code here
	if ((NULL != m_hPThreadEvents[0])&&(NULL != m_hPThreadEvents[1]))
	{
		SetEvent(m_hPThreadEvents[1]);
	}
	if (NULL != m_hCloseEvent1)
	{
		WaitForSingleObject(m_hCloseEvent1,INFINITE);
		ResetEvent(m_hCloseEvent1);
		CloseHandle(m_hCloseEvent1);
		m_hCloseEvent1 = NULL;
		CloseHandle(m_hPaintThread);
		m_hPaintThread = NULL;
	}

	if (hRGBCapture1)
	{
		PauseRunSignal1 = FALSE;
//		this->RedrawWindow();
		stopVGA(m_hWnd, &hRGBCapture1, pCaptureBuffer1);	
	}
	ddrawRGB.DDRelease();
	delete pData;
//	DeleteObject(m_Brush);
}

/******************************************************************************/ 
/*
*释放缓存区
*/
BOOL CRGBSAMPDlg::FreeBuffers(HRGBCAPTURE hRGBCapture, HWND hWnd, RGBCAPTUREBUFFER *pCaptureBuffer)
{
	RGBERROR error;
	HDC hDC = ::GetDC(hWnd);

	if (bUsingAccBuffer1 && (hRGBCapture == hRGBCapture1))
   {
	  bUsingAccBuffer1 = false;  //修改标志缓存是否使用的参数为false
   }


   ::ReleaseDC(hWnd, hDC);
    
    if (pCaptureBuffer[0].LpVoidBuffer)
	{
		//////////////////////////////////////////////////////////////////////////
	    /*删除当前的VGA采集缓存区在使用队列中的索引*/
		 error = RGBCaptureReleaseBuffer(hRGBCapture, pCaptureBuffer[0].Index);
		 if (RGBERROR_NO_ERROR != error)
		 {
			 switch(error)
			{
				case RGBERROR_INVALID_HRGBCAPTURE:
				{
					TRACE("RGBCaptureReleaseBuffer(),   删除当前的VGA采集缓存区在使用队列中的索引，注册失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
					break;
				}

				case RGBERROR_INVALID_POINTER:
				{
					TRACE("RGBCaptureReleaseBuffer(),    删除当前的VGA采集缓存区在使用队列中的索引，没有找到指向当前缓存存储区的指针\n");//没有找到指向当前缓存存储区的指针
					break;
				}

				default:
				{
					break;
				}
			}
			return FALSE;	
		 }
		//////////////////////////////////////////////////////////////////////////
	    /*释放缓存区*/
		 GlobalFree(pCaptureBuffer[0].LpVoidBuffer); 
		 pCaptureBuffer[0].LpVoidBuffer = NULL;
	}
	if (pCaptureBuffer[1].LpVoidBuffer)
	{
		//////////////////////////////////////////////////////////////////////////
	    /*删除当前的VGA采集缓存区在使用队列中的索引*/
		 error = RGBCaptureReleaseBuffer(hRGBCapture, pCaptureBuffer[1].Index);
		 if (RGBERROR_NO_ERROR != error)
		 {
			 switch(error)
			{
				case RGBERROR_INVALID_HRGBCAPTURE:
				{
					TRACE("RGBCaptureReleaseBuffer(),   删除当前的VGA采集缓存区在使用队列中的索引，注册失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
					break;
				}

				case RGBERROR_INVALID_POINTER:
				{
					TRACE("RGBCaptureReleaseBuffer(),    删除当前的VGA采集缓存区在使用队列中的索引，没有找到指向当前缓存存储区的指针\n");//没有找到指向当前缓存存储区的指针
					break;
				}

				default:
				{
					break;
				}
			}
			return FALSE;	
		 }
		//////////////////////////////////////////////////////////////////////////
	    /*释放缓存区*/
		 GlobalFree(pCaptureBuffer[1].LpVoidBuffer); 
		 pCaptureBuffer[1].LpVoidBuffer = NULL;
	}
	delete (pCaptureBuffer);
	return TRUE;
}

/******************************************************************************/
/*
*处理SDK发出的消息RGBWM_FRAMECAPTURED
*消息RGBWM_FRAMECAPTURED:表明当前采集卡缓存准备就绪，其中有一帧图象数据等待被处理
*/
void CRGBSAMPDlg::OnMyMessage_Sta1(WPARAM wParam, LPARAM lParam)
{	

	if ((NULL == hRGBCapture1) || (0 == hRGBCapture1))
	{
		return;
	}
	if (NULL == m_hCapEvent)
	{
		return;
	}
	WaitForSingleObject(m_hCapEvent, INFINITE);
	ResetEvent(m_hCapEvent);

	RGBERROR error;
	if (!signalflag)
	{
		signalflag = TRUE;
//		SetWindowText("");
	}
	pABuffer->bufferflags = TRUE;
	//////////////////////////////////////////////////////////////////////////
	if (FALSE == pCaptureBuffer1[0].bufferflags)
	{
		pABuffer = &pCaptureBuffer1[0];
	}
	else
	{
		pPBuffer = &pCaptureBuffer1[0];
	}

	if (FALSE == pCaptureBuffer1[1].bufferflags)
	{
		pABuffer = &pCaptureBuffer1[1];
	}
	else
	{
		pPBuffer = &pCaptureBuffer1[1];
	}
	error = RGBCaptureFrameBufferReady (hRGBCapture1,
                                       pABuffer->Index,
                                       bufferSize1);
    if (error != RGBERROR_NO_ERROR)
	{
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureFrameBufferReady(),  检测当前VGA采集卡缓存是否就绪，检测失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
				break;
			}

			case RGBERROR_INVALID_INDEX:
			{
				TRACE("RGBCaptureFrameBufferReady(),  检测当前VGA采集卡缓存是否就绪，没有找到设备索引\n");//没有找到参数指针
				break;
			}

			case RGBERROR_BUFFER_TOO_SMALL:
			{
				TRACE("RGBCaptureFrameBufferReady(),  检测当前VGA采集卡缓存是否就绪，缓存太小\n");//没有找到参数指针
				break;
			}

			default:
			{
				break;
			}
		}
    //    char text[80];
    //    wsprintf(text, "RGBCaptureFrameBufferReady returned: %s", RGBErrorToString(error));
    //    ::MessageBox(m_hWnd, text, NULL, MB_OK | MB_ICONERROR);
	}   
	SetEvent(m_hPThreadEvents[0]);

}

namespace
{
	int RGB24WriteBmp(int width, int height, unsigned char *R,unsigned char *G,unsigned char *B, int datasize,char *BmpFileName) 
	{ 
		//	fprintf(stdout, "[%.3f]:Begin BMP To:%s\n",_now,BmpFileName);
		
		int x=0; 
		int y=0; 
		int i=0; 
		int j=0; 
		FILE *fp; 
		unsigned char *WRGB; 
		unsigned char *WRGB_Start; 
		int yu = width*3%4; 
		int BytePerLine = 0; 
		
		yu = yu!=0 ? 4-yu : yu; 
		BytePerLine = width*3+yu; 
		
		if((fp = fopen(BmpFileName, "wb")) == NULL) 
			return 0; 
		WRGB = (unsigned char*)malloc(BytePerLine*height+54); 
		memset(WRGB, 0, BytePerLine*height+54); 
		
		//BMP头 
		WRGB[0] = 'B'; 
		WRGB[1] = 'M'; 
		*((unsigned int*)(WRGB+2)) = BytePerLine*height+54; 
		*((unsigned int*)(WRGB+10)) = 54; 
		*((unsigned int*)(WRGB+14)) = 40; 
		*((unsigned int*)(WRGB+18)) = width; 
		*((unsigned int*)(WRGB+22)) = height; 
		*((unsigned short*)(WRGB+26)) = 1; 
		*((unsigned short*)(WRGB+28)) = 24; 
		*((unsigned short*)(WRGB+34)) = BytePerLine*height; 
		
		WRGB_Start = WRGB + 54; 
		
		for(y=height-1,j=0; y >= 0; y--,j++) 
		{ 
			for(x=0,i=0; x <width; x++) 
			{ 
				if((j*width+x)*3<datasize)
				{
					WRGB_Start[y*BytePerLine+i++] = B[(j*width+x)*3]; 
					WRGB_Start[y*BytePerLine+i++] = G[(j*width+x)*3]; 
					WRGB_Start[y*BytePerLine+i++] = R[(j*width+x)*3]; 
				}
			} 
		} 
		
		fwrite(WRGB, 1, BytePerLine*height+54, fp); 
		free(WRGB); 
		fclose(fp);
		
		//		fprintf(stdout, "[%.3f]:End Save BMP To:%s\n",_now,BmpFileName);
		
		return 1; 
	}


	int RGB24WriteBmp_EX(int width, int height, unsigned char *R,unsigned char *G,unsigned char *B, int datasize,char *BmpFileName) 
	{ 
		//	fprintf(stdout, "[%.3f]:Begin BMP To:%s\n",_now,BmpFileName);
		
		int x=0; 
		int y=0; 
		int i=0; 
		int j=0; 
		FILE *fp; 
		unsigned char *WRGB; 
		unsigned char *WRGB_Start; 
		int yu = width*3%4; 
		int BytePerLine = 0; 
		
		yu = yu!=0 ? 4-yu : yu; 
		BytePerLine = width*3+yu; 
		
		if((fp = fopen(BmpFileName, "wb")) == NULL) 
			return 0; 
		WRGB = (unsigned char*)malloc(BytePerLine*height+54); 
		memset(WRGB, 0, BytePerLine*height+54); 
		
		//BMP头 
		WRGB[0] = 'B'; 
		WRGB[1] = 'M'; 
		*((unsigned int*)(WRGB+2)) = BytePerLine*height+54; 
		*((unsigned int*)(WRGB+10)) = 54; 
		*((unsigned int*)(WRGB+14)) = 40; 
		*((unsigned int*)(WRGB+18)) = width; 
		*((unsigned int*)(WRGB+22)) = height; 
		*((unsigned short*)(WRGB+26)) = 1; 
		*((unsigned short*)(WRGB+28)) = 24; 
		*((unsigned short*)(WRGB+34)) = BytePerLine*height; 
		
		WRGB_Start = WRGB + 54; 
		
		for(y=height-1,j=0; y >= 0; y--,j++) 
		{ 
			for(x=0,i=0; x <width; x++) 
			{ 
				if((j*width+x)*4<datasize)
				{
					WRGB_Start[y*BytePerLine+i++] = B[(j*width+x)*4]; 
					WRGB_Start[y*BytePerLine+i++] = G[(j*width+x)*4]; 
					WRGB_Start[y*BytePerLine+i++] = R[(j*width+x)*4]; 
				}
			} 
		} 
		
		fwrite(WRGB, 1, BytePerLine*height+54, fp); 
		free(WRGB); 
		fclose(fp);
		
		//		fprintf(stdout, "[%.3f]:End Save BMP To:%s\n",_now,BmpFileName);
		
		return 1; 
	}
}


/******************************************************************************/
/*
 *画出图象
 */
void CRGBSAMPDlg::PaintFrame(HWND hWnd, HDC hDC, RGBCAPTUREBUFFER captureBuffer,RGBBITMAP *pRGBBitmap)
/*
* This function is only called in response to a WM_PAINT message, and is,
* essentially, outside of the state  machine.  The receipt of a notification
* that a frame has been captured will invalidate the client rect, causing a
* paint to happen.
*/
{ 
	DWORD height = -pRGBBitmap->bmiHeader.biHeight;
	DWORD width = pRGBBitmap->bmiHeader.biWidth;

	memset(pData,0,1600*1200*4);
	TPicRegion pDest;
	pDest.pdata = (TARGB32*)pData;
	pDest.height =height;
	pDest.width = width;
	pDest.byte_width = width << 2;
	if (1 == datamode)
	{
		DECODE_RGB_TO_BGRA((const TUInt8 *)captureBuffer.LpVoidBuffer, pDest);
	}
	else if (0 == datamode)
	{
		DECODE_UYVY_TO_RGB32((const TUInt8 *)captureBuffer.LpVoidBuffer, pDest);
	}

//	char temp2[128];
//	sprintf(temp2,"height:%d,width=%d,bwidth=%d,datamode=%d",height,width,width<<2,datamode);
//	MessageBox(temp2,"Info",MB_ICONINFORMATION|MB_OK);

	BYTE * pbuffer = (BYTE *)pData;

	static int intCount=0;	
	char bmpName[128];
	sprintf(bmpName,"%s\\%d_%d_%3d.bmp",m_WorkFolder.c_str(),width,height,intCount++ % m_maxCachePicNumber);
	RGB24WriteBmp_EX(width,height,pbuffer+2,pbuffer+1,pbuffer+0,1600*1200*4,bmpName);

	//////////////////////////////////////////////////////////////////////////
	/*directdraw*/

// 	RECT derect;
// 	::GetWindowRect(hWnd,&derect);
// 	if (DD_OK !=ddrawRGB.DDDraw(hWnd,pbuffer,width*height*4,derect,width,height))
// 	{
// 		TRACE("CRGBSAMPDlg::PaintFrame DDRAW?????!!! %d %d %d %d\n",derect.top, derect.bottom, derect.left, derect.right);
// 		//////////////////////////////////////////////////////////////////////////
// 		CRect recttmp;	//??????
// 		::GetClientRect(hWnd, &recttmp);  //?????????
//    		//int height = -pRGBBitmap->bmiHeader.biHeight;
// 		int   i   =   SetStretchBltMode(hDC,COLORONCOLOR);
// 		if ( GDI_ERROR == StretchDIBits(hDC,
// 					   recttmp.left, recttmp.top, recttmp.Width(), recttmp.Height(),
// 					   0, 0, pRGBBitmap->bmiHeader.biWidth, height,
// 					   pData,
// 					   (BITMAPINFO *)pRGBBitmap,
// 					   DIB_RGB_COLORS, SRCCOPY))
// 		{
// 			DWORD dwErrorCode = GetLastError();
// 			TRACE("CRGBSAMPDlg::PaintFrame ?????!!! %d\n", dwErrorCode);
// 			SetStretchBltMode(hDC,i);
// 			return;
// 		}
// 		SetStretchBltMode(hDC,i);
// 		//////////////////////////////////////////////////////////////////////////
// 
// 	}	
	//////////////////////////////////////////////////////////////////////////

// 	//////////////////////////////////////////////////////////////////////////
// 	CRect recttmp;	//存放窗口尺寸
// 	::GetClientRect(hWnd, &recttmp);  //获取当前窗口的尺寸
//    	//int height = -pRGBBitmap->bmiHeader.biHeight;
// 	int   i   =   SetStretchBltMode(hDC,COLORONCOLOR);
// 	if ( GDI_ERROR == StretchDIBits(hDC,
// 				   recttmp.left, recttmp.top, recttmp.Width(), recttmp.Height(),
// 				   0, 0, pRGBBitmap->bmiHeader.biWidth, height,
// 				   pData,
// 				   (BITMAPINFO *)pRGBBitmap,
// 				   DIB_RGB_COLORS, SRCCOPY))
// 	{
// 		DWORD dwErrorCode = GetLastError();
// 		TRACE("CRGBSAMPDlg::PaintFrame 画图不成功!!! %d\n", dwErrorCode);
// 		SetStretchBltMode(hDC,i);
// 		return;
// 	}
// 	SetStretchBltMode(hDC,i);
// 	//////////////////////////////////////////////////////////////////////////

// 	//////////////////////////////////////////////////////////////////////////
// 	SetDIBitsToDevice(hDC,
//                            0, 0, pRGBBitmap->bmiHeader.biWidth,
//                            height, 0, 0, 0, height,
//                            pData,
//                            (BITMAPINFO *)pRGBBitmap,
//                            DIB_RGB_COLORS);
// 	//////////////////////////////////////////////////////////////////////////

	/*标志如果为TRUE则记录画出帧数*/
	if (hWnd == m_hWnd && startcount1)
	{
		g_framecount1++;      //画图总数量记录
	}
}

/*****************************************************************************/
/*
*处理像素发生改变时SDK发出的消息RGBWM_MODECHANGED
*重新设置采集参数
*/
void CRGBSAMPDlg::OnMyMessage_modechange(WPARAM wParam, LPARAM lParam)
{
	unsigned long *pbufferSize = 0L;	
	RGBCAPTUREPARMS CaptureParms;
	if (!SetCapture(m_hWnd, &CaptureParms, &hRGBCapture1, pCaptureBuffer1, &bufferSize1, &RGBBitmap1))  //设置采集参数
	{
		TRACE("SetCapture(),设置采集参数失败");
		return;
	}
}

/*****************************************************************************/
/*
*处理当SDK采集不到正确的数据时发出的消息RGBWM_NOSIGNAL
*/
void CRGBSAMPDlg::OnMyMessage_nosignal(WPARAM wParam, LPARAM lParam)
{
	CString strInfo;
	switch(wParam)
	{
		case NOSIGNAL_NO_RESOLUTION:
		{
			strInfo = _T("无 VGA 信 号(错误的分辨率 !!!)");
			break;
		}
		case NOSIGNAL_DDRCHECK_ERROR:
		{
			strInfo = _T("无 VGA 信 号(DDR 自检出错 !!!)");
			break;
		}
		case NOSIGNAL_H_OVERFLOW:
		{
			strInfo = _T("无 VGA 信 号(水平偏移量溢出 !!!)");
			break;
		}
		case NOSIGNAL_V_OVERFLOW:
		{
			strInfo = _T("无 VGA 信 号(垂直偏移量溢出 !!!)");
			break;
		}
		default:
		{
			break;
		}
	}
	signalflag = FALSE;
		
// 	CRect rc;
// 	CDC	*pDC;
// 	int x;
// 	int y;
// 	//////////////////////////////////////////////////////////////////////////
// 	/*创建字体*/
// 	LOGFONT ft;	
// 	
// 	ft.lfHeight			= 32;
// 	ft.lfWidth			= 16;
// 	ft.lfEscapement		= 0;
// 	ft.lfOrientation	= 0;
// 	ft.lfWeight			= FW_HEAVY;
// 	ft.lfItalic			= FALSE;
// 	ft.lfUnderline		= FALSE;
// 	ft.lfStrikeOut		= FALSE;
// 	ft.lfCharSet		= ANSI_CHARSET;
// 	ft.lfOutPrecision	= OUT_DEFAULT_PRECIS;
// 	ft.lfClipPrecision	= CLIP_DEFAULT_PRECIS;
// 	ft.lfQuality		= DEFAULT_QUALITY;
// 	ft.lfPitchAndFamily = VARIABLE_PITCH;	
// 	strcpy(ft.lfFaceName,"宋体");
// 	
// 	HFONT	hFont = ::CreateFontIndirect(&ft);
// 	pDC = GetDC();
// 	GetClientRect(rc);
// 	this->RedrawWindow();
// 	x = ( rc.Width() - strInfo.GetLength() * ft.lfWidth ) / 2;
// 	y = ( rc.Height() - ft.lfHeight ) / 2;
// 	pDC->SelectObject(hFont);
// 	//////////////////////////////////////////////////////////////////////////
// 	/*创建画刷*/
// 	CBrush brush;
// 	brush.CreateSolidBrush(RGB(0,0,0));
// 	/*画图*/
// 	pDC->SelectObject(&brush);
// 	pDC->SetBkColor(RGB(0,0,0));
// 	pDC->SetTextColor(RGB(255,255,255));
// 	pDC->TextOut(x,y,strInfo);
// 
// 	ReleaseDC(pDC);
//	this->RedrawWindow();
//	this->m_static.ShowWindow(SW_SHOW);
//	this->m_static.SetWindowText(strInfo);
//	this->SetWindowText(strInfo);
}


/*****************************************************************************/
/*
*停止采集图象
*/
void CRGBSAMPDlg::OnStopVGA1() 
{
	// TODO: Add your command handler code here
	this->m_static.ShowWindow(SW_HIDE);
	if ((NULL != m_hPThreadEvents[0])&&(NULL != m_hPThreadEvents[1]))
	{
		SetEvent(m_hPThreadEvents[1]);
	}
	if (NULL != m_hCloseEvent1)
	{
		WaitForSingleObject(m_hCloseEvent1,INFINITE);
		ResetEvent(m_hCloseEvent1);
		CloseHandle(m_hCloseEvent1);
		m_hCloseEvent1 = NULL;
		CloseHandle(m_hPaintThread);
		m_hPaintThread = NULL;
		CloseHandle(m_hCapEvent);
		m_hCapEvent = NULL;
	}

	//////////////////////////////////////////////////////////////////////////
	/*设置窗口标题*/
	char szWndTitle[120];
	wsprintf(szWndTitle, "Stop");
    //::SetWindowText(::GetParent(this->m_hWnd), szWndTitle);

	SetWindowText("");
	//////////////////////////////////////////////////////////////////////////
	if (!signalflag)
	{
		signalflag = TRUE;
		SetWindowText("");
	}

	if (hRGBCapture1)
	{
		PauseRunSignal1 = FALSE;
//		this->RedrawWindow();
		stopVGA(m_hWnd, &hRGBCapture1, pCaptureBuffer1);
	}
	else
	{
		::MessageBox(GetTopWindow()->m_hWnd, "未运行VGA1采集卡 !","系统提示",MB_ICONWARNING | MB_OK);
	}
}

void CRGBSAMPDlg::stopVGA(HWND hWnd, HRGBCAPTURE *phRGBCapture, RGBCAPTUREBUFFER *pCaptureBuffer)
{
	// TODO: Add your command handler code here

	//////////////////////////////////////////////////////////////////////////
	/*释放缓存*/
	if (!FreeBuffers(*phRGBCapture, hWnd, pCaptureBuffer))
	{
		TRACE("FreeBuffers(),释放缓存失败！");
		return;
	}
	//////////////////////////////////////////////////////////////////////////

	//////////////////////////////////////////////////////////////////////////
	/*停止采集事件*/
	RGBERROR error;	
	error = RGBCaptureEnable(*phRGBCapture, FALSE);
	if (RGBERROR_NO_ERROR != error)     //打开设备失败，返回结果如下：
	{	
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureEnable() 启动采集卡，打开队列中没有找到该硬件设备句柄\n");//打开队列中没有找到该硬件设备句柄
				break;
			}

			case RGBERROR_CAPTURE_OUTSTANDING:
			{
				TRACE("RGBCaptureEnable() 启动采集卡，硬件设备没有打开\n");//硬件设备没有打开
				break;
			}

			case RGBERROR_THREAD_FAILURE:
			{
				TRACE("RGBCaptureEnable() 启动采集卡，启动线程失败\n");//启动线程失败
				break;
			}

			default:
			{
				break;
			}
		}
		return;
	}

	/*关闭采集卡*/
	RGBCaptureClose(*phRGBCapture);
	*phRGBCapture = 0;

}



/*****************************************************************************/
/*
*调整采集卡参数，弹出参数调整的窗口
*/
void CRGBSAMPDlg::OnAdjustVga1() 
{
	// TODO: Add your command handler code here
	
	if ( 0 == hRGBCapture1)
	{
		::MessageBox(m_hWnd, "未运行VGA采集卡 !","系统提示",MB_ICONWARNING | MB_OK);
	}
	else
	{
		SendMessageW(::GetParent(this->m_hWnd),RGBSAMPWM_QUALADJUST,0,(LPARAM)this);
	}
}


/*****************************************************************************/
/*
*打开质量管理窗口
*/
void CRGBSAMPDlg::OnQualpropVga1() 
{
	// TODO: Add your command handler code here
	
	if ( 0 == hRGBCapture1 )
	{
		::MessageBox(m_hWnd, "未运行VGA采集卡 !","系统提示",MB_ICONWARNING | MB_OK);
	}
	else
	{
		SendMessageW(::GetParent(this->m_hWnd),RGBSAMPWM_QUALPARA,0,(LPARAM)this);
	}
}

/*****************************************************************************/
/*
*暂停采集
*/
void CRGBSAMPDlg::OnPauseVga1() 
{
	// TODO: Add your command handler code here
	if (0 == hRGBCapture1)
	{
		::MessageBox(GetTopWindow()->m_hWnd, "未运行VGA1采集卡 !","系统提示",MB_ICONWARNING | MB_OK);
		return;
	}
	if (PauseRunSignal1)
	{
		::MessageBox(GetTopWindow()->m_hWnd, "已暂停VGA1采集卡 !","系统提示",MB_ICONWARNING | MB_OK);
		return;
	}
	PauseRunSignal1 = TRUE;
}

/*****************************************************************************/
/*
*继续采集
*/
void CRGBSAMPDlg::OnGoonVga1() 
{
	// TODO: Add your command handler code here
	HRGBCAPTURE hRGBCapture;
	hRGBCapture = hRGBCapture1;
	if (0 == hRGBCapture1)
	{
		::MessageBox(GetTopWindow()->m_hWnd, "未运行VGA1采集卡 !","系统提示",MB_ICONWARNING | MB_OK);
		return;
	}
		if (!PauseRunSignal1)
	{
		::MessageBox(GetTopWindow()->m_hWnd, "未暂停VGA1采集卡 !","系统提示",MB_ICONWARNING | MB_OK);
		return;
	}
	PauseRunSignal1 = FALSE;
	//////////////////////////////////////////////////////////////////////////
	/*此处检测缓存是否就绪，成功则继续采集事件*/
	RGBERROR error;
	error = RGBCaptureFrameBufferReady (hRGBCapture,
                                       pCaptureBuffer1->Index,
                                       bufferSize1);
   if (error != RGBERROR_NO_ERROR)
	{
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureFrameBufferReady(),  检测当前VGA采集卡缓存是否就绪，检测失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
				break;
			}

			case RGBERROR_INVALID_INDEX:
			{
				TRACE("RGBCaptureFrameBufferReady(),  检测当前VGA采集卡缓存是否就绪，没有找到设备索引\n");//没有找到参数指针
				break;
			}

			case RGBERROR_BUFFER_TOO_SMALL:
			{
				TRACE("RGBCaptureFrameBufferReady(),  检测当前VGA采集卡缓存是否就绪，缓存太小\n");//没有找到参数指针
				break;
			}

			default:
			{
				break;
			}
		}
        char text[80];
        wsprintf(text, "RGBCaptureFrameBufferReady returned: %s", RGBErrorToString(error));
        ::MessageBox(m_hWnd, text, NULL, MB_OK | MB_ICONERROR);
	    return;
	}
	
}

// *************************************************************************** 
// *双击，控制当前采集窗口放大/缩小
void CRGBSAMPDlg::OnLButtonDblClk(UINT nFlags, CPoint point) 
{
//	SendMessageW(::GetParent(this->m_hWnd),RGBSAMPWM_MAXSIZE,0, (LPARAM)this->m_capnum);
//	RedrawWindow();
// 	// TODO: Add your message handler code here and/or call default
// //	RECT rect; //存放主窗口的尺寸
// 	
// 	//this->RedrawWindow();
// 
// 	//////////////////////////////////////////////////////////////////////////
// 	根据当前活动窗口参数采取不同的措施//	this->GetClientRect(&rect);  //获取主窗口的尺寸  
// 	switch (ActiveStatic)
// 	{
// 		case 0://最大化窗口位置
// 		{
// 			ModifyStyle(WS_CAPTION,0,0);
// 			this->GetClientRect(&client);  //保存主窗口最大化之前的尺寸
// 			this->GetWindowRect(&screen);
// 			
// 			
// 			int   nFullWidth=GetSystemMetrics(SM_CXSCREEN)+8;   
// 		 	int   nFullHeight=GetSystemMetrics(SM_CYSCREEN)+8;
// 
// 			ClientToScreen(&point);
// 			if (PtInRect(&screen, point))
// 			{
// 				TRACE("OnRButtonUp(),鼠标在窗口中");	//鼠标在第状态控件中
// 
// 				改变窗口和显示控件的尺寸				this->MoveWindow(-4, -4, nFullWidth, nFullHeight,TRUE);
// 				m_static1.MoveWindow(-4, -4, nFullWidth, nFullHeight,TRUE);
// 				ActiveStatic = 1;	
// 			}
// 			break;
// 		}		
// 		case 1: //双击还原窗口
// 		{
// 			ModifyStyle(0,WS_CAPTION,0);
// 			this->MoveWindow(screen.left, screen.top, client.right+6, client.bottom+6, TRUE);
// 			this->GetClientRect(&client);
// 			m_static1.MoveWindow(0, 0, client.right, client.bottom, TRUE);
// 			ActiveStatic = 0;
// 			break;
// 		}
// 		default:
// 		{
// 			TRACE("【ERROR 双击事件未匹配 LINE:%d】\n", __LINE__);
// 			break;
// 		}
// 	}
// 	RedrawWindow();
	CDialog::OnLButtonDblClk(nFlags, point);
}

LRESULT CRGBSAMPDlg::OnMyMessage_datamode(WPARAM wParam, LPARAM lParam)
{
//	if (IDOK == ::MessageBox(m_hWnd, "请选择输入数据模式:\n  选择RGB模式请点击“确定”;\n  选择YUV模式请点击“取消”","系统提示",MB_ICONWARNING | MB_OKCANCEL))
	{
		datamode = 1;
		return(1);
	}
//	else
//	{
//		datamode = 0;
//		return(0);
//	}	
}

void CRGBSAMPDlg::OnMyMessage_ddrerror(WPARAM wParam, LPARAM lParam)
{	
// 	CRect rc;
// 	CDC	*pDC;
// 	int x;
// 	int y;
// 	CString strInfo = _T("DDR????!");
// 	//////////////////////////////////////////////////////////////////////////
// 	/*????*/
// 	LOGFONT ft;	
// 	
// 	ft.lfHeight			= 32;
// 	ft.lfWidth			= 16;
// 	ft.lfEscapement		= 0;
// 	ft.lfOrientation	= 0;
// 	ft.lfWeight			= FW_HEAVY;
// 	ft.lfItalic			= FALSE;
// 	ft.lfUnderline		= FALSE;
// 	ft.lfStrikeOut		= FALSE;
// 	ft.lfCharSet		= ANSI_CHARSET;
// 	ft.lfOutPrecision	= OUT_DEFAULT_PRECIS;
// 	ft.lfClipPrecision	= CLIP_DEFAULT_PRECIS;
// 	ft.lfQuality		= DEFAULT_QUALITY;
// 	ft.lfPitchAndFamily = VARIABLE_PITCH;	
// 	strcpy(ft.lfFaceName,"??");
// 	
// 	HFONT	hFont = ::CreateFontIndirect(&ft);
// 	pDC = GetDC();
// 	GetClientRect(rc);
// 	this->RedrawWindow();
// 	x = ( rc.Width() - strInfo.GetLength() * ft.lfWidth ) / 2;
// 	y = ( rc.Height() - ft.lfHeight ) / 2;
// 	pDC->SelectObject(hFont);
// 	//////////////////////////////////////////////////////////////////////////
// 	/*????*/
// 	CBrush brush;
// 	brush.CreateSolidBrush(RGB(0,0,0));
// 	/*??*/
// 	pDC->SelectObject(&brush);
// 	pDC->SetBkColor(RGB(0,0,0));
// 	pDC->SetTextColor(RGB(255,255,255));
// 	pDC->TextOut(x,y,strInfo);
// 
// 	ReleaseDC(pDC);
}
void CRGBSAMPDlg::OnCancel()
{

}

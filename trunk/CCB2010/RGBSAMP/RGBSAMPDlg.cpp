// RGBSAMPDlg.cpp : implementation file
// *****************************************************************************
//  RGBSAMPDlg   VERSION:  1.0   DATE: 2008-10-29
//  ----------------------------------------------------------------------------
//		FileName: 	RGBSAMPDlg.cpp
//		Created:	2008-10-29   16:57
//		Author:		�Ͼ�
//		Purpose:	�ɼ�ͼ����ʾͼ��ͨ���޸�ͼ�����������ʾͼ������
//		Version:	
//					��V1.0 2008-10-29 �Ͼ꡿
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
*������Ŵ��󱨸���Ϣ�ĺ���
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
// �����޸��˸������ֵ��ȫ����������������Ϊ�ڲ��ֻ��������У����ڴ�Խ��



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
		/*���ݵ�ǰ��Ϣ��lParam�������ж���Ϣ�����ĸ��ɼ����Ĳɼ��¼�*/
		hWnd = pRGBsamp->m_hWnd;  //�˴��̶��˵�1���ɼ���ֻ�ܶ�Ӧ��1�����ڿؼ�
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
*�������ڣ���ʼ���ɼ���
*/
int CRGBSAMPDlg::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
//	MessageBox("Create Dialog","Info",MB_ICONINFORMATION|MB_OK);
	
	if (CDialog::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	// TODO: Add your specialized creation code here
	if (!InitDevice())	//��ʼ���ɼ���
	{
		return -1;
	}
	
	return 0;
}

/*****************************************************************************/
/*
*��ʼ���豸
*/
BOOL CRGBSAMPDlg::InitDevice()
{
	unsigned long ulNumber = 0;  //��ż�⵽�Ĳɼ���������
	RGBERROR error;
	error = RGBCaptureInitialise(&ulNumber);

	if (RGBERROR_NO_ERROR != error)
	{
		//��ʼ��ʧ�ܣ�����ֵ��Ϊ��RGBERROR_HARDWARE_NOT_FOUND
		if (RGBERROR_HARDWARE_NOT_FOUND == error)
		{
			//û���ҵ�Ӳ���豸,��ӡ��־
			TRACE("RGBCaptureInitialise() ��ʼ����û���ҵ�Ӳ���豸\n");
			return(FALSE);
		}
	}
	if (0 == ulNumber)
	{
		MessageBox("û���ҵ�Ӳ���豸","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return(FALSE);
	}
	return(TRUE);
}

/*****************************************************************************/
/*
*�Ի����ʼ�����趨���ڴ�С������������ˢ
*/
BOOL CRGBSAMPDlg::OnInitDialog()
{
	CDialog::OnInitDialog();	
	// TODO: Add extra initialization here
//  	ModifyStyle(WS_CAPTION,0,0);   //�������ȥ����������ȥ���þ䡣
//     SendMessage(WM_SYSCOMMAND,SC_MAXIMIZE,0); 
 //	ShowWindow(SW_SHOWMAXIMIZED);   
	hRGBCapture1 = 0;
    //////////////////////////////////////////////////////////////////////////
    /*�趨���ڴ�С*/
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
	ActiveStatic = 0;   //��־��ǰ���ڵ�״̬:0-û����󻯣�1����󻯴��ڡ�
    PauseRunSignal1 = false;    //�ɼ��¼���ͣ��־��true��ͣ��false����
	bUsingAccBuffer1=false;  //��־��ǰ�����Ƿ��ѱ�ʹ�ã�true�ǣ�false��
    bufferSize1 = 0L;         //��1���ɼ�����Ӧ�Ļ����С
	startcount1 = FALSE;
	g_framecount1 = 0;  //��ŵ�һ���ɼ����ɼ�ͼ������
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
*������ɫ����
*/
HBRUSH CRGBSAMPDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor) 
{
 	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);
// 	
// 	// TODO: Change any attributes of the DC here
// 	hbr = m_Brush;  //??????
// 	switch(nCtlColor)
//     {
//        case CTLCOLOR_STATIC: //�����о�̬�ı��ؼ�������
//       {
// 			CFont * cFont=new CFont;
// 			cFont->CreateFont(30,0,0,0,FW_SEMIBOLD,FALSE,FALSE,0, 
// 									  ANSI_CHARSET,OUT_DEFAULT_PRECIS,
// 									  CLIP_DEFAULT_PRECIS,DEFAULT_QUALITY,
// 									  DEFAULT_PITCH&FF_SWISS,"Arial");
// 			pDC->SetBkMode(TRANSPARENT); 
// 			//���ñ���Ϊ͸��
// 			pDC->SetTextColor(RGB(255,255,255)); //����������ɫ
// 			pWnd->SetFont(cFont); //��������
// 			delete cFont;
//       }
// 	}

	
	// TODO: Return a different brush if the default is not desired
	return hbr;
}


/*****************************************************************************/
/*
*�һ��������Ի���
*/
void CRGBSAMPDlg::OnRButtonUp(UINT nFlags, CPoint point) 
{	
	RECT clientRect;	//�����ʾ�ؼ��ĳߴ�
	GetWindowRect(&clientRect);	//��ȡ��һ����ʾ�ؼ��ĳߴ�
	this->GetWindowRect(&clientRect);
	ClientToScreen(&point);
	if (PtInRect(&clientRect, point))
	{
		TRACE("OnRButtonUp(),�����״̬�ؼ���\n");	//����ڵ�һ��״̬�ؼ���
		CMenu menuPopup;
		if (menuPopup.CreatePopupMenu())
		{
			//��˵� menuPopup ����Ӳ˵���
			menuPopup.AppendMenu(MF_STRING, ID_Run_VGA1, "��ʼVGA");
			menuPopup.AppendMenu(MF_STRING, ID_Stop_VGA1, "ֹͣVGA");
			menuPopup.AppendMenu(MF_SEPARATOR);
			menuPopup.AppendMenu(MF_STRING, ID_ADJUST_VGA1, "VGA��������");
			menuPopup.AppendMenu(MF_STRING, ID_QUALPROP_VGA1, "��������");
			menuPopup.AppendMenu(MF_SEPARATOR);
			menuPopup.AppendMenu(MF_STRING, ID_PAUSE_VGA1, "��ͣVGA");
			menuPopup.AppendMenu(MF_STRING, ID_GOON_VGA1, "����VGA");
			menuPopup.AppendMenu(MF_SEPARATOR);
			
			//��ʾ����ʽ�˵�,�����û�ѡ��Ĳ˵���������Ӧ
			
			menuPopup.TrackPopupMenu(TPM_RIGHTBUTTON, point.x, point.y, this, 0);
		}
	}	
	CDialog::OnRButtonUp(nFlags, point); 
}

/*****************************************************************************/
/*
*���вɼ���
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
//		::MessageBox(m_hWnd, "VGA1�ɼ��������� !","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
	}
}

/*****************************************************************************/
/*
 *���вɼ�����
 */
void CRGBSAMPDlg::RunVGA(  HWND hWnd 
						 , HRGBCAPTURE *phRGBCapture 
						 , RGBCAPTUREBUFFER *pCaptureBuffer 
						 , unsigned long *pBufferSize
						 , RGBBITMAP        *pRGBBitmap
						 , unsigned long usDevice)
{	
	RGBCAPTUREPARMS CaptureParms;  //��ŵ�ǰ�ɼ����Ĳ���
	//////////////////////////////////////////////////////////////////////////
	/*���òɼ�������*/
	if (!SetVGAParameter(m_hWnd,&CaptureParms, phRGBCapture, pRGBBitmap, usDevice))   
	{
		TRACE("SetVGAParameter(),���òɼ�������ʧ��");
		MessageBox("û���ҵ�Ӳ���豸","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return;
	}
	Sleep(1);
    //////////////////////////////////////////////////////////////////////////
    /*��ȡ��ǰ����ģʽ*/
   // pixelFormat = DetectPixelFormat (hWnd,(DWORD *)&pRGBBitmap->bmiColors[0]);
    switch (pixelFormat)
	{

        case RGBPIXELFORMAT_555:
        case RGBPIXELFORMAT_565:
		{
			pRGBBitmap->bmiHeader.biBitCount = 16;
			break;
		}

		case RGBPIXELFORMAT_888: //��ǰֻ֧�����ģʽ
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
    /* ȷ������Ĵ�Сֵ �����仺��*/
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
	/*���òɼ�����*/
	if (!SetCapture(hWnd, &CaptureParms, phRGBCapture, pCaptureBuffer, pBufferSize, pRGBBitmap)) 
	{
		TRACE("SetCapture(),���òɼ�����ʧ��");
		return;
	}

}

/*****************************************************************************/
/*
*���òɼ�������
*/
BOOL CRGBSAMPDlg::SetVGAParameter(HWND hWnd
								  , RGBCAPTUREPARMS *pCaptureParms
								  , HRGBCAPTURE *phRGBCapture
								  , RGBBITMAP *pRGBBitmap
								  , unsigned long usDevice)
{
	RGBERROR error;
	//unsigned short pixelFormat;  //��ŵ�ǰ��⵽������ģʽ
	//////////////////////////////////////////////////////////////////////////
	/* �򿪲ɼ���*/
	error = RGBCaptureOpen(usDevice,phRGBCapture);
	if (RGBERROR_NO_ERROR != error)     //���豸ʧ�ܣ����ؽ�����£�
	{	
		switch (error)
		{
	        case RGBERROR_HARDWARE_NOT_FOUND :
			{
				TRACE("RGBCaptureOpen() ���豸��û���ҵ�Ӳ���豸\n");//û���ҵ��豸
				break;
			}

		    case RGBERROR_INVALID_POINTER :
			{
				TRACE("RGBCaptureOpen() ���豸��û���ҵ�ָ���Ӳ���豸��ָ��\n");//û���ҵ��豸
				break;
			}

			case RGBERROR_INVALID_INDEX :
			{
				TRACE("RGBCaptureOpen() ���豸��û���ҵ�Ӳ���豸������\n");//û���ҵ��豸
				break;
			}

			case RGBERROR_DEVICE_IN_USE :
			{
				TRACE("RGBCaptureOpen() ���豸��Ӳ���豸���ڱ�ʹ��\n");//Ӳ���豸���ڱ�ʹ��
				break;
			}

			case RGBERROR_UNABLE_TO_LOAD_DRIVER :
			{
				TRACE("RGBCaptureOpen() ���豸�����ܼ�������\n");//��Ӳ��������
				break;
			}

			case RGBERROR_INVALID_DEVICE :
			{
				TRACE("RGBCaptureOpen() ���豸��Ӳ��������\n");//��Ӳ��������
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
	/*���òɼ�����*/
	pCaptureParms->Size = sizeof (*pCaptureParms);
    pCaptureParms->Flags = RGBCAPTURE_PARM_PIXELFORMAT
                           | RGBCAPTURE_PARM_HWND
                           | RGBCAPTURE_PARM_NOTIFY_FLAGS
						   | RGBCAPTURE_PARM_VDIF_DESCRIPTION;    //���õĲ����У���ǰ����ģʽ�������ھ����֪ͨ��Ϣģʽ
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
				 TRACE("RGBCaptureSetParameters() ���òɼ���������û���ҵ�Ӳ���豸\n");//û���ҵ�Ӳ���豸
				 break;
			}

			case RGBERROR_INVALID_POINTER:
			{  
				TRACE("RGBCaptureSetParameters() ���òɼ���������û���ҵ�����ָ��\n");//û���ҵ�����ָ��
				break;
			}

			case RGBERROR_INVALID_SIZE:
			{
				TRACE("RGBCaptureSetParameters() ���òɼ�������������ָ���С����\n");//����ָ���С����
				break;
			}

			case RGBERROR_INVALID_FLAGS:
			{
				TRACE("RGBCaptureSetParameters() ���òɼ�����������֧�ֵı�־λ����\n");//��֧�ֵı�־λ����
				break;
			}

			case RGBERROR_INVALID_FORMAT:
			{
				TRACE("RGBCaptureSetParameters() ���òɼ�����������֧�ֵ������źŸ�ʽ\n");//��֧�ֵ������źŸ�ʽ
				break;
			}

			case RGBERROR_INVALID_BLACKLEVEL:
			{
				TRACE("RGBCaptureSetParameters() ���òɼ�����������֧�ֵĺ�ɫ��\n");//��֧�ֵĺ�ɫ��
				break;
			}

			case RGBERROR_INVALID_PIXEL_FORMAT:
			{   
				TRACE("RGBCaptureSetParameters() ���òɼ�����������֧�ֵ�����ģʽ\n");//��֧�ֵ�����ģʽ
				break;
			}

			case RGBERROR_INVALID_HWND:
			{
				TRACE("RGBCaptureSetParameters() ���òɼ������������ǵ�ǰ���ھ��\n");//���ǵ�ǰ���ھ��
				break;
			}

			case RGBERROR_INVALID_SYNCEDGE:
			{   
				TRACE("RGBCaptureSetParameters() ���òɼ�����������֧�ֵ�ͬ����\n");//��֧�ֵ�ͬ����
				break;
			}

			case RGBERROR_HSCALED_OUT_OF_RANGE:
			{  
				TRACE("RGBCaptureSetParameters() ���òɼ�����������֧�ֵ����ű���\n");//��֧�ֵ����ű���
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
   /*��ʼ�ɼ��¼�*/
	error = RGBCaptureEnable(*phRGBCapture, TRUE);
	if (RGBERROR_NO_ERROR != error)     //���豸ʧ�ܣ����ؽ�����£�
	{	
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureEnable() �����ɼ������򿪶�����û���ҵ���Ӳ���豸���\n");//�򿪶�����û���ҵ���Ӳ���豸���
				break;
			}

			case RGBERROR_CAPTURE_OUTSTANDING:
			{
				TRACE("RGBCaptureEnable() �����ɼ�����Ӳ���豸û�д�\n");//Ӳ���豸û�д�
				break;
			}

			case RGBERROR_THREAD_FAILURE:
			{
				TRACE("RGBCaptureEnable() �����ɼ����������߳�ʧ��\n");//�����߳�ʧ��
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
	/*���ô��ڲ���*/
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
*���òɼ�����
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
    /*��⵱ǰ�����VGA��Ƶ�źŷֱ���*/
	error = RGBCaptureDetectVideoMode(*phRGBCapture, 0, 0, &pCaptureParms->VideoTimings, FALSE);
	if (RGBERROR_NO_ERROR != error)
	{
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureDetectVideoMode(), ��⵱ǰ�����VGA��Ƶ�źŷֱ��ʣ����ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
				break;
			}

			case RGBERROR_INVALID_POINTER:
			{
				TRACE("RGBCaptureDetectVideoMode(), ��⵱ǰ�����VGA��Ƶ�źŷֱ��ʣ�û���ҵ�����ָ��\n");//û���ҵ�����ָ��
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
	/*���ô��ڱ���*/
	char szWndTitle[120];
	wsprintf(szWndTitle, "VGA�ɼ����� ");
    ::SetWindowText(::GetParent(this->m_hWnd), szWndTitle);

	//////////////////////////////////////////////////////////////////////////
    /* ����λͼ����Ϊ����ɼ�������׼�� */

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
	/*�˴���⻺���Ƿ�������ɹ�������ɼ��¼�*/
	error = RGBCaptureFrameBufferReady (*phRGBCapture,
                                       pCaptureBuffer[0].Index,
                                       *pBufferSize);
	if (error != RGBERROR_NO_ERROR)
	{
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureFrameBufferReady(),  ��⵱ǰVGA�ɼ��������Ƿ���������ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
				break;
			}

			case RGBERROR_INVALID_INDEX:
			{
				TRACE("RGBCaptureFrameBufferReady(),  ��⵱ǰVGA�ɼ��������Ƿ������û���ҵ��豸����\n");//û���ҵ�����ָ��
				break;
			}

			case RGBERROR_BUFFER_TOO_SMALL:
			{
				TRACE("RGBCaptureFrameBufferReady(),  ��⵱ǰVGA�ɼ��������Ƿ����������̫С\n");//û���ҵ�����ָ��
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
			TRACE("FreeBuffers(),�ͷŻ���ʧ�ܣ�");
			return FALSE;
		}

		return FALSE;
	}
	pCaptureBuffer[0].bufferflags = TRUE;
	return TRUE;
}

/*****************************************************************************/ 
/*
*��⵱ǰ����ģʽ
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
 *��������λ��
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
 *����ָ����С���仺��
 */
BOOL CRGBSAMPDlg::AllocateBuffers(HRGBCAPTURE hRGBCapture, 
                                      RGBCAPTUREBUFFER *pCaptureBuffer, 
									  HWND hWnd, 
									  UINT BufferSize)
{
	//////////////////////////////////////////////////////////////////////////
   /*���仺��*/
   pCaptureBuffer->LpVoidBuffer = GlobalAlloc(GMEM_FIXED, BufferSize);
   if (pCaptureBuffer->LpVoidBuffer == NULL)
   {
	   return FALSE;
   }
	TRACE("%d",GlobalSize(pCaptureBuffer->LpVoidBuffer));
   //////////////////////////////////////////////////////////////////////////
   /*ע�ᵱǰ��VGA�ɼ��¼�*/
   RGBERROR error;
   error = RGBCaptureUseMemoryBuffer(hRGBCapture, pCaptureBuffer->LpVoidBuffer, BufferSize, &pCaptureBuffer->Index);
   if (RGBERROR_NO_ERROR != error)
   {
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  ע�ᵱǰ��VGA�ɼ��¼���ע��ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
				break;
			}

			case RGBERROR_INVALID_POINTER:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  ע�ᵱǰ��VGA�ɼ��¼���û���ҵ�ָ��ǰ����洢����ָ��\n");//û���ҵ�ָ��ǰ����洢����ָ��
				break;
			}

			case RGBERROR_INVALID_INDEX:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  ע�ᵱǰ��VGA�ɼ��¼���û���ҵ���Ӧ��ǰ����洢��������\n");//û���ҵ���Ӧ��ǰ����洢��������
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
	  bUsingAccBuffer1 = true;  //�޸ı�־����ʹ�õĲ���ΪTRUE
   }

   return TRUE;
}

/*****************************************************************************/
/*
*���ڳߴ����
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
// 	if ( MessageBox("ȷ��Ҫ�˳�ϵͳ�� ?\t","ϵͳ��ʾ",MB_ICONQUESTION | MB_OKCANCEL ) != IDOK )
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
*�ͷŻ�����
*/
BOOL CRGBSAMPDlg::FreeBuffers(HRGBCAPTURE hRGBCapture, HWND hWnd, RGBCAPTUREBUFFER *pCaptureBuffer)
{
	RGBERROR error;
	HDC hDC = ::GetDC(hWnd);

	if (bUsingAccBuffer1 && (hRGBCapture == hRGBCapture1))
   {
	  bUsingAccBuffer1 = false;  //�޸ı�־�����Ƿ�ʹ�õĲ���Ϊfalse
   }


   ::ReleaseDC(hWnd, hDC);
    
    if (pCaptureBuffer[0].LpVoidBuffer)
	{
		//////////////////////////////////////////////////////////////////////////
	    /*ɾ����ǰ��VGA�ɼ���������ʹ�ö����е�����*/
		 error = RGBCaptureReleaseBuffer(hRGBCapture, pCaptureBuffer[0].Index);
		 if (RGBERROR_NO_ERROR != error)
		 {
			 switch(error)
			{
				case RGBERROR_INVALID_HRGBCAPTURE:
				{
					TRACE("RGBCaptureReleaseBuffer(),   ɾ����ǰ��VGA�ɼ���������ʹ�ö����е�������ע��ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
					break;
				}

				case RGBERROR_INVALID_POINTER:
				{
					TRACE("RGBCaptureReleaseBuffer(),    ɾ����ǰ��VGA�ɼ���������ʹ�ö����е�������û���ҵ�ָ��ǰ����洢����ָ��\n");//û���ҵ�ָ��ǰ����洢����ָ��
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
	    /*�ͷŻ�����*/
		 GlobalFree(pCaptureBuffer[0].LpVoidBuffer); 
		 pCaptureBuffer[0].LpVoidBuffer = NULL;
	}
	if (pCaptureBuffer[1].LpVoidBuffer)
	{
		//////////////////////////////////////////////////////////////////////////
	    /*ɾ����ǰ��VGA�ɼ���������ʹ�ö����е�����*/
		 error = RGBCaptureReleaseBuffer(hRGBCapture, pCaptureBuffer[1].Index);
		 if (RGBERROR_NO_ERROR != error)
		 {
			 switch(error)
			{
				case RGBERROR_INVALID_HRGBCAPTURE:
				{
					TRACE("RGBCaptureReleaseBuffer(),   ɾ����ǰ��VGA�ɼ���������ʹ�ö����е�������ע��ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
					break;
				}

				case RGBERROR_INVALID_POINTER:
				{
					TRACE("RGBCaptureReleaseBuffer(),    ɾ����ǰ��VGA�ɼ���������ʹ�ö����е�������û���ҵ�ָ��ǰ����洢����ָ��\n");//û���ҵ�ָ��ǰ����洢����ָ��
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
	    /*�ͷŻ�����*/
		 GlobalFree(pCaptureBuffer[1].LpVoidBuffer); 
		 pCaptureBuffer[1].LpVoidBuffer = NULL;
	}
	delete (pCaptureBuffer);
	return TRUE;
}

/******************************************************************************/
/*
*����SDK��������ϢRGBWM_FRAMECAPTURED
*��ϢRGBWM_FRAMECAPTURED:������ǰ�ɼ�������׼��������������һ֡ͼ�����ݵȴ�������
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
				TRACE("RGBCaptureFrameBufferReady(),  ��⵱ǰVGA�ɼ��������Ƿ���������ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
				break;
			}

			case RGBERROR_INVALID_INDEX:
			{
				TRACE("RGBCaptureFrameBufferReady(),  ��⵱ǰVGA�ɼ��������Ƿ������û���ҵ��豸����\n");//û���ҵ�����ָ��
				break;
			}

			case RGBERROR_BUFFER_TOO_SMALL:
			{
				TRACE("RGBCaptureFrameBufferReady(),  ��⵱ǰVGA�ɼ��������Ƿ����������̫С\n");//û���ҵ�����ָ��
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
		
		//BMPͷ 
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
		
		//BMPͷ 
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
 *����ͼ��
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
// 	CRect recttmp;	//��Ŵ��ڳߴ�
// 	::GetClientRect(hWnd, &recttmp);  //��ȡ��ǰ���ڵĳߴ�
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
// 		TRACE("CRGBSAMPDlg::PaintFrame ��ͼ���ɹ�!!! %d\n", dwErrorCode);
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

	/*��־���ΪTRUE���¼����֡��*/
	if (hWnd == m_hWnd && startcount1)
	{
		g_framecount1++;      //��ͼ��������¼
	}
}

/*****************************************************************************/
/*
*�������ط����ı�ʱSDK��������ϢRGBWM_MODECHANGED
*�������òɼ�����
*/
void CRGBSAMPDlg::OnMyMessage_modechange(WPARAM wParam, LPARAM lParam)
{
	unsigned long *pbufferSize = 0L;	
	RGBCAPTUREPARMS CaptureParms;
	if (!SetCapture(m_hWnd, &CaptureParms, &hRGBCapture1, pCaptureBuffer1, &bufferSize1, &RGBBitmap1))  //���òɼ�����
	{
		TRACE("SetCapture(),���òɼ�����ʧ��");
		return;
	}
}

/*****************************************************************************/
/*
*����SDK�ɼ�������ȷ������ʱ��������ϢRGBWM_NOSIGNAL
*/
void CRGBSAMPDlg::OnMyMessage_nosignal(WPARAM wParam, LPARAM lParam)
{
	CString strInfo;
	switch(wParam)
	{
		case NOSIGNAL_NO_RESOLUTION:
		{
			strInfo = _T("�� VGA �� ��(����ķֱ��� !!!)");
			break;
		}
		case NOSIGNAL_DDRCHECK_ERROR:
		{
			strInfo = _T("�� VGA �� ��(DDR �Լ���� !!!)");
			break;
		}
		case NOSIGNAL_H_OVERFLOW:
		{
			strInfo = _T("�� VGA �� ��(ˮƽƫ������� !!!)");
			break;
		}
		case NOSIGNAL_V_OVERFLOW:
		{
			strInfo = _T("�� VGA �� ��(��ֱƫ������� !!!)");
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
// 	/*��������*/
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
// 	strcpy(ft.lfFaceName,"����");
// 	
// 	HFONT	hFont = ::CreateFontIndirect(&ft);
// 	pDC = GetDC();
// 	GetClientRect(rc);
// 	this->RedrawWindow();
// 	x = ( rc.Width() - strInfo.GetLength() * ft.lfWidth ) / 2;
// 	y = ( rc.Height() - ft.lfHeight ) / 2;
// 	pDC->SelectObject(hFont);
// 	//////////////////////////////////////////////////////////////////////////
// 	/*������ˢ*/
// 	CBrush brush;
// 	brush.CreateSolidBrush(RGB(0,0,0));
// 	/*��ͼ*/
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
*ֹͣ�ɼ�ͼ��
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
	/*���ô��ڱ���*/
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
		::MessageBox(GetTopWindow()->m_hWnd, "δ����VGA1�ɼ��� !","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
	}
}

void CRGBSAMPDlg::stopVGA(HWND hWnd, HRGBCAPTURE *phRGBCapture, RGBCAPTUREBUFFER *pCaptureBuffer)
{
	// TODO: Add your command handler code here

	//////////////////////////////////////////////////////////////////////////
	/*�ͷŻ���*/
	if (!FreeBuffers(*phRGBCapture, hWnd, pCaptureBuffer))
	{
		TRACE("FreeBuffers(),�ͷŻ���ʧ�ܣ�");
		return;
	}
	//////////////////////////////////////////////////////////////////////////

	//////////////////////////////////////////////////////////////////////////
	/*ֹͣ�ɼ��¼�*/
	RGBERROR error;	
	error = RGBCaptureEnable(*phRGBCapture, FALSE);
	if (RGBERROR_NO_ERROR != error)     //���豸ʧ�ܣ����ؽ�����£�
	{	
		switch (error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureEnable() �����ɼ������򿪶�����û���ҵ���Ӳ���豸���\n");//�򿪶�����û���ҵ���Ӳ���豸���
				break;
			}

			case RGBERROR_CAPTURE_OUTSTANDING:
			{
				TRACE("RGBCaptureEnable() �����ɼ�����Ӳ���豸û�д�\n");//Ӳ���豸û�д�
				break;
			}

			case RGBERROR_THREAD_FAILURE:
			{
				TRACE("RGBCaptureEnable() �����ɼ����������߳�ʧ��\n");//�����߳�ʧ��
				break;
			}

			default:
			{
				break;
			}
		}
		return;
	}

	/*�رղɼ���*/
	RGBCaptureClose(*phRGBCapture);
	*phRGBCapture = 0;

}



/*****************************************************************************/
/*
*�����ɼ����������������������Ĵ���
*/
void CRGBSAMPDlg::OnAdjustVga1() 
{
	// TODO: Add your command handler code here
	
	if ( 0 == hRGBCapture1)
	{
		::MessageBox(m_hWnd, "δ����VGA�ɼ��� !","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
	}
	else
	{
		SendMessageW(::GetParent(this->m_hWnd),RGBSAMPWM_QUALADJUST,0,(LPARAM)this);
	}
}


/*****************************************************************************/
/*
*������������
*/
void CRGBSAMPDlg::OnQualpropVga1() 
{
	// TODO: Add your command handler code here
	
	if ( 0 == hRGBCapture1 )
	{
		::MessageBox(m_hWnd, "δ����VGA�ɼ��� !","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
	}
	else
	{
		SendMessageW(::GetParent(this->m_hWnd),RGBSAMPWM_QUALPARA,0,(LPARAM)this);
	}
}

/*****************************************************************************/
/*
*��ͣ�ɼ�
*/
void CRGBSAMPDlg::OnPauseVga1() 
{
	// TODO: Add your command handler code here
	if (0 == hRGBCapture1)
	{
		::MessageBox(GetTopWindow()->m_hWnd, "δ����VGA1�ɼ��� !","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return;
	}
	if (PauseRunSignal1)
	{
		::MessageBox(GetTopWindow()->m_hWnd, "����ͣVGA1�ɼ��� !","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return;
	}
	PauseRunSignal1 = TRUE;
}

/*****************************************************************************/
/*
*�����ɼ�
*/
void CRGBSAMPDlg::OnGoonVga1() 
{
	// TODO: Add your command handler code here
	HRGBCAPTURE hRGBCapture;
	hRGBCapture = hRGBCapture1;
	if (0 == hRGBCapture1)
	{
		::MessageBox(GetTopWindow()->m_hWnd, "δ����VGA1�ɼ��� !","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return;
	}
		if (!PauseRunSignal1)
	{
		::MessageBox(GetTopWindow()->m_hWnd, "δ��ͣVGA1�ɼ��� !","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return;
	}
	PauseRunSignal1 = FALSE;
	//////////////////////////////////////////////////////////////////////////
	/*�˴���⻺���Ƿ�������ɹ�������ɼ��¼�*/
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
				TRACE("RGBCaptureFrameBufferReady(),  ��⵱ǰVGA�ɼ��������Ƿ���������ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
				break;
			}

			case RGBERROR_INVALID_INDEX:
			{
				TRACE("RGBCaptureFrameBufferReady(),  ��⵱ǰVGA�ɼ��������Ƿ������û���ҵ��豸����\n");//û���ҵ�����ָ��
				break;
			}

			case RGBERROR_BUFFER_TOO_SMALL:
			{
				TRACE("RGBCaptureFrameBufferReady(),  ��⵱ǰVGA�ɼ��������Ƿ����������̫С\n");//û���ҵ�����ָ��
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
// *˫�������Ƶ�ǰ�ɼ����ڷŴ�/��С
void CRGBSAMPDlg::OnLButtonDblClk(UINT nFlags, CPoint point) 
{
//	SendMessageW(::GetParent(this->m_hWnd),RGBSAMPWM_MAXSIZE,0, (LPARAM)this->m_capnum);
//	RedrawWindow();
// 	// TODO: Add your message handler code here and/or call default
// //	RECT rect; //��������ڵĳߴ�
// 	
// 	//this->RedrawWindow();
// 
// 	//////////////////////////////////////////////////////////////////////////
// 	���ݵ�ǰ����ڲ�����ȡ��ͬ�Ĵ�ʩ//	this->GetClientRect(&rect);  //��ȡ�����ڵĳߴ�  
// 	switch (ActiveStatic)
// 	{
// 		case 0://��󻯴���λ��
// 		{
// 			ModifyStyle(WS_CAPTION,0,0);
// 			this->GetClientRect(&client);  //�������������֮ǰ�ĳߴ�
// 			this->GetWindowRect(&screen);
// 			
// 			
// 			int   nFullWidth=GetSystemMetrics(SM_CXSCREEN)+8;   
// 		 	int   nFullHeight=GetSystemMetrics(SM_CYSCREEN)+8;
// 
// 			ClientToScreen(&point);
// 			if (PtInRect(&screen, point))
// 			{
// 				TRACE("OnRButtonUp(),����ڴ�����");	//����ڵ�״̬�ؼ���
// 
// 				�ı䴰�ں���ʾ�ؼ��ĳߴ�				this->MoveWindow(-4, -4, nFullWidth, nFullHeight,TRUE);
// 				m_static1.MoveWindow(-4, -4, nFullWidth, nFullHeight,TRUE);
// 				ActiveStatic = 1;	
// 			}
// 			break;
// 		}		
// 		case 1: //˫����ԭ����
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
// 			TRACE("��ERROR ˫���¼�δƥ�� LINE:%d��\n", __LINE__);
// 			break;
// 		}
// 	}
// 	RedrawWindow();
	CDialog::OnLButtonDblClk(nFlags, point);
}

LRESULT CRGBSAMPDlg::OnMyMessage_datamode(WPARAM wParam, LPARAM lParam)
{
//	if (IDOK == ::MessageBox(m_hWnd, "��ѡ����������ģʽ:\n  ѡ��RGBģʽ������ȷ����;\n  ѡ��YUVģʽ������ȡ����","ϵͳ��ʾ",MB_ICONWARNING | MB_OKCANCEL))
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

// QualPropDlg.cpp : implementation file
//
// *****************************************************************************
//  QualPropDlg   VERSION:  1.1   DATE: 2008-10-15
//  ----------------------------------------------------------------------------
//		FileName: 	QualPropDlg.cpp
//		Created:	2008-10-15   16:50
//		Author:		MengJuan
//		Purpose:	显示当前采集图象质量参数
//		Version:	
//					【V1.1 2008-10-15 mengjuan】
//						Initial
//					  创建类CAdjParamDlg
//					  初始化OnInitDialog方法					
//                    关闭OnDestroy方法
//					  定时OnTimer方法
//		Remark:		N/A
//  ----------------------------------------------------------------------------
//  Copyright (C) 2008 
//	Nanjing Talent Electronics and Technology Co., LTD.
//  All Rights Reserved
// *****************************************************************************


#include "stdafx.h"
#include "RGBSAMP.h"
#include "QualPropDlg.h"
#include "./include/TC1000API.h"
#include "RGBCAPDLG.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

extern CRGBSAMPDlg * pRGBSAMPact;
// /////////////////////////////////////////////////////////////////////////////
// // CAdjParamDlg dialog
/*
 *声明RGBErrorToString方法
 */
 char * RGBErrorToString(RGBERROR error);

CQualPropDlg::CQualPropDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CQualPropDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CQualPropDlg)
	m_fAvgFrameRate = 0.0f;
	m_nFrameDraw = 0;
	m_nHScaled = 0;
	m_nVScaled = 0;
	m_nDataMode = _T("");
	m_nRefreshRate = 0;
	//}}AFX_DATA_INIT
}


void CQualPropDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CQualPropDlg)
	DDX_Text(pDX, IDC_EDIT_AVGFRAMERATE, m_fAvgFrameRate);
	DDX_Text(pDX, IDC_EDIT_HSCALED, m_nHScaled);
	DDX_Text(pDX, IDC_EDIT_VSCALED, m_nVScaled);
	DDX_Text(pDX, IDC_EDIT_MODE, m_nDataMode);
	DDX_Text(pDX, IDC_EDIT_REFRESHRATE, m_nRefreshRate);
	DDV_MinMaxInt(pDX, m_nRefreshRate, 60, 85);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CQualPropDlg, CDialog)
	//{{AFX_MSG_MAP(CQualPropDlg)
	ON_WM_DESTROY()
	ON_WM_TIMER()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CQualPropDlg message handlers

/*****************************************************************************/ 
/*
 *对话框初始化，设定时间
 */
BOOL CQualPropDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	
	SetTimer(TALENT_TIMER_QUALPROP,1000,NULL);  //设置定时时间为1000毫秒

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

/*****************************************************************************/
/*
 *关闭对话框
 */
void CQualPropDlg::OnDestroy() 
{
	CDialog::OnDestroy();
	
	// TODO: Add your message handler code here
	if (pRGBSAMPact->startcount1)
	{
		pRGBSAMPact->startcount1 = FALSE;	//窗口关闭后将记录帧数标志关闭
	}
	pRGBSAMPact = NULL;
	KillTimer(TALENT_TIMER_QUALPROP);
	
}

/*****************************************************************************/
/*
 *设定时间到时获取此时的采集图象质量参数
 */
void CQualPropDlg::OnTimer(UINT nIDEvent) 

{
	// TODO: Add your message handler code here and/or call default

	if (TALENT_TIMER_QUALPROP == nIDEvent)
	{
		//////////////////////////////////////////////////////////////////////////
		/*获取当前的采集参数*/
		char Descript[128];
		wsprintf(Descript, "质量管理%s", "VGA:");

		/*判断当前是否开始记录数据如果没有则开启标志*/
		if (!pRGBSAMPact->startcount1)
		{
			pRGBSAMPact->startcount1 = TRUE;
		}
		/*修改对话框标题*/
		::SetWindowText(m_hWnd, Descript);
		/*获取当前采集卡的参数*/
		RGBCAPTUREPARMS CurCaptureParms;
		RGBERROR   Error = RGBERROR_NO_ERROR;
		CurCaptureParms.Size = sizeof(RGBCAPTUREPARMS);
		CurCaptureParms.Flags =RGBCAPTURE_PARM_ALL;  //表明获取当前采集卡的全部参数值
		Error = RGBCaptureGetParameters(pRGBSAMPact->hRGBCapture1, &CurCaptureParms, 
								RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
		if (RGBERROR_NO_ERROR !=Error)
		{
			switch (Error)
			{
				case RGBERROR_INVALID_HRGBCAPTURE:
				{
					TRACE("RGBCaptureUseMemoryBuffer(),  获取当前的VGA采集事件的参数，获取失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
					return;
					break;
				}

				case RGBERROR_INVALID_POINTER:
				{
					TRACE("RGBCaptureUseMemoryBuffer(), 获取当前的VGA采集事件的参数，没有找到指向当前缓存存储区的指针\n");//没有找到参数指针
					return;
					break;
				}

				case RGBERROR_INVALID_FLAGS:
				{
					TRACE("RGBCaptureUseMemoryBuffer(),  获取当前的VGA采集事件的参数，不存在的参数标志\n");//没有找到参数指针
					return;
					break;
				}

				case RGBERROR_INVALID_SIZE:
				{
					TRACE("RGBCaptureUseMemoryBuffer(),  获取当前的VGA采集事件的参数，参数大小有误\n");//没有找到参数指针
					return;
					break;
				}

				default:
				{
					return;
					break;
				}
			}
			char text[80];
			wsprintf ( text, "CQualPropDlg::RGBCaptureGetParameters (current) returned: %s", RGBErrorToString(Error));
			MessageBox (text, NULL, MB_OK | MB_ICONERROR);
			return;
		}
		//////////////////////////////////////////////////////////////////////////
		/*将采集参数传给对话框的相应参数*/
		m_nHScaled = CurCaptureParms.HScaled;  //水平分辨率
		m_nVScaled = CurCaptureParms.VScaled;  //垂直分辨率
		m_nRefreshRate = CurCaptureParms.VideoTimings.VerFrequency;
		m_fAvgFrameRate = (float)(pRGBSAMPact->g_framecount1);   //每秒帧数，用前一秒的帧数减去当前的帧数得到每秒帧数
		
		unsigned long uldatamode = 0;
		if (!RGBCaptureGetDataMode(pRGBSAMPact->hRGBCapture1, &uldatamode))
		{
			m_nDataMode = "NO";
		}
		else
		{
			if (1 == uldatamode)
			{
				m_nDataMode = "RGB";
			}
			else
			{
				if (0 == uldatamode)
				m_nDataMode = "YUV";
			}
			
		}

		pRGBSAMPact->g_framecount1 = 0;

	}

	UpdateData(FALSE);
	
	CDialog::OnTimer(nIDEvent);
}

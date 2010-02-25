// AdjParamDlg.cpp : implementation file
//
// *****************************************************************************
//  AdjParamDlg   VERSION:  1.1   DATE: 2008-10-15
//  ----------------------------------------------------------------------------
//		FileName: 	AdjParamDlg.cpp
//		Created:	2008-10-15   16:50
//		Author:		MengJuan
//		Purpose:	采集卡参数调整
//		Version:	
//					【V1.1 2008-10-15 mengjuan】
//						Initial
//					  创建类CAdjParamDlg
//					  初始化OnInitDialog方法					
//                    点击“默认值”（黑 色 度）OnButtonBlacklevel方法
//					  点击“默认值”（亮    度）OnButtonBrightness方法
//					  点击“默认值”（对 比 度）OnButtonContrast方法
//					  点击“默认值”（水平位置）OnButtonHoroffset方法
//					  点击“默认值”（相    位）OnButtonPhase方法
//					  点击“默认值”（帧    率）OnButtonSamprate方法
//					  点击“默认值”（垂直位置）OnButtonVeroffset方法
//                    滑块滑动OnHScroll方法
//		Remark:		N/A
//  ----------------------------------------------------------------------------
//  Copyright (C) 2008 
//	Nanjing Talent Electronics and Technology Co., LTD.
//  All Rights Reserved
// *****************************************************************************


#include "stdafx.h"
#include "RGBSAMP.h"
#include "AdjParamDlg.h"
#include "./include/TC1000API.h"
#include "RGBCAPDLG.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/*
 *	定义全局变量DefCaptureParms,用于存放默认采集卡参数
 */
RGBCAPTUREPARMS DefCaptureParms;
/*
 *	定义全局变量DError，用来存放错误码
 */
RGBERROR   DError = RGBERROR_NO_ERROR;
/////////////////////////////////////////////////////////////////////////////
// CAdjParamDlg dialog

/*
 *	声明RGBErrorToString方法
 */
char * RGBErrorToString(RGBERROR error);

extern CRGBSAMPDlg * pRGBSAMPact;

CAdjParamDlg::CAdjParamDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CAdjParamDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CAdjParamDlg)
	m_nBlacklevel = 0;
	m_nBrightness = 0;
	m_nContrast = 0;
	m_nPhase = 0;
	m_nSamprate = 0;
	m_nVeroffset = 0;
	m_nHoroffset = 0;
	m_nClampDuration = 0;
	m_nClampPlacement = 0;
	//}}AFX_DATA_INIT
}


void CAdjParamDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAdjParamDlg)
	DDX_Control(pDX, IDC_SLIDER_CLAMP_DURATION, m_ctrlClampDuration);
	DDX_Control(pDX, IDC_SLIDE_CLAMP_PLACEMENT, m_ctrlClampPlacement);
	DDX_Control(pDX, IDC_SLIDER_VEROFFSET, m_ctrlVeroffset);
	DDX_Control(pDX, IDC_SLIDER_HOROFFSET, m_ctrlHoroffset);
	DDX_Control(pDX, IDC_SLIDER_SAMPRATE, m_ctrlSamprate);
	DDX_Control(pDX, IDC_SLIDER_PHASE, m_ctrlPhase);
	DDX_Control(pDX, IDC_SLIDER_CONTRAST, m_ctrlContrast);
	DDX_Control(pDX, IDC_SLIDER_BRIGHTNESS, m_ctrlBrightness);
	DDX_Control(pDX, IDC_SLIDER_BLACKLEVEL, m_ctrlBlacklevel);
	DDX_Text(pDX, IDC_EDIT_BLACKLEVEL, m_nBlacklevel);
	DDV_MinMaxInt(pDX, m_nBlacklevel, 0, 0);
	DDX_Text(pDX, IDC_EDIT_BRIGHTNESS, m_nBrightness);
	DDV_MinMaxInt(pDX, m_nBrightness, 0, 255);
	DDX_Text(pDX, IDC_EDIT_CONTRAST, m_nContrast);
	DDV_MinMaxInt(pDX, m_nContrast, 0, 255);
	DDX_Text(pDX, IDC_EDIT_PHASE, m_nPhase);
	DDV_MinMaxInt(pDX, m_nPhase, 0, 31);
	DDX_Text(pDX, IDC_EDIT_SAMPRATE, m_nSamprate);
	DDV_MinMaxInt(pDX, m_nSamprate, 1, 60);
	DDX_Text(pDX, IDC_EDIT_VEROFFSET, m_nVeroffset);
	DDV_MinMaxInt(pDX, m_nVeroffset, 0, 255);
	DDX_Text(pDX, IDC_EDIT_HOROFFSET, m_nHoroffset);
	DDV_MinMaxInt(pDX, m_nHoroffset, 0, 511);
	DDX_Text(pDX, IDC_EDIT_CLAMP_DURATION, m_nClampDuration);
	DDV_MinMaxInt(pDX, m_nClampDuration, 0, 255);
	DDX_Text(pDX, IDC_EDIT_CLAMP_PLACEMENT, m_nClampPlacement);
	DDV_MinMaxInt(pDX, m_nClampPlacement, 0, 255);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CAdjParamDlg, CDialog)
	//{{AFX_MSG_MAP(CAdjParamDlg)
	ON_BN_CLICKED(IDC_BUTTON_BLACKLEVEL, OnButtonBlacklevel)
	ON_BN_CLICKED(IDC_BUTTON_BRIGHTNESS, OnButtonBrightness)
	ON_BN_CLICKED(IDC_BUTTON_CONTRAST, OnButtonContrast)
	ON_BN_CLICKED(IDC_BUTTON_HOROFFSET, OnButtonHoroffset)
	ON_BN_CLICKED(IDC_BUTTON_PHASE, OnButtonPhase)
	ON_BN_CLICKED(IDC_BUTTON_SAMPRATE, OnButtonSamprate)
	ON_BN_CLICKED(IDC_BUTTON_VEROFFSET, OnButtonVeroffset)
	ON_WM_HSCROLL()
	ON_BN_CLICKED(IDC_BUTTON_CLAMP_PLACEMENT, OnButtonClampPlacement)
	ON_BN_CLICKED(IDC_BUTTON_CLAMP_DURATION, OnButtonClampDuration)
	ON_WM_LBUTTONDBLCLK()
	ON_WM_DESTROY()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAdjParamDlg message handlers

/*****************************************************************************/
/*
 *	初始化对话框，获取当前和默认的视频采集参数
 */
BOOL CAdjParamDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	char Descript[128];
	/*
	*根据当前选择的窗口标志值确定调整的采集卡
	*/
	wsprintf(Descript, "参数调节%s", "VGA1:");
	
	/*修改对话框标题*/
	::SetWindowText(m_hWnd, Descript);

	long	lBrightness = 0;
	long	lContrast   = 0;
	long	lPhase      = 0;
	long	lSamprate   = 0;
	long	lHoroffset  = 0;
	long	lVeroffset  = 0;
	long    lBlacklevel = 0;
	RGBERROR   Error = RGBERROR_NO_ERROR;
	
	//////////////////////////////////////////////////////////////////////////
	/*获取默认的采集卡参数，为下面的操作做准备*/
	DefCaptureParms.Size = sizeof(RGBCAPTUREPARMS);
	DefCaptureParms.Flags = RGBCAPTURE_PARM_PHASE  |
							RGBCAPTURE_PARM_BRIGHTNESS |
							RGBCAPTURE_PARM_BLACKLEVEL |
							RGBCAPTURE_PARM_VDIF_HTIMINGS |
							RGBCAPTURE_PARM_VDIF_VTIMINGS |
							RGBCAPTURE_PARM_SAMPLERATE |
							RGBCAPTURE_PARM_CONTRAST;
	DError = RGBCaptureGetParameters(pRGBSAMPact->hRGBCapture1, &DefCaptureParms, 
									 RGBCAPTURE_FLAG_DEFAULT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR !=Error)
	{
		switch (Error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  获取默认的VGA采集事件的参数，获取失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
				break;
			}

			case RGBERROR_INVALID_POINTER:
			{
				TRACE("RGBCaptureUseMemoryBuffer(), 获取默认的VGA采集事件的参数，没有找到指向当前缓存存储区的指针\n");//没有找到参数指针
				break;
			}

			case RGBERROR_INVALID_FLAGS:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  获取默认的VGA采集事件的参数，不存在的参数标志\n");//没有找到参数指针
				break;
			}

			case RGBERROR_INVALID_SIZE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  获取默认的VGA采集事件的参数，参数大小有误\n");//没有找到参数指针
				break;
			}

			default:
			{
				break;
			}
		}
// 		char text[80];
// 		wsprintf ( text, "RGBCaptureGetParameters (default) returned: %s", RGBErrorToString(DError) );
// 		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return FALSE;
	}

	//////////////////////////////////////////////////////////////////////////

	//////////////////////////////////////////////////////////////////////////
	/*获取当前的采集卡参数，并将参数值传给相应的EDIT变量*/
	RGBCAPTUREPARMS CurCaptureParms;
	CurCaptureParms.Size = sizeof(RGBCAPTUREPARMS);
	CurCaptureParms.Flags = RGBCAPTURE_PARM_PHASE  |
							RGBCAPTURE_PARM_BRIGHTNESS |
							RGBCAPTURE_PARM_BLACKLEVEL |
							RGBCAPTURE_PARM_VDIF_HTIMINGS |
							RGBCAPTURE_PARM_VDIF_VTIMINGS |
							RGBCAPTURE_PARM_SAMPLERATE |
							RGBCAPTURE_PARM_CONTRAST;
	Error = RGBCaptureGetParameters(pRGBSAMPact->hRGBCapture1, &CurCaptureParms, 
									RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR !=Error)
	{
		switch(Error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  获取默认的VGA采集事件的参数，获取失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
				break;
			}

			case RGBERROR_INVALID_POINTER:
			{
				TRACE("RGBCaptureUseMemoryBuffer(), 获取默认的VGA采集事件的参数，没有找到指向当前缓存存储区的指针\n");//没有找到参数指针
				break;
			}

			case RGBERROR_INVALID_FLAGS:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  获取默认的VGA采集事件的参数，不存在的参数标志\n");//没有找到参数指针
				break;
			}

			case RGBERROR_INVALID_SIZE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  获取默认的VGA采集事件的参数，参数大小有误\n");//没有找到参数指针
				break;
			}

			default:
			{
				break;
			}
		}
		char text[80];
		wsprintf ( text, "RGBCaptureGetParameters (default) returned: %s", RGBErrorToString(Error) );
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return FALSE;
	}
	lBrightness = CurCaptureParms.Brightness;
	lBlacklevel = CurCaptureParms.BlackLevel;
	lContrast   = CurCaptureParms.Contrast;
	lSamprate   = CurCaptureParms.SampleRate;
	lHoroffset  = CurCaptureParms.HorOffset;
	lVeroffset  = CurCaptureParms.VerOffset;
	lPhase      = CurCaptureParms.Phase;

	m_nBrightness = lBrightness;
	m_nContrast   = lContrast;
	m_nPhase	  = lPhase;
	m_nHoroffset  = lHoroffset;
	m_nVeroffset  = lVeroffset;
	m_nSamprate   = lSamprate;
	m_nBlacklevel = lBlacklevel;
	m_nClampPlacement = 10;
	m_nClampDuration = 10;
	//////////////////////////////////////////////////////////////////////////

	//////////////////////////////////////////////////////////////////////////
	/*设置滑块的滑动范围*/
	m_ctrlBrightness.SetRange(0,255);
	m_ctrlBrightness.SetPos(lBrightness);

	m_ctrlContrast.SetRange(0,255);
	m_ctrlContrast.SetPos(lContrast);

	m_ctrlPhase.SetRange(0,31);
	m_ctrlPhase.SetPos(lPhase);

	m_ctrlHoroffset.SetRange(0,511);//(3,255)
	m_ctrlHoroffset.SetPos(lHoroffset);

	m_ctrlVeroffset.SetRange(0,255);//(0,255)
	m_ctrlVeroffset.SetPos(lVeroffset);

	m_ctrlSamprate.SetRange(1,60);
	m_ctrlSamprate.SetPos(lSamprate);

	m_ctrlBlacklevel.SetRange(0,0);
	m_ctrlBlacklevel.SetPos(lBlacklevel);

	m_ctrlClampPlacement.SetRange(0,255);
	m_ctrlClampPlacement.SetPos(10);

	m_ctrlClampDuration.SetRange(0,255);
	m_ctrlClampDuration.SetPos(10);


	UpdateData(FALSE);
	//////////////////////////////////////////////////////////////////////////
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

/*****************************************************************************/
/*
 *	设置黑色度为默认值
 */
void CAdjParamDlg::OnButtonBlacklevel() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("设置默认黑色度失败 !\t","系统提示",MB_ICONWARNING | MB_OK);
		return ;
	}
	m_nBlacklevel = DefCaptureParms.BlackLevel;
	m_ctrlBlacklevel.SetPos(m_nBlacklevel);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_BLACKLEVEL;  //设置标志位为指向相应参数的标志
	Error = RGBCaptureSetParameters(pRGBSAMPact->hRGBCapture1,&DefCaptureParms,
									RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR != Error)
	{
		char text[80];
		wsprintf ( text, "RGBCaptureSetParameters (current) returned: %s", RGBErrorToString(Error) );
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return ;
	}
	UpdateData(FALSE);
}

/*****************************************************************************/
/*
 *	设置亮度为默认值
 */
void CAdjParamDlg::OnButtonBrightness() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("设置默认亮度失败 !\t","系统提示",MB_ICONWARNING | MB_OK);
		return ;
	}

	m_nBrightness = DefCaptureParms.Brightness;
	m_ctrlBrightness.SetPos(m_nBrightness);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_BRIGHTNESS;//设置标志位为指向相应参数的标志
	Error = RGBCaptureSetParameters(pRGBSAMPact->hRGBCapture1,&DefCaptureParms,
									RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR != Error)
	{
		char text[80];
		wsprintf ( text, "RGBCaptureSetParameters (current) returned: %s", RGBErrorToString(Error) );
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return ;
	}
	UpdateData(FALSE);
}

/*****************************************************************************/
/*
 *	设置对比度为默认值
 */
void CAdjParamDlg::OnButtonContrast() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("设置默认对比度失败 !\t","系统提示",MB_ICONWARNING | MB_OK);
		return ;
	}

	m_nContrast = DefCaptureParms.Contrast;
	m_ctrlContrast.SetPos(m_nContrast);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_CONTRAST;//设置标志位为指向相应参数的标志
	Error = RGBCaptureSetParameters(pRGBSAMPact->hRGBCapture1,&DefCaptureParms,
									RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR != Error)
	{
		char text[80];
		wsprintf ( text, "RGBCaptureSetParameters (current) returned: %s", RGBErrorToString(Error) );
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return ;
	}
	UpdateData(FALSE);
}

/*****************************************************************************/
/*
 *	设置水平位置为默认值
 */
void CAdjParamDlg::OnButtonHoroffset() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("设置默认水平位置失败 !\t","系统提示",MB_ICONWARNING | MB_OK);
		return ;
	}

	m_nHoroffset = DefCaptureParms.HorOffset;
	m_ctrlHoroffset.SetPos(m_nHoroffset);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_VDIF_HTIMINGS;//设置标志位为指向相应参数的标志
	Error = RGBCaptureSetParameters(pRGBSAMPact->hRGBCapture1,&DefCaptureParms,
									RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR != Error)
	{

		char text[80];
		wsprintf ( text, "RGBCaptureSetParameters (current) returned: %s", RGBErrorToString(Error) );
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return ;
	}
	UpdateData(FALSE);
}

/*****************************************************************************/
/*
 *	设置相位为默认值
 */
void CAdjParamDlg::OnButtonPhase() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("设置默认相位失败 !\t","系统提示",MB_ICONWARNING | MB_OK);
		return ;
	}

	m_nPhase = DefCaptureParms.Phase;
	m_ctrlPhase.SetPos(m_nPhase);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_PHASE;//设置标志位为指向相应参数的标志
	Error = RGBCaptureSetParameters(pRGBSAMPact->hRGBCapture1,&DefCaptureParms,
									RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR != Error)
	{

		char text[80];
		wsprintf ( text, "RGBCaptureSetParameters (current) returned: %s", RGBErrorToString(Error) );
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return ;
	}
	UpdateData(FALSE);
}

/*****************************************************************************/
/*
 *	设置帧率为默认值
 */
void CAdjParamDlg::OnButtonSamprate() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("设置默认帧率失败 !\t","系统提示",MB_ICONWARNING | MB_OK);
		return ;
	}

	m_nSamprate = DefCaptureParms.SampleRate;
	m_ctrlSamprate.SetPos(m_nSamprate);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_SAMPLERATE;//设置标志位为指向相应参数的标志
	Error = RGBCaptureSetParameters(pRGBSAMPact->hRGBCapture1,&DefCaptureParms,
									RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR != Error)
	{

		char text[80];
		wsprintf ( text, "RGBCaptureSetParameters (current) returned: %s", RGBErrorToString(Error) );
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return ;
	}
	UpdateData(FALSE);
}

/*****************************************************************************/
/*
 *	设置垂直位置为默认值
 */
void CAdjParamDlg::OnButtonVeroffset() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("设置默认垂直位置失败 !\t","系统提示",MB_ICONWARNING | MB_OK);
		return ;
	}
	
	m_nVeroffset = DefCaptureParms.VerOffset;
	m_ctrlVeroffset.SetPos(m_nVeroffset);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_VDIF_VTIMINGS;//设置标志位为指向相应参数的标志
	Error = RGBCaptureSetParameters(pRGBSAMPact->hRGBCapture1,&DefCaptureParms,
									RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR != Error)
	{

		char text[80];
		wsprintf ( text, "RGBCaptureSetParameters (current) returned: %s", RGBErrorToString(Error) );
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return ;
	}
	UpdateData(FALSE);
}

/*****************************************************************************/ 
/*
 *	根据当前活动滑块位置来改变指定参数的值
 */
void CAdjParamDlg::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar) 
{
	// TODO: Add your message handler code here and/or call default
	//////////////////////////////////////////////////////////////////////////
	/*获取当前的采集卡参数*/
	RGBERROR Error = RGBERROR_NO_ERROR;
	RGBCAPTUREPARMS AdjParameter;
	AdjParameter.Size = sizeof(RGBCAPTUREPARMS);
	AdjParameter.Flags = RGBCAPTURE_PARM_PHASE  |
						 RGBCAPTURE_PARM_BRIGHTNESS |
						 RGBCAPTURE_PARM_BLACKLEVEL |
						 RGBCAPTURE_PARM_VDIF_HTIMINGS |
						 RGBCAPTURE_PARM_VDIF_VTIMINGS |
						 RGBCAPTURE_PARM_SAMPLERATE |
						 RGBCAPTURE_PARM_CONTRAST;
	Error = RGBCaptureGetParameters(pRGBSAMPact->hRGBCapture1, &AdjParameter, 
							RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR !=Error)
	{
		switch(Error)
		{
			case RGBERROR_INVALID_HRGBCAPTURE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  获取默认的VGA采集事件的参数，获取失败（打开的设备队列中没有找到该硬件设备句柄）\n");//检测失败（打开的设备队列中没有找到该硬件设备句柄）
				break;
			}
			case RGBERROR_INVALID_POINTER:
			{
				TRACE("RGBCaptureUseMemoryBuffer(), 获取默认的VGA采集事件的参数，没有找到指向当前缓存存储区的指针\n");//没有找到参数指针
				break;
			}
			case RGBERROR_INVALID_FLAGS:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  获取默认的VGA采集事件的参数，不存在的参数标志\n");//没有找到参数指针
				break;
			}
			case RGBERROR_INVALID_SIZE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  获取默认的VGA采集事件的参数，参数大小有误\n");//没有找到参数指针
				break;
			}
			default:
			{
				break;
			}
		}
		char text[80];
		wsprintf ( text, "RGBCaptureGetParameters (default) returned: %s", RGBErrorToString(DError) );
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return;
	}
	//////////////////////////////////////////////////////////////////////////
	/*确定当前是哪个是活动滑块，确定滑块位置*/
	long	lPos = 0;  //存放当前滑块的位置
	if ( ( CScrollBar* )&m_ctrlBrightness == pScrollBar  )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_BRIGHTNESS;   //当前活动滑块指向亮度
		lPos = m_ctrlBrightness.GetPos();
		AdjParameter.Brightness = lPos;
		m_nBrightness = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlContrast == pScrollBar )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_CONTRAST;     //当前活动滑块指对比度
		lPos = m_ctrlContrast.GetPos();
		AdjParameter.Contrast = lPos;
		m_nContrast = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlPhase == pScrollBar )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_PHASE;        //当前活动滑块指向相位
		lPos = m_ctrlPhase.GetPos();
		AdjParameter.Phase = lPos;
		m_nPhase = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlHoroffset == pScrollBar )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_VDIF_HTIMINGS; //当前活动滑块指水平位置
		lPos = m_ctrlHoroffset.GetPos();
		AdjParameter.HorOffset = lPos;
		m_nHoroffset = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlVeroffset == pScrollBar )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_VDIF_VTIMINGS; //当前活动滑块指向垂直位置
		lPos = m_ctrlVeroffset.GetPos();
		AdjParameter.VerOffset = lPos;
		m_nVeroffset = lPos;
		
	}
	else if ( ( CScrollBar* )&m_ctrlBlacklevel == pScrollBar )
	{                                                        //当前活动滑块指向黑度
		AdjParameter.Flags = RGBCAPTURE_PARM_BLACKLEVEL; 
		lPos = m_ctrlVeroffset.GetPos();
		AdjParameter.VerOffset = lPos;
		m_nVeroffset = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlSamprate == pScrollBar )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_SAMPLERATE;      //当前活动滑块指向帧率
		lPos = m_ctrlSamprate.GetPos();
		AdjParameter.SampleRate = lPos;
		m_nSamprate = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlClampPlacement == pScrollBar )
	{
		   //当前活动滑块指向帧率
		lPos = m_ctrlClampPlacement.GetPos();
		if (!RGBCaptureSetClampPlacement(pRGBSAMPact->hRGBCapture1,lPos))
		{
			TRACE("CAdjParamDlg::OnHScroll :RGBCaptureSetClampPlacement error!!\n");
			return;
		}
		m_nClampPlacement = lPos;
		UpdateData(FALSE);
		CDialog::OnHScroll(nSBCode, nPos, pScrollBar);
		return;
	}

	else if ( ( CScrollBar* )&m_ctrlClampDuration == pScrollBar )
	{
		   //当前活动滑块指向帧率
		lPos = m_ctrlClampDuration.GetPos();
		if (!RGBCaptureSetClampDuration(pRGBSAMPact->hRGBCapture1,lPos))
		{
			TRACE("CAdjParamDlg::OnHScroll :RGBCaptureSetClampDuration error!!\n");
			return;
		}
		m_nClampDuration = lPos;
		UpdateData(FALSE);
		CDialog::OnHScroll(nSBCode, nPos, pScrollBar);
		return;
	}

	//////////////////////////////////////////////////////////////////////////
	/*重新设置采集卡相应改变了的参数*/
	Error = RGBCaptureSetParameters(pRGBSAMPact->hRGBCapture1,&AdjParameter,
									RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
	if (RGBERROR_NO_ERROR != Error)
	{
		char text[80];
		wsprintf ( text, "RGBCaptureSetParameters (current) returned: %s", RGBErrorToString(Error) );
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return ;
	}
	//////////////////////////////////////////////////////////////////////////
	UpdateData(FALSE);
	
	CDialog::OnHScroll(nSBCode, nPos, pScrollBar);
}


void CAdjParamDlg::OnButtonClampPlacement() 
{
	// TODO: Add your control notification handler code here
	m_nClampPlacement = 10;
	m_ctrlClampPlacement.SetPos(m_nClampPlacement);
	
	if (!RGBCaptureSetClampPlacement(pRGBSAMPact->hRGBCapture1,10))
	{

		char text[80];
		wsprintf ( text, "RGBCaptureSetClampPlacement () error!");
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return ;
	}
	UpdateData(FALSE);
}

void CAdjParamDlg::OnButtonClampDuration() 
{
	// TODO: Add your control notification handler code here
	m_nClampDuration = 10;
	m_ctrlClampDuration.SetPos(m_nClampDuration);
	
	if (!RGBCaptureSetClampDuration(pRGBSAMPact->hRGBCapture1,10))
	{

		char text[80];
		wsprintf ( text, "RGBCaptureSetClampDuration () error!");
		MessageBox (text, NULL, MB_OK | MB_ICONERROR );
        return ;
	}
	UpdateData(FALSE);
}

void CAdjParamDlg::OnDestroy() 
{
	CDialog::OnDestroy();
	pRGBSAMPact = NULL;
	// TODO: Add your message handler code here
	
}

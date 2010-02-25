// AdjParamDlg.cpp : implementation file
//
// *****************************************************************************
//  AdjParamDlg   VERSION:  1.1   DATE: 2008-10-15
//  ----------------------------------------------------------------------------
//		FileName: 	AdjParamDlg.cpp
//		Created:	2008-10-15   16:50
//		Author:		MengJuan
//		Purpose:	�ɼ�����������
//		Version:	
//					��V1.1 2008-10-15 mengjuan��
//						Initial
//					  ������CAdjParamDlg
//					  ��ʼ��OnInitDialog����					
//                    �����Ĭ��ֵ������ ɫ �ȣ�OnButtonBlacklevel����
//					  �����Ĭ��ֵ������    �ȣ�OnButtonBrightness����
//					  �����Ĭ��ֵ������ �� �ȣ�OnButtonContrast����
//					  �����Ĭ��ֵ����ˮƽλ�ã�OnButtonHoroffset����
//					  �����Ĭ��ֵ������    λ��OnButtonPhase����
//					  �����Ĭ��ֵ����֡    �ʣ�OnButtonSamprate����
//					  �����Ĭ��ֵ������ֱλ�ã�OnButtonVeroffset����
//                    ���黬��OnHScroll����
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
 *	����ȫ�ֱ���DefCaptureParms,���ڴ��Ĭ�ϲɼ�������
 */
RGBCAPTUREPARMS DefCaptureParms;
/*
 *	����ȫ�ֱ���DError��������Ŵ�����
 */
RGBERROR   DError = RGBERROR_NO_ERROR;
/////////////////////////////////////////////////////////////////////////////
// CAdjParamDlg dialog

/*
 *	����RGBErrorToString����
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
 *	��ʼ���Ի��򣬻�ȡ��ǰ��Ĭ�ϵ���Ƶ�ɼ�����
 */
BOOL CAdjParamDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	char Descript[128];
	/*
	*���ݵ�ǰѡ��Ĵ��ڱ�־ֵȷ�������Ĳɼ���
	*/
	wsprintf(Descript, "��������%s", "VGA1:");
	
	/*�޸ĶԻ������*/
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
	/*��ȡĬ�ϵĲɼ���������Ϊ����Ĳ�����׼��*/
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
				TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ�������ȡʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
				break;
			}

			case RGBERROR_INVALID_POINTER:
			{
				TRACE("RGBCaptureUseMemoryBuffer(), ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ�����û���ҵ�ָ��ǰ����洢����ָ��\n");//û���ҵ�����ָ��
				break;
			}

			case RGBERROR_INVALID_FLAGS:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ����������ڵĲ�����־\n");//û���ҵ�����ָ��
				break;
			}

			case RGBERROR_INVALID_SIZE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ�����������С����\n");//û���ҵ�����ָ��
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
	/*��ȡ��ǰ�Ĳɼ�����������������ֵ������Ӧ��EDIT����*/
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
				TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ�������ȡʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
				break;
			}

			case RGBERROR_INVALID_POINTER:
			{
				TRACE("RGBCaptureUseMemoryBuffer(), ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ�����û���ҵ�ָ��ǰ����洢����ָ��\n");//û���ҵ�����ָ��
				break;
			}

			case RGBERROR_INVALID_FLAGS:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ����������ڵĲ�����־\n");//û���ҵ�����ָ��
				break;
			}

			case RGBERROR_INVALID_SIZE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ�����������С����\n");//û���ҵ�����ָ��
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
	/*���û���Ļ�����Χ*/
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
 *	���ú�ɫ��ΪĬ��ֵ
 */
void CAdjParamDlg::OnButtonBlacklevel() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("����Ĭ�Ϻ�ɫ��ʧ�� !\t","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return ;
	}
	m_nBlacklevel = DefCaptureParms.BlackLevel;
	m_ctrlBlacklevel.SetPos(m_nBlacklevel);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_BLACKLEVEL;  //���ñ�־λΪָ����Ӧ�����ı�־
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
 *	��������ΪĬ��ֵ
 */
void CAdjParamDlg::OnButtonBrightness() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("����Ĭ������ʧ�� !\t","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return ;
	}

	m_nBrightness = DefCaptureParms.Brightness;
	m_ctrlBrightness.SetPos(m_nBrightness);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_BRIGHTNESS;//���ñ�־λΪָ����Ӧ�����ı�־
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
 *	���öԱȶ�ΪĬ��ֵ
 */
void CAdjParamDlg::OnButtonContrast() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("����Ĭ�϶Աȶ�ʧ�� !\t","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return ;
	}

	m_nContrast = DefCaptureParms.Contrast;
	m_ctrlContrast.SetPos(m_nContrast);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_CONTRAST;//���ñ�־λΪָ����Ӧ�����ı�־
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
 *	����ˮƽλ��ΪĬ��ֵ
 */
void CAdjParamDlg::OnButtonHoroffset() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("����Ĭ��ˮƽλ��ʧ�� !\t","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return ;
	}

	m_nHoroffset = DefCaptureParms.HorOffset;
	m_ctrlHoroffset.SetPos(m_nHoroffset);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_VDIF_HTIMINGS;//���ñ�־λΪָ����Ӧ�����ı�־
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
 *	������λΪĬ��ֵ
 */
void CAdjParamDlg::OnButtonPhase() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("����Ĭ����λʧ�� !\t","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return ;
	}

	m_nPhase = DefCaptureParms.Phase;
	m_ctrlPhase.SetPos(m_nPhase);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_PHASE;//���ñ�־λΪָ����Ӧ�����ı�־
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
 *	����֡��ΪĬ��ֵ
 */
void CAdjParamDlg::OnButtonSamprate() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("����Ĭ��֡��ʧ�� !\t","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return ;
	}

	m_nSamprate = DefCaptureParms.SampleRate;
	m_ctrlSamprate.SetPos(m_nSamprate);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_SAMPLERATE;//���ñ�־λΪָ����Ӧ�����ı�־
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
 *	���ô�ֱλ��ΪĬ��ֵ
 */
void CAdjParamDlg::OnButtonVeroffset() 
{
	// TODO: Add your control notification handler code here
	if (RGBERROR_NO_ERROR !=DError)
	{
		MessageBox("����Ĭ�ϴ�ֱλ��ʧ�� !\t","ϵͳ��ʾ",MB_ICONWARNING | MB_OK);
		return ;
	}
	
	m_nVeroffset = DefCaptureParms.VerOffset;
	m_ctrlVeroffset.SetPos(m_nVeroffset);
	RGBERROR   Error = RGBERROR_NO_ERROR;
	DefCaptureParms.Flags = RGBCAPTURE_PARM_VDIF_VTIMINGS;//���ñ�־λΪָ����Ӧ�����ı�־
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
 *	���ݵ�ǰ�����λ�����ı�ָ��������ֵ
 */
void CAdjParamDlg::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar) 
{
	// TODO: Add your message handler code here and/or call default
	//////////////////////////////////////////////////////////////////////////
	/*��ȡ��ǰ�Ĳɼ�������*/
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
				TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ�������ȡʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
				break;
			}
			case RGBERROR_INVALID_POINTER:
			{
				TRACE("RGBCaptureUseMemoryBuffer(), ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ�����û���ҵ�ָ��ǰ����洢����ָ��\n");//û���ҵ�����ָ��
				break;
			}
			case RGBERROR_INVALID_FLAGS:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ����������ڵĲ�����־\n");//û���ҵ�����ָ��
				break;
			}
			case RGBERROR_INVALID_SIZE:
			{
				TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡĬ�ϵ�VGA�ɼ��¼��Ĳ�����������С����\n");//û���ҵ�����ָ��
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
	/*ȷ����ǰ���ĸ��ǻ���飬ȷ������λ��*/
	long	lPos = 0;  //��ŵ�ǰ�����λ��
	if ( ( CScrollBar* )&m_ctrlBrightness == pScrollBar  )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_BRIGHTNESS;   //��ǰ�����ָ������
		lPos = m_ctrlBrightness.GetPos();
		AdjParameter.Brightness = lPos;
		m_nBrightness = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlContrast == pScrollBar )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_CONTRAST;     //��ǰ�����ָ�Աȶ�
		lPos = m_ctrlContrast.GetPos();
		AdjParameter.Contrast = lPos;
		m_nContrast = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlPhase == pScrollBar )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_PHASE;        //��ǰ�����ָ����λ
		lPos = m_ctrlPhase.GetPos();
		AdjParameter.Phase = lPos;
		m_nPhase = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlHoroffset == pScrollBar )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_VDIF_HTIMINGS; //��ǰ�����ָˮƽλ��
		lPos = m_ctrlHoroffset.GetPos();
		AdjParameter.HorOffset = lPos;
		m_nHoroffset = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlVeroffset == pScrollBar )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_VDIF_VTIMINGS; //��ǰ�����ָ��ֱλ��
		lPos = m_ctrlVeroffset.GetPos();
		AdjParameter.VerOffset = lPos;
		m_nVeroffset = lPos;
		
	}
	else if ( ( CScrollBar* )&m_ctrlBlacklevel == pScrollBar )
	{                                                        //��ǰ�����ָ��ڶ�
		AdjParameter.Flags = RGBCAPTURE_PARM_BLACKLEVEL; 
		lPos = m_ctrlVeroffset.GetPos();
		AdjParameter.VerOffset = lPos;
		m_nVeroffset = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlSamprate == pScrollBar )
	{
		AdjParameter.Flags = RGBCAPTURE_PARM_SAMPLERATE;      //��ǰ�����ָ��֡��
		lPos = m_ctrlSamprate.GetPos();
		AdjParameter.SampleRate = lPos;
		m_nSamprate = lPos;
	}
	else if ( ( CScrollBar* )&m_ctrlClampPlacement == pScrollBar )
	{
		   //��ǰ�����ָ��֡��
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
		   //��ǰ�����ָ��֡��
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
	/*�������òɼ�����Ӧ�ı��˵Ĳ���*/
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

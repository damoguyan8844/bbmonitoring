// QualPropDlg.cpp : implementation file
//
// *****************************************************************************
//  QualPropDlg   VERSION:  1.1   DATE: 2008-10-15
//  ----------------------------------------------------------------------------
//		FileName: 	QualPropDlg.cpp
//		Created:	2008-10-15   16:50
//		Author:		MengJuan
//		Purpose:	��ʾ��ǰ�ɼ�ͼ����������
//		Version:	
//					��V1.1 2008-10-15 mengjuan��
//						Initial
//					  ������CAdjParamDlg
//					  ��ʼ��OnInitDialog����					
//                    �ر�OnDestroy����
//					  ��ʱOnTimer����
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
 *����RGBErrorToString����
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
 *�Ի����ʼ�����趨ʱ��
 */
BOOL CQualPropDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	
	SetTimer(TALENT_TIMER_QUALPROP,1000,NULL);  //���ö�ʱʱ��Ϊ1000����

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

/*****************************************************************************/
/*
 *�رնԻ���
 */
void CQualPropDlg::OnDestroy() 
{
	CDialog::OnDestroy();
	
	// TODO: Add your message handler code here
	if (pRGBSAMPact->startcount1)
	{
		pRGBSAMPact->startcount1 = FALSE;	//���ڹرպ󽫼�¼֡����־�ر�
	}
	pRGBSAMPact = NULL;
	KillTimer(TALENT_TIMER_QUALPROP);
	
}

/*****************************************************************************/
/*
 *�趨ʱ�䵽ʱ��ȡ��ʱ�Ĳɼ�ͼ����������
 */
void CQualPropDlg::OnTimer(UINT nIDEvent) 

{
	// TODO: Add your message handler code here and/or call default

	if (TALENT_TIMER_QUALPROP == nIDEvent)
	{
		//////////////////////////////////////////////////////////////////////////
		/*��ȡ��ǰ�Ĳɼ�����*/
		char Descript[128];
		wsprintf(Descript, "��������%s", "VGA:");

		/*�жϵ�ǰ�Ƿ�ʼ��¼�������û��������־*/
		if (!pRGBSAMPact->startcount1)
		{
			pRGBSAMPact->startcount1 = TRUE;
		}
		/*�޸ĶԻ������*/
		::SetWindowText(m_hWnd, Descript);
		/*��ȡ��ǰ�ɼ����Ĳ���*/
		RGBCAPTUREPARMS CurCaptureParms;
		RGBERROR   Error = RGBERROR_NO_ERROR;
		CurCaptureParms.Size = sizeof(RGBCAPTUREPARMS);
		CurCaptureParms.Flags =RGBCAPTURE_PARM_ALL;  //������ȡ��ǰ�ɼ�����ȫ������ֵ
		Error = RGBCaptureGetParameters(pRGBSAMPact->hRGBCapture1, &CurCaptureParms, 
								RGBCAPTURE_FLAG_CURRENT | RGBCAPTURE_FLAG_REAL_WORLD);
		if (RGBERROR_NO_ERROR !=Error)
		{
			switch (Error)
			{
				case RGBERROR_INVALID_HRGBCAPTURE:
				{
					TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡ��ǰ��VGA�ɼ��¼��Ĳ�������ȡʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����\n");//���ʧ�ܣ��򿪵��豸������û���ҵ���Ӳ���豸�����
					return;
					break;
				}

				case RGBERROR_INVALID_POINTER:
				{
					TRACE("RGBCaptureUseMemoryBuffer(), ��ȡ��ǰ��VGA�ɼ��¼��Ĳ�����û���ҵ�ָ��ǰ����洢����ָ��\n");//û���ҵ�����ָ��
					return;
					break;
				}

				case RGBERROR_INVALID_FLAGS:
				{
					TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡ��ǰ��VGA�ɼ��¼��Ĳ����������ڵĲ�����־\n");//û���ҵ�����ָ��
					return;
					break;
				}

				case RGBERROR_INVALID_SIZE:
				{
					TRACE("RGBCaptureUseMemoryBuffer(),  ��ȡ��ǰ��VGA�ɼ��¼��Ĳ�����������С����\n");//û���ҵ�����ָ��
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
		/*���ɼ����������Ի������Ӧ����*/
		m_nHScaled = CurCaptureParms.HScaled;  //ˮƽ�ֱ���
		m_nVScaled = CurCaptureParms.VScaled;  //��ֱ�ֱ���
		m_nRefreshRate = CurCaptureParms.VideoTimings.VerFrequency;
		m_fAvgFrameRate = (float)(pRGBSAMPact->g_framecount1);   //ÿ��֡������ǰһ���֡����ȥ��ǰ��֡���õ�ÿ��֡��
		
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

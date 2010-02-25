// *****************************************************************************
//  AdjParamDlg   VERSION:  1.1   DATE: 2008-10-15
//  ----------------------------------------------------------------------------
//		FileName: 	AdjParamDlg.h
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


#if !defined(AFX_ADJPARAMDLG_H__D0F50F00_B137_4795_93B4_99C6524C6F27__INCLUDED_)
#define AFX_ADJPARAMDLG_H__D0F50F00_B137_4795_93B4_99C6524C6F27__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// AdjParamDlg.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CAdjParamDlg dialog

class CAdjParamDlg : public CDialog
{
// Construction
public:
	CAdjParamDlg(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CAdjParamDlg)
	enum { IDD = IDD_PARAM };
	CSliderCtrl	m_ctrlClampDuration;
	CSliderCtrl	m_ctrlClampPlacement;
	CSliderCtrl	m_ctrlVeroffset;
	CSliderCtrl	m_ctrlHoroffset;
	CSliderCtrl	m_ctrlSamprate;
	CSliderCtrl	m_ctrlPhase;
	CSliderCtrl	m_ctrlContrast;
	CSliderCtrl	m_ctrlBrightness;
	CSliderCtrl	m_ctrlBlacklevel;
	int		m_nBlacklevel;
	int		m_nBrightness;
	int		m_nContrast;
	int		m_nPhase;
	int		m_nSamprate;
	int		m_nVeroffset;
	int		m_nHoroffset;
	int		m_nClampDuration;
	int		m_nClampPlacement;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAdjParamDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CAdjParamDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnButtonBlacklevel();
	afx_msg void OnButtonBrightness();
	afx_msg void OnButtonContrast();
	afx_msg void OnButtonHoroffset();
	afx_msg void OnButtonPhase();
	afx_msg void OnButtonSamprate();
	afx_msg void OnButtonVeroffset();
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnButtonClampPlacement();
	afx_msg void OnButtonClampDuration();
	afx_msg void OnDestroy();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ADJPARAMDLG_H__D0F50F00_B137_4795_93B4_99C6524C6F27__INCLUDED_)

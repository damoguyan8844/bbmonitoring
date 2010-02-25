
// *****************************************************************************
//  QualPropDlg   VERSION:  1.1   DATE: 2008-10-15
//  ----------------------------------------------------------------------------
//		FileName: 	QualPropDlg.h
//		Created:	2008-10-15   16:50
//		Author:		MengJuan
//		Purpose:	显示当前采集图象质量参数
//		Version:	
//					【V1.1 2008-10-15 mengjuan】
//						Initial
//					  创建类CQualPropDlg
//					  初始化OnInitDialog方法					
//                    关闭OnDestroy方法
//					  定时OnTimer方法
//		Remark:		N/A
//  ----------------------------------------------------------------------------
//  Copyright (C) 2008 
//	Nanjing Talent Electronics and Technology Co., LTD.
//  All Rights Reserved
// *****************************************************************************

#if !defined(AFX_QUALPROPDLG_H__B87190E9_B205_49CC_9A27_7DB364BB2265__INCLUDED_)
#define AFX_QUALPROPDLG_H__B87190E9_B205_49CC_9A27_7DB364BB2265__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// QualPropDlg.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CQualPropDlg dialog

#define		TALENT_TIMER_QUALPROP		0
class CQualPropDlg : public CDialog
{
// Construction
public:
	CQualPropDlg(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CQualPropDlg)
	enum { IDD = IDD_QUALPROP };
	float	m_fAvgFrameRate;
	int		m_nFrameDraw;
	int		m_nHScaled;
	int		m_nVScaled;
	CString	m_nDataMode;
	int		m_nRefreshRate;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CQualPropDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CQualPropDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnDestroy();
	afx_msg void OnTimer(UINT nIDEvent);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_QUALPROPDLG_H__B87190E9_B205_49CC_9A27_7DB364BB2265__INCLUDED_)

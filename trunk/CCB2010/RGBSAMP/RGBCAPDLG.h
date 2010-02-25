#if !defined(AFX_RGBCAPDLG_H__91D2DA63_C26B_4F41_AA0F_C1148244BD5D__INCLUDED_)
#define AFX_RGBCAPDLG_H__91D2DA63_C26B_4F41_AA0F_C1148244BD5D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// RGBCAPDLG.h : header file
//
#include "RGBSAMPDlg.h"
/////////////////////////////////////////////////////////////////////////////
// CRGBCAPDLG dialog

class CRGBCAPDLG : public CDialog
{
// Construction
public:
	CRGBCAPDLG(CWnd* pParent = NULL);   // standard constructor
	~CRGBCAPDLG();
// Dialog Data
	//{{AFX_DATA(CRGBCAPDLG)
	enum { IDD = IDD_DIALOG1 };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRGBCAPDLG)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//virtual void OnCancel();
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CRGBCAPDLG)
	virtual void OnSysCommand(UINT nID, LPARAM lParam);
	virtual BOOL OnInitDialog();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnChildMsg_MaxSize(WPARAM wParam, LPARAM lParam);
	afx_msg void OnChildMsg_QualPara(WPARAM wParam, LPARAM lParam);
	afx_msg void OnChildMsg_QualAdjust(WPARAM wParam, LPARAM lParam);
	afx_msg void OnClose();
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnPaint();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
public:

	CRGBSAMPDlg * m_pRGBSAMP1;
	CRGBSAMPDlg * m_pRGBSAMP2;

	RECT m_client;
	RECT m_screen;
	BOOL bmaxmize;

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_RGBCAPDLG_H__91D2DA63_C26B_4F41_AA0F_C1148244BD5D__INCLUDED_)

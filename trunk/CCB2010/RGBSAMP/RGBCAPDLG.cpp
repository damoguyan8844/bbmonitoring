// RGBCAPDLG.cpp : implementation file
//

#include "stdafx.h"
#include "RGBSAMP.h"
#include "RGBCAPDLG.h"
#include "RGBSAMPDlg.h"
#include "QualPropDlg.h"
#include "AdjParamDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CRGBSAMPDlg * pRGBSAMPact = NULL;

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	//{{AFX_MSG(CAboutDlg)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}
void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// No message handlers
		
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRGBCAPDLG dialog


CRGBCAPDLG::CRGBCAPDLG(CWnd* pParent /*=NULL*/)
	: CDialog(CRGBCAPDLG::IDD, pParent)
{
	//{{AFX_DATA_INIT(CRGBCAPDLG)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);

}
CRGBCAPDLG::~CRGBCAPDLG()
{
	delete m_pRGBSAMP1;
	delete m_pRGBSAMP2;
}
void CRGBCAPDLG::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CRGBCAPDLG)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CRGBCAPDLG, CDialog)
	//{{AFX_MSG_MAP(CRGBCAPDLG)
	ON_WM_SYSCOMMAND()
	ON_WM_SIZE()
	ON_MESSAGE(RGBSAMPWM_MAXSIZE, OnChildMsg_MaxSize)
	ON_MESSAGE(RGBSAMPWM_QUALPARA, OnChildMsg_QualPara)
	ON_MESSAGE(RGBSAMPWM_QUALADJUST, OnChildMsg_QualAdjust)
	ON_WM_CLOSE()
	ON_WM_CREATE()
	ON_WM_PAINT()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRGBCAPDLG message handlers

BOOL CRGBCAPDLG::OnInitDialog() 
{
	CDialog::OnInitDialog();
	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu *pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	// TODO: Add extra initialization here
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}


void CRGBCAPDLG::OnSize(UINT nType, int cx, int cy) 
{
	CDialog::OnSize(nType, cx, cy);
	
	// TODO: Add your message handler code here
	if (!bmaxmize)
	{
		m_pRGBSAMP1->MoveWindow(0, 0, cx/2, cy, TRUE);
		m_pRGBSAMP2->MoveWindow(cx/2, 0, cx/2, cy, TRUE);
	}
}
void CRGBCAPDLG::OnChildMsg_MaxSize(WPARAM wParam, LPARAM lParam)
{
	//CRGBSAMPDlg * pRGBSamp = (CRGBSAMPDlg *) lParam;
	// TODO: Add your message handler code here and/or call default
//	RECT rect; //存放主窗口的尺寸
	
	//this->RedrawWindow();
//RECT client;
	//////////////////////////////////////////////////////////////////////////
	//根据当前活动窗口参数采取不同的措施// 
	if (!bmaxmize)
	{
		bmaxmize = TRUE;
		ModifyStyle(WS_CAPTION,0,0);
		this->GetClientRect(&m_client);  //保存主窗口最大化之前的尺寸
		this->GetWindowRect(&m_screen);
		
		
		int   nFullWidth=GetSystemMetrics(SM_CXSCREEN);   
		int   nFullHeight=GetSystemMetrics(SM_CYSCREEN);

		
		this->MoveWindow(0, 0, nFullWidth, nFullHeight,TRUE);
		switch(lParam)
		{
			case 1:
			{
				m_pRGBSAMP1->MoveWindow(-3, -3, nFullWidth, nFullHeight,TRUE);
				m_pRGBSAMP2->ShowWindow(SW_HIDE);
				pRGBSAMPact = m_pRGBSAMP1;
				break;
			}
			case 2:
		   	{
				m_pRGBSAMP2->MoveWindow(-3, -3, nFullWidth, nFullHeight,TRUE);
				m_pRGBSAMP1->ShowWindow(SW_HIDE);
				pRGBSAMPact = m_pRGBSAMP2;
				break;
			}
			default:
				break;
		}
			
	}		
	else
	{
		bmaxmize = FALSE;
		ModifyStyle(0,WS_CAPTION,0);
		this->MoveWindow(m_screen.left, m_screen.top, m_client.right+6, m_client.bottom+6, TRUE);
		this->GetClientRect(&m_client);
		switch(lParam)
		{
			case 1:
			{
				m_pRGBSAMP1->MoveWindow(0, 0, m_client.right/2, m_client.bottom, TRUE);
				m_pRGBSAMP2->ShowWindow(SW_SHOW);
				break;
			}
			case 2:
			{
				m_pRGBSAMP2->MoveWindow(m_client.right/2, 0, m_client.right/2, m_client.bottom, TRUE);
				m_pRGBSAMP1->ShowWindow(SW_SHOW);
				break;
			}
			default:
				break;
		}
		pRGBSAMPact = NULL;
		
	}
	RedrawWindow();
}

void CRGBCAPDLG::OnChildMsg_QualPara(WPARAM wParam, LPARAM lParam)
{
	pRGBSAMPact = (CRGBSAMPDlg*)lParam;
	CQualPropDlg DlgQual(this);
	DlgQual.DoModal();
}
void CRGBCAPDLG::OnChildMsg_QualAdjust(WPARAM wParam, LPARAM lParam)
{
	pRGBSAMPact = (CRGBSAMPDlg*)lParam;
	CAdjParamDlg DlgQual(this);
	DlgQual.DoModal();
}


void CRGBCAPDLG::OnClose() 
{
	// TODO: Add your message handler code here and/or call default
//	if ( MessageBox("确定要退出系统吗 ?\t","系统提示",MB_ICONQUESTION | MB_OKCANCEL ) != IDOK )
//	{
//		return ;
//	}
	CDialog::OnClose();
	//this->DestroyWindow();
}

int CRGBCAPDLG::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	if (CDialog::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	// TODO: Add your specialized creation code here
		//CRGBSAMPDlg * pRGBSAMP;
	
//	MessageBox("CRGBCAPDLG::OnCreate","Info",MB_ICONINFORMATION|MB_OK);
	m_pRGBSAMP1 = new CRGBSAMPDlg(this);
	m_pRGBSAMP1->Create(IDD_RGBSAMP_DIALOG, this);
	m_pRGBSAMP1->m_capnum = 1;
	m_pRGBSAMP1->m_iIndex = 0;

	m_pRGBSAMP1->OnRunVGA1();	
//	MessageBox("CRGBCAPDLG::OnCreate 2","Info",MB_ICONINFORMATION|MB_OK);

	//CRGBSAMPDlg * pRGBSAMP2;
	m_pRGBSAMP2 = new CRGBSAMPDlg(this);
	m_pRGBSAMP2->Create(IDD_RGBSAMP_DIALOG, this);
	m_pRGBSAMP2->m_capnum = 2;
	m_pRGBSAMP2->m_iIndex = 1;

	m_pRGBSAMP1->OnRunVGA1();

// 	RECT MainRect;
//     this->GetClientRect(&MainRect);
// 	m_pRGBSAMP1->MoveWindow(0, 0, MainRect.right/2, MainRect.bottom, TRUE);
// 	m_pRGBSAMP1->MoveWindow(0, 0, MainRect.right/2, MainRect.bottom, TRUE);
// 	m_pRGBSAMP2->MoveWindow(MainRect.right/2, 0, MainRect.right/2, MainRect.bottom, TRUE);
// 	m_pRGBSAMP2->MoveWindow(MainRect.right/2, 0, MainRect.right/2, MainRect.bottom, TRUE);
//     m_pRGBSAMP1->GetWindowRect(&MainRect);
// 	m_pRGBSAMP1->m_static1.GetWindowRect(&MainRect);
// 	m_pRGBSAMP2->GetWindowRect(&MainRect);
// 	m_pRGBSAMP2->m_static1.GetWindowRect(&MainRect);
// 	this->GetWindowRect(&MainRect);
	bmaxmize = FALSE; //初始化当前子窗口的状态

	
	return 0;
}
// void CRGBCAPDLG::OnCancel()
// {
// 
//}
void CRGBCAPDLG::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
}

void CRGBCAPDLG::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	CDialog::OnPaint();	
}
// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CRGBCAPDLG::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

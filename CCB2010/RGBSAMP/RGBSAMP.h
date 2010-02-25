// RGBSAMP.h : main header file for the RGBSAMP application
//
// *****************************************************************************
//  UserInfoManage   VERSION:  1.0   DATE: 2008-07-11
//  ----------------------------------------------------------------------------
//		FileName: 	UserInfoManage.h
//		Created:	2008-07-11   16:50
//		Author:		MengJuan
//		Purpose:	用户信息管理文档
//		Version:	
//					【V1.0 2008-07-11 mengjuan】
//						Initial
//                    创建结构SUserInfo用于存放用户基本信息
//					  创建类CUserInfoManage
//					  添加用户adduser方法					
//                    插入用户Insertuser方法
//                    根据用户id号查找用户信息searchbyid方法
//					  根据用户id删除用户信息removebyid方法
//					  返回链表信息returnqueue方法
//		Remark:		N/A
//  ----------------------------------------------------------------------------
//  Copyright (C) 2008 
//	Nanjing Talent Electronics and Technology Co., LTD.
//  All Rights Reserved
// *****************************************************************************


#if !defined(AFX_RGBSAMP_H__E0F51F7F_B140_40DB_A681_A2B951CB21B6__INCLUDED_)
#define AFX_RGBSAMP_H__E0F51F7F_B140_40DB_A681_A2B951CB21B6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"		// main symbols

/////////////////////////////////////////////////////////////////////////////
// CRGBSAMPApp:
// See RGBSAMP.cpp for the implementation of this class
//

class CRGBSAMPApp : public CWinApp
{
public:
	CRGBSAMPApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRGBSAMPApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CRGBSAMPApp)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_RGBSAMP_H__E0F51F7F_B140_40DB_A681_A2B951CB21B6__INCLUDED_)

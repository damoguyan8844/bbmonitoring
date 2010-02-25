// DDrawRGB32.h: interface for the CDDrawRGB32 class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_DDRAWRGB32_H__D7B51146_20B2_43B1_BA62_CB17EF9F648E__INCLUDED_)
#define AFX_DDRAWRGB32_H__D7B51146_20B2_43B1_BA62_CB17EF9F648E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
#include <ddraw.h>

class CDDrawRGB32  
{
public:
	CDDrawRGB32();
	virtual ~CDDrawRGB32();
public:
	HRESULT DDInit(HWND hWnd, DWORD width, DWORD height);
	HRESULT DDDraw(HWND hWnd, BYTE * pInputData, DWORD length, RECT &rct, DWORD width, DWORD height);
	HRESULT DDRelease();

private:
	//directdraw
	LPDIRECTDRAW m_lpdd;
	LPDIRECTDRAWSURFACE m_lpddsPrimary;
	LPDIRECTDRAWSURFACE m_lpddsBack;
	LPDIRECTDRAWCLIPPER m_lpddClipper;
};

#endif // !defined(AFX_DDRAWRGB32_H__D7B51146_20B2_43B1_BA62_CB17EF9F648E__INCLUDED_)

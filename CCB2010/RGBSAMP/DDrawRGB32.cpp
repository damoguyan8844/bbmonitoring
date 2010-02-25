// DDrawRGB32.cpp: implementation of the CDDrawRGB32 class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "RGBSAMP.h"
#include "DDrawRGB32.h"


#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CDDrawRGB32::CDDrawRGB32()
{
	m_lpdd = NULL;
	m_lpddClipper = NULL;
	m_lpddsPrimary = NULL;
	m_lpddsBack = NULL;
}

CDDrawRGB32::~CDDrawRGB32()
{

}

HRESULT CDDrawRGB32::DDInit(HWND hWnd, DWORD width, DWORD height)
{
	if (hWnd == NULL)
	{
		TRACE("CDDrawRGB32::DDInit failed!!!hWnd == NULL\n");
		return E_FAIL;
	}

	if ( !::IsWindow(hWnd) )
	{
		TRACE("CDDrawRGB32::DDInit failed!!!IsWindow(hWnd)\n");
		return E_FAIL;
	}

	HRESULT hr = DD_OK;

	hr = DirectDrawCreate(NULL, &m_lpdd, NULL);
	if(FAILED(hr))
	{
		TRACE("CDDrawRGB32::DDInit failed!!!DirectDrawCreate\n");
		return hr;
	}

	m_lpdd->SetCooperativeLevel(hWnd,DDSCL_NORMAL);
	if(FAILED(hr))
	{
		TRACE("CDDrawRGB32::DDInit failed!!!SetCooperativeLevel\n");
		m_lpdd->Release();
		m_lpdd = NULL; 
		return hr;
	}

	/*
	 * Create the primary surface
	 */
	DDSURFACEDESC ddsd;
	ZeroMemory(&ddsd, sizeof(ddsd));
	ddsd.dwSize            = sizeof( ddsd );

	ddsd.dwFlags           = DDSD_CAPS;
	ddsd.ddsCaps.dwCaps    = DDSCAPS_PRIMARYSURFACE | DDSCAPS_VIDEOMEMORY;

	hr = m_lpdd->CreateSurface(&ddsd, &m_lpddsPrimary, NULL);
	if(FAILED(hr))
	{
		TRACE("CDDrawRGB32::DDInit failed!!!CreateSurface\n");
		m_lpdd->Release();
		m_lpdd = NULL;
		return hr;
	}


	/*
	 * Now set Clipping
	 */
	hr = m_lpdd->CreateClipper(0, &m_lpddClipper, NULL);
	if(FAILED(hr)) 
	{
		TRACE("CDDrawRGB32::DDInit failed!!!CreateClipper\n");
		m_lpddsPrimary->Release();
		m_lpddsPrimary = NULL;
		m_lpdd->Release();
		m_lpdd = NULL;

		return hr;
	}

	hr = m_lpddClipper->SetHWnd(0, hWnd);
	if(FAILED(hr)) 
	{
		TRACE("CDDrawRGB32::DDInit failed!!!SetHWnd\n");
		m_lpddsPrimary->Release();
		m_lpddsPrimary = NULL;

		m_lpddClipper->Release();
		m_lpddClipper = NULL;
		
		m_lpdd->Release();
		m_lpdd = NULL;

		return hr;
	}

	hr = m_lpddsPrimary->SetClipper(m_lpddClipper);
	if(FAILED(hr)) 
	{
		TRACE("CDDrawRGB32::DDInit failed!!!SetClipper\n");
		m_lpddsPrimary->Release();
		m_lpddsPrimary = NULL;
		
		m_lpddClipper->Release();
		m_lpddClipper = NULL;

		m_lpdd->Release();
		m_lpdd = NULL;

		return hr;
	}

	/*
	 * Finally Create Back Surface
	 */
	ZeroMemory(&ddsd, sizeof(ddsd));
    ddsd.dwSize     = sizeof(ddsd);

	/* for overly */
	DDPIXELFORMAT ddpFormat = { sizeof(DDPIXELFORMAT), DDPF_RGB, 0, 32, 0xff0000, 0xff00};

	ddsd.dwFlags        = DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT;
	ddsd.ddsCaps.dwCaps = DDSCAPS_VIDEOMEMORY | DDSCAPS_OFFSCREENPLAIN;
	
	ddsd.dwWidth  = width;
	ddsd.dwHeight = height;
	ddsd.ddpfPixelFormat = ddpFormat;

	hr = m_lpdd->CreateSurface(&ddsd, &m_lpddsBack, NULL);
	if(FAILED(hr))
	{
		TRACE("CDDrawRGB32::DDInit failed!!!CreateSurface\n");
		m_lpddsPrimary->Release();
		m_lpddsPrimary = NULL;

		m_lpddClipper->Release();
		m_lpddClipper = NULL;

		m_lpdd->Release();
		m_lpdd = NULL;

		return hr;
	}
	TRACE("CDDrawRGB32::DDInit !!!CreateSurface m_lpddsBack\n");

	return DD_OK;
}
#define  DD_FALSE S_FALSE;

#define AlphaRGB(a,r,g,b)          ( ((r)<<16) + ((g)<<8) + (b) )
HRESULT CDDrawRGB32::DDDraw(HWND hWnd,  BYTE * pInputData, DWORD length, RECT &rct, DWORD width, DWORD height)
{
	if ( pInputData == NULL || length <= 0 )
	{
		TRACE("CDDrawRGB32::DDDraw failed!!!pInputData length\n");
		return DD_FALSE;
	}

	HRESULT hr = DD_OK;
	DDSURFACEDESC ddsd;
	ZeroMemory(&ddsd, sizeof(ddsd));
	ddsd.dwSize     = sizeof(ddsd);


	RECT rctSrc = { 0, 0, width, height };

	if (NULL == m_lpddsBack)
	{
		TRACE("m_lpddsBack = null!\n");
		return DD_FALSE;
	}
	hr = m_lpddsBack->Lock(NULL, &ddsd, DDLOCK_SURFACEMEMORYPTR | DDLOCK_WAIT, NULL);
	if (FAILED(hr))
	{
		TRACE("CDDrawRGB32::DDDraw failed!!!Lock\n");
		return hr;
	}
	
	DWORD i,j;
	DWORD* videoBuf = (DWORD*)ddsd.lpSurface;
	DWORD wPixels = (DWORD)( ddsd.lPitch >> 2 );
	for (i=0; i< height; i++)
	{
		for (j=0; j<width; j++)
		{
			videoBuf[j] = AlphaRGB(pInputData[3], pInputData[2], pInputData[1], pInputData[0]);
			pInputData += 4;
		}
		videoBuf += wPixels;
	}

	hr = m_lpddsBack->Unlock(NULL);
	if (FAILED(hr))
	{
		TRACE("CDDrawRGB32::DDDraw failed!!!Unlock\n");
		return hr;
	}

	hr = m_lpddsPrimary->Blt(&rct, m_lpddsBack, &rctSrc, DDBLT_ASYNC | DDBLT_WAIT, NULL);
	if (FAILED(hr))
	{
		TRACE("CDDrawRGB32::DDDraw failed!!!Blt\n");
		return hr;
	}

	return DD_OK;

}

HRESULT CDDrawRGB32::DDRelease()
{
	HRESULT hr = DD_OK;


	if(m_lpddsBack) {
		m_lpddsBack->Release();
		m_lpddsBack = NULL;
	}

	if(m_lpddClipper) {
		m_lpddClipper->Release();
		m_lpddClipper = NULL;
	}

	if(m_lpddsPrimary) {
		m_lpddsPrimary->Release();
		m_lpddsPrimary = NULL;
	}

	if(m_lpdd) {
		m_lpdd->Release();
		m_lpdd = NULL;
	}
	return DD_OK;
}


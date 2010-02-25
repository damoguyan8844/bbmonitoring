// TCAPI.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "./include/rgbdefs.h"
#include "./include/TC1000API.h"

#include "TCAPI.h"
#include "RGBSAMPDlg.h"
#include "ColorSpace.h"

#include <string>

using namespace std;

#define CLASS_NAME "TCCardMessageCapturer"
#define WINDOW_TITLE "TC Card Message Capturer"

HWND g_HWndCapture=0;
string g_WorkFolder;
fun_NewCapture g_NewCapture=0;
fun_Logger g_Logger=0;
CRGBSAMPDlg *g_DummyDlg=0; //第一个采集卡
CRGBSAMPDlg *g_DummyDlg2=0; //第二个采集卡

long g_VGACAP_Framerate=100;

CRITICAL_SECTION _cs;
string g_latestCapture;
volatile long g_latestCaptureIndex=0;

LPVOID g_Capture_Buffer1=0;
int g_Capture_Buffer1_Width=0;
int g_Capture_Buffer1_Height=0;


LPVOID g_Capture_Buffer2=0;
int g_Capture_Buffer2_Width=0;
int g_Capture_Buffer2_Height=0;

volatile bool   g_Capture_Buffer_UseAble=false;
volatile bool   g_Capture_Use_Buffer1=false;
volatile int	g_Capture_Index=0;


BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
    switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
			::InitializeCriticalSection(&_cs);
			break;
		case DLL_THREAD_ATTACH:
			break;
		case DLL_THREAD_DETACH:
			break;
		case DLL_PROCESS_DETACH:
			::DeleteCriticalSection(&_cs);
			ReleaseCard();
			break;
    }
    return TRUE;
}
/************************************************************************/
// 	ON_COMMAND(ID_Run_VGA1, OnRunVGA1)
// 	ON_MESSAGE(RGBWM_FRAMECAPTURED, OnMyMessage_Sta1)
// 	ON_COMMAND(ID_Stop_VGA1, OnStopVGA1)
// 	ON_WM_CREATE()
// 	ON_WM_CTLCOLOR()
// 	ON_COMMAND(ID_ADJUST_VGA1, OnAdjustVga1)
// 	ON_MESSAGE(RGBWM_MODECHANGED, OnMyMessage_modechange)
// 	ON_COMMAND(ID_PAUSE_VGA1, OnPauseVga1)
// 	ON_COMMAND(ID_GOON_VGA1, OnGoonVga1)
// 	ON_WM_LBUTTONDBLCLK()
// 	ON_COMMAND(ID_QUALPROP_VGA1, OnQualpropVga1)
// 	ON_MESSAGE(RGBWM_NOSIGNAL, OnMyMessage_nosignal)
// 	ON_MESSAGE(RGBWM_DATAMODE, OnMyMessage_datamode)
// 	ON_MESSAGE(RGBWM_DDRERROR, OnMyMessage_ddrerror)
/************************************************************************/
LRESULT WINAPI CapturerWindowsProc(HWND hWnd,UINT message , WPARAM wParam , LPARAM lParam)
{
//	char temp[256];
//	sprintf(temp,"hWnd=%u,message=%u,hWnd2=%u",hWnd,message,g_DummyDlg!=0? g_DummyDlg->m_hWnd: 0);
//	LogMessage(temp);
	if(g_DummyDlg!=0 && hWnd== g_DummyDlg->m_hWnd)
	{
		switch(message)
		{
			case RGBWM_FRAMECAPTURED:
			{
				g_DummyDlg->OnMyMessage_Sta1(wParam,lParam);
				return S_OK;
			}
			case RGBWM_MODECHANGED:
			{
				g_DummyDlg->OnMyMessage_modechange(wParam,lParam);
				return S_OK;
			}
			case RGBWM_NOSIGNAL:
			{
				g_DummyDlg->OnMyMessage_nosignal(wParam,lParam);
				return S_OK;
			}	
			case RGBWM_DATAMODE:
			{
				return g_DummyDlg->OnMyMessage_datamode(wParam,lParam);
			}
			case RGBWM_DDRERROR:
			{
				g_DummyDlg->OnMyMessage_ddrerror(wParam,lParam);
				return S_OK;
			}
		}
	}
	return DefWindowProc(hWnd, message, wParam, lParam); 
}

TCAPI_API int InitCard(long VGACAP_Framerate)
{
	try
	{
		g_VGACAP_Framerate=VGACAP_Framerate;
		
		WNDCLASSEX   wcex;   
		wcex.cbSize = sizeof(WNDCLASSEX);     
		wcex.style  =  0 | SW_HIDE ;   
		wcex.lpfnWndProc = (WNDPROC)CapturerWindowsProc;   
		wcex.cbClsExtra  =   0;   
		wcex.cbWndExtra  =   0;   
		wcex.hInstance  =   (HINSTANCE)GetModuleHandle(0);   
		wcex.hIcon      =   0;   
		wcex.hCursor       =   0;   
		wcex.hbrBackground     =  0;   
		wcex.lpszMenuName       =   0;   
		wcex.lpszClassName     =   CLASS_NAME;   
		wcex.hIconSm                 =   0;   
  
		if(!RegisterClassEx(&wcex))   
		{   
		//	DWORD drRet=GetLastError();
			LogMessage("RegisterClassEx Failure",LOG_ERROR);
			return -1;   
		}   
 
		g_HWndCapture =   CreateWindow(CLASS_NAME,   
			WINDOW_TITLE,   
			WS_OVERLAPPEDWINDOW,   
			CW_USEDEFAULT,   
			0,   
			CW_USEDEFAULT,   
			0,   
			0,        
			0,     
			(HINSTANCE)GetModuleHandle(0),   
			0);
		
		if(!g_HWndCapture)   
		{  
			LogMessage("CreateWindow Failure",LOG_ERROR);
			return  -2;   
		}   


		if(g_DummyDlg!=0)
		{
			delete g_DummyDlg;
		}

		g_DummyDlg= new CRGBSAMPDlg(NULL,g_HWndCapture);
		

		int retValue=g_DummyDlg->OnCreate(0);
		
		g_DummyDlg->OnInitDialog();

		g_DummyDlg->m_capnum = 1;
		g_DummyDlg->m_iIndex = 0;

		return retValue;
	}
	catch(...)
	{
		LogMessage("Init Card Failure",LOG_ERROR);
	}
	return -1;
}

TCAPI_API int StartCapture( long lngMaxCachePicNum,LPSTR workFolder, fun_NewCapture newCapture )
{

	try
	{
		if(g_DummyDlg==0)
		{
			g_DummyDlg= new CRGBSAMPDlg(NULL,g_HWndCapture);
			
			g_DummyDlg->OnCreate(0);
			g_DummyDlg->OnInitDialog();

			g_DummyDlg->m_capnum = 1;
			g_DummyDlg->m_iIndex = 0;

		}
		
		
		g_DummyDlg->m_WorkFolder=workFolder;
		g_DummyDlg->m_NewCapture=newCapture;

		g_WorkFolder=g_DummyDlg->m_WorkFolder;
		g_NewCapture=g_DummyDlg->m_NewCapture;
		g_latestCapture=g_DummyDlg->m_WorkFolder;
		
		g_DummyDlg->m_maxCachePicNumber=lngMaxCachePicNum;
		
		g_Capture_Buffer1 = GlobalAlloc(GMEM_FIXED, 1600*1200*4);
		g_Capture_Buffer1_Width=g_Capture_Buffer1_Height=0;
		g_Capture_Buffer2 = GlobalAlloc(GMEM_FIXED, 1600*1200*4);
		g_Capture_Buffer2_Width=g_Capture_Buffer2_Height=0;

		g_DummyDlg->OnRunVGA1();
		
		return g_DummyDlg->m_isInitSucceed?0:-1;
	}
	catch(...)
	{
		LogMessage("Start Capture Failure",LOG_ERROR);
	}

	return -1;
}

TCAPI_API int StopCapture()
{
	if(g_DummyDlg!=0)
		g_DummyDlg->OnStopVGA1();

	if(g_Capture_Buffer1)
	{
		GlobalFree(g_Capture_Buffer1); 
		g_Capture_Buffer1=0;
	}

	
	if(g_Capture_Buffer2)
	{
		GlobalFree(g_Capture_Buffer2); 
		g_Capture_Buffer2=0;
	}

	g_Capture_Buffer1_Width=g_Capture_Buffer1_Height=0;
	g_Capture_Buffer2_Width=g_Capture_Buffer2_Height=0;

	return 0;
}

void __stdcall SetLogHandler( fun_Logger logger )
{
	g_Logger=logger;
}

TCAPI_API int LogMessage(LPSTR strContent,long logType)
{
	if(g_Logger)
		g_Logger(logType,strContent);
	return 0;	
}

TCAPI_API int ReleaseCard()
{
	if(g_HWndCapture!=0)
	{
		SendMessage(g_HWndCapture, WM_CLOSE,0,0);
		DestroyWindow(g_HWndCapture);
		g_HWndCapture=0;
	}
	
	if(g_DummyDlg!=0)
	{
		delete g_DummyDlg;
		g_DummyDlg=0;
	}
	return 0;
}

TCAPI_API bool GetLatestCapture(LPSTR content)
{
	if(content==0) 
		return false;
	else
	{
		Lock lock(&_cs);
		g_latestCaptureIndex++;

		if(!g_latestCapture.empty())
		{
			strncpy(content,g_latestCapture.c_str(),g_latestCapture.size());
		}
	}	

	return true;
}

TCAPI_API bool GetLatestCaptureBytes(int * captureIndex,int * width,int * height, LPSTR content)
{
	if(content==0) 
		return false;
	try
	{
		*captureIndex=g_Capture_Index;

		Lock lock(&_cs);
		if(g_Capture_Use_Buffer1)
		{
			if (g_Capture_Buffer2_Width && g_Capture_Buffer2_Height)
			{
				*width=g_Capture_Buffer2_Width;
				*height=g_Capture_Buffer2_Height;
				//memcpy((char *)content,(const char *)g_Capture_Buffer2,g_Capture_Buffer2_Width*g_Capture_Buffer2_Height*3);
				DECODE_RGB_TO_BGR(g_Capture_Buffer2_Width,g_Capture_Buffer2_Height,(const unsigned char *)g_Capture_Buffer2,(unsigned char *)content);
				//DECODE_RGB_TO_BGRA_2(g_Capture_Buffer2_Width,g_Capture_Buffer2_Height,(const unsigned char *)g_Capture_Buffer2,(unsigned char *)content);
				return true;
			}
		}
		else
		{
			
			if (g_Capture_Buffer1_Width && g_Capture_Buffer1_Height)
			{
				*width=g_Capture_Buffer1_Width;
				*height=g_Capture_Buffer1_Height;
				//memcpy((char *)content,(const char *)g_Capture_Buffer1,g_Capture_Buffer1_Width*g_Capture_Buffer1_Height*3);
				DECODE_RGB_TO_BGR(g_Capture_Buffer1_Width,g_Capture_Buffer1_Height,(const unsigned char *)g_Capture_Buffer1,(unsigned char *)content);
				//DECODE_RGB_TO_BGRA_2(g_Capture_Buffer1_Width,g_Capture_Buffer1_Height,(const unsigned char *)g_Capture_Buffer1,(unsigned char *)content);
				return true;
			}
		}
	}
	catch(...)
	{
		LogMessage("GetLatestCaptureBytes Come To Error!",LOG_ERROR);
	}

	return false;		
}

TCAPI_API int IsHasSignal()
{
	int ret=-1;
	if(g_DummyDlg!=0 && g_DummyDlg->signalflag==true)
		ret=0;

	return -1;
}

TCAPI_API LPSTR GetSignalInfo()
{
	string strRet;
	if(IsHasSignal()==0)
		strRet="信号正常";
	else if(g_DummyDlg!=0)
	{
		strRet=g_DummyDlg->singnalInfo;
	}
	return (char *) strRet.c_str();

}

TCAPI_API bool NextCapture()
{
	Lock lock(&_cs);

	return true;
}

TCAPI_API bool SetUseCaptureBytes( bool isUseCaptureBytes )
{
	g_Capture_Buffer_UseAble=isUseCaptureBytes;
	return true;
}
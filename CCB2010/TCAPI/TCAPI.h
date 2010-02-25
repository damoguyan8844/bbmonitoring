
// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the TCAPI_EXPORTS
// symbol defined on the command line. this symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// TCAPI_API functions as being imported from a DLL, wheras this DLL sees symbols
// defined with this macro as being exported.
#ifndef TC_API_H
#define TC_API_H

#ifdef TCAPI_EXPORTS
#define TCAPI_API __declspec(dllexport)
#else
#define TCAPI_API __declspec(dllimport)
#endif

#define  LOG_ERROR 0x0000
#define  LOG_INFO 0x0010
#define  LOG_DEBUG 0x0100

TCAPI_API int LogMessage(LPSTR strContent,long logType=LOG_INFO);


typedef void(__stdcall *  fun_Logger)(int logType,LPSTR strContent);
void __stdcall SetLogHandler( fun_Logger logger);

TCAPI_API int InitCard(long VGACAP_Framerate);

typedef void (__stdcall * fun_NewCapture)( LPSTR newCaptureFile);
TCAPI_API int StartCapture(long lngMaxCachePicNum,LPSTR workFolder,fun_NewCapture newCapture);

TCAPI_API int	IsHasSignal();
TCAPI_API LPSTR GetSignalInfo();

TCAPI_API int StopCapture();

TCAPI_API int ReleaseCard();

TCAPI_API bool GetLatestCapture(LPSTR content);

TCAPI_API bool NextCapture();

TCAPI_API bool SetUseCaptureBytes(bool isUseCaptureBytes);
TCAPI_API bool GetLatestCaptureBytes(int * width,int * height, LPSTR content);

#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TCApp
{
    public delegate void CallbackDelegate(string newCapture);
    public delegate void TCSDK_LogDelegate(Int32 logType, string message);

    public class TCSDKWrapper
    {
        public static readonly Int32 TCSDK_LOG_ERROR = 0x0000;
        public static readonly Int32 TCSDK_LOG_INFO = 0x0010;
        public static readonly Int32 TCSDK_LOG_DEBUG = 0x0100;

        [DllImport("TCAPI.dll", EntryPoint = "InitCard")]
        public static extern int InitCard(Int32 VGACap_framRate);

        [DllImport("TCAPI.dll", EntryPoint = "StartCapture")]
        public static extern int StartCapture(Int32 lngMaxCachePicNum,string workFolder, CallbackDelegate callBackFile);

        [DllImport("TCAPI.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "SetLogHandler")]
        public static extern void SetLogHandler(TCSDK_LogDelegate logger);

        [DllImport("TCAPI.dll", EntryPoint = "StopCapture")]
        public static extern int StopCapture();

        [DllImport("TCAPI.dll", EntryPoint = "GetLatestCapture")]
        public static extern bool GetLatestCapture(byte[] content);

        [DllImport("TCAPI.dll", EntryPoint = "NextCapture")]
        public static extern bool NextCapture();

        [DllImport("TCAPI.dll", EntryPoint = "ReleaseCard")]
        public static extern int ReleaseCard();
        [DllImport("TCAPI.dll", EntryPoint = "GetSignalInfo")]
        public static extern string GetSignalInfo();

        [DllImport("TCAPI.dll", EntryPoint = "IsHasSignal")]
        public static extern int IsHasSignal();

        [DllImport("TCAPI.dll", EntryPoint = "GetLatestCaptureHDIB")]
        public static extern IntPtr GetLatestCaptureHDIB();

        [DllImport("TCAPI.dll", EntryPoint = "GetLatestCaptureBytes")]
        public static extern bool GetLatestCaptureBytes(ref Int32 captureIndex,ref Int32 width, ref Int32 height, byte[] content);

        [DllImport("TCAPI.dll", EntryPoint = "SetUseCaptureBytes")]
        public static extern bool SetUseCaptureBytes(bool isEnable);
    }
}

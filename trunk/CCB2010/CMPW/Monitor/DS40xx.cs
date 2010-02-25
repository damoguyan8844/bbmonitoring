using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace HikServer.DS40xxSDK
{
    #region enum

    /// <summary>
    /// 板卡类型
    /// </summary>
    public enum BOARD_TYPE_DS : uint
    {
        DS400XM = 0,        //M卡
        DS400XH = 1,        //H卡
        DS4004HC = 2,        //4004HC
        DS4008HC = 3,        //4008HC
        DS4016HC = 4,        //4016HC
        DS4001HF = 5,        //4001HF
        DS4004HF = 6,        //4004HF
        DS4002MD = 7,        //4002MD
        DS4004MD = 8,        //4004MD
        DS4016HCS = 9,        //4016HCS
        DS4002HT = 10,    //4002HT
        DS4004HT = 11,    //4004HT
        DS4008HT = 12,    //4008HT
        DS4004HC_PLUS = 13,    //4004HC+
        DS4008HC_PLUS = 14,    //4008HC+
        DS4016HC_PLUS = 15,    //4016HC+
        DS4008HF = 16,    //4008HF
        DS4008MD = 17,    //4008MD
        DS4008HS = 18,    //4008HS
        DS4016HS = 19,    //4016HS
        INVALID_BOARD_TYPE = 0xffffffff,
    }

    /// <summary>
    /// 视频预览格式
    /// </summary>
    public enum TypeVideoFormat
    {
        vdfRGB8A_233 = 0x00000001,
        vdfRGB8R_332 = 0x00000002,
        vdfRGB15Alpha = 0x00000004,
        /// <summary>
        /// 16位RGB视频压缩格式
        /// </summary>
        vdfRGB16 = 0x00000008,
        /// <summary>
        /// 24位RGB视频压缩格式
        /// </summary>
        vdfRGB24 = 0x00000010,
        vdfRGB24Alpha = 0x00000020,

        vdfYUV420Planar = 0x00000040,
        /// <summary>
        /// YUV422视频压缩格式
        /// </summary>
        vdfYUV422Planar = 0x00000080,
        vdfYUV411Planar = 0x00000100,
        vdfYUV420Interspersed = 0x00000200,
        vdfYUV422Interspersed = 0x00000400,
        vdfYUV411Interspersed = 0x00000800,
        vdfYUV422Sequence = 0x00001000,   /* U0, Y0, V0, Y1:  For VO overlay */
        vdfYUV422SequenceAlpha = 0x00002000,
        /* U0, Y0, V0, Y1:  For VO overlay, with low bit for alpha blending */
        vdfMono = 0x00004000,  /* 8 bit monochrome */

        vdfYUV444Planar = 0x00008000,
    };

    /// <summary>
    /// 视频制式
    /// </summary>
    public enum VideoStandard_t : uint
    {
        /// <summary>
        /// 无视频信号
        /// </summary>
        StandardNone = 0x80000000,
        /// <summary>
        /// NTSC制式
        /// </summary>
        StandardNTSC = 0x00000001,
        /// <summary>
        /// PAL制式
        /// </summary>
        StandardPAL = 0x00000002,
        StandardSECAM = 0x00000004,
    } ;

    /// <summary>
    /// 编码图像分辨率
    /// </summary>
    public enum PictureFormat_t
    {
        ENC_CIF_FORMAT = 0,
        ENC_QCIF_FORMAT = 1,
        ENC_2CIF_FORMAT = 2,
        ENC_4CIF_FORMAT = 3,
        ENC_QQCIF_FORMAT = 4,
        ENC_CIFQCIF_FORMAT = 5,
        ENC_CIFQQCIF_FORMAT = 6,
        ENC_DCIF_FORMAT = 7
    };

    /// <summary>
    /// 码流控制方式
    /// </summary>
    public enum BitrateControlType_t
    {
        /// <summary>
        /// 变码率
        /// </summary>
        brCBR = 0,
        /// <summary>
        /// 恒定码率
        /// </summary>
        brVBR = 1,
    };

    public enum FrameType_t
    {
        PktError = 0,
        PktIFrames = 0x0001,
        PktPFrames = 0x0002,
        PktBBPFrames = 0x0004,
        PktAudioFrames = 0x0008,
        PktMotionDetection = 0x00010,
        PktDspStatus = 0x00020,
        PktOrigImage = 0x00040,
        PktSysHeader = 0x00080,
        PktBPFrames = 0x00100,
        PktSFrames = 0x00200,
        PktSubIFrames = 0x00400,
        PktSubPFrames = 0x00800,
        PktSubBBPFrames = 0x01000,
        PktSubSysHeader = 0x02000
    };

    #endregion

    #region struct

    /// <summary>
    /// 板卡信息结构体 
    /// </summary>
    public struct DS_BOARD_DETAIL
    {
        /// <summary>
        /// 板卡类型
        /// </summary>
        BOARD_TYPE_DS type;
        /// <summary>
        /// 序列号
        /// BYTE sn[16];        
        /// </summary>
        [MarshalAs( UnmanagedType.ByValArray, SizeConst = 16 )]
        byte[] sn;
        /// <summary>
        /// 板卡包含的DSP个数
        /// </summary>
        uint dspCount;
        /// <summary>
        /// 板卡上第一个DSP的索引
        /// </summary>
        uint firstDspIndex;
        /// <summary>
        /// 板卡包含的编码通道个数
        /// </summary>
        uint encodeChannelCount;
        /// <summary>
        /// 板卡上第一个编码通道的索引
        /// </summary>
        uint firstEncodeChannelIndex;
        /// <summary>
        /// 板卡包含的解码通道个数
        /// </summary>
        uint decodeChannelCount;
        /// <summary>
        /// 板卡上第一个解码通道的索引
        /// </summary>
        uint firstDecodeChannelIndex;
        /// <summary>
        /// 板卡包含的视频输出通道个数
        /// </summary>
        uint displayChannelCount;
        /// <summary>
        /// 板卡上第一个视频输出通道的索引
        /// </summary>
        uint firstDisplayChannelIndex;
        uint reserved1;
        uint reserved2;
        uint reserved3;
        /// <summary>
        /// 硬件版本,format:major.minor.build,major:bit 16-19,minor: bit 8-15,build: bit 0-7
        /// </summary>
        uint version;
    }

    /// <summary>
    /// DSP信息结构体 
    /// </summary>
    public struct DSP_DETAIL
    {
        /// <summary>
        /// 此DSP所包含的编码通道个数
        /// </summary>
        uint encodeChannelCount;
        /// <summary>
        /// 此DSP上第一个编码通道在所有编码通道中的索引
        /// </summary>
        uint firstEncodeChannelIndex;
        /// <summary>
        /// 此DSP所包含的解码通道个数
        /// </summary>
        uint decodeChannelCount;
        /// <summary>
        /// 此DSP上第一个解码通道在所有解码通道中的索引
        /// </summary>
        uint firstDecodeChannelIndex;
        /// <summary>
        /// 此DSP包含的显示通道个数
        /// </summary>
        uint displayChannelCount;
        /// <summary>
        /// 此DSP上第一个显示通道在所有显示通道中的索引
        /// </summary>
        uint firstDisplayChannelIndex;
        uint reserved1;
        uint reserved2;
        uint reserved3;
        uint reserved4;
    }

    /// <summary>
    /// 特殊功能结构体
    /// </summary>
    public struct CHANNEL_CAPABILITY
    {
        /// <summary>
        /// 音频预览
        /// </summary>
        byte[] bAudioPreview;
        /// <summary>
        /// 报警信号
        /// </summary>
        byte[] bAlarmIO;
        /// <summary>
        /// 看家狗
        /// </summary>
        byte[] bWatchDog;
    }

    /// <summary>
    /// 版本信息
    /// </summary>
    public struct PVERSION_INFO
    {
        /// <summary>
        /// DSP版本号，DSP的BUILD号，用于软件升级时标明该版本的最后修改时间 
        /// </summary>
        ulong DspVersion, DspBuildNum;
        /// <summary>
        /// Driver版本号，Driver的BUILD号，用于软件升级时标明该版本的最后修改时间 
        /// </summary>
        ulong DriverVersion, DriverBuildNum;
        /// <summary>
        /// SDK版本号，SDK的BUILD号，用于软件升级时标明该版本的最后修改时间 
        /// </summary>
        ulong SDKVersion, SDKBuildNum;
    }

    /// <summary>
    /// 显示窗口内的矩形区域
    /// </summary>
    //[StructLayout(LayoutKind.
    //public struct RECT
    //{
    //    public long left;
    //    public long top;
    //    public long right;
    //    public long bottom;
    //}

    /// <summary>
    /// 帧统计信息结构体 
    /// </summary>
    public struct PFRAMES_STATISTICS
    {
        /// <summary>
        /// 视频帧
        /// </summary>
        ulong VideoFrames;
        /// <summary>
        /// 音频帧
        /// </summary>
        ulong AudioFrames;
        /// <summary>
        /// 丢失帧
        /// </summary>
        ulong FramesLost;
        /// <summary>
        /// 丢失的码流（字节）
        /// </summary>
        ulong QueueOverflow;
        /// <summary>
        /// 当前的帧率（bps）
        /// </summary>
        ulong CurBps;
    }

    /// <summary>
    /// 版本信息结构体 
    /// </summary>
    public struct PHW_VERSION
    {
        /// <summary>
        /// DSP程序的版本号和Build号 
        /// </summary>
        ulong DspVersion, DspBuildNum;
        /// <summary>
        /// 驱动程序的版本号和Build号 
        /// </summary>
        ulong DriverVersion, DriverBuildNum;
        /// <summary>
        /// SDK 的版本号和Build号 
        /// </summary>
        ulong SDKVersion, SDKBuildNum;
    }

    /// <summary>
    /// 系统时间
    /// </summary>
    public struct SYSTEMTIME
    {
        ushort wYear;
        ushort wMonth;
        ushort wDayOfWeek;
        ushort wDay;
        ushort wHour;
        ushort wMinute;
        ushort wSecond;
        ushort wMilliseconds;
    }

    #endregion

    #region delegate

    /// <summary>
    /// 原始图像流设置
    /// 
    /// typedef void (*IMAGE_STREAM_CALLBACK)(UINT channelNumber,void *context);
    /// </summary>
    /// <param name="channelNumber">通道号</param>
    /// <param name="context">设备上下文</param>
    public delegate void IMAGE_STREAM_CALLBACK( uint channelNumber, IntPtr context );

    /// <summary>
    /// 编码数据流直接读取回调函数
    /// 
    /// typedef int (*STREAM_DIRECT_READ_CALLBACK)(ULONG channelNumber,void *DataBuf,DWORD Length,int FrameType,void *context);
    /// </summary>
    /// <param name="channelNumber">通道号</param>
    /// <param name="DataBuf">缓冲区地址</param>
    /// <param name="Length">缓冲区长度</param>
    /// <param name="FrameType">缓冲区数据帧类型</param>
    /// <param name="context">设备上下文</param>
    /// <returns></returns>
    [UnmanagedFunctionPointer( CallingConvention.Cdecl )]
    //public delegate int STREAM_DIRECT_READ_CALLBACK(uint channelNumber, byte[] DataBuf, uint Length, FrameType_t FrameType, IntPtr context);
    public delegate int STREAM_DIRECT_READ_CALLBACK( int channelNumber, IntPtr DataBuf, int Length, FrameType_t FrameType, IntPtr context );
    //public unsafe delegate int STREAM_DIRECT_READ_CALLBACK(uint channelNumber, void * DataBuf, uint Length, int FrameType, IntPtr context);

    /// <summary>
    /// 直接读取码流回调函数
    /// 
    /// typedef int (*STREAM_READ_CALLBACK)(ULONG channelNumber, void *context) 
    /// </summary>
    /// <param name="channelNumber">通道号</param>
    /// <param name="context">设备上下文</param>
    /// <returns></returns>
    public delegate int STREAM_READ_CALLBACK( ulong channelNumber, IntPtr context );

    /// <summary>
    /// 移动侦测结果回调函数
    /// 
    /// typedef void (*MOTION_DETECTION_CALLBACK)(ULONG channelNumber, BOOL bMotionDetected,void *context) 
    /// </summary>
    /// <param name="channelNumber">通道号</param>
    /// <param name="bMotionDetected">
    /// 移动侦测发生标志，如果当前通道所设置的移动侦测
    /// 区域内产生了移动侦测，则被置为True；如果当前通道所设置的移动侦测区域内自上
    /// 一次产生移动侦测后delay秒内没有发生移动侦测，则被置为False。
    /// </param>
    /// <param name="context">设备上下文</param>
    public delegate void MOTION_DETECTION_CALLBACK( ulong channelNumber, bool bMotionDetected, IntPtr context );

    /// <summary>
    /// 画图回调函数
    /// 
    /// #define DRAWFUN(x)   void  (CALLBACK* x)(long nPort,HDC hDc,LONG nUser) 
    /// </summary>
    /// <param name="nPort">通道号</param>
    /// <param name="HDC">offscreen表面设备上下文，相当于显示窗口中的DC</param>
    /// <param name="nUser">用户数据</param>
    public delegate void DrawFun( long nPort, IntPtr HDC, long nUser );

    /// <summary>
    /// 解码回调函数
    /// 
    /// typedef void (*DECODER_VIDEO_CAPTURE_CALLBACK)(UINT nChannelNumber, void *DataBuf,UINT width,UINT height,UINT nFrameNum,UINT nFrameTime, SYSTEMTIME *pFrameAbsoluteTime,void *context) 
    /// </summary>
    /// <param name="nChannelNumber">解码通道句柄</param>
    /// <param name="DataBuf">缓冲区地址</param>
    /// <param name="width">图像宽度</param>
    /// <param name="height">图像高度</param>
    /// <param name="nFrameNum">捕获的当前帧的序号</param>
    /// <param name="nFrameTime">捕获的当前帧的相对时间，单位：毫秒</param>
    /// <param name="pFrameAbsoluteTime">捕获的当前帧的绝对时间</param>
    /// <param name="context">设备上下文</param>
    public delegate void DECODER_VIDEO_CAPTURE_CALLBACK( uint nChannelNumber, IntPtr DataBuf, uint width, uint height, uint nFrameNum, uint nFrameTime, SYSTEMTIME pFrameAbsoluteTime, IntPtr context );

    /// <summary>
    /// 创建索引完成回调函数
    /// 
    /// typedef void (*FILE_REF_DONE_CALLBACK)(UINT nChannel,UINT nSize)
    /// </summary>
    /// <param name="nChannel">通道号</param>
    /// <param name="nSize">索引大小（暂时无效，以后可以增加索引导出、导入功能） </param>
    public delegate void FILE_REF_DONE_CALLBACK( uint nChannel, uint nSize );

    #endregion



    /// <summary>
    /// DS40xxSDK.dll
    /// </summary>
    public class HikVisionSDK
    {
        /// <summary>
        /// 状态
        /// </summary>
        public static readonly List<string> state = new List<string>( new string[]{
            "", "正在打开", "音频信号丢失", "视频信号丢失", "有物体移动", //0-4 
            "自动分割录像", "开始录像", "停止录像", "启动声音监听", "停止声音监听", //5-9 
            "启动视频预览", "停止视频预览", "启动录像", "停止录像", "启动视频报警", //10-14 
            "关闭视频报警", "启动音频报警", "停止音频报警", "启动移动侦测", "停止移动侦测", //15-19 
            "启动视频遮挡", "关闭视频遮挡", "开始屏幕输出", "停止屏幕输出", "启动视频LOGO", //20-24 
            "停止视频LOGO", "开始视频OSD", "停止视频OSD", "切换为黑白视频", "切换为彩色视频", //25-29 
            "切换为黑屏显示", "切换为白屏显示", "视频色彩复位", "启动全屏显示", "采集卡已经加载", //30-34 
            "采集卡已经卸截", "视频服务启动成功", "视频服务已停止", "静音", "音量恢复", //35-39 
            "云台控制命令发送", "系统出现未知错误", "录像文件大小", "配置端口号成功", "连接服务端成功", //40-44 
            "正在连接", "开始接收图象", "异常退出", "接收完毕，退出", "无法联系服务端", //45-49 
            "服务端拒绝访问", "无效", "停止客户端连接", "图像抓取成功", "初始化服务端网络连接成功", //50-54 
            "视频服务启动失败", "退出全屏预览", "", "", "" //55-59 
        } );

        //可以用新版函数替代功能或者无效的API 
        //GetTotalChannels：可用GetEncodeChannelCount替代 
        //GetTotalDSPs：可用GetDspCount 替代 
        //SetupDateTime：4.0版本起无效 
        //HW_GetChannelNum：无效，请使用GetBoardDetail 
        //HW_GetDeviceSerialNo：无效，请使用GetBoardDetail 
        //HW_SetVideoOutStandard：无效，请使用SetDisplayStandard或SetDefaultVideoStandard 
        //HW_SetDspDeadlockMsg：无效 
        //HW_ResetDsp：无效 
        //HW_SetDisplayPara：DISPLAY_PARA结构中bToVideoOut无效，MD卡模拟视频输出功能
        //已经整合到视频矩阵之中。 

        #region 流类型宏定义

        /// <summary>
        /// 视频流 
        /// #define STREAM_TYPE_VIDEO 
        /// </summary>
        private const int STREAM_TYPE_VIDEO = 1;
        /// <summary>
        /// 音频流 
        /// #define STREAM_TYPE_AUDIO   
        /// </summary>
        private const int STREAM_TYPE_AUDIO = 2;
        /// <summary>
        /// 音视频复合流 
        /// #define STREAM_TYPE_AVSYNC 
        /// </summary>
        private const int STREAM_TYPE_AVSYNC = 3;

        #endregion

        #region 1.板卡初始化及卸载

        /// <summary>
        /// 1.1初始化DSP InitDSPs
        ///     说  明： 初始化系统中每一块板卡，应在应用软件程序启动时完成。如果返回值为0则表
        ///     明初始化失败，可能没有找到相应的DSP软件模块。
        /// 
        /// int __stdcall InitDSPs()
        /// </summary>
        /// <returns>系统内可用的编码通道个数。 </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int InitDSPs();


        /// <summary>
        /// 1.2卸载DSP DeInitDSPs
        ///     说  明：  关闭每一块板卡上的功能，应在应用软件程序退出时调用。 
        /// 
        /// int __stdcall DeInitDSPs()
        /// </summary>
        /// <returns>0</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int DeInitDSPs();

        #endregion

        #region 2.板卡信息获取

        /// <summary>
        /// 2.1获取系统中板卡的张数GetBoardCount 
        ///     说  明：  获取系统中所有板卡的张数，包含编码卡和解码卡。
        /// 
        /// unsigned int __stdcall GetBoardCount() 
        /// </summary>
        /// <returns>系统中板卡的总张数。</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern uint GetBoardCount();

        /// <summary>
        /// 2.2获取系统中DSP的个数GetDspCount 
        ///     说  明：  获取系统中所有板卡的DSP的个数。 
        ///     
        /// unsigned int __stdcall GetDspCount()
        /// </summary>
        /// <returns>系统中DSP的总个数</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern uint GetDspCount();

        /// <summary>
        /// GetTotalDSPs：可用GetDspCount 替代
        /// 获得实际可用DSP
        /// </summary>
        /// <returns></returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetTotalDSPs();

        /// <summary>
        /// 2.3获取系统中编码通道的个数GetEncodeChannelCount
        ///     说  明：  获取系统中所有编码卡的编码通道总个数，包含H系列和HC系列编码卡。
        /// 
        /// unsigned int __stdcall GetEncodeChannelCount()
        /// </summary>
        /// <returns>系统中编码通道的个数</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern uint GetEncodeChannelCount();

        /// <summary>
        /// 2.4获取系统中解码通道的个数GetDecodeChannelCount 
        ///     说  明：  获取系统中MD卡的解码通道个数 
        /// 
        /// unsigned int __stdcall GetDecodeChannelCount() 
        /// </summary>
        /// <returns></returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern uint GetDecodeChannelCount();

        /// <summary>
        /// 2.5获取系统中解码显示通道的个数GetDisplayChannelCount
        ///     说  明：  获取系统中MD卡显示通道的个数，即模拟视频输出通道的个数
        /// 
        /// unsigned int __stdcall GetDisplayChannelCount()
        /// </summary>
        /// <returns>系统中显示通道的个数</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern uint GetDisplayChannelCount();

        /// <summary>
        /// 2.6获取板卡详细信息GetBoardDetail 
        ///     说  明：  获取某张板卡的详细信息 
        /// 
        /// int __stdcall GetBoardDetail(UINT boardNum,DS_BOARD_DETAIL *pBoardDetail)
        /// </summary>
        /// <param name="boardNum">板卡索引</param>
        /// <param name="pBoardDetail">板卡信息</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetBoardDetail( uint boardNum, ref DS_BOARD_DETAIL pBoardDetail );

        /// <summary>
        /// 2.7获取DSP详细信息GetDspDetail
        ///     说  明：  获取某个DSP的详细信息 
        /// 
        /// int __stdcall GetDspDetail(UINT dspNum,DSP_DETAIL *pDspDetail)
        /// </summary>
        /// <param name="dspNum">DSP索引</param>
        /// <param name="pDspDetail">DSP信息</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetDspDetail( uint dspNum, ref DSP_DETAIL pDspDetail );

        /// <summary>
        /// 2.8获取板卡型号及序列号信息GetBoardInfo
        ///     说  明：  获取板卡的型号及序列号信息
        /// 
        /// int __stdcall GetBoardInfo(HANDLE hChannelHandle, ULONG *BoardType,  UCHAR *SerialNo)
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="BoardType">板卡型号</param>
        /// <param name="SerialNo">
        /// 板卡ID号, 内容为板卡序列号的ASCII的数值，次序为SerialNo[0] 对应最高位，
        /// SerialNo[11]对应最低位。比如卡号为“40000002345”的值对应为 4,0,0,0,0,1,0,0,2,3,4,5 的整形数组。
        /// </param>
        /// <returns>成功为0；失败返回错误号 </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetBoardInfo( IntPtr hChannelHandle, ulong BoardType, byte[] SerialNo );


        /// <summary>
        /// 2.9获取板卡特殊功能信息GetCapability
        ///     说  明：  获取板卡特殊功能信息
        /// 
        /// int __stdcall GetCapability(HANDLE hChannelHandle,  CHANNEL_CAPABILITY *Capability) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="Capability">特殊功能 </param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetCapability( IntPtr hChannelHandle, CHANNEL_CAPABILITY Capability );


        /// <summary>
        /// 2.10获取板卡SDK信息GetSDKVersion
        ///     说  明：  获取当前所使用的DSP、Driver、SDK版本号
        /// 
        /// int __stdcall GetSDKVersion(PVERSION_INFO VersionInfo) 
        /// </summary>
        /// <param name="VersionInfo">版本信息</param>
        /// <returns>成功返回0；失败返回错误号。</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetSDKVersion( ref PVERSION_INFO VersionInfo );

        /// <summary>
        /// 2.11获取板卡SDK及DSP错误报告GetLastErrorNum*，此函数只对H卡有效
        ///     说  明：  获取SDK及DSP错误报告。此函数只对H卡有效，用于HC卡上返回0且无效
        /// 
        /// int __stdcall GetLastErrorNum(HANDLE hChannelHandle, ULONG *DspError,  ULONG *SdkError) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="DspError">DSP错误</param>
        /// <param name="SdkError">SDK错误</param>
        /// <returns>DSP错误信息、SDK错误信息</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetLastErrorNum( IntPtr hChannelHandle, ref int DspError, ref int SdkError );

        #endregion

        #region 3.编码卡API

        #region 3.1通道打开及关闭

        /// <summary>
        /// 3.1.1打开通道ChannelOpen
        ///     说  明：  打开通道，获取编码通道的操作句柄，与通道相关的操作需使用相对应的句柄。 
        /// 
        /// HANDLE __stdcall ChannelOpen(int ChannelNum) 
        /// </summary>
        /// <param name="ChannelNum">通道号（从0开始）</param>
        /// <returns>成功返回有效句柄（值可能为0）；失败返回0xFFFFFFFF。</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern IntPtr ChannelOpen( int ChannelNum );

        /// <summary>
        /// 3.1.2关闭通道ChannelClose
        ///     说  明：  关闭通道，释放相关资源 
        /// 
        /// int __stdcall ChannelClose(HANDLE hChannelHandle) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int ChannelClose( IntPtr hChannelHandle );

        #endregion

        #region 3.2视频预览

        #region 3.2.1 Overlay预览模式
        //        释  义：  overlay预览模式 
        //Overlay通常被称为重叠页面或者是覆盖层，是一种需要特定的硬件支持的页面，通常
        //被用于显示实时视频于主页面之上，而不需要Blit操作到主页面或用任何方法改变主页面的
        //内容。使用该方式进行预览可以提高预览的画质和降低CPU利用率。

        /// <summary>
        /// 3.2.1.1设置视频预览模式SetPreviewOverlayMode
        ///     说  明：  SDK自3.2版本起在部分显卡中实现了HC卡以overlay方式预览的功能（此功
        ///     能不支持与H卡混插的状态下），可以提高预览的画质和降低CPU利用率。当预览画面小 
        ///     于704*576时，需要调用该函数来启动overlay模式，如不设置则自动切换到offscreen模式
        ///     进行预览显示，当预览画面大于704*576时，SDK自动切换到overlay模式
        /// 
        /// int __stdcall SetPreviewOverlayMode(BOOL bTrue)
        /// </summary>
        /// <param name="bTrue">是否设置overlay预览方式，也适用于MD卡</param>
        /// <returns>0表示显卡支持板卡的overlay预览方式；其他值表示显卡不支持 </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetPreviewOverlayMode( bool bTrue );

        /// <summary>
        /// 3.2.1.2设置overlay关键色SetOverlayColorKey
        ///     注意：需要在StartVideoPreview前调用该函数。
        ///     说  明：  板卡在显示范围小于704*576时，调用SetPreviewOverlayMode可以开启overlay 
        ///     预览模式，需调用SetOverlayColorKey设置overlay关键色；当显示范围大于704*576时，
        ///     板卡自动切换到overlay预览模式，关键色默认设置为RGB（10，10，10），也可调用 
        ///     SetOverlayColorKey修改关键色。在这两种情况下，都需要将显示窗口的底色设置为和关键
        ///     色相一致。否则图像将难以显示。
        ///     
        /// 
        /// int __stdcall SetOverlayColorKey(COLORREF DestColorKey) 
        /// </summary>
        /// <param name="DestColorKey">overlay关键色参数（RGB（*，*，*））</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetOverlayColorKey( int DestColorKey );
        //public static extern int SetOverlayColorKey(Color DestColorKey);

        /// <summary>
        /// 3.2.1.3恢复当前丢失的表面RestoreOverlay 
        ///     说  明：  恢复当前丢失的表面，例如：当系统按下CTRL+ALT+DEL时系统的OVERLAY
        ///     表面会被强制关闭，调用该函数可以恢复OVERLAY表面 
        /// 
        /// int __stdcall RestoreOverlay()
        /// </summary>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int RestoreOverlay();

        #endregion

        #region 3.2.2 开启及停止视频预览

        /// <summary>
        /// 3.2.2.1开启视频预览StartVideoPreview
        ///     说  明：  启动视频预览，调用SetPreviewOverlayMode后，可进行overlay模式预览，否则，
        ///     将默认采用offscreen模式预览。当画面大于704*576时，SDK自动切换到overlay预览模式。
        /// 
        /// int __stdcall StartVideoPreview(HANDLE hChannelHandle,HWND WndHandle,  RECT *rect,BOOLEAN bOverlay, int VideoFormat, int FrameRate) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="WndHandle">显示窗口句柄</param>
        /// <param name="rect">
        ///     显示窗口内的矩形区域
        ///     Rect.right  必须为8的倍数
        ///     Rect.bottom必须为16的倍数 
        /// </param>
        /// <param name="bOverlay">是否启用Overlay预览模式</param>
        /// <param name="VideoFormat">视频预览格式（目前无效）</param>
        /// <param name="FrameRate">视频预览帧率（PAL：1-25，NTSC：1-30）</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StartVideoPreview( IntPtr hChannelHandle, IntPtr WndHandle, ref Rectangle rect, bool bOverlay, int VideoFormat, int FrameRate );
        //public static extern int StartVideoPreview(IntPtr hChannelHandle, IntPtr WndHandle, ref RECT rect, bool bOverlay, int VideoFormat, int FrameRate);

        /// <summary>
        /// 3.2.2.2停止视频预览StopVideoPreview
        ///     说  明：  停止视频预览
        /// 
        /// int __stdcall StopVideoPreview(HANDLE hChannelHandle) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StopVideoPreview( IntPtr hChannelHandle );

        #endregion

        #region 3.2.3 视频参数的设置及获取

        /// <summary>
        /// 3.2.3.1设置视频参数SetVideoPara
        ///     说  明：  设置视频参数
        /// 
        /// int __stdcall SetVideoPara(HANDLE hChannelHandle, int Brightness, int Contrast, int Saturation, int Hue) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="Brightness">亮度值（0-255）</param>
        /// <param name="Contrast">对比度（0-127）</param>
        /// <param name="Saturation">饱和度（0-127）</param>
        /// <param name="Hue">色调（0-255）</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetVideoPara( IntPtr hChannelHandle, int Brightness, int Contrast, int Saturation, int Hue );

        /// <summary>
        /// 3.2.3.2获取视频参数GetVideoPara
        ///     说  明：  获取视频参数
        /// 
        /// int __stdcall GetVideoPara(HANDLE hChannelHandle,  VideoStandard_t *VideoStandard, int*Brightness, int *Contrast, int *Saturation, int *Hue) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="VideoStandard">视频制式</param>
        /// <param name="Brightness">亮度指针值（0-255）</param>
        /// <param name="Contrast">对比度指针值（0-127）</param>
        /// <param name="Saturation">饱和度指针值（0-127）</param>
        /// <param name="Hue">色调指针值（0-255）</param>
        /// <returns>  成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetVideoPara( IntPtr hChannelHandle, VideoStandard_t VideoStandard, out int Brightness, out int Contrast, out int Saturation, out int Hue );

        #endregion

        #endregion

        #region 3.3视频信号设置（制式、状况、输入位置等）

        /// <summary>
        /// 3.3.1设置视频制式SetVideoStandard，此函数只对H卡有效
        ///     说  明：  设置视频制式，在某一制式的摄像头已经接好的情况下启动系统时可不必调用该
        ///     函数，如果没有接摄像头的情况下启动系统然后再接NTSC制式的摄像头则必须调用该函    
        ///     数，或者中途调换不同制式的摄像头也必须调用该函数。
        /// 
        /// int __stdcall SetVideoStandard(HANDLE hChannelHandle,  VideoStandard_t VideoStandard) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="VideoStandard">视频制式</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetVideoStandard( IntPtr hChannelHandle, VideoStandard_t VideoStandard );

        /// <summary>
        /// 3.3.2设置系统默认的视频制式SetDefaultVideoStandard
        ///     注意：该函数只能在系统初始化（InitDSPs）之前运行，否则无效
        ///     说  明：  设置系统默认的视频制式，系统中所有的视频输入通道如果无视频输入或者在系
        ///     统启动的时候，通道会按照所设置的系统默认视频制式进行处理。 
        /// 
        /// int __stdcall SetDefaultVideoStandard(VideoStandard_t VideoStandard) 
        /// </summary>
        /// <param name="VideoStandard">视频制式，默认为PAL </param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetDefaultVideoStandard( VideoStandard_t VideoStandard );

        /// <summary>
        /// 3.3.3设置视频信号灵敏度SetVideoDetectPrecision
        ///     说  明：  设置视频信号检测的灵敏度。如果视频信号的强度比较弱，或者信号通断的切换    
        ///     比较频繁，会出现“无视频信号”的提示字样，为了避免提示字样影响图像，可以更改视频
        ///     信号检测的灵敏度。灵敏度取值越大，检测精度越低，出现“无视频信号”提示字样的频率
        ///     越低。当将value值设置为0xffffffff时，将不会再出现“无视频信号”的提示字样。
        /// 
        /// int __stdcall SetVideoDetectPrecision(HANDLE hChannel,unsigned int value)
        /// </summary>
        /// <param name="hChannel">通道句柄</param>
        /// <param name="value">灵敏度。取值范围：0-100，默认为20</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetVideoDetectPrecision( IntPtr hChannel, uint value );

        /// <summary>
        /// 3.3.4获取视频信号输入情况GetVideoSignal
        ///     说  明：  获取视频信号的输入情况，用于视频丢失报警 
        /// 
        /// int __stdcall GetVideoSignal(HANDLE  hChannelHandle)
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>信号正常返回0；返回其他值说明信号异常或有错误</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetVideoSignal( IntPtr hChannelHandle );

        /// <summary>
        /// 3.3.5调整视频信号输入位置SetInputVideoPosition
        ///     说  明：  设置视频信号的输入位置。（x，y）为系统处理图像的左上角在摄像机输入的原
        ///     始图像中的坐标，某些摄像机输入的图像在预览时可能在左边会有黑边，可以通过此函数进
        ///     行调节，x必须设置为2的整数倍。（x，y）的取值和摄像机的型号有关，如果指定的值和
        ///     摄像机的输入参数不匹配，可能会导致图像静止、水平垂直方向滚动或者黑屏，请谨慎使用。 
        /// 
        /// int __stdcall SetInputVideoPosition(HANDLE hChannel,UINT x,UINT y) 
        /// </summary>
        /// <param name="hChannel">通道句柄</param>
        /// <param name="x">X轴坐标，默认值为8</param>
        /// <param name="y">Y轴坐标，默认值为2</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetInputVideoPosition( IntPtr hChannel, uint x, uint y );

        /// <summary>
        /// 3.3.6设置反隔行变换及强度SetDeInterlace
        ///     说  明：  设置是否采用反隔行算法，已经采用反隔行时的强度
        /// 
        /// 释  义：  反隔行变换 
        /// 如果该通道的图像需要进行4CIF的预览或编码，此时的图像中会同时包含奇、偶两
        /// 场的数据，由于奇场图像和偶场图像不同步，导致图像中运动的部分发生错位、边缘模糊，
        /// 此时需要对图像进行反隔行变换来去掉这种现象。如果用户能够确定使用的是逐行扫描格式
        /// 的摄像机，或者主要应用在静止场景，此时可以关掉反隔行变换功能，或者降低强度，这样
        /// 可以提高系统运行效率，并降低反隔行变换对图像质量带来的损失。 
        /// 
        /// int __stdcall SetDeInterlace(HANDLE hChannelHandle,UINT mode,UINT level) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="mode">
        ///     0表示该通道不进行反隔行变换,此时level参数无效；
        ///     1表示使用旧的算法；
        ///     2表示使用默认算法（系统默认值）。
        /// </param>
        /// <param name="level">
        ///     mode＝1时有效，其它时无效。
        ///     0－10，反隔行变换的强度逐渐加强，0最弱，对图像的损失最小，10最强，对图像的损失最大。
        /// </param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetDeInterlace( IntPtr hChannelHandle, uint mode, uint level );



        #endregion

        #region 3.4视频编码参数设置

        //        释  义：  双编码功能（主、子通道） 
        //  对一路视频图像进行两路视频编码，两路视频可以有不同的码流类型、不同分辨率、不
        //同码率等。3.0版本对双编码功能做了增强，子通道的所有参数都可以任意设置。 
        //双编码中主通道和子通道唯一的区别在于：子通道占用的系统资源比主通道少，优先级
        //比主通道低。当系统忙时，会尽量保证主通道编码，并先从子通道开始丢帧。由于占用资源
        //少，因此可以利用子通道来实现多路高分辨率的非实时编码。例如：可以把DS-4000HC中
        //的每个子通道全部设置为4CIF分辨率（SetSubStreamType），而不使用主通道编码，这样就
        //可以实现全部通道的4CIF编码。在一般场景下，每路图像都可以达到15帧以上。 

        /// <summary>
        /// 3.4.1主、子通道切换SetupSubChannel
        ///     说  明：  配合双编码模式使用。当设置某个通道为双编码模式时，如主通道编码CIF，子
        ///     通道编码QCIF，这时可对主/子通道分别设置某些参数。关键帧间隔、OSD、LOGO等参数
        ///     对主/子通道是一样的；在设置帧率、量化系数、变码流/定码流模式、码流大小等参数时应
        ///     调用此函数分别对主/子通道进行设置，缺省是对主通道进行设置
        /// 
        /// int __stdcall SetupSubChannel(HANDLE hChannelHandle, int iSubChannel) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="iSubChannel">子通道号（0表示主通道，1表示主通道）</param>
        /// <returns>成功返回0；失败返回错误号 </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetupSubChannel( IntPtr hChannelHandle, int iSubChannel );


        /// <summary>
        /// 3.4.2获取双编码时数据流类型GetSubChannelStreamType
        ///     说  明：  配合双编码模式使用，当设置双编码模式时，启动录像后，DSP会向上送两种
        ///     数据流，调用此函数得到主通道和子通道的数据流类型，供应用程序使用。
        /// 
        /// int __stdcall GetSubChannelStreamType(void  *DataBuf, int FrameType) 
        /// </summary>
        /// <param name="DataBuf">数据缓存区</param>
        /// <param name="FrameType">帧类型</param>
        /// <returns>
        ///     0 其他数据  
        ///     1 主通道数据流的文件头
        ///     2 子通道数据流的文件头 
        ///     3 主通道数据流的视频帧类型 
        ///     4 子通道数据流的视频帧类型 
        ///     5 数据流的音频帧 
        /// </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetSubChannelStreamType( IntPtr DataBuf, int FrameType );

        #region 3.4.3编码流类型的设置及获取（不支持动态修改）

        /// <summary>
        /// 3.4.3.1设置主通道编码流类型SetStreamType
        ///     说  明：  设置主通道编码流类型。此函数需在启动编码前进行设置
        /// 
        /// int __stdcall SetStreamType(HANDLE hChannelHandle, USHORT Type) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="Type">流类型</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetStreamType( IntPtr hChannelHandle, ushort Type );

        /// <summary>
        /// 3.4.3.2获取主通道编码流类型GetStreamType
        ///     说  明：  获取主通道编码流类型
        /// 
        /// int __stdcall GetStreamType(HANDLE hChannelHandle, USHORT *StreamType) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="StreamType">流类型</param>
        /// <returns>成功返回0；失败返回错误号 </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetStreamType( IntPtr hChannelHandle, ref ushort StreamType );

        /// <summary>
        /// 3.4.3.3设置子通道编码流类型SetSubStreamType 
        ///     说  明：  设置子通道编码流类型，此函数需在启动编码前进行设置
        /// 
        /// int __stdcall SetSubStreamType(HANDLE hChannelHandle, USHORT Type) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="Type">流类型</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetSubStreamType( IntPtr hChannelHandle, ref ushort Type );

        /// <summary>
        /// 3.4.3.4获取子通道编码流类型GetSubStreamType
        ///     说  明：  获取子通道编码流类型
        /// 
        /// int __stdcall GetSubStreamType(HANDLE hChannelHandle, USHORT *StreamType) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="StreamType">流类型</param>
        /// <returns></returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetSubStreamType( IntPtr hChannelHandle, ref ushort StreamType );

        #endregion

        #region 3.4.4（支持动态修改）的编码参数设置

        /// <summary>
        /// 3.4.4.1设置编码图像质量SetDefaultQuant
        ///     说  明：  设置图像量化系数，用于调整图像质量。量化系数越小图像质量越高。系统默认量化系数值为18，18，23。 
        ///     释 义： 量化系数
        ///     量化系数是强烈影响MPEG标准中编码图像质量和码率的参数，当量化系数越低，图
        ///     像质量就会越高，码率也就越高，反之，图形质量就会越低，码率也就越低 
        /// 
        /// int __stdcall SetDefaultQuant(HANDLE hChannelHandle, int IQuantVal,  int PQuantVal, int BQuantVal) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="IQuantVal">I帧量化系数，取值范围：12-30</param>
        /// <param name="PQuantVal">P帧量化系数。取值范围：12-30(目前无效)</param>
        /// <param name="BQuantVal">B帧量化系数。取值范围：12-30（目前无效）</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetDefaultQuant( IntPtr hChannelHandle, int IQuantVal, int PQuantVal, int BQuantVal );

        /// <summary>
        /// 3.4.4.2设置编码帧结构、帧率SetIBPMode
        ///     说 明：  设置编码帧结构和帧率。支持动态修改
        ///     释 义： 关键帧间隔
        ///     关键帧为编码码流中采用帧内压缩的图像帧，其特点是图像清晰度好，但数据量大，通
        ///     常作为帧间编码的原始参考帧。关键帧间隔是连续的帧间编码的帧个数，因H264(MPEG4)
        ///     编码是有损压缩，关键帧的个数会影响图像质量，因此关键帧的间隔需要合理设计。
        /// 
        /// int __stdcall SetIBPMode(HANDLE hChannelHandle, int KeyFrameIntervals, int BFrames, int PFrames, int FrameRate) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="KeyFrameIntervals">关键帧间隔。取值范围１-400，系统默认为100</param>
        /// <param name="BFrames">B帧数量，取值为0或者2，系统默认为2 </param>
        /// <param name="PFrames">P帧数量。目前暂取值无效</param>
        /// <param name="FrameRate">帧率，帧率范围1-25（PAL）、1-30（NTSC）</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetIBPMode( IntPtr hChannelHandle, int KeyFrameIntervals, int BFrames, int PFrames, int FrameRate );

        #region 3.4.4.3设置编码分辨率

        /// <summary>
        /// 3.4.4.3.1设置主通道分辨率SetEncoderPictureFormat
        ///     说 明：  设置主通道编码分辨率。支持动态修改。
        /// 
        /// int __stdcall SetEncoderPictureFormat(HANDLE hChannelHandle,  PictureFormat_t PictureFormat) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="PictureFormat">编码图像分辨率（4CIF、DCIF、2CIF、CIF、QCIF） </param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetEncoderPictureFormat( IntPtr hChannelHandle, PictureFormat_t PictureFormat );

        /// <summary>
        /// 3.4.4.3.2设置子通道编码分辨率SetSubEncoderPictureFormat
        ///     说 明：  设置双编码模式时子通道的编码分辨率，支持动态修改。
        /// 
        /// int __stdcall SetSubEncoderPictureFormat(HANDLE hChannelHandle,  PictureFormat_t PictureFormat) 
        /// </summary>
        /// <param name="hChannelHandle">子通道句柄</param>
        /// <param name="PictureFormat">子通道编码图像分辨率（4CIF、DCIF、2CIF、CIF、 QCIF） </param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetSubEncoderPictureFormat( IntPtr hChannelHandle, PictureFormat_t PictureFormat );

        #endregion

        #region 3.4.4.4设置码率及码流控制模式

        /// <summary>
        /// 3.4.4.4.1设置码流最大比特率SetupBitrateControl 
        ///     说 明：  设置编码的最大比特率。设置为0时码流控制无效，设置为某一最大比特率时，
        ///     当编码码流超过该值时，DSP会自动调整编码参数来保证不超过最大比特率，当编码码流
        ///     低于最大比特率时，DSP不进行干涉。调整误差<10%
        /// 
        /// int __stdcall SetupBitrateControl(HANDLE hChannelHandle, ULONG MaxBps) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="MaxBps">最大比特率。取值：10000以上</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetupBitrateControl( IntPtr hChannelHandle, ulong MaxBps );

        /// <summary>
        /// 3.4.4.4.2设置码流控制方式SetBitrateControlMode
        ///     说 明：   设置编码码流控制方式。配合SetupBitrateControl使用。当设置为变码率（brVBR） 
        ///     时，最大比特率将作为编码码流上限，由DSP在码流上限下自动控制码率，一般会自动回
        ///     落到最低的状态（由设定的图像质量参数和关键帧间隔决定），能最大程度地降低带宽和存
        ///     储空间，但存储容量一般难以估算；当设置为定码率（brCBR）时，以最大比特率作为编码
        ///     码率参数恒定输出码流，不会自动回落到低码流状态，存储容量可根据设定码率的大小进行估算。
        /// 
        /// int __stdcall SetBitrateControlMode(HANDLE hChannelHandle, BitrateControlType_t brc) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="brc">码流控制方式，分为变码率（brVBR）和恒定码率 （brCBR）两种方式 </param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetBitrateControlMode( IntPtr hChannelHandle, BitrateControlType_t brc );


        #endregion

        /// <summary>
        /// 3.4.5强制设定I帧CaptureIFrame
        ///     说 明：  将当前编码帧强制设定为I帧模式，可从码流中提取该帧单独用于网络传送。
        /// 
        /// int __stdcall CaptureIFrame(HANDLE hChannelHandle)
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int CaptureIFrame( IntPtr hChannelHandle );

        /// <summary>
        /// 3.4.6获取帧统计信息GetFramesStatistics
        ///     说 明：  获取帧统计信息
        /// 
        /// int __stdcall GetFramesStatistics(HANDLE hChannelHandle,PFRAMES_STATISTICS framesStatistics) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="framesStatistics">帧统计信息</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetFramesStatistics( IntPtr hChannelHandle, PFRAMES_STATISTICS framesStatistics );

        #endregion

        #endregion

        #region 3.5数据捕获

        #region 3.5.1抓图（获取单帧图像数据）

        #region 3.5.1.1抓取BMP格式图像

        /// <summary>
        /// 3.5.1.1.1获取原始yuv422格式数据GetOriginalImage
        ///     说 明：  获得原始yuv422格式图像，DS4000HC原始图像是4CIF图像格式(包括QCIF编码)，
        ///     DS-4000HS原始图像为CIF图像格式，DS400xH卡的原始图象是CIF图象格式。
        /// 
        /// int __stdcall GetOriginalImage(HANDLE hChannelHandle, UCHAR *ImageBuf,  ULONG *Size) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="ImageBuf">原始yuv422格式图像指针</param>
        /// <param name="Size">原始yuv422格式图像尺寸，函数调用前是ImageBuf的大小，调用后是实际图像所占用的字节数 </param>
        /// <returns>成功返回0，失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetOriginalImage( IntPtr hChannelHandle, byte[] ImageBuf, out int Size );

        /// <summary>
        /// 3.5.1.1.2图像格式转换YUVtoBMP SaveYUVToBmpFile
        ///     说 明：  用户程序可调用此函数来生成24位的bmp文件，如果是DS4000HC卡抓图则
        ///     Width 为704，Height 为576PAL/480NTSC，如果是DS400xH卡抓图则Width可能为352 
        ///     或176，Height为288、240、144或120，要根据缓冲区的大小来判断。
        /// 
        /// int __stdcall SaveYUVToBmpFile(char *FileName, unsigned char *yuv, int Width, int Height) 
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="yuv">yuv422格式图像指针</param>
        /// <param name="Width">图像宽度</param>
        /// <param name="Height">图像高度</param>
        /// <returns></returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SaveYUVToBmpFile( string FileName, byte[] yuv, int Width, int Height );

        /// <summary>
        /// 3.5.1.2抓取JPEG格式图像GetJpegImage
        ///     说 明：  抓取JPEG格式图像
        /// 
        /// int __stdcall GetJpegImage(HANDLE hChannelHandle,UCHAR *ImageBuf, ULONG *Size,UINT nQuality) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="ImageBuf">JPEG图像指针</param>
        /// <param name="Size">JPEG图像尺寸，函数调用前是ImageBuf的大小，调用后是实际图像所占用的字节数 </param>
        /// <param name="nQuality">JPEG图像质量，取值范围1-100，取值100时质量最好</param>
        /// <returns></returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetJpegImage( IntPtr hChannelHandle, byte[] ImageBuf, out int Size, uint nQuality );

        #endregion

        #region 3.5.2原始图像数据流捕获（获取YUV420格式数据流）

        /// <summary>
        /// 3.5.2.1注册原始图像数据流回调函数RegisterImageStreamCallback
        ///     说 明：  注册获取原始图像数据流函数，用户可以获取实时的YUV420格式的预览数据
        /// 
        /// int __stdcall RegisterImageStreamCallback (IMAGE_STREAM_CALLBACK ImageStreamCallback,void *context) 
        /// </summary>
        /// <param name="ImageStreamCallback">原始图像数据流回调函数 </param>
        /// <param name="context">设备上下文</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int RegisterImageStreamCallback( IMAGE_STREAM_CALLBACK ImageStreamCallback, IntPtr context );

        /// <summary>
        /// 3.5.2.2开启及停止原始数据流捕获SetImageStream
        ///     说明：    开启或停止原始图像数据流捕获，此函数依赖主机的处理速度。DS-4000HS只能捕获不大于CIF格式的数据流
        /// 
        /// 函 数：  int __stdcall SetImageStream(HANDLE hChannel,BOOL bStart,UINT fps, UINT width,UINT height,unsigned char *imageBuffer) 
        /// </summary>
        /// <param name="hChannel">通道句柄</param>
        /// <param name="bStart">是否启动捕获</param>
        /// <param name="fps">帧率</param>
        /// <param name="width">图像宽度，必须是4CIF宽度的1/8，1/4，1/2或原始大小704</param>
        /// <param name="height">图象高度，必须是4CIF高度的1/8，1/4，1/2或原始大小576PAL/480NTSC</param>
        /// <param name="imageBuffer">数据存储缓存</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetImageStream( IntPtr hChannel, bool bStart, uint fps, uint width, uint height, char[] imageBuffer );

        #endregion

        #region 3.5.3编码数据流捕获（获取编码后H.264格式数据流）即录像

        //注意：注册直接回调或者消息回调后，需要启动编码数据流捕获函数才能进行数据回调。三
        //种数据回调方式，只需选取其中一种使用即可。对于HC系列板卡（包括HC、HC+、HCS、
        //HS），推荐使用第一种读取方式。对于H系列板卡，只能使用后两种读取方式。

        #region 3.5.3.1编码数据流捕获方式设置

        /// <summary>
        /// 3.5.3.1.1.1注册编码图像数据流直接读取回调函数
        ///     说 明：  DS4000HC系列板卡新增的一种数据流读取方式，当启动数据捕获后，   
        ///     StreamDirectReadCallback会提供数据流的地址、长度、帧类型等，供用户程序直接处理。
        /// 
        /// 函 数：  int __stdcall RegisterStreamDirectReadCallback (STREAM_DIRECT_READ_CALLBACK StreamDirectReadCallback,void *Context) 
        /// </summary>
        /// <param name="StreamDirectReadCallback">编码数据流直接读取回调函数</param>
        /// <param name="context">设备上下文</param>
        /// <returns>成功返回0；失败返回错误号</returns>

        [DllImport( "DS40xxSDK.dll" )]
        public static extern int RegisterStreamDirectReadCallback( STREAM_DIRECT_READ_CALLBACK StreamDirectReadCallback, IntPtr context );

        #endregion

        #region 3.5.3.1.2方式二、消息读取方式

        /// <summary>
        /// 3.5.3.1.2.1设置消息读取伐值，此函数只对H卡有效
        ///     说 明：  设置消息读取的伐值，可以将缓冲区的数据在OnDataReady中一次性取走
        /// 
        /// int __stdcall SetupNotifyThreshold(HANDLE hChannelHandle,  int iFramesThreshold) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="iFramesThreshold">读取消息伐值，范围1-10</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetupNotifyThreshold( IntPtr hChannelHandle, int iFramesThreshold );

        /// <summary>
        /// 3.5.3.1.2.2注册消息读取码流函数
        ///     说 明：  当数据准备好时，SDK会向hWnd窗口发送MessageId消息，目标窗口收到
        ///     Message后调用ReadStreamData读取一帧数据。如果HC卡与H卡混插，可以先调用 
        ///     RegisterStreamDirectReadCallback函数来注册HC卡取码流回调函数，再调用
        ///     RegisterMessageNotifyHandle函数来注册H卡取码流消息函数。
        ///     HC系列板卡建议使用方式一进行数据捕获。 
        /// 
        /// int __stdcall RegisterMessageNotifyHandle(HWND hWnd, UINT MessageId)
        /// </summary>
        /// <param name="hWnd">通道句柄</param>
        /// <param name="MessageId">自定义消息</param>
        /// <returns>成功返回0；失败返回错误号 </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int RegisterMessageNotifyHandle( IntPtr hWnd, uint MessageId );

        #endregion

        #region 3.5.3.1.3方式三、另一种直接读取方式

        /// <summary>
        /// 3.5.3.1.3.1注册直接读取码流回调函数
        ///     说 明：  另一种数据流读取方式
        /// 
        /// int __stdcall RegisterStreamReadCallback (STREAM_READ_CALLBACK StreamReadCallback, void *Context) 
        /// </summary>
        /// <param name="StreamReadCallback">直接读取码流回调函数</param>
        /// <param name="Context">设备上下文</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int RegisterStreamReadCallback( STREAM_READ_CALLBACK StreamReadCallback, IntPtr Context );

        #endregion

        /// <summary>
        /// 3.5.3.2读取码流函数
        ///     说 明：  读指定长度的数据流，适用于方式二及方式三。当调用StartVideoCapture 或
        ///     StartMotionDetection后，SDK线程会向已注册的用户窗口消息处理函数发送指定的消息，
        ///     并提供消息来源的通道号。当用户程序收到该消息时，可调用本函数来读取数据，Length 在
        ///     作为输入时必须提供缓冲的大小，ReadStreamData会判断缓冲是否足够，如果缓冲足够大
        ///     则返回值为当前的读取的帧长度，否则返回错误。
        ///         在HC卡中，如果已经先调用了RegisterStreamDirectReadCallback()函数，则不需调用 
        ///     ReadStreamData来读取数据，因为音视频数据会在RegisterStreamDirectReadCallback所注册
        ///     的回调函数中直接返回。
        /// 
        /// int __stdcall ReadStreamData(HANDLE hChannelHandle, void *DataBuf,  DWORD *Length, int *FrameType) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="DataBuf">自定义的数据缓存区</param>
        /// <param name="Length">输入：缓存区的大小；输出：一帧数据的大小</param>
        /// <param name="FrameType">帧类型</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int ReadStreamData( IntPtr hChannelHandle, byte[] DataBuf, out ulong Length, out int FrameType );

        #region 3.5.3.3开启及停止录像

        /// <summary>
        /// 3.5.3.3.1启动主通道编码数据流捕获
        ///     说 明：  启动主通道编码数据流捕获。用户程序可以使用直接读取方式，使用
        ///     StreamDirectReadCallback回调函数直接对数据流进行处理；也可以与H卡一样，通过消息
        ///     读取方式，等SDK向用户程序发送在RegisterMessageNotifyHandle中注册的消息，用户程
        ///     序使用ReadStreamData来读取数据流。
        /// 
        /// int __stdcall StartVideoCapture(HANDLE hChannelHandle) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StartVideoCapture( IntPtr hChannelHandle );

        /// <summary>
        /// 3.5.3.3.2停止主通道编码数据流捕获
        ///     说 明：  停止主通道编码数据流捕获
        /// 
        /// int __stdcall StopVideoCapture(HANDLE hChannelHandle)
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StopVideoCapture( IntPtr hChannelHandle );

        /// <summary>
        /// 3.5.3.3.3启动子通道编码数据流捕获
        ///     说 明：  启动子通道编码数据流捕获 
        /// 
        /// int __stdcall StartSubVideoCapture(HANDLE hChannelHandle)
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StartSubVideoCapture( IntPtr hChannelHandle );


        /// <summary>
        /// 3.5.3.3.4停止子通道编码数据流捕获
        ///     说 明：  停止子通道编码数据流捕获
        /// 
        /// int __stdcall StopSubVideoCapture(HANDLE hChannelHandle) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>成功返回0；失败返回错误号 </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StopSubVideoCapture( IntPtr hChannelHandle );

        #endregion

        #endregion

        #endregion

        #endregion

        #region 3.6移动侦测

        //        释 义： 移动侦测 
        //DS4000HC提供运动强度信息来处理运动检测，设置移动侦测区域时以32*32像素块为
        //单位，按4CIF（704*576）分辨率计算，每行有22个块（704/32），PAL时18行（576/32），
        //NTSC时15行（480/32），与编码格式无关。经过测试，这种方法比H卡提高了灵敏度和可
        //靠性，并简化了返回的数据，返回的值是18个DWORD，对应屏幕高度576/32=18行（PAL）,
        //每个DWORD的0-21位对应屏幕宽度704/32=22, 其中1为运动,0为静止，也可以调用原有
        //MotionAnalyzer分析结果 
        //4.0版本的SDK新增了接口函数SetupMotionDetectionEx，提供了更灵活的功能，并且
        //简化了用户的工作量。 

        #region 3.6.1设置方式一

        //        设置移动侦测相关参数并启动移动侦测后，运动检测信息会通过数据流传送，用户程序
        //发现PktMotionDetection帧类型时，可调用MotionAnalyzer来处理运动信息，结果由
        //MotionAnalyzer在iResult中返回。也可以按照SDK提供的数据格式来自己分析，运动信息格
        //式参见移动侦测释义。 

        /// <summary>
        /// 3.6.1.1设置移动侦测灵敏度
        ///     说 明：  调整运动分析的灵敏度，支持动态调整，此函数决定DSP全局运动分析的灵敏度。
        ///     而MotionAnalyzer的iThreshold则主要用于分析指定区域的运动统计结果。
        /// 
        /// int __stdcall AdjustMotionDetectPrecision(HANDLE hChannelHandle, int iGrade, int iFastMotionDetectFps, int iSlowMotionDetectFps) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="iGrade">
        ///     运动分析灵敏度等级，取值范围0-6，0级最灵敏，6级最迟钝，推荐值为2。
        ///     将iGrade和“0x80000000“做”或“操作，会对移动侦测启用自适应分析。
        /// </param>
        /// <param name="iFastMotionDetectFps">高速运动检测的帧间隔，取值范围0-12，0表示不作高速运动检测，通常值取为2</param>
        /// <param name="iSlowMotionDetectFps">低速运动检测的帧间隔，取值范围13以上，当取值为0时，不作低速运动检测</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int AdjustMotionDetectPrecision( IntPtr hChannelHandle, int iGrade, int iFastMotionDetectFps, int iSlowMotionDetectFps );


        /// <summary>
        /// 3.6.1.2设置移动侦测区域范围及个数
        ///     说 明：  设置运动检测区域；当收到运动信息的数据帧（PktMotionDetection）时,调用
        ///     MotionAnalyzer；MotionAnalyzer会根据在SetupMotionDetection中的设置来分析每个需要
        ///     检测的区域，当某区域的阀值(MotionAnalyzer的iThreshold)到达时，会在返回的区域数组    
        ///     （MotionAnalyzer的iResult）标明最后的判断；矩形框范围是（0，0，703，575）
        /// 
        /// int __stdcall SetupMotionDetection(HANDLE hChannelHandle, RECT *RectList, int iAreas) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="RectList">矩形框数组</param>
        /// <param name="iAreas">矩形框个数，最大值为100</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetupMotionDetection( IntPtr hChannelHandle, ref Rectangle RectList, int iAreas );

        /// <summary>
        /// 3.6.1.3移动侦测分析
        ///     说 明：  动态监测分析，移动侦测由DSP完成，DSP送出的IPktMotionData帧就是已经
        ///     分析好的运动信息，区域的运动分析由主机完成，数据源由码流中的PktMotionData帧提供，
        ///     结果在iResult中说明，2．0以上版本的运动分析基于DSP提供的运动强度，不再使用运动
        ///     矢量，其灵敏度及可靠性有大幅提高，用户软件可使用由码流提供的运动强度信息来自己分
        ///     析或调用该函数来进行区域分析
        /// 
        /// 函 数：  int __stdcall MotionAnalyzer(HANDLE hChannelHandle, char *MotionData,  int iThreshold, int *iResult) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="MotionData">运动矢量指针</param>
        /// <param name="iThreshold">判断运动程度的伐值</param>
        /// <param name="iResult">按照伐值指定的强度分析后的结果数组。数组的大小在SetupMotionDetection的numberOfAreas指定，如果某数组单元的值大于零则表明有该区域有该值表明的运动强度</param>
        /// <returns>成功返回0；失败返回错误号 </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int MotionAnalyzer( IntPtr hChannelHandle, char[] MotionData, int iThreshold, ref int iResult );
        #endregion

        #region 3.6.2设置方式二

        /// <summary>
        /// 3.6.2.1设置移动侦测（扩展）
        ///     说 明：  设置移动侦测，这种设置方式可替代设置方式一中3个函数的共同作用，在这种情况下，DSP将不再反馈移动侦测帧。
        ///     参数delay：画面静止之后的延时时间，单位为秒，若在该延时时间内没有产生
        ///     移动侦测，则将回调函数MotionDetectionCallback之中的参数bMotionDetected标志为False，
        ///     若在该延时时间之内，在当前所设置的区域内产生移动侦测，则bMotionDetected被标志为
        ///     True，并且在产生移动侦测之后的delay时间内，DSP不会对在此时间段之内的视频帧进行
        ///     移动侦测分析，因此DSP和主机都省却了在此时间段对产生的视频运动进行频繁判断和分
        ///     析。直至超过了此delay秒延时时间，DSP才会对此时刻的视频进行判断，若产生了移动侦
        ///     测，则回调函数中的bMotionDetected被再次标志为True，否则标志为False。
        ///     
        /// 
        /// 函 数：  int __stdcall SetupMotionDetectionEx(HANDLE hChannelHandle,int iGrade, int iFastMotionDetectFps, int iSlowMotionDetectFps,UINT delay,RECT *RectList, int  iAreas, MOTION_DETECTION_CALLBACK MotionDetectionCallback,int reserved) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="iGrade">
        ///     运动分析灵敏度等级，取值范围0-6，0级最灵敏，6级最迟钝， 
        ///     推荐值2。将iGrade和“0x80000000“做”或“操作，会对移动侦测启用 
        ///     自适应分析。
        /// </param>
        /// <param name="iFastMotionDetectFps">高速运动检测的帧间隔，取值范围0-12，0表示不作高速运动检测，通常值取为2</param>
        /// <param name="iSlowMotionDetectFps">低速运动检测的帧间隔，取值范围13以上，当取值为0时，不作低速运动检测</param>
        /// <param name="delay">前一次移动帧测产生后的延时时间</param>
        /// <param name="RectList">进行移动侦测的矩形框数组</param>
        /// <param name="iAreas">矩形框个数（最大个数为100）</param>
        /// <param name="MotionDetectionCallback">移动侦测结果回调函数 </param>
        /// <param name="reserved">保留参数</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetupMotionDetectionEx( IntPtr hChannelHandle, int iGrade, int iFastMotionDetectFps, int iSlowMotionDetectFps, uint delay, ref Rectangle RectList, int iAreas, MOTION_DETECTION_CALLBACK MotionDetectionCallback, int reserved );


        #endregion

        #region 3.6.3启动及停止移动侦测

        /// <summary>
        /// 3.6.3.1启动移动侦测 
        ///     说 明：  启动移动侦测。
        ///     注意：移动侦测和编码相互独立，用户程序可在不启动编码的情况下进行运动检测
        /// 
        /// int __stdcall StartMotionDetection(HANDLE hChannelHandle) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StartMotionDetection( IntPtr hChannelHandle );

        /// <summary>
        /// 3.6.3.2停止移动侦测
        ///     说 明：  停止移动侦测
        /// 
        /// int __stdcall StopMotionDetection(HANDLE hChannelHandle) 
        /// </summary>
        /// <param name="hChannelHandle"></param>
        /// <returns></returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StopMotionDetection( IntPtr hChannelHandle );

        #endregion

        #endregion

        #region 3.7视频信息叠加

        #region 3.7.1信息叠入视频编码（OSD、LOGO、MASK）

        //注意：使用此部分函数时，在录像文件中，包含所叠加的信息 

        #region 3.7.1.1 OSD

        /// <summary>
        /// 3.7.1.1.1设置OSD显示模式
        /// 
        /// 函 数：  int __stdcall SetOsdDisplayMode(HANDLE hChannelHandle, int Brightness,  BOOL Translucent, int parameter, USHORT *Format1, USHORT *Format2) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="Brightness">OSD显示亮度。0最暗，255最亮</param>
        /// <param name="Translucent">OSD图像是否做半透明处理</param>
        /// <param name="parameter">Bit0：是否自动进行颜色翻转 Bit16-23垂直放大倍数 Bit24-31水平 </param>
        /// <param name="Format1">描述字符叠加的位置和次序的格式</param>
        /// <param name="Format2"></param>
        #region USHORT    *Forma1, *Format2
        //USHORT    *Forma1, *Format2 
        //描述字符叠加的位置和次序的格式串，具体定义如下： 
        //USHORT X， USHORT Y， CHAR0， CHAR1， CHAR2，… CHARN,  NULL 
        //  其中X，Y 是该字串在标准4CIF图象的起始位置，X必须是16的倍数，Y可以在图
        //象高度内取值即（0-575）PAL 、（0-479）NTSC；CHARN也是USHORT型的参数，可以
        //是ASCII码也可以是汉字，当想要显示当前时间时，可以指定为固定的时间定义值，其值
        //如下： 
        //_OSD_YEAR4   四位的年显示，如2004 
        //_OSD_YEAR2   两位的年显示，如02 
        //_OSD_MONTH3   英文的月显示，如 Jan  
        //_OSD_MONTH2   两位阿拉伯数字的月显示，如07 
        //_OSD_DAY     两位的阿拉伯数字的日显示，如31 
        //_OSD_WEEK3   英文的星期显示，如Tue 
        //_OSD_CWEEK1   中文的星期显示，如星期二 
        //    _OSD_HOUR24   24小时的时钟显示，如18 
        //_OSD_HOUR12   12小时的时钟显示，如AM09或PM09 
        //_OSD_MINUTE   两位分钟的显示 
        //_OSD_SECOND   两位秒的显示 
        //在格式字符串的最后必须以NULL（0）结尾，否则会显示错误的内容。 
        //字符串和时间显示可以在FORMAT1 也可以在FORMAT2，也可以混合在一起，但不得超
        //过一行4CIF 图象的宽度。 
        //例如： 
        //要显示位置在16，19的字符串“办公室”的格式字符串如下： 
        //  USHORT Format[] = {16, 19, ‘办’,’公’,’室’, ‘\0’}; 
        //要显示位置在8， 3的时间字符串可以如下： 
        //  USHORT Format[]={8, 3, _OSD_YEAR4, ‘年’,_OSD_MONTH2,’月’,_OSD_DAY, 
        //‘日’,_OSD_HOUR24,’:’, _OSD_MINUTE, ‘:’, _OSD_SECOND, ‘\0’}; 
        //如果只想显示其中一行，则将起始的字符串定义如下： 
        //  USHORT FormatNoDisplay[]={0, 0, ‘\0’}; 
        //返回值：  成功返回0；失败返回错误号 
        //说 明：  OSD字符中，ASCII字符的标准分辨率为8×16，汉字的标准分辨率为16×16。 
        //由于在编码之前需要对原始图像进行缩小才能产生编码所需的分辨率，此时为了保证在
        //缩小后的编码图像上能够看清OSD字符，就需要先把OSD字符放大以后再叠加在4CIF的
        //原始图像上。 
        //如果不指定放大倍数（采用默认设置），则系统会根据该通道录像的分辨率自动设置，
        //这样在任何分辨率下都可以保证回放时能够看清OSD内容，但是这会导致OSD的大小和位
        //置在原始图像中不固定。 
        //为了避免上面的现象，用户可以指定OSD的大小。例如，如果应用程序想以CIF、DCIF、
        //2CIF、4CIF的分辨率录像，这时候可以将放大系数设为2、2，此时OSD的位置始终固定，
        //但在不同的编码分辨率下，OSD字符的分辨率也不同，所以需要特别注意。如果此时使用
        //QCIF录像，则OSD字符会变得模糊不清（因为QCIF要对图像进行1/4缩小，而对OSD
        //字符只进行了2倍的放大）。具体配置详见下表： 
        //水平放大倍数  垂直放大倍数  适合的录像分辨率  说明 
        //1  1  4CIF  其它分辨率下会模糊 
        //1  2  2CIF  小于2CIF时无法分辩 
        //2  2  CIF、DCIF  QCIF时无法分辩 
        //4  4  QCIF  在其它分辨率下字符会很大 
        //任意系数为0  自动设置（默认值）  
        //其它无效值  按水平2、垂直2处理  
        //注意：因为字符的位置会随着不同的录像分辨率而改变，在位置改变后，某些OSD字符的
        //位置可能会超出图像的范围，此时这些字符将无法显示，但系统并不会返回错误。 
        #endregion
        /// <returns></returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetOsdDisplayMode( IntPtr hChannelHandle, int Brightness, bool Translucent, int parameter, ushort[] Format1, ushort[] Format2 );

        /// <summary>
        /// 3.7.1.1.2设置OSD显示模式（扩展）
        ///     说 明：  此函数为SetOsdDisplayMode的扩展，SetOsdDisplayModeEx函数支持最多8行OSD字符串的显示。
        /// 
        /// int __stdcall SetOsdDisplayModeEx(HANDLE hChannelHandle,int color, BOOL Translucent,int param,int nLineCount,USHORT **Format) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="Brightness">OSD显示亮度。0最暗，255最亮</param>
        /// <param name="Translucent">OSD图像是否做半透明处理</param>
        /// <param name="param">Bit0：是否自动进行颜色翻转 Bit16-23垂直放大倍数 Bit24-31水平放大倍数</param>
        /// <param name="nLineCount">OSD显示的行数，最多为8行</param>
        /// <param name="Format">多行字符显示
        /// USHORT **Format；多行字符显示，描述字符叠加的位置和次序的格式串，  
        /// 其中每一行的第一元素X和第二元素Y 是该字串在标准4CIF图象的起始位置，X必须是
        /// 16的倍数，Y可以在图象高度内取值即（0-575）PAL 、（0-479）NTSC；可以是ASCII码
        /// 也可以是汉字，当想要显示当前时间时，可以指定为固定的时间定义值，其值如下： 
        /// _OSD_YEAR4   四位的年显示，如2004 
        /// _OSD_YEAR2   两位的年显示，如02 
        /// _OSD_MONTH3   英文的月显示，如 Jan  
        /// _OSD_MONTH2   两位阿拉伯数字的月显示，如07 
        /// _OSD_DAY     两位的阿拉伯数字的日显示，如31 
        /// _OSD_WEEK3   英文的星期显示，如Tue 
        /// _OSD_CWEEK1   中文的星期显示，如星期二 
        /// _OSD_HOUR24   24小时的时钟显示，如18 
        /// _OSD_HOUR12   12小时的时钟显示，如AM09或PM09 
        /// _OSD_MINUTE   两位分钟的显示 
        /// _OSD_SECOND   两位秒的显示 
        /// 在格式字符串的每一行最后一个元素必须以NULL（0）结尾，否则会显示错误的内容。 
        /// 返回值：  成功返回0；失败返回错误号。 
        /// 说 明：  此函数为SetOsdDisplayMode的扩展，SetOsdDisplayModeEx函数支持最多8行
        /// OSD字符串的显示。 
        /// </param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetOsdDisplayModeEx( IntPtr hChannelHandle, int Brightness, bool Translucent, int param, int nLineCount, char[] Format );

        /// <summary>
        /// 3.7.1.1.3设置OSD显示
        ///     说 明：  设置OSD显示，将当前的系统时间年月日星期时分秒等信息叠加在视频之上，并可作透明处理。
        /// 
        /// int __stdcall SetOsd(HANDLE hChannelHandle, BOOL Enable) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="Enable">OSD是否显示</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetOsd( IntPtr hChannelHandle, bool Enable );

        #endregion

        #region 3.7.1.2 LOGO

        /// <summary>
        /// 3.7.1.2.1数据格式转换
        ///     说 明：  将24位bmp格式图像转换为yuv422格式图像。
        ///     注意：bmp位图的长宽必须为16的倍数，图像尺寸最大支持128*128，4.3版本SDK图像尺寸扩大为256*128
        /// 
        /// 函 数：  int __stdcall LoadYUVFromBmpFile(char *FileName, unsigned char *yuv,  int BufLen, int *Width, int *Height) 
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="yuv">YUV422图像指针 </param>
        /// <param name="BufLen">YUV422图像缓存大小</param>
        /// <param name="Width">返回的YUV422图像的宽度</param>
        /// <param name="Height">返回的YUV422图像的高度</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int LoadYUVFromBmpFile( string FileName, string yuv, int BufLen, ref int Width, ref int Height );

        /// <summary>
        /// 3.7.1.2.2设置LOGO显示模式
        ///     说 明：  设置LOGO显示模式
        /// 
        /// 函 数：  int __stdcall SetLogoDisplayMode(HANDLE hChannelHandle,  COLORREF ColorKey, BOOL Translucent, int TwinkleInterval) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="ColorKey">LOGO图像中该颜色在显示时将会全透明</param>
        /// <param name="Translucent">LOGO图像是否作半透明处理</param>
        /// <param name="TwinkleInterval">闪烁的时间设置，由16进制数表示为0xXXYY，其中XX为显示的秒数，YY为停止的秒数，XXYY均为0时正常显示。</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetLogoDisplayMode( IntPtr hChannelHandle, Color ColorKey, bool Translucent, int TwinkleInterval );

        /// <summary>
        /// 3.7.1.2.3设置LOGO显示位置及数据
        ///     说 明：  设置LOGO图像位置及数据，用户程序可先调用LoadYUVFromBmpFile将24
        ///     位bmp文件中转化为YUV422格式数据，,透明处理由DSP完成。
        ///     注意：HS卡的x，w需要按照32对齐，y，h仍为8对齐。
        /// 
        /// 函 数：  int __stdcall SetLogo(HANDLE hChannelHandle, int x, int y, int w, int h,  unsigned char *yuv) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="x">LOGO左上角x坐标位置，取值范围0-703，需按16对齐</param>
        /// <param name="y">LOGO左上角y坐标位置，取值范围0-575，需按8对齐</param>
        /// <param name="w">LOGO宽度，最大值为256，需按16对齐，此宽度必须和LOGO图片的宽度相一致</param>
        /// <param name="h">LOGO高度，最大值为128，需按8对齐</param>
        /// <param name="yuv">LOGO图片指针（yuv422格式）</param>
        /// <returns>成功返回0；失败返回错误号 </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetLogo( IntPtr hChannelHandle, int x, int y, int w, int h, string yuv );

        /// <summary>
        /// 3.7.1.2.4停止LOGO显示
        ///     说 明：  停止LOGO显示
        /// 
        /// int __stdcall StopLogo(HANDLE hChannelHandle)
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StopLogo( IntPtr hChannelHandle );

        #endregion

        #region 3.7.1.3视频遮挡MASK

        /// <summary>
        /// 3.7.1.3.1设置屏幕遮挡
        ///     说 明：  设置屏幕遮挡，最多可以设置32个
        /// 
        /// int __stdcall SetupMask(HANDLE hChannelHandle, RECT *RectList, int iAreas) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="RectList">矩形框数组，宽度范围0-704，按16对齐；高度范围0-576，按</param>
        /// <param name="iAreas">矩形框个数</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetupMask( IntPtr hChannelHandle, ref Rectangle RectList, int iAreas );

        /// <summary>
        /// 3.7.1.3.1停止屏幕遮挡
        ///     说 明：  停止屏幕遮挡
        /// 
        /// int __stdcall StopMask(HANDLE hChannelHandle) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StopMask( IntPtr hChannelHandle );

        #endregion

        #endregion

        #region 3.7.2仅在预览画面上叠加信息

        //        注意：当采用overlay预览模式时，可在overlay表面上直接叠加字符或画图，当采用offscreen
        //预览模式时，需要调用画图回调函数进行信息叠加，所叠加信息仅在预览屏幕上显示，不进
        //入编码。 

        //Offscreen预览模式下画图回调函数

        /// <summary>
        /// 3.7.2.1注册画图回调函数
        ///     说 明：  获取当前offscreen表面的device context，HC系列板卡采用创建offscreen的方 
        ///     式，所以在窗口客户区中的DC中无法画图或者鞋子，必须使用DirectDraw里的offscreen表面的DC。
        ///     注意：如果采用overlay预览模式，则直接在overlay表面画图即可，无需调用此函数
        /// 
        /// int __stdcall RegisterDrawFun(DWORD nport, DRAWFUN(DrawFun),LONG nUser)
        /// </summary>
        /// <param name="nport">通道号索引</param>
        /// <param name="df">画图回调函数</param>
        /// <param name="nUser">用户数据</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int RegisterDrawFun( ulong nport, DrawFun df, long nUser );
        //public static extern int RegisterDrawFun(ulong nport, DrawFun df, IntPtr nUser);

        /// <summary>
        /// 3.7.2.2停止画图回调
        ///     说 明：  停止画图回调。在某些显卡上进行画图回调，会使得CPU的利用率变高，所以可以在适当的时候（画图回调结束）停止调用。
        /// 
        /// int __stdcall StopRegisterDrawFun(DWORD nport) 
        /// </summary>
        /// <param name="nport">通道索引</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int StopRegisterDrawFun( ulong nport );



        #endregion

        #endregion

        #region 3.8音频

        /// <summary>
        /// 3.8.1设置音频预览
        ///     说 明：  设置音频预览与否，同一时间，系统只支持一路音频预览。需要将4针线和声卡音频输入口联接。
        /// 
        /// int __stdcall SetAudioPreview(HANDLE hChannelHandle, BOOL bEnable) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <param name="bEnable">使能</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetAudioPreview( IntPtr hChannelHandle, bool bEnable );

        /// <summary>
        /// 3.8.2获取音频输入音量幅度
        ///     说 明：  获取当前通道的现场声音幅度。
        ///     注意：当无声音输入时因背景噪声的原因返回值并不为0。 
        /// 
        /// int __stdcall GetSoundLevel(HANDLE hChannelHandle) 
        /// </summary>
        /// <param name="hChannelHandle">通道句柄</param>
        /// <returns>当前通道的音频输入幅度</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int GetSoundLevel( IntPtr hChannelHandle );


        #endregion

        #region 3.9其他

        /// <summary>
        /// 3.9.1复位DSP
        ///     此函数目前无效
        ///     说 明：  复位某个DSP，注意请谨慎调用该函数，请确定DSP故障无法软件恢复时再关闭相关的资源后复位DSP。
        /// 
        /// int __stdcall ResetDSP(int DspNumber);
        /// </summary>
        /// <param name="DspNumber">DSP索引号</param>
        /// <returns>  成功返回0；失败返回错误号 </returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int ResetDSP( int DspNumber );

        /// <summary>
        /// 3.9.2设置看门狗
        ///     说 明：  设置看门狗。DS-4016HCS提供4pin复位接口，用户需要把主机机箱的Reset
        ///     接线连接到板卡上相邻的2pin复位接口，板卡上的另外相邻的2pin接口连接到主板的Reset，
        ///     这样就可以实现对上层软件和系统中所有压缩板卡的运行状态监控。
        /// 
        /// int __stdcall SetWatchDog(UINT boardNumber,BOOL bEnable) 
        /// </summary>
        /// <param name="boardNumber">板卡索引</param>
        /// <param name="bEnable">使能</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetWatchDog( uint boardNumber, bool bEnable );

        #region 3.9.3码流数字水印校验

        /// <summary>
        /// 3.9.3.1设置主通道数字水印校验
        ///     说 明：  此函数不支持动态设置，设置后会在下一次启动录像后生效。
        /// 
        /// int __stdcall SetChannelStreamCRC(HANDLE hChannel,BOOL bEnable) 
        /// </summary>
        /// <param name="hChannel">通道句柄</param>
        /// <param name="bEnable">使能</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetChannelStreamCRC( IntPtr hChannel, bool bEnable );

        /// <summary>
        /// 3.9.3.2设置子通道数字水印校验
        ///     说 明：  此函数不支持动态设置，设置后会在下一次启动录像后生效。
        /// 
        /// int __stdcall SetSubChannelStreamCRC(HANDLE hChannel,BOOL bEnable)
        /// </summary>
        /// <param name="hChannel">通道句柄</param>
        /// <param name="bEnable">使能</param>
        /// <returns>成功返回0；失败返回错误号</returns>
        [DllImport( "DS40xxSDK.dll" )]
        public static extern int SetSubChannelStreamCRC( IntPtr hChannel, bool bEnable );


        #endregion

        #endregion

        #endregion

    }
}

#ifndef __DATAPATH_RGBDEFS_INCLUDED_
#define __DATAPATH_RGBDEFS_INCLUDED_

#define RGBCAPTURE_FLAG_CURRENT                    0x00000000    //��ʾ��ǰ�Ĳɼ�����
#define RGBCAPTURE_FLAG_REAL_WORLD                 0x04000000    //���ڽ���־��ע��ֵת��Ϊʵ��ֵ
#define RGBCAPTURE_FLAG_DETECT                     0x08000000    //��ʾ���Ĳɼ�����
#define RGBCAPTURE_FLAG_MINIMUM                    0x10000000    //��ʾ��С�Ĳɼ�����  
#define RGBCAPTURE_FLAG_DEFAULT                    0x20000000    //��ʾĬ�ϵĲɼ�����
#define RGBCAPTURE_FLAG_MAXIMUM                    0x40000000    //��ʾ���Ĳɼ�����
#define RGBCAPTURE_FLAG_TEST                       0x80000000    //��ʾ���ԵĲɼ�����

#define RGBCAPTURE_PARM_INPUT                      0x00000001    //��Ӧ�ɼ�������Input(����˿�)
#define RGBCAPTURE_PARM_FORMAT                     0x00000002    //��Ӧ�ɼ�������Format(�����źŸ�ʽ)
#define RGBCAPTURE_PARM_VDIF_CLOCKS                0x00000004    //��Ӧ�ɼ�������VideoTimings.PixelClock(����ʱ��)
#define RGBCAPTURE_PARM_VDIF_FLAGS                 0x00000008    //��Ӧ�ɼ�������VideoTimings.Flags(ͼ���־)
#define RGBCAPTURE_PARM_VDIF_HTIMINGS              0x00000010    //��Ӧ�ɼ�������HorOffset(ˮƽλ��)
#define RGBCAPTURE_PARM_VDIF_VTIMINGS              0x00000020    //��Ӧ�ɼ�������HorOffset(��ֱλ��)
#define RGBCAPTURE_PARM_VDIF_DESCRIPTION           0x00000040    //��Ӧ�ɼ�������VideoTimings.Description(ͼ������)
#define RGBCAPTURE_PARM_PHASE                      0x00000080    //��Ӧ�ɼ�������Phase(��λ)
#define RGBCAPTURE_PARM_BRIGHTNESS                 0x00000100    //��Ӧ�ɼ�������Brightness(����)
#define RGBCAPTURE_PARM_CONTRAST                   0x00000200    //��Ӧ�ɼ�������Contrast(�Աȶ�)
#define RGBCAPTURE_PARM_BLACKLEVEL                 0x00000400    //��Ӧ�ɼ�������BlackLevel(��ɫ��)     
#define RGBCAPTURE_PARM_SAMPLERATE                 0x00000800    //��Ӧ�ɼ�������SampleRate(�����ٶ�)
#define RGBCAPTURE_PARM_PIXELFORMAT                0x00001000    //��Ӧ�ɼ�������PixelFormat(������ʾģʽ)
#define RGBCAPTURE_PARM_HWND                       0x00002000    //��Ӧ�ɼ�������HWnd(������Ϣ���ھ��)
#define RGBCAPTURE_PARM_SYNCEDGE                   0x00004000    //��Ӧ�ɼ�������SyncEdge(ͬ����)
#define RGBCAPTURE_PARM_SCALED_SIZE                0x00020000    //��Ӧ�ɼ�������HScaled��VScaled(���ű�)
#define RGBCAPTURE_PARM_NOTIFY_FN                  0x00040000    //��Ӧ�ɼ�������
#define RGBCAPTURE_PARM_NOTIFY_FLAGS               0x00080000    //��Ӧ�ɼ�������NotifyFlags(֪ͨ��Ϣ)
#define RGBCAPTURE_PARM_BLOCKING                   0x00100000    //��Ӧ�ɼ�������Flags(����ģʽ)

#define RGBCAPTURE_PARM_ALL                                             \
    (RGBCAPTURE_PARM_INPUT            | RGBCAPTURE_PARM_FORMAT          \
   | RGBCAPTURE_PARM_VDIF_CLOCKS      | RGBCAPTURE_PARM_VDIF_FLAGS      \
   | RGBCAPTURE_PARM_VDIF_HTIMINGS    | RGBCAPTURE_PARM_VDIF_VTIMINGS   \
   | RGBCAPTURE_PARM_VDIF_DESCRIPTION | RGBCAPTURE_PARM_PHASE           \
   | RGBCAPTURE_PARM_BRIGHTNESS       | RGBCAPTURE_PARM_CONTRAST        \
   | RGBCAPTURE_PARM_BLACKLEVEL       | RGBCAPTURE_PARM_SAMPLERATE      \
   | RGBCAPTURE_PARM_PIXELFORMAT      | RGBCAPTURE_PARM_HWND            \
   | RGBCAPTURE_PARM_SYNCEDGE         | RGBCAPTURE_PARM_SCALED_SIZE     \
   | RGBCAPTURE_PARM_NOTIFY_FN        | RGBCAPTURE_PARM_NOTIFY_FLAGS    \
   | RGBCAPTURE_PARM_BLOCKING)

/* Notification flags returned by RGBCaptureGetCapabilities to indicate the
 * notification capabilities of the hardware.
 * The same flags are used in calls to RGBCaptureGetParameters and
 * RGBCaptureSetParameters to set and query the required notification messages.
 * Note: The RGBWM_FRAMECAPTURED message (or its FrameCapturedFn equivalent) is
 * always sent.
 */
#define RGBNOTIFY_NO_SIGNAL                        0x00000001
#define RGBNOTIFY_MODE_CHANGE                      0x00000002

#define RGBCAPTURE_USERDATA_MAX                   8

enum
{
   RGBFORMAT_3WIRE_SYNC_ON_GREEN,
   RGBFORMAT_4WIRE_COMPOSITE_SYNC,
   RGBFORMAT_5WIRE_SEPARATE_SYNCS,
};

enum
{
   RGBPIXELFORMAT_UNKNOWN=0,
   RGBPIXELFORMAT_555=1,
   RGBPIXELFORMAT_565=2,
   RGBPIXELFORMAT_888=3,
   RGBPIXELFORMAT_UYVY=4,
   RGBPIXELFORMAT_COUNT=5        /* Should always be the last in the list. */
};

enum
{
   RGBSYNCEDGE_LEADING,
   RGBSYNCEDGE_TRAILING,
};

#define RGBCAPTURECAPS_FLAG_INTERLACED             0x00000001
#define RGBCAPTURECAPS_FLAG_MERGE_BUFFERS          0x00000002
#define RGBCAPTURECAPS_FLAG_AUTOMATIC_SYNC         0x00000004
#define RGBCAPTURECAPS_FLAG_IMAGE_SCALING          0x00000008
#define RGBCAPTURECAPS_FLAG_NOTIFICATION           0x00000010
#define RGBCAPTURECAPS_FLAG_DETECT_INTERLACED      0x00010000
#define RGBCAPTURECAPS_FLAG_DETECT_VSYNCPOLARITY   0x00020000
#define RGBCAPTURECAPS_FLAG_DETECT_HSYNCPOLARITY   0x00040000
#define RGBCAPTURECAPS_FLAG_DETECT_RGBFORMAT       0x00080000
#define RGBCAPTURECAPS_FLAG_DETECT_VERTOTAL        0x00100000
#define RGBCAPTURECAPS_FLAG_DETECT_VERFREQUENCY    0x00200000
#define RGBCAPTURECAPS_FLAG_IMAGE_MANIPULATION     0x00400000

#define RGBCAPTURE_IMG_FLIP_VERTICAL               0x00000001
#define RGBCAPTURE_IMG_FLIP_HORIZONTAL             0x00000002
#define RGBCAPTURE_IMG_FLIP_TRANSPOSE              0x00000004

#define RGBERROR_BASE 0x10000

typedef enum tagRGBERROR {
 /*����*/
RGBERROR_NO_ERROR                        = 0,
 /**/
RGBERROR_DRIVER_NOT_FOUND                =( RGBERROR_BASE + 0x0000 ),
/*�����ڴ���Դʧ��*/
RGBERROR_UNABLE_TO_LOAD_DRIVER           =( RGBERROR_BASE + 0x0001 ),
/*û���ҵ�VGA��Ƶ�ɼ���*/
RGBERROR_HARDWARE_NOT_FOUND              =( RGBERROR_BASE + 0x0002 ),
/*��Ч������*/
RGBERROR_INVALID_INDEX                   =( RGBERROR_BASE + 0x0003 ),
/*�豸�ѱ�ռ��*/
RGBERROR_DEVICE_IN_USE                   =( RGBERROR_BASE + 0x0004 ),
/*�豸δ��ʹ��*/
RGBERROR_INVALID_HRGBCAPTURE             =( RGBERROR_BASE + 0x0005 ),
/*�����ָ��*/
RGBERROR_INVALID_POINTER                 =( RGBERROR_BASE + 0x0006 ),
/*��������ò�����С*/
RGBERROR_INVALID_SIZE                    =( RGBERROR_BASE + 0x0007 ),
 /* ��֧�ֵ� ulFlag*/
RGBERROR_INVALID_FLAGS                   =( RGBERROR_BASE + 0x0008 ),
 /* ���豸ʧ�� */
RGBERROR_INVALID_DEVICE                  =( RGBERROR_BASE + 0x0009 ),
/*��֧�ֵĻ�����ʧ�� ����˿�*/
RGBERROR_INVALID_INPUT                   =( RGBERROR_BASE + 0x000A ),
/*��֧�ֵ������źŸ�ʽ*/
RGBERROR_INVALID_FORMAT                  =( RGBERROR_BASE + 0x000D ),
/**/
RGBERROR_INVALID_VDIF_CLOCKS             =( RGBERROR_BASE + 0x000E ),
/*��֧�ֻ�����ʧ����λ*/
RGBERROR_INVALID_PHASE                   =( RGBERROR_BASE + 0x0011 ),
/*��֧�ֻ�����ʧ������*/
RGBERROR_INVALID_BRIGHTNESS              =( RGBERROR_BASE + 0x0013 ),
/*��֧�ֻ�����ʧ�ܶԱȶ�*/
RGBERROR_INVALID_CONTRAST                =( RGBERROR_BASE + 0x0014 ),
/*��֧�ֵĺ�ɫ��*/
RGBERROR_INVALID_BLACKLEVEL              =( RGBERROR_BASE + 0x0015 ),
/**/
RGBERROR_INVALID_SAMPLERATE              =( RGBERROR_BASE + 0x0016 ),
/*��֧�ֵ���ʾģʽ*/
RGBERROR_INVALID_PIXEL_FORMAT            =( RGBERROR_BASE + 0x0017 ),
/*��֧�ֵ���Ϣ���ܴ���*/
RGBERROR_INVALID_HWND                    =( RGBERROR_BASE + 0x0018 ),
/**/
RGBERROR_INSUFFICIENT_RESOURCES          =( RGBERROR_BASE + 0x0019 ),
 /* While the specified parameters are valid for the device, some of the
 * resources required are already in use. */
RGBERROR_INSUFFICIENT_BUFFERS            =( RGBERROR_BASE + 0x001A ),
/**/
RGBERROR_INSUFFICIENT_MEMORY             =( RGBERROR_BASE + 0x001B ),
/**/
RGBERROR_SIGNAL_NOT_DETECTED             =( RGBERROR_BASE + 0x001C ),
/*��֧�ֵ�ͬ����*/
RGBERROR_INVALID_SYNCEDGE                =( RGBERROR_BASE + 0x001D ),
/**/
RGBERROR_OLD_FIRMWARE                    =( RGBERROR_BASE + 0x001E ),
 /* Both a Window handle and a function pointer for frame capturing have
 * been specified.*/
RGBERROR_HWND_AND_FRAMECAPTUREDFN        =( RGBERROR_BASE + 0x001F ),
/*��֧�ֵ�ˮƽ���ű���*/
RGBERROR_HSCALED_OUT_OF_RANGE            =( RGBERROR_BASE + 0x0020 ),
/*��֧�ֵĴ�ֱ���ű���*/
RGBERROR_VSCALED_OUT_OF_RANGE            =( RGBERROR_BASE + 0x0021 ),
/*��֧�ֻ�����ˮƽƫ��ʧ��*/
RGBERROR_SCALING_NOT_SUPPORTED           =( RGBERROR_BASE + 0x0022 ),
/*�ɼ�����̫С*/
RGBERROR_BUFFER_TOO_SMALL                =( RGBERROR_BASE + 0x0023 ),
 /* Information error messages that the horizontal values have
 * been adjusted to conform to the hardware rules. */
RGBERROR_HSCALE_NOT_WORD_DIVISIBLE       =( RGBERROR_BASE + 0x0024 ),
/**/
RGBERROR_HSCALE_NOT_DWORD_DIVISIBLE      =( RGBERROR_BASE + 0x0025 ),
/**/
RGBERROR_HORADDRTIME_NOT_WORD_DIVISIBLE  =( RGBERROR_BASE + 0x0026 ),
/**/
RGBERROR_HORADDRTIME_NOT_DWORD_DIVISIBLE =( RGBERROR_BASE + 0x0027 ),
 /* Version error - indicates that the user DLL and driver
 * version are incompatible. */
RGBERROR_VERSION_MISMATCH                =( RGBERROR_BASE + 0x0028 ),
 /* This error is a return from the AccPerformDraw function,
 * which does the accelerated draw into the window.  If this
 * "error" is returned, then something has happened to the
 * display surface which has caused the horizon driver to
 * require a reallocation, which should be done before any more
 * calls to RGBCaptureFrameBufferReady are made.
 */
RGBERROR_ACC_REALLOCATE_BUFFERS          =( RGBERROR_BASE + 0x0029 ),
/**/
RGBERROR_BUFFER_NOT_VALID                =( RGBERROR_BASE + 0x002A ),
/**/
RGBERROR_BUFFERS_STILL_ALLOCATED         =( RGBERROR_BASE + 0x002B ),
/**/
RGBERROR_NO_NOTIFICATION_SET             =( RGBERROR_BASE + 0x002C ),
/**/
RGBERROR_CAPTURE_DISABLED                =( RGBERROR_BASE + 0x002D ),
/**/
RGBERROR_INVALID_PIXELFORMAT             =( RGBERROR_BASE + 0x002E ),
/**/
RGBERROR_ILLEGAL_CALL                    =( RGBERROR_BASE + 0x002F ),
/*�豸δ����*/
RGBERROR_CAPTURE_OUTSTANDING             =( RGBERROR_BASE + 0x0030 ),
 /*
 *  Error define for the mode database reader/writer.
 */
RGBERROR_MODE_NOT_FOUND                  =( RGBERROR_BASE + 0x0031 ),
/**/
RGBERROR_CANNOT_DETECT                   =( RGBERROR_BASE + 0x0032 ),
/**/
RGBERROR_NO_MODE_DATABASE                =( RGBERROR_BASE + 0x0033 ),
/**/
RGBERROR_CANT_DELETE_MODE                =( RGBERROR_BASE + 0x0034 ),
/*
*   Error codes that should have been in here from the start.
*/
RGBERROR_MUTEX_FAILURE                   =( RGBERROR_BASE + 0x0035 ),
/*�����źŵ�ʧ��*/
RGBERROR_THREAD_FAILURE                  =( RGBERROR_BASE + 0x0036 ),
/**/
RGBERROR_NO_COMPLETION                   =( RGBERROR_BASE + 0x0037 ),
/**/
RGBERROR_INSUFFICIENT_RESOURCES_HALLOC   =( RGBERROR_BASE + 0x0038 ),
/**/
RGBERROR_INSUFFICIENT_RESOURCES_RGBLIST  =( RGBERROR_BASE + 0x0039 ),
/**/
RGBERROR_INVALID_PITCH                   =( RGBERROR_BASE + 0x0040 ),

RGBERROR_DDR_ERROR                   =( RGBERROR_BASE + 0x0041 ),

RGBERROR_FPGAor9888_WRITE_FAILED                   =( RGBERROR_BASE + 0x0042 ),

} RGBERROR;


#define RGBCAPTURE_MAX_BUFFERS                  16

#define RGBCAPTURE_BUFFER_HEADER                32

/******************************************************************************/
#ifndef KERNEL_MODE

#define RGBWM_BASE                              WM_USER

/*
 * Message: RGBWM_FRAMECAPTURED
 *          �������ڲ���һ֡��������ʱ��������Ϣ
 *
 * wParam:  Not used.
 *
 * lParam:  ��¼��ǰ��Ĳɼ���������ֵ���Ա�࿨��ͬʹ��ʱ����������Ϣ��Դ
 *
 * Return:  N/A.
 */
#define RGBWM_FRAMECAPTURED                     ( RGBWM_BASE + 0x0100 )

/*
 * Message: RGBWM_NOSIGNAL
 *          ���ɼ����ɼ�����һ֡������Ϣʱ��������Ϣ
 *
 * wParam:  Not used.
 *
 * lParam:  ��¼��ǰ��Ĳɼ���������ֵ���Ա�࿨��ͬʹ��ʱ����������Ϣ��Դ
 *
 * Return:  N/A.
 */

#define RGBWM_NOSIGNAL                          ( RGBWM_BASE + 0x0001 )

/*
 * Message: RGBWM_MODECHANGED
 *          �ɼ�����������ʱ���ֱַ��ʸķ�������Ϣ
 *
 * wParam:  Not used
 *
 * lParam:  ��¼��ǰ��Ĳɼ���������ֵ���Ա�࿨��ͬʹ��ʱ����������Ϣ��Դ
 *
 * Return:  N/A.
 */

#define RGBWM_MODECHANGED                       ( RGBWM_BASE + 0x0002 )

/*
 * Message: RGBWM_DATAMODE
 *          �ɼ�����ʼ�ɼ�����ǰ��������Ϣ����ȷ�������źŵ�����ģʽ��YUV/RGB��
 *
 * wParam:  Not used
 *
 * lParam:  ��¼��ǰ��Ĳɼ���������ֵ���Ա�࿨��ͬʹ��ʱ����������Ϣ��Դ
 *
 * Return:  ���ؿͻ�ѡ�������ģʽ
 */

#define RGBWM_DATAMODE                       ( RGBWM_BASE + 0x0003 )

/*
 * Message: RGBWM_DDRERROR
 *          �ɼ�����ʼ�ɼ�����ǰDDR�Լ����
 *
 * wParam:  Not used
 *
 * lParam:  ��¼��ǰi2c ��0X28�Ĵ�����ֵ���жϵ�ǰ������ģʽ
 *
 * Return:  ���ؿͻ�ѡ�������ģʽ
 */

#define RGBWM_DDRERROR                       ( RGBWM_BASE + 0x0004 )



#define WPARAM_LINE_COUNT_MASK         (0x0000FFFF)
#define LPARAM_VERTICAL_REFRESH_MASK   (0x0003FFFF)

/*
 * Message: RGBWM_INTERNALERROR
 *          Not used.
 *
 * wParam:  Not used.
 *
 * lParam:  Not used.
 *
 * Return:  N/A.
 */
#define RGBWM_INTERNALERROR                     ( RGBWM_BASE + 0x0003 )

/*
 * Message: RGBWM_CAPTURE_ERROR
 *          Not used.
 *
 * wParam:  Error code indicating failure case.
 *
 * lParam:  Not used.
 *
 * Return:  N/A.
 */
#define RGBWM_CAPTURE_ERROR                     ( RGBWM_BASE + 0x0004 )

/******************************************************************************/

#endif // not KERNEL_MODE

#endif

//////////////////////////////////////////////////////////////////////////
//FPGA����
#define FPGA_BUILD_DATE			0x4//
#define FPGA_PIX_STATUS			0x8//
#define FPGA_ERROR_STATUS		0xc//
#define FPGA_H_PIX_ADDR			0x10//
#define FPGA_H_OFFSET_ADDR		0x14//
#define FPGA_V_PIX_ADDR			0x18//
#define FPGA_V_OFFSET_ADDR		0x1c//
#define FPGA_GRAY_TEST			0x20//
#define FPGA_DMA_CTR			0x24//
#define FPGA_SIGNAL_MODE		0x28//


//////////////////////////////////////////////////////////////////////
//I2C
#define I2C_9888_ADDR	0x9A//
#define I2C_2016_ADDR	0xa0//
#define I2C_SCALE_LO	0x10000//
#define I2C_SCALE_HI	0x10004//

#define I2C_CTR			0x10008//
#define I2C_CTR_ENABLE	0x80  //enable I2C controler

#define I2C_TXR			0x1000c//

#define I2C_RXR			0x1000c//

#define I2C_CR			0x10010//
#define I2C_CR_STA		0x80//
#define I2C_CR_STO		0x40//
#define I2C_CR_RD		0x20//
#define I2C_CR_WR		0x10//
#define I2C_CR_NACK		0x08//
#define I2C_CR_IACK		0x01

#define I2C_SR			0x10010
#define I2C_SR_NACK		0x80
#define I2C_SR_BSY		0x40
#define I2C_SR_TIP		0x02
#define I2C_SR_IF		0x01

#define I2C_DEBUG 1
#define I2C_DELAY 5//
//////////////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////////////
//AD9888A PLL i2caddr
#define I2CADDR_CHIP_REVISION		       	0x0//
#define I2CADDR_PLL_DIV_MSB		        	0x1//
#define I2CADDR_PLL_DIV_LSB					0x2//
#define I2CADDR_VCO_CPMP					0x3//
#define I2CADDR_PHASE_ADJUST    			0x4//
#define I2CADDR_CLAMP_PLACEMENT    			0x5//
#define I2CADDR_CLAMP_DURATION    			0x6//
#define I2CADDR_HSYNC_OUTPUT_PULSEWIDTH    	0x7//
#define I2CADDR_RED_GAIN        			0x8//
#define I2CADDR_GREEN_GAIN					0x9//
#define I2CADDR_BLUE_GAIN					0xa//
#define I2CADDR_RED_OFFSET					0xb//
#define I2CADDR_GREEN_OFFSET    			0xc//
#define I2CADDR_BLUE_OFFSET     			0xd//
#define I2CADDR_SYNC_CONTROL    			0xe//
#define I2CADDR_CLAMP_CONTROL  		    	0xf////
#define I2CADDR_SYNC_ON_GREEN_THRESHOLD		0x10//
#define I2CADDR_SYNC_SEPARATOR_THRESHOLD	0x11//

#define I2CADDR_PRE_COAST		        	0x12//
#define I2CADDR_POST_COAST					0x13//
#define I2CADDR_SYNC_DETECT					0x14//
#define I2CADDR_OTHER_CONTROL				0x15//
#define I2CADDR_TEST_REGISTER1     			0x16//
#define I2CADDR_TEST_REGISTER2     			0x17//
#define I2CADDR_TEST_REGISTER3     			0x18//





#define NOSIGNAL_NO_RESOLUTION     0
#define NOSIGNAL_DDRCHECK_ERROR    1
#define NOSIGNAL_H_OVERFLOW        2
#define NOSIGNAL_V_OVERFLOW        3
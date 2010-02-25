using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using HikServer.DS40xxSDK;

namespace JOYFULL.CMPW.Monitor
{
    class ChannelInfo
    {
        public IntPtr m_Handle;
        public bool m_preReset = true;
        public bool m_bEncodeCifAndQcif = true;
        public int m_testVideo = 0;
        public int m_testAudio = 0;
        public bool m_bMoving = false;

        public int m_bright = 0;
        public int m_contrast = 0;
        public int m_sat = 0;
        public int m_hue = 0;

//      public VideoStandard_t m_chstandard = VideoStandard_t.StandardNone;
    }
}

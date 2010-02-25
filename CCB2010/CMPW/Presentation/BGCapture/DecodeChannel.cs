using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JOYFULL.CMPW.Monitor;
using System.Configuration;

namespace JOYFULL.CMPW.Presentation.BGCapture
{
    public class DecodeChannel
    {
        private int m_channelID = 0;
        public DecodeChannel(int channelID)
        {
            m_channelID = channelID;
        }
        public System.Drawing.Bitmap CaptureScreen()
        {
            return ChannelDSP.ChanelCapture(m_channelID);
        }
        
        public System.Drawing.Bitmap CaptureScreen_EX(out Int32 captureIndex)
        {
            captureIndex = 0;
            return ChannelDSP.GetChanelCaptureBitmap(m_channelID, out captureIndex);
        }
        public string CaptureScreenFile()
        {
            return ChannelDSP.ChanelCaptureFile(m_channelID);
        }
        //public int m_lastBenchMark = 0;

        //public bool SaveScreenTo(int checkMark, string toFile)
        //{
        //    System.Drawing.Bitmap bmpContent = CaptureScreen();
        //    bmpContent.Save(toFile);
        //    m_lastBenchMark = checkMark;
        //    return true;
        //}
        //public int GetLastBenchMark(string toPath)
        //{
        //    return m_lastBenchMark;
        //}
    }
}

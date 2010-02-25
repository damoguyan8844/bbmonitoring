using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Threading;
using JOYFULL.CMPW.Monitor;
using log4net;

namespace JOYFULL.CMPW.Presentation.BGCapture
{
    public class Capturer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Capturer));
        private static readonly int maxCachePicNum = 20;
        public static  int captureIdleSeconds = 50;

        private Thread m_thread = null;

        public Capturer(string sharedPath,int channelID)
        {
            ChannelDSP.InitDSP(maxCachePicNum, captureIdleSeconds, sharedPath+"Captures");
            m_thread = new Thread(new ThreadStart(new CaptureThread(sharedPath, channelID).CaptureMain));
            StartCapture();
        }

        public void StartCapture()
        {
            m_thread.Priority = ThreadPriority.BelowNormal;
            m_thread.Start();
        }
        public void StopCapture()
        {
            m_thread.Abort();
            ChannelDSP.DeInitDSP();
        }

        private class CaptureThread
        {
            private DecodeChannel m_channel=null;
            private string m_sharePath = "";
            
            public CaptureThread(string sharepath,int channelID)
            {
                m_sharePath = sharepath;
                m_channel = new DecodeChannel(channelID);

            }
            public void CaptureMain()
            {
                //SequenceTimeSpanCheck timeCheck =
                //    new SequenceTimeSpanCheck(new TimeSpan(0, 0, 0), new TimeSpan(23, 59, 59), 300);
                //timeCheck.ResetTo(DateTime.Now.TimeOfDay);

                //return;
                //string lastfileName="";
                log.Info("CaptureMain Thread ID:" + SystemsHandler.SystemsHandler.GetCurrentThreadId().ToString());
 
                Int32 lastCaptureIndex = -1;

                while(true) 
                {
                    
                    try
                    {
                        //log.Error("Start Capture Screen");
                        Int32 captureIndex = lastCaptureIndex;
                        System.Drawing.Bitmap bitContent = m_channel.CaptureScreen_EX(out captureIndex);
                        //string[] files = { "C:\\RiZhong.bmp",
                        //                    "C:\\Exception.bmp"/*,*/
                        //                   /*"C:\\RiZhongAndException.bmp"/ *,* /*/
                        //                   /*"C:\\NoException.bmp"*/};

                        //captureIndex++;
                        // System.Drawing.Bitmap bitContent = new System.Drawing.Bitmap(files[captureIndex % files.Length]); 
                       
						if(bitContent==null) continue;
                        //string fileName = m_channel.CaptureScreenFile();

                        if (captureIndex != lastCaptureIndex)
                        {
                            lastCaptureIndex = captureIndex;
                            log.Info("New Capture Screen: Index " + captureIndex);

                            //System.Drawing.Bitmap bitContent = new System.Drawing.Bitmap(fileName);
                            int sysID = SystemsHandler.SystemsHandler.CheckSystemID(bitContent);

                            log.Info("Return SysID:" + sysID.ToString());
                        }
                        bitContent.Dispose();
                        bitContent = null;
                        Thread.Sleep(captureIdleSeconds);
                    }
                    catch(System.Exception e)
                    {
                        log.Error("\r\nSource:"+e.Source +"\r\nMessage:"+e.Message+"\r\n"+e.StackTrace);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Media;

namespace JOYFULL.CMPW.Alert
{
    public class AlarmBuzzer
    {
       
        private SoundPlayer m_soundPlayer = new SoundPlayer();
        private Thread m_stopThread = null;

        public AlarmBuzzer()
        {
        }

        public bool Play(int seconds,string title,string source)
        {
            m_source = source;
            m_Title = title;
            m_length = seconds;

            if (m_soundPlayer != null) m_soundPlayer.Stop();
            m_soundPlayer.SoundLocation = @source;

            m_soundPlayer.PlayLooping();
            m_stopThread=new Thread(new ThreadStart(this.InternalStop));

            m_stopThread.Start();
            
            return true;
        }
        public bool Stop()
        {
            if (m_soundPlayer != null)
            {
                m_soundPlayer.Stop();
                m_length=0;
            }
            return true;
        }
    
        private void InternalStop()
        {
            if (m_length < 0) m_length = 3600;
            Thread.Sleep(m_length*1000);
            Stop();
        }
        private string m_source = "";
        public string Source
        {
            get
            {
                return m_source;
            }
        }

        private string m_Title = "";
        public string Title
        {
            get
            {
                return m_Title;
            }
        }

        private int m_length = 0;
        public int Length
        {
            get
            {
                return m_length;
            }
        }
    }
}

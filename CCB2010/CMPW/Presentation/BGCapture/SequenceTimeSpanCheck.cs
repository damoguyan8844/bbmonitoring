using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

namespace JOYFULL.CMPW.Presentation.BGCapture
{
    public class SequenceTimeSpanCheck
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SequenceTimeSpanCheck));

        private TimeSpan m_beginTime = new TimeSpan(0, 0, 0);
        private long m_lastCheckSeconds = 0;
        private long m_beginSeconds = 0;
        private long m_endMilliSeconds = 24 * 60 * 60*  - 1;
        private long m_freq = 1;


        public SequenceTimeSpanCheck(TimeSpan beginTime, TimeSpan endTime, int freqSeconds)
        {
            m_beginTime = beginTime;
            m_beginSeconds = beginTime.Hours * 3600 + beginTime.Minutes * 60 + beginTime.Seconds;
            m_endMilliSeconds = endTime.Hours * 3600 + endTime.Minutes * 60 + endTime.Seconds;

            if (freqSeconds > 0)
            {
                m_freq = freqSeconds;
            }
            else
            {
                log.Error("Invalid Freq:" + freqSeconds.ToString());
            }

            ResetTo(m_beginTime);
        }
        /*
         * After converting timePoint to seconds, check if it bigger than m_lastCheckSeconds. 
         */
        public bool IsTimePointOn(TimeSpan timePoint, out long checkDatumMark)
        {
            checkDatumMark = m_lastCheckSeconds;
            
            int timeSeconds = timePoint.Hours * 3600 + timePoint.Minutes * 60 + timePoint.Seconds;

            if (timeSeconds < m_beginSeconds || timeSeconds > m_endMilliSeconds)
                return false;

            if (m_lastCheckSeconds > m_endMilliSeconds)
            {
                //reset to begin time until new round begin 
                if (timeSeconds > m_beginSeconds - m_endMilliSeconds % m_freq)
                    return false;

                ResetTo(m_beginTime);
            }
            if (timeSeconds < m_lastCheckSeconds)
            {
                //m_lastCheckMilliSeconds = timeSeconds - timeSeconds % m_freq;
                return false;
            }
            /* To Next Time Segment, Bigger then timePoint */
            m_lastCheckSeconds += m_freq;

            if (m_lastCheckSeconds < timeSeconds)
            {
                m_lastCheckSeconds = timeSeconds - timeSeconds % m_freq + m_freq;
            }
            return true;
        }

        public void ResetTo(TimeSpan timePoint)
        {
            long timeSeconds = timePoint.Hours * 3600 + timePoint.Minutes * 60 + timePoint.Seconds;
            m_lastCheckSeconds = timeSeconds - timeSeconds % m_freq;
        }

        public static long GetMilliSeconds(TimeSpan timePoint)
        {
            return timePoint.Hours * 3600 + timePoint.Minutes * 60 + timePoint.Seconds;
        }
    }
}

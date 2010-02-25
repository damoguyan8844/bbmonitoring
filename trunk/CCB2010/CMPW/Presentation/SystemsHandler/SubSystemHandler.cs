using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using JOYFULL.CMPW.Digit;
using JOYFULL.CMPW.Alert;
using JOYFULL.CMPW.Model;

using System.IO;
using System.Windows.Forms;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    public class SubSystemHandler
    {
        private Object _lock = new object();
        private static readonly ILog log = LogManager.GetLogger(typeof(SubSystemHandler));

        private SystemsSetting.System m_setting=null;
        private Model.SubSystem m_sysDb=null;
        private AlarmBuzzer altBuzzer = new AlarmBuzzer();
        private SubSystemLogic m_logic=null;

        public BGCapture.SequenceTimeSpanCheck m_sysTimeCheck = null;

        private Dictionary<int, BGCapture.SequenceTimeSpanCheck> timCheck = new Dictionary<int, BGCapture.SequenceTimeSpanCheck>();
        public SubSystemHandler(SubSystemLogic sysLogic, SystemsSetting.System sysSetting, Model.SubSystem sysDb)
        {
            m_setting = sysSetting;
            m_sysDb = sysDb;
            IsEnable = false;
            m_logic = sysLogic;

            TimeSpan sysBeginTime = new TimeSpan(23, 59, 59);
            TimeSpan sysEndTime = new TimeSpan(0, 0, 0);
            int sysFreq = 6000;

            foreach (KeyValuePair<int, SystemsSetting.Condition> condition in m_setting.alertConditions)
            {
                TimeSpan beginTime=SystemsSetting.BeginTimeParameter.DefaultBeginTime();
                if (m_setting.beginPars.ContainsKey(condition.Key))
                    beginTime = m_setting.beginPars[condition.Key].BeginTime;

                TimeSpan endTime = SystemsSetting.EndTimeParameter.DefaultEndTime();
                if (m_setting.endPars.ContainsKey(condition.Key))
                    endTime = m_setting.endPars[condition.Key].EndTime;

                int freq = 1; //default freq to ten seconds
                if (m_setting.freqPars.ContainsKey(condition.Key))
                    freq = m_setting.freqPars[condition.Key].Freq;

                if (beginTime < sysBeginTime)
                    sysBeginTime = beginTime;
                if (endTime > sysEndTime)
                    sysEndTime = endTime;
                //if (freq < sysFreq)
                    sysFreq = GetGongYueShu(freq,sysFreq);

                timCheck.Add(condition.Key, new BGCapture.SequenceTimeSpanCheck(beginTime, endTime, freq));
            }

            m_sysTimeCheck = new BGCapture.SequenceTimeSpanCheck(sysBeginTime, sysEndTime, sysFreq);
        }
        public int GetGongYueShu(int frq1,int frq2)
        {
            int max = Math.Max(frq1, frq2);
            int min = Math.Min(frq1, frq2);
            while( max % min != 0 )
            {
                int tmp = min;
                min = max % min;
                max = tmp;
            }
            return min;
        }
        public bool CheckSubsystem()
        {
            if (IsEnable==false) return false;
            if ( !m_logic.EnsureSystem() )
            {
                return false;//废图
            }
            bool sysConditionChecked = false;

            TimeSpan current = DateTime.Now.TimeOfDay;
            string sDesc = "";
            foreach (KeyValuePair<int, SystemsSetting.Condition> conditionPair in m_setting.alertConditions)
            {
                long  checkDatumMark=0;
                if (conditionPair.Value.IsHasException || 
                    timCheck[conditionPair.Key].IsTimePointOn(current, out checkDatumMark))
                {
                    if (m_logic != null)
                    {
                        var c = conditionPair.Value;
                        m_logic.CheckCondition(c);
                        if (c.IsHasException && string.IsNullOrEmpty(sDesc))
                        {
                            sDesc = "异常内容: " + c.Label +
                                "\t标准值: " + c.StrValue;
                            ExceptionDesc = sDesc;
                        }
                    }
                    sysConditionChecked=true;
                }
            }
            ExceptionDesc = sDesc;
            m_logic.RecordToDb( m_setting );
            //log.Error("\r\n" +GetSystemInfo());

            return sysConditionChecked;
        }


        public string ExceptionDesc { get; private set; }
        public bool IsEnable
        {
            get;
            set;
        }
        public string GetSystemInfo()
        {
            lock( _lock )
            {
	            string retStr="";
	            foreach (KeyValuePair<int, SystemsSetting.Condition> conditionPair in m_setting.alertConditions)
	            {
	                if (conditionPair.Value.Enable)
	                {
                        
                        var c = conditionPair.Value;
                        
                        if( c.Type == JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_STRING)
                        {
                            if (c.IsHasException)
                                retStr += conditionPair.Value.Label + ":发现异常\r\n";
                            else
                            {
                                string s = conditionPair.Value.StrValue;
                                if (s != "未检测")
                                    retStr += conditionPair.Value.Label + ":无异常\r\n";
                                else
                                    retStr += conditionPair.Value.Label + ":" + conditionPair.Value.lastCheckValue;
                            }
                        }
                        else
	                        retStr += conditionPair.Value.Label + ":" + conditionPair.Value.lastCheckValue+"\r\n";
	                }
	            }
	            return retStr;
            }
        }

        public bool HasException()
        {
            foreach (KeyValuePair<int, SystemsSetting.Condition> conditionPair in m_setting.alertConditions)
            {
                if ( conditionPair.Value.IsHasException && conditionPair.Value.Altertype != 3 )
                    return true;
            }
            return false;
        }
        public bool IsRiZhongException()
        {
            foreach (KeyValuePair<int, SystemsSetting.Condition> conditionPair in m_setting.alertConditions)
            {
                if (conditionPair.Value.IsHasException && conditionPair.Value.Altertype==3)
                    return true;
            }

            return false;
        }

        public void LanchAlert()
        {
            foreach (KeyValuePair<int, SystemsSetting.Condition> conditionPair in m_setting.alertConditions)
            {
                if (conditionPair.Value.IsHasException)
                {
                    LanchAlert(conditionPair.Value.Altertype);
                    break;
                }
            }
        }

        /// <summary>
        /// 如果异常和日终同时出现，优先提示异常
        /// </summary>
        /// <returns></returns>
        public int GetAlertType()
        {
            foreach (KeyValuePair<int, SystemsSetting.Condition> conditionPair in m_setting.alertConditions)
            {
                if (conditionPair.Value.IsHasException)
                {
                    return conditionPair.Value.Altertype;
                }
            }
            return 0;
        }

        public void StopAlert()
        {
            altBuzzer.Stop();
        }
        public void LanchAlert(int alterType)
        {
            if (!SystemsHandler.m_settings.alerts.ContainsKey(alterType)) return;

            SystemsSetting.Alert alt=SystemsHandler.m_settings.alerts[alterType];

            if(alt!=null)
            {
                altBuzzer.Play(alt.Time, alt.Name, alt.Source);
            }
            else
            {
                log.Error("Error Alter Type:" + alterType.ToString());
            }
        }

        /// <summary>
        /// 记录系统日终前最后一次数据
        /// </summary>
        public void RecordLast()
        {
            m_logic.RecordLast( m_setting );
        }
    }
}

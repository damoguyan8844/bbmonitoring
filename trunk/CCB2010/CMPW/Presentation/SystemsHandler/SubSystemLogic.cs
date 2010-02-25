using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using JOYFULL.CMPW.DAL;
using JOYFULL.CMPW.Model;
using JOYFULL.CMPW.Digit;
using System.Windows.Forms;
using log4net;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    public class SubSystemLogic
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(SubSystemLogic));

        public Digit.Digit m_DigitParser = new Digit.Digit();

        private static object sign_lock = new object();
        public static bool CheckSystemSign(string dest, string StrValue)
        {
            lock(sign_lock)
            {
                string[] strs = StrValue.Split(';');

                foreach (string strTemp in strs)
                {
                    if (dest.Contains(strTemp))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public virtual bool EnsureSystem()
        {
            return true;
        }

        public virtual string GetConditionValue(int sysID,SystemsSetting.Condition condition)
        {
            string systemFolder = SystemsHandler.GetSystemFolder(sysID);
            if (systemFolder.Length >1)
            {
                string conditionFile=systemFolder + condition.Label + ".bmp";
                Digit.Digit.DigitType type = condition.Type;

                string ret = string.Empty;
                if (m_DigitParser.TryParse(conditionFile, type, out ret))
                    return ret;

            }
            return string.Empty;
        }

        public virtual string GetConditionValue(int sysID, Digit.Digit.DigitType type,string label)
        {
            string systemFolder = SystemsHandler.GetSystemFolder(sysID);
            if (systemFolder.Length > 1)
            {
                string conditionFile = systemFolder + label + ".bmp";
               
                string ret = "";
                if (m_DigitParser.TryParse(conditionFile, type, out ret))
                    return ret;

            }
            return "";
        }

        public virtual void CheckCondition(SystemsSetting.Condition condition)
        {
            MessageBox.Show("Subsystem Has Must Override The CheckCondition Function");
        }

        public virtual void CheckCondition(int sysID, SystemsSetting.Condition condition)
        {
            string ret = GetConditionValue(sysID, condition);

            if (ret == "") return;
            if (ret == "11" && sysID == 11) return;

            CheckConditionWithValue(sysID, condition, ret,true);
        }

        public void CheckConditionWithValue(int sysID, SystemsSetting.Condition condition,string value,bool updateSign)
        {
            if (updateSign)
            {

                //log.Info("SysID:" + sysID.ToString() + "\tCondition:" + condition.Label + "\tValue:" + value.ToString());

                bool oldCondition = condition.IsHasException;
                if (condition.CheckCondition(value, updateSign))
                {
                    if (oldCondition) // Exception End
                    {
                        //DAL.ExceptionalEventsDal excepDal = new DAL.ExceptionalEventsDal();
                        //condition.ExceptionEvent.EndTime = condition.ExceptionEndTime;
                        //excepDal.UpdateExceptionalEvent(condition.ExceptionEvent);

                        //防止 CCBS 系统日终连续报异常
                        if (sysID == Logic_CCBSZhongDuanXiTong._SystemID)
                        {
                            TimeSpan seg = DateTime.Now - condition.ExceptionEvent.Found;
                            if (seg.TotalSeconds < 60)
                                return;
                        }

                        DAL.ExEventDal eeDal = new ExEventDal();
                        condition.ExceptionEvent.Solved = condition.ExceptionEndTime;
                        eeDal.Update(condition.ExceptionEvent);

                        condition.IsHasException = false; //Reset Exception to False 
                        condition.ExceptionEvent = null;
                    }
                }
                else
                {
                    condition.IsHasException = true;
                    log.Info(CreateExDescription(condition, value));

                    if (!oldCondition || condition.ExceptionEvent == null) // New Exception 
                    {
                        DAL.ExEventDal eeDal = new ExEventDal();
                        Model.ExEvent evt = new ExEvent();
                        evt.SystemID = sysID;
                        evt.Description = CreateExDescription(condition, value);
                        evt.Found = condition.ExceptionHappenTime;
                        evt = eeDal.Add(evt);
                        condition.ExceptionEvent = evt;

                        SaveExceptionImage(sysID, evt.Description);
                    }
                }
            }
            else
            {
                condition.CheckCondition(value, updateSign);
            }
        }

        void SaveExceptionImage( int sysId, string info )
        {

            string folder = System.Configuration.ConfigurationManager.AppSettings["ExceptionFolder"] +
                DateTime.Today.ToString("yyyyMMdd") + "\\";
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folder);
            if (!di.Exists)
            {
                di.Create();
            }
            System.Drawing.Bitmap bmp = SystemsHandler.GetSystemScreen(sysId);
            bmp.Save(folder + DateTime.Now.ToString("HHmmss") + "_" +
                SystemsHandler.GetSystemName(sysId) + ".bmp");
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                folder + DateTime.Now.ToString("HHmmss") + "_" +
                SystemsHandler.GetSystemName(sysId) + "异常描述.txt");
            sw.Write(info);
            sw.Flush();
            sw.Close();
        }

        string CreateExDescription( SystemsSetting.Condition c, string value )
        {
            string info = "异常信息:    ";
            info += "异常情况 - " + c.Label + "    ";
            info += "标准值 - " + c.StrValue + "    ";
            info += "比较关系 - " + c.Compare.ToString() + "    ";
            info += "检测值 - " + value + "    ";

            return info;            
        }

        public virtual void RecordToDb( SystemsSetting.System sys )
        {
            // null implementation
            // override at DA_E_YE_WU & XIAO_E_YE_WU
        }

        public virtual void RecordLast( SystemsSetting.System sys )
        {
            // null implementation
            // override at DA_E_YE_WU & DA_E_SHI_WU
        }
    }
}

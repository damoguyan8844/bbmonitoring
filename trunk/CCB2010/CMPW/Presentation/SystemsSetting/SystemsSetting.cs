using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using log4net;
using System.Configuration;

namespace JOYFULL.CMPW.Presentation.SystemsSetting
{
    public class SystemsSetting
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SystemsSetting));

        readonly string FILE_NAME =
            ConfigurationManager.AppSettings["SysSettingFile"];

        public Dictionary<int,System> systems = new Dictionary<int,System>();
        public Dictionary<int, Alert> alerts = new Dictionary<int,Alert>();

        private XmlDocument m_doc = new XmlDocument();
        public SystemsSetting()
        {
            m_doc.Load(FILE_NAME);

            XmlNodeList sysArray = m_doc.SelectSingleNode("Systems").SelectNodes("System");
            foreach (XmlNode sys in sysArray)
            {
                System obj = new System(sys);
                systems.Add(obj.Order,obj);
            }

            XmlNodeList sysAlert = m_doc.SelectSingleNode("Systems").SelectSingleNode("Alerts").SelectNodes("Alert");
            foreach (XmlNode sys in sysAlert)
            {
                Alert obj = new Alert(sys);
                alerts.Add(obj.Order, obj);
            }
        }
        public bool UpdateConditionValue(string systemLabel,string conditionLabel,string newValue)
        {
            System sysSetting = null;
            Condition condition = null;

            if (!GetSystemSettingAndCondition(systemLabel, conditionLabel, out sysSetting, out condition))
                return false;

            condition.StrValue = newValue;
            m_doc.Save(FILE_NAME);
            return true;
        }
        public bool UpdateConditionAlert(string systemLabel, string conditionLabel, string newValue)
        {
            System sysSetting = null;
            Condition condition = null;

            if (!GetSystemSettingAndCondition(systemLabel, conditionLabel, out sysSetting, out condition))
                return false;

            Alert alert = getAlert(newValue);

            if(alert==null)
            {
                log.Error("Not Found Alert:" + newValue.ToString());
                return false;
            }

            condition.Altertype = alert.Order;
            m_doc.Save(FILE_NAME);
            return true;
        }
        //public bool UpdateConditionPara(Parameter.ParameterEnum paraType,string systemLabel, string conditionLabel,string newValue)
        //{
        //    System sysSetting = null;
        //    Parameter para = null;

        //    if (!GetSystemSettingAndParameter(systemLabel, conditionLabel, out sysSetting, out para))
        //        return false;

        //    para.Value = newValue;
        //    System sysSetting = getSystemSetting(systemLabel);
        //    if (sysSetting == null) return false;

        //    if (paraType == Parameter.ParameterEnum.BeginTimePara)
        //    {
        //        if (sysSetting.beginPars.Count > 0)
        //        {

        //            BeginTimeParameter begPara = sysSetting.beginPars[para.Order];
        //            List<int> oldApplayOn = begPara.ApplayOn;

        //            if (oldApplayOn.Count == 1 && oldApplayOn[0] == para.Order)  // If only apply on Single Condition , Update Parameter Directly
        //            {
        //                begPara.Value = newValue;
        //            }
        //            else
        //            {
        //                List<int> newApplayOn = oldApplayOn;
        //                newApplayOn.Remove(para.Order);

        //                begPara.ApplayOn = newApplayOn;
        //                sysSetting.AddConditionPara(paraType, begPara.Type, begPara.Label, newValue, para.Order.ToString());
        //            }
        //        }
        //        else
        //        {
        //            sysSetting.AddConditionPara(paraType, Digit.Digit.DigitType.DIGIT_TIME, para.Label + " 检查开始时间(hh:mm)", newValue, para.Order.ToString());
        //        }
        //    }
        //    else if (paraType == Parameter.ParameterEnum.FreqPara)
        //    {
        //        if (sysSetting.freqPars.Count > 0)
        //        {
        //            FreqParameter freqPara = sysSetting.freqPars[para.Order];
        //            List<int> oldApplayOn = freqPara.ApplayOn;

        //            if (oldApplayOn.Count == 1 && oldApplayOn[0] == para.Order)  // If only apply on Single Condition , Update Parameter Directly
        //            {
        //                freqPara.Value = newValue;
        //            }
        //            else
        //            {
        //                List<int> newApplayOn = oldApplayOn;
        //                newApplayOn.Remove(para.Order);

        //                freqPara.ApplayOn = newApplayOn;
        //                sysSetting.AddConditionPara(paraType, freqPara.Type, freqPara.Label, newValue, para.Order.ToString());
        //            }
        //        }
        //        else
        //        {
        //            sysSetting.AddConditionPara(paraType, Digit.Digit.DigitType.DIGIT_INT, para.Label + " 检查时间间隔(分)", newValue, para.Order.ToString());
        //        }
        //    }
        //    else if (paraType == Parameter.ParameterEnum.EndTimePara)
        //    {
        //        if (sysSetting.endPars.Count > 0)
        //        {
        //            EndTimeParameter endPara = sysSetting.endPars[para.Order];
        //            List<int> oldApplayOn = endPara.ApplayOn;

        //            if (oldApplayOn.Count == 1 && oldApplayOn[0] == para.Order)  // If only apply on Single Condition , Update Parameter Directly
        //            {
        //                endPara.Value = newValue;
        //            }
        //            else
        //            {
        //                List<int> newApplayOn = oldApplayOn;
        //                newApplayOn.Remove(para.Order);

        //                endPara.ApplayOn = newApplayOn;
        //                sysSetting.AddConditionPara(paraType, endPara.Type, endPara.Label, newValue, para.Order.ToString());
        //            }
        //        }
        //        else
        //        {
        //            sysSetting.AddConditionPara(paraType, Digit.Digit.DigitType.DIGIT_TIME, para.Label + " 检查结束时间(hh:mm)", newValue, para.Order.ToString());
        //        }
        //    }
        //    m_doc.Save(FILE_NAME);
        //    return true;
        //}

        public bool UpdateConditionPara(string systemLabel, string conditionLabel, string newValue)
        {
            System sysSetting = null;
            Parameter para = null;

            if (!GetSystemSettingAndParameter(systemLabel, conditionLabel, out sysSetting, out para))
                return false;

            para.Value = newValue;
            m_doc.Save(FILE_NAME);
            return true;
        }

        private bool GetSystemSettingAndCondition(string systemLabel, string conditionLabel, out System sysSetting, out Condition condition)
        {
            sysSetting = null;
            condition = null;

            sysSetting = getSystemSetting(systemLabel);
            if (sysSetting == null)
            {
                log.Error("Not Found System:" + systemLabel.ToString());
                return false;
            }
            condition = sysSetting.getCondition(conditionLabel);

            if (condition == null)
            {
                log.Error("Not Found Condition:" + conditionLabel.ToString());
                return false;
            }
            return true;
        }

        private bool GetSystemSettingAndParameter( string sysLabel, string paraLabel, out System sysSetting, out Parameter para )
        {
            sysSetting = null;
            para = null;

            sysSetting = getSystemSetting(sysLabel);
            if (sysSetting == null)
            {
                log.Error("Not Found System:" + sysLabel.ToString());
                return false;
            }
            para = sysSetting.getBeginPara( paraLabel );
            if( para == null )
                para = sysSetting.getEndPara( paraLabel );
            if( para == null )
                para = sysSetting.getFreqPara( paraLabel );
            if( para == null )
            {
                 log.Error( "Not Found Parameter:" + paraLabel );
                return false;
            }
            return true;
        }
        private System getSystemSetting(string systemLabel)
        {
            foreach (KeyValuePair<int, System> keyPair in systems)
            {
                if(keyPair.Value.Name==systemLabel)
                {
                    return keyPair.Value;
                }
            }
            return null;
        }
        private Alert getAlert(string alertLabel)
        {
            foreach (KeyValuePair<int, Alert> keyPair in alerts)
            {
                if (keyPair.Value.Name == alertLabel)
                {
                    return keyPair.Value;
                }
            }
            return null;
        }
    }
}

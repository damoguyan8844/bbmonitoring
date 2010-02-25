using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using log4net;

namespace JOYFULL.CMPW.Presentation.SystemsSetting
{
    public class Parameter
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(Condition));

        public enum ParameterEnum
        {
            BeginTimePara = 0x0001,
            FreqPara = 0x0010,
            EndTimePara = 0x0100
        }

        public XmlNode m_xmlNode = null;
        public Parameter(XmlNode para)
        {
            m_xmlNode = para;

            Order = Int32.Parse(para.Attributes["order"].Value);
            Type = Condition.TypeParser(para.Attributes["type"].Value);
            Label = para.Attributes["label"].Value;
            Value = para.Attributes["value"].Value;
            string strTemp = para.Attributes["applayon"].Value;

            
            foreach (string item in strTemp.Split(';'))
            {
                if (item.Length > 0)
                {
                    int num = 0;
                    int.TryParse( item, out num );
                    m_applays.Add(num);
                }
            }
        }

        public int Order { get; set; }
        public Digit.Digit.DigitType Type { get; set; }
        public string Label { get; set; }
        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                m_xmlNode.Attributes["value"].Value = value;
            }
        }

        private List<int> m_applays = new List<int>();
        public List<int> ApplayOn 
        { 
            get
            {
                return m_applays;
            }
            set
            {
                m_applays=value;
                string strTemp="";
                for(int index=0; index<m_applays.Count; ++index)
                {
                    if(index==0)
                    {
                        strTemp=m_applays[index].ToString();
                    }
                    else
                    {
                        strTemp = strTemp + ";" + m_applays[index].ToString();
                    }
                }
                m_xmlNode.Attributes["applayon"].Value=strTemp;
            }
        }

    }
    public class FreqParameter: Parameter
    {
        public FreqParameter(XmlNode para):base(para)
        {
        }
        public int Freq 
        {
            get
            {
                int ret = -1;
                if (!Int32.TryParse(Value, out ret))
                    log.Error("Freq Error:" + Value);
                return ret;
            } 
            set
            {
                Value = value.ToString();
            } 
        }
    }
    public class BeginTimeParameter : Parameter
    {
        public BeginTimeParameter(XmlNode para): base(para)
        {

        }
        
        public TimeSpan BeginTime
        {
            get
            {
                TimeSpan ret = DefaultBeginTime();
                if (!TimeSpan.TryParse(Value, out ret))
                    log.Error("BeginTime Error:" + Value);
                return ret;
            }
            set
            {
                Value = value.ToString();
            }
        }
        static public TimeSpan DefaultBeginTime()
        {
            return new TimeSpan(0, 0, 0);
        }
    }
    public class EndTimeParameter : Parameter
    {
        public EndTimeParameter(XmlNode para):base(para)
        {

        }

        public TimeSpan EndTime
        {
            get
            {
                TimeSpan ret = DefaultEndTime();
                if (!TimeSpan.TryParse(Value, out ret))
                    log.Error("EndTime Error:" + Value);
                return ret;
            }
            set
            {
                Value = value.ToString();
            }
        }
        static public TimeSpan DefaultEndTime()
        {
            return new TimeSpan(23, 59, 59);
        }
    }
}

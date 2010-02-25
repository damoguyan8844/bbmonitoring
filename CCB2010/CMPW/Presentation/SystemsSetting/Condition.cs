using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

using log4net;
using JOYFULL.CMPW.Digit;

namespace JOYFULL.CMPW.Presentation.SystemsSetting
{
    public class Condition
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Condition));

        private XmlNode m_XmlNode = null;
        public enum CompareEnum
        {
            EQUAL=0x0001,
            UNEQUAL=0x0010,
            BIG = 0x0100,
            SMALL =0x1000,
            BIG_EQUAL = 0x002,
            SMALL_EQUAL = 0x003

        }

        public Condition(XmlNode condition )
        {
            m_XmlNode = condition;
            Order = Int32.Parse(condition.Attributes["order"].Value);
            Type  = TypeParser(condition.Attributes["type"].Value);
            Label = condition.Attributes["label"].Value;
            StrValue = condition.Attributes["value"].Value;
            Compare = CompareParser(condition.Attributes["compare"].Value);
            Enable = condition.Attributes["enable"].Value=="true";
            Altertype = Int32.Parse(condition.Attributes["alerttype"].Value);
            Range = new Rect(condition.Attributes["rect"].Value);

            lastChangeValue = "";
            lastChangeTime = DateTime.Now;
            lastCheckValue = "未检测";
            IsHasException = false;
        }
        static public Digit.Digit.DigitType TypeParser(string str)
        {
            Digit.Digit.DigitType ret = Digit.Digit.DigitType.DIGIT_INT;
            str=str.ToLower();

            if (str.Equals("int"))
                ret = Digit.Digit.DigitType.DIGIT_INT;
            else if (str.Equals("string"))
                ret = Digit.Digit.DigitType.DIGIT_STRING;
            else if (str.Equals("time"))
                ret = Digit.Digit.DigitType.DIGIT_TIME;
            else
                log.Error("Error Type:" + str);
            return ret;
        }

        private CompareEnum CompareParser(string str)
        {
            CompareEnum ret = CompareEnum.EQUAL;
            str = str.ToLower();

            if (str.Equals("equal") || str.Equals("is"))
                ret = CompareEnum.EQUAL;
            else if (str.Equals("unequal") || str.Equals("not"))
                ret = CompareEnum.UNEQUAL;
            else if (str.Equals("big"))
                ret = CompareEnum.BIG;
            else if (str.Equals("bigequal"))
                ret = CompareEnum.BIG_EQUAL;
            else if (str.Equals("small"))
                ret = CompareEnum.SMALL;
            else if (str.Equals("smallequal"))
                ret = CompareEnum.SMALL_EQUAL;
            else
                log.Error("Error Compare:" + str);
            return ret;
        }

        public int Order { get; set; }
        public Digit.Digit.DigitType Type { get; set; }
        public string Label { get; set; }
        public CompareEnum Compare { get; set; }
        public bool Enable { get; set; }
        
        public Rect Range { get; set; }

        public string lastChangeValue { get; set; }
        public DateTime lastChangeTime { get; set; }
 
        public bool IsHasException { get ; set ; }
        public DateTime ExceptionHappenTime { get; set;}
        public DateTime ExceptionEndTime { get; set;}
        public String ExceptionValue { get; set; }
        public string lastCheckValue { get; set; }

        //public Model.ExceptionalEvent ExceptionEvent { get; set; }
        public Model.ExEvent ExceptionEvent { get; set; }

        public Bitmap ConditionMap { get; set; }

        private int m_alertType = 0;
        public int Altertype 
        {
            get
            {
                return m_alertType;
            }
            set
            {
                m_alertType = value;
                m_XmlNode.Attributes["alerttype"].Value = m_alertType.ToString();
            }
        }

        private string m_strValue="";
        public string StrValue 
        { 
            get
            {
                return m_strValue;
            }
            set
            {
                m_strValue = value;
                m_XmlNode.Attributes["value"].Value=m_strValue;
            }
        }
        public double DblValue
        {
            get
            {
                double dblVale=-1.0;
                if(!Double.TryParse(StrValue,out dblVale))
                    log.Error("Parse to Double Error" + StrValue);
                return dblVale;
            }
            set
            {
                StrValue=value.ToString();
            }
        }
        public int IntValue
        {
            get
            {
                int intVale = -1;
                if (!Int32.TryParse(StrValue, out intVale))
                    log.Error("Parse to Int Error" + StrValue);
                return intVale;
            }
            set
            {
                StrValue = value.ToString();
            }
        }
        public TimeSpan TimeValue
        {
            get
            {
                TimeSpan timeVale =new TimeSpan(0,0,0);
                if (!TimeSpan.TryParse(StrValue, out timeVale))
                    log.Error("Parse to Time Error" + StrValue);
                return timeVale;
            }
            set
            {
                StrValue = value.ToString();
            }
        }
        public bool CheckCondition(string dest,bool updateSign)
        {
            bool exNotFound = true;
            try
            {
                if (dest.Length < 1)
                {
                    exNotFound = true;
                }
                else if (Type == Digit.Digit.DigitType.DIGIT_STRING)
                {
                    string[] strs = StrValue.Split(';');

                    if (Compare == CompareEnum.EQUAL)
                    {
                        foreach (string strTemp in strs)
                        {
                            if (dest.Contains(strTemp))
                            {
                                exNotFound = false;
                                break;
                            }
                        }
                    }
                    else if (Compare == CompareEnum.UNEQUAL)
                    {
                        exNotFound = false;
                        foreach (string strTemp in strs)
                        {
                            if (dest.Contains(strTemp))
                            {
                                exNotFound = true;
                                break;
                            }
                        }
                    }

                }
                else if (Type == Digit.Digit.DigitType.DIGIT_INT)
                {
                    int intDest = Int32.Parse(dest);
                    int benchMarkValue = IntValue;
                    bool exFound=false;
                    if (Compare == CompareEnum.BIG)
                    {
                        exFound = intDest > benchMarkValue;
                    }
                    else if (Compare == (CompareEnum.BIG_EQUAL))
                    {
                        exFound = intDest >= benchMarkValue;
                    }
                    else if (Compare == CompareEnum.SMALL)
                    {
                        exFound = intDest < benchMarkValue;
                    }
                    else if (Compare == (CompareEnum.SMALL_EQUAL))
                    {
                        exFound = intDest >= benchMarkValue;
                    }
                    else if (Compare == CompareEnum.EQUAL)
                    {
                        exFound = intDest == benchMarkValue;
                    }
                    else if (Compare == CompareEnum.UNEQUAL)
                    {
                        exFound = intDest != benchMarkValue;
                    }

                    exNotFound = !exFound;
                }
                else
                {
                    log.Error("CheckCondition Error: " + Label.ToString()+",Value:" + dest.ToString());
                }
            }
            catch
            {
                log.Error("CheckCondition Exception: " + Label.ToString() + ",Value:" +dest.ToString());
            }

            if (updateSign)
            {
                lastCheckValue = dest;

                //Found Exception
                if (exNotFound == false)
                {
                    ExceptionValue = dest;
                    ExceptionHappenTime = DateTime.Now;
                    IsHasException = true;
                }
                //Exception End
                else if (IsHasException)
                {
                    ExceptionEndTime = DateTime.Now;
                }
            }
            else
            {
                //Set Found Exception Sign
                if (exNotFound == false)
                {
                    IsHasException = true;
                }
            }
            return exNotFound;
        }
    }
}

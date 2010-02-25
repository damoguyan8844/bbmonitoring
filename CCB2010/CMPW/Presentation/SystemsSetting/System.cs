using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JOYFULL.CMPW.Presentation.SystemsSetting
{
    public class System
    {
       
        public Dictionary<int, Condition> alertConditions= new Dictionary<int,Condition>();
        public Dictionary<int, BeginTimeParameter> beginPars=new Dictionary<int,BeginTimeParameter>();
        public Dictionary<int, FreqParameter> freqPars= new Dictionary<int,FreqParameter>();
        public Dictionary<int, EndTimeParameter> endPars= new Dictionary<int,EndTimeParameter>();

        public XmlNode m_system = null;
       
        public System( XmlNode system)
        {
            m_system = system;

            List<int> allApplayOnList = new List<int>();
            XmlNodeList sysArray = system.SelectSingleNode("AlertCondition").SelectNodes("Condition");
            foreach (XmlNode sys in sysArray)
            {
                Condition obj = new Condition(sys);
                alertConditions.Add(obj.Order, obj);
                allApplayOnList.Add(obj.Order);
            }

            XmlNodeList bParaNodes =system.SelectSingleNode("CaptureParameter").SelectNodes("BeginTimeParameter");
            foreach (XmlNode sys in bParaNodes)
            {
                BeginTimeParameter beginPar = new BeginTimeParameter(sys);

                if (beginPar.ApplayOn == null || beginPar.ApplayOn.Count < 1)
                    beginPar.ApplayOn = allApplayOnList;
                foreach (int appOns in beginPar.ApplayOn)
                {
                    beginPars.Add(appOns, beginPar);
                }
            }

            XmlNodeList fParaNodes = system.SelectSingleNode("CaptureParameter").SelectNodes("FreqParameter");
            foreach (XmlNode sys in fParaNodes)
            {
                FreqParameter frePar = new FreqParameter(sys);

                if (frePar.ApplayOn == null || frePar.ApplayOn.Count < 1)
                    frePar.ApplayOn = allApplayOnList;

                foreach (int appOns in frePar.ApplayOn)
                {
                    freqPars.Add(appOns, frePar);
                }
            }

            XmlNodeList eParaNodes = system.SelectSingleNode("CaptureParameter").SelectNodes("EndTimeParameter");
            foreach (XmlNode sys in eParaNodes)
            {
                EndTimeParameter endPar = new EndTimeParameter(sys);

                if (endPar.ApplayOn == null || endPar.ApplayOn.Count < 1)
                    endPar.ApplayOn = allApplayOnList;

                foreach (int appOns in endPar.ApplayOn)
                {
                    endPars.Add(appOns, endPar);
                }
            }
            
            Order = Int32.Parse(system.Attributes["order"].Value);
            Name = system.Attributes["name"].Value;
            Sign = system.Attributes["sign"].Value;
        }

        public Condition getCondition(string conditionLabel)
        {
            foreach (KeyValuePair<int,Condition> keyPair in alertConditions)
            {
                if (keyPair.Value.Label == conditionLabel)
                    return keyPair.Value;
            }
            return null;
        }

        public BeginTimeParameter getBeginPara( string label )
        {
            foreach (KeyValuePair<int, BeginTimeParameter> keyPair in beginPars)
            {
                if (keyPair.Value.Label == label)
                    return keyPair.Value;
            }
            return null;
        }

        public EndTimeParameter getEndPara(string label)
        {
            foreach (KeyValuePair<int, EndTimeParameter> keyPair in endPars)
            {
                if (keyPair.Value.Label == label)
                    return keyPair.Value;
            }
            return null;
        }

        public FreqParameter getFreqPara(string label)
        {
            foreach (KeyValuePair<int, FreqParameter> keyPair in freqPars)
            {
                if (keyPair.Value.Label == label)
                    return keyPair.Value;
            }
            return null;
        }

        public void AddConditionPara(Parameter.ParameterEnum paraType, Digit.Digit.DigitType digitType,string label,string value,string applayOn)
        {
            XmlNode xmlNode=m_system.SelectSingleNode("CaptureParameter");
            string title;
            

            if(paraType==Parameter.ParameterEnum.BeginTimePara)
            {
                title="BeginTimeParameter";
            }
            else if(paraType==Parameter.ParameterEnum.FreqPara)
            {
                title="FreqParameter";
            }
            else if(paraType==Parameter.ParameterEnum.BeginTimePara)
            {   
                title="EndTimeParameter";
            }
            else
                return;

            int order =0;
            if(xmlNode.SelectNodes(title)!=null)
                order = xmlNode.SelectNodes(title).Count;

            string type;
            if (digitType == Digit.Digit.DigitType.DIGIT_INT)
                type = "int";
            else if (digitType == Digit.Digit.DigitType.DIGIT_TIME)
                type = "time";
            else if (digitType == Digit.Digit.DigitType.DIGIT_STRING)
                type = "string";
            else
                return;
            XmlDocument xmlOwner=xmlNode.OwnerDocument;
            XmlElement xmlEm = xmlOwner.CreateElement(title);
            xmlEm.SetAttribute("order",order.ToString());
            xmlEm.SetAttribute("type", type.ToString());
            xmlEm.SetAttribute("label", label.ToString());
            xmlEm.SetAttribute("value", value.ToString());
            xmlEm.SetAttribute("applayon", applayOn.ToString());

            xmlNode.AppendChild(xmlEm);
        }
        public int Order { get; set; }
        public string Name { get; set; }
        public string Sign { get; private set; }
    }
}

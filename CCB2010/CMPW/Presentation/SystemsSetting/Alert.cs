using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using JOYFULL.CMPW.Alert;
using System.Configuration;

namespace JOYFULL.CMPW.Presentation.SystemsSetting
{
    public class Alert
    {
        readonly string ALERT_FOLDER =
            ConfigurationManager.AppSettings["AlertFolder"];

        public int Order { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public int Time { get; set; }
        private AlarmBuzzer buzzer = new AlarmBuzzer();

        public Alert(XmlNode alert)
        {
            Order = Int32.Parse(alert.Attributes["order"].Value);
            Name = alert.Attributes["name"].Value;
            Source = ALERT_FOLDER+alert.Attributes["source"].Value;
            Time = Int32.Parse(alert.Attributes["time"].Value);
        }

        public void Action(int seconds)
        {
            buzzer.Play(seconds,Name,Source);    
        }
        public void Stop()
        {
            buzzer.Stop();
        }
    }
}

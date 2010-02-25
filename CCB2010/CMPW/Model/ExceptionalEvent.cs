using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Model
{
    public class ExceptionalEvent
    {
        public int ID { get; set; }
        public int SubSystemMonitorJobID { get; set; }
        public string EventDes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}

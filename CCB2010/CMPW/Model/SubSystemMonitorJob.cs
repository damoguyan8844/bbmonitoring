using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Model
{
    public class SubSystemMonitorJob
    {
        public int ID { get; set; }
        public int MonitorJobID { get; set; }
        public int SysytemID { get; set; }
        public DateTime SignedInTime { get; set; }
        public DateTime SignedOutTime { get; set; }
    }
}

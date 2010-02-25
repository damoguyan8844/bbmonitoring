using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Model
{
    public class MonitorJob
    {
        public int ID { get; set; }
        public int OperatorID { get; set; }
        public int TaskDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}

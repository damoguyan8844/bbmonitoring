using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Model
{
    public class ExEvent
    {
        public int ID { get; set; }
        public int SystemID { get; set; }
        public string Description { get; set; }
        public DateTime Found { get; set; }
        public DateTime Solved { get; set; }
    }
}

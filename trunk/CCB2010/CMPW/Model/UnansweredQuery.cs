using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Model
{
    public class UnansweredQuery
    {
        public UnansweredQuery()
        {

        }

        public int ID { get; set; }
        public int Date { get; set; }
        public DateTime Time { get; set; }
        public int Value { get; set; }
    }
}

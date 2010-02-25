using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Model
{
    public class Operator
    {
        public int ID { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public bool IsAdmin
        {
            get { return Priority == 1; }
        }
    }
}

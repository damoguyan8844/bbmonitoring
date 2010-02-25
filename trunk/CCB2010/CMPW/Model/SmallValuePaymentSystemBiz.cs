using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Model
{
    public class SmallValuePaymentSystemBiz
    {
        public int ID { get; set; }
        public int BizDate { get; set; }
        public DateTime Time { get; set; }

        public int ExpenditureAccept { get; set; }
        public int ExpenditureException { get; set; }
        public int SentPackageCount { get; set; }

        public int ReceiptAccept { get; set; }
        public int ReceiptException { get; set; }
        public int ReceiptPackageCount { get; set; }
    }
}

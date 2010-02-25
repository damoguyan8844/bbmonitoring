using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Model
{
    public class LargeValuePaymentSystemBiz
    {
        public int ID { get; set; }
        public int BizDate { get; set; }
        public DateTime Time { get; set; }

        public int ExpenditureAccept { get; set; }
        public int ExpenditureReject { get; set; }
        public int ExpenditureFailure { get; set; }
        public int Queue { get; set; }
        public int ReceiptAccept { get; set; }
        public int ReceiptReject { get; set; }
        public int ReceiptFailure { get; set; }
    }
}

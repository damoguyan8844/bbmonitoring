using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using JOYFULL.CMPW.Data;
using JOYFULL.CMPW.Data.Access;
using JOYFULL.CMPW.Model;

namespace JOYFULL.CMPW.DAL
{
    public class SmallValuePaymentSystemBizDal
    {
        public SmallValuePaymentSystemBiz GetSubSystemById(int id)
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "SELECT * FROM SmallValuePaymentSystemBiz where id=" + id.ToString();
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetSmallValuePaymentSystemBizByDataRow(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        public void AddSmallValuePaymentSystemBiz(SmallValuePaymentSystemBiz objOpr)
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "Insert into SmallValuePaymentSystemBizs([BizDate],[Time],[ExpenditureAccept],[ExpenditureException],[SentPackageCount],[ReceiptAccept],[ReceiptException],[ReceiptPackageCount]) " +
                    " Values(" + objOpr.BizDate.ToString() +
                    " ,'" + objOpr.Time.ToString() +
                    " '," + objOpr.ExpenditureAccept.ToString() +
                    " ," + objOpr.ExpenditureException.ToString() +
                    " ," + objOpr.SentPackageCount.ToString() +
                    " ," + objOpr.ReceiptAccept.ToString() +
                    " ," + objOpr.ReceiptException.ToString() +
                    " ," + objOpr.ReceiptPackageCount.ToString() +
                    " )";

            DBCommandWrapper cmd =
                    dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;

            dbCore.ExecuteNonQuery(cmd);
        }

        public void RemoveSmallValuePaymentSystemBizByBizDate(int bizDate)
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "Delete FROM SmallValuePaymentSystemBizs where [BizDate]=" + bizDate.ToString();
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            dbCore.ExecuteNonQuery(cmd);
        }

        private SmallValuePaymentSystemBiz GetSmallValuePaymentSystemBizByDataRow(DataRow dr)
        {
            SmallValuePaymentSystemBiz o = new SmallValuePaymentSystemBiz();
            o.ID = Int32.Parse(dr["ID"].ToString());
            o.BizDate = Int32.Parse(dr["BizDate"].ToString());

            o.Time = Convert.ToDateTime(dr["Time"]);
            o.ExpenditureAccept = Int32.Parse(dr["ExpenditureAccept"].ToString());
            o.ExpenditureException = Int32.Parse(dr["ExpenditureException"].ToString());
            o.SentPackageCount = Int32.Parse(dr["SentPackageCount"].ToString());
           
            o.ReceiptAccept = Int32.Parse(dr["ReceiptAccept"].ToString());
            o.ReceiptException = Int32.Parse(dr["ReceiptException"].ToString());
            o.ReceiptPackageCount = Int32.Parse(dr["ReceiptPackageCount"].ToString());

            return o;
        }

        public SmallValuePaymentSystemBiz[] GetAllByDate(int date)
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "select * from SmallValuePaymentSystemBizs where [BizDate]=" +
                date.ToString() + " order by [id]";
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds == null || ds.Tables.Count == 0)
                return null;
            List<SmallValuePaymentSystemBiz> list =
                new List<SmallValuePaymentSystemBiz>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var item = GetSmallValuePaymentSystemBizByDataRow(dr);
                list.Add(item);
            }
            return list.ToArray();
        }
    }
}

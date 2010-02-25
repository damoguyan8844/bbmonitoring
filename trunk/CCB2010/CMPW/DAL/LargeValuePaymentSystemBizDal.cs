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
    public class LargeValuePaymentSystemBizDal
    {
        public LargeValuePaymentSystemBiz GetLargeValuePaymentSystemBiz(int id)
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "SELECT * FROM LargeValuePaymentSystemBizs where [id]=" + id.ToString();
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetLargeValuePaymentSystemBizByDataRow(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        public void AddLargeValuePaymentSystemBiz(LargeValuePaymentSystemBiz objOpr)
        {

            if (objOpr != null)
            {
                string sql = "Insert into LargeValuePaymentSystemBizs([BizDate],[Time],[ExpenditureAccept],[ExpenditureReject],[ExpenditureFailure],[Queue],[ReceiptAccept],[ReceiptReject],[ReceiptFailure])" +
                    " Values(" + objOpr.BizDate.ToString() +
                    " ,'" + objOpr.Time.ToString() + "'"+
                    " ," + objOpr.ExpenditureAccept.ToString() +
                    " ," + objOpr.ExpenditureReject.ToString() +
                    " ," + objOpr.ExpenditureFailure.ToString() +
                    " ," + objOpr.Queue.ToString() +
                    " ," + objOpr.ReceiptAccept.ToString() +
                    " ," + objOpr.ReceiptReject.ToString() +
                    " ," + objOpr.ReceiptFailure.ToString() +
                    " )";

                DalHelper.ExcuteSqlWithTransaction(sql);
            }
        }

        public void RemoveLargeValuePaymentSystemBizByBizDate(int bizDate)
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "Delete FROM LargeValuePaymentSystemBizs where [BizDate]=" + bizDate.ToString();
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            dbCore.ExecuteNonQuery(sql);
        }

        public LargeValuePaymentSystemBiz[] GetAllByDate( int date )
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "select * from LargeValuePaymentSystemBizs where [BizDate]=" +
                date.ToString() + " order by id";
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds == null || ds.Tables.Count == 0)
                return null;
            List<LargeValuePaymentSystemBiz> list = 
                new List<LargeValuePaymentSystemBiz>();
            foreach( DataRow dr in ds.Tables[ 0 ].Rows )
            {
                var item = GetLargeValuePaymentSystemBizByDataRow(dr);
                list.Add(item);
            }
            return list.ToArray();
        }


        private LargeValuePaymentSystemBiz GetLargeValuePaymentSystemBizByDataRow(DataRow dr)
        {
            LargeValuePaymentSystemBiz o = new LargeValuePaymentSystemBiz();
            o.ID = Int32.Parse(dr["ID"].ToString());
            o.BizDate = Int32.Parse(dr["BizDate"].ToString());

            o.Time = Convert.ToDateTime(dr["Time"]);
            o.ExpenditureAccept = Int32.Parse(dr["ExpenditureAccept"].ToString());
            o.ExpenditureReject = Int32.Parse(dr["ExpenditureReject"].ToString());
            o.ExpenditureFailure = Int32.Parse(dr["ExpenditureFailure"].ToString());
            o.Queue = Int32.Parse(dr["Queue"].ToString());
            o.ReceiptAccept = Int32.Parse(dr["ReceiptAccept"].ToString());
            o.ReceiptReject = Int32.Parse(dr["ReceiptReject"].ToString());
            o.ReceiptFailure = Int32.Parse(dr["ReceiptFailure"].ToString());
          
            return o;
        }
    }
}

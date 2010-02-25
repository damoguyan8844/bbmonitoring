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
    public class ExceptionalEventsDal
    {
         public ExceptionalEvent GetExceptionalEventById(int id)
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "SELECT * FROM ExceptionalEvents where id=" + id.ToString();
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetExceptionalEventByDataRow(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        //public ExceptionalEvent[] GetExceptionalEventBySSMJID( int id )

        public void AddExceptionalEvent(ref ExceptionalEvent objOpr)
        {
            if (objOpr != null)
            {
                string sql = "Insert Into  ExceptionalEvents(SubSystemMonitorJobID,EventDes,StartTime,EndTime) " +
                    " Values(" + objOpr.SubSystemMonitorJobID.ToString() +
                    " ,'" + objOpr.EventDes.ToString() + "'" +
                    " ,'" + objOpr.StartTime.ToString() +"'" +
                    " ,'" + objOpr.EndTime.ToString() + "'" +
                    " )";
                DalHelper.ExcuteSqlWithTransaction(sql);

                sql = "SELECT  Top 1 ID FROM ExceptionalEvents Order by id Desc";

                DbCore dbCore = new DbCore("CMPW");

                DBCommandWrapper cmd = dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
                DataSet ds = dbCore.ExecuteDataSet(cmd);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    objOpr.ID = Int32.Parse(dr["ID"].ToString());
                }
            }
        }

        public void UpdateExceptionalEvent(ExceptionalEvent objOpr)
        {
            if (objOpr != null)
            {
                string sql = "Update ExceptionalEvents Set " +
                    " SubSystemMonitorJobID=" + objOpr.SubSystemMonitorJobID.ToString() +
                    " ,EventDes='" + objOpr.EventDes.ToString() +"'"+
                    " ,StartTime='" + objOpr.StartTime.ToString() +"'"+
                    " ,EndTime='" + objOpr.EndTime.ToString() + "'"+
                    " Where  ID=" + objOpr.ID.ToString();
                DalHelper.ExcuteSqlWithTransaction(sql); 
            }
        }

        public void RemoveExceptionalEventBySubSystemMonitorJobID(int SubSystemMonitorJobID)
        {
            List<string> conditions = new List<string>();
            conditions.Add("SubSystemMonitorJobID =" + SubSystemMonitorJobID.ToString());
            RemoveMonitorJodOnConditions(conditions);
        }

        private void RemoveMonitorJodOnConditions(List<string> conditions)
        {
            if (conditions.Count < 0) return;
            DbCore dbCore = new DbCore("CMPW");
            string sql = "Delete FROM ExceptionalEvents where " + conditions[0];
            for (int index = 1; index < conditions.Count; ++index)
            {
                sql += " And " + conditions[index];
            }

            DalHelper.ExcuteSqlWithTransaction(sql); 
        }

        private ExceptionalEvent GetExceptionalEventByDataRow(DataRow dr)
        {
            ExceptionalEvent o = new ExceptionalEvent();
            o.ID = Int32.Parse(dr["ID"].ToString());
            o.SubSystemMonitorJobID = Int32.Parse(dr["SubSysytemMonitorJobID"].ToString());

            o.EventDes = dr["EventDes"].ToString();
            o.StartTime = Convert.ToDateTime(dr["StartTime"]);
            o.EndTime = Convert.ToDateTime(dr["EndTime"]);
            return o;
        }
    }
}

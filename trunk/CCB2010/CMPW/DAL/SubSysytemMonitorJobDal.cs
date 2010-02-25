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
    public class SubSystemMonitorJobDal
    {
        public SubSystemMonitorJob GetSubSystemById(int id)
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "SELECT * FROM SubSystemMonitorJobs where id=" + id.ToString();
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetSubSystemMonitorJobByDataRow(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        public void AddSubSystemMonitorJob( SubSystemMonitorJob objOpr)
        {
            if (objOpr != null)
            {
                string sql = "Insert Into  SubSystemMonitorJobs(MonitorJobID,SysytemID,SignedInTime,SignedOutTime) " +
                    " Values(" + objOpr.MonitorJobID.ToString() +
                    " ," + objOpr.SysytemID.ToString() +
                    " ,'" + objOpr.SignedInTime.ToString() +"'"+
                    " ,'" + objOpr.SignedOutTime.ToString() +"'"+
                    " )";
                DalHelper.ExcuteSqlWithTransaction(sql);
                //sql = "SELECT Top 1 ID FROM SubSystemMonitorJobs Order by id Desc";
                
                //DbCore dbCore = new DbCore("CMPW");
                
                //DBCommandWrapper cmd =dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
                //DataSet ds = dbCore.ExecuteDataSet(cmd);
                //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    DataRow dr=ds.Tables[0].Rows[0];
                //    objOpr.ID=Int32.Parse(dr["ID"].ToString());
                //}
            }
        }

        //public SubSystemMonitorJob GetSubSystemBy2ID(int monitJobID,int sysID)
        //{
        //    DbCore dbCore = new DbCore("CMPW");
        //    string sql = "SELECT Top 1 * FROM SubSystemMonitorJobs where SysytemID = " + sysID.ToString() + " and MonitorJobID =" + monitJobID.ToString() + " Order by SignedInTime Desc";
        //    DBCommandWrapper cmd =
        //        dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
        //    DataSet ds = dbCore.ExecuteDataSet(cmd);
        //    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        return GetSubSystemMonitorJobByDataRow(ds.Tables[0].Rows[0]);
        //    }
        //    return null;
        //}

        public SubSystemMonitorJob GetByDateAndSystemId( DateTime date, int sysId )
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "SELECT * FROM SubSystemMonitorJobs where SysytemID = "
                + sysId.ToString() + " and SignedInTime >='" + date.ToString() +
                "' and SignedInTime < '" + date.AddDays(1).ToString() + "'";
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetSubSystemMonitorJobByDataRow(ds.Tables[0].Rows[0]);
            }
            return null;

        }

        public void UpdateSubSystemMonitorJob(SubSystemMonitorJob objOpr)
        {
            if (objOpr != null)
            {
                string sql = "Update SubSystemMonitorJobs Set " +
                    " MonitorJobID=" + objOpr.MonitorJobID.ToString() +
                    " ,SysytemID=" + objOpr.SysytemID.ToString() +
                    " ,SignedInTime='" + objOpr.SignedInTime.ToString() +"'"+
                    " ,SignedOutTime='" + objOpr.SignedOutTime.ToString() +"'"+
                    " Where  ID=" + objOpr.ID.ToString();
                DalHelper.ExcuteSqlWithTransaction(sql); 
            }
        }

        //public void RemoveSubSystemMonitorJobByMonitorJobID(int MonitorJobID)
        //{
        //    List<string> conditions = new List<string>();
        //    conditions.Add("MonitorJobID =" + MonitorJobID.ToString());
        //    RemoveMonitorJodOnConditions(conditions);
        //}

        //public void RemoveSubSystemMonitorJobBySysytemID(int SysytemID)
        //{
        //    List<string> conditions = new List<string>();
        //    conditions.Add("SysytemID= " + SysytemID.ToString());
        //    RemoveMonitorJodOnConditions(conditions);
        //}

        private void RemoveMonitorJodOnConditions(List<string> conditions)
        {
            if (conditions.Count < 0) return;
           
            string sql = "Delete FROM SubSystemMonitorJobs where " + conditions[0];
            for (int index = 1; index < conditions.Count; ++index)
            {
                sql += " And " + conditions[index];
            }

            DalHelper.ExcuteSqlWithTransaction(sql); 
        }

        //public void RemoveMeaninglessRecords()
        //{
        //    DbCore dbCore = new DbCore("CMPW");
        //    string sql = "Delete from SubSystemMonitorJobs where SignedInTime = '" +
        //        new DateTime( 2049, 1, 1 ).ToString() + "' ";
        //    DBCommandWrapper cmd =
        //        dbCore.GetSqlStringCommandWrapper( sql ) as AccessCommandWrapper;
        //    dbCore.ExecuteNonQuery( cmd );
        //}

        private SubSystemMonitorJob GetSubSystemMonitorJobByDataRow(DataRow dr)
        {
            SubSystemMonitorJob o = new SubSystemMonitorJob();
            o.ID = Int32.Parse(dr["ID"].ToString());
            o.MonitorJobID = Int32.Parse(dr["MonitorJobID"].ToString());
            o.SysytemID = Int32.Parse(dr["SysytemID"].ToString());
            o.SignedInTime = Convert.ToDateTime(dr["SignedInTime"]);
            o.SignedOutTime = Convert.ToDateTime(dr["SignedOutTime"]);
            return o;
        }

    }
}

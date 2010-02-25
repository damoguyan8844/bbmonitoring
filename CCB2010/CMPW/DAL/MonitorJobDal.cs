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
    public class MonitorJobDal
    {
        public MonitorJob GetMonitorJobById(int id)
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "SELECT * FROM MonitorJobs where id=" + id.ToString();
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetMonitorJobByDataRow(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// 取某天第一个登陆的MonitorJob
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public MonitorJob GetFirst( int date )
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "select top 1 * from MonitorJobs where TaskDate=" +
                date.ToString() + " order by StartTime";
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetMonitorJobByDataRow(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        public MonitorJob GetMonitorJob(int today, int opId )
        {
            DbCore dbCore = new DbCore( "CMPW" );
            string sql = "select * from MonitorJobs where TaskDate=" +
                today.ToString() + " and OperatorID=" + opId.ToString() + 
                " order by StartTime";
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper( sql ) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet( cmd );
            if ( ds.Tables.Count > 0 && ds.Tables[ 0 ].Rows.Count > 0 )
            {
                return GetMonitorJobByDataRow( ds.Tables[ 0 ].Rows[ 0 ] );
            }
            return null;
        }
        public MonitorJob GetMonitorJobByDate(int operatorID,DateTime date)
        {
            DbCore dbCore = new DbCore("CMPW");
            int intDate=(int)date.ToOADate();

            string sql = "SELECT * FROM MonitorJobs where TaskDate=" + intDate.ToString() + " and OperatorID=" + operatorID.ToString();
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetMonitorJobByDataRow(ds.Tables[0].Rows[0]);
            }
            return null;
        }
        public void AddMonitorJob(MonitorJob objOpr)
        {
            if (objOpr != null)
            {
                string sql = "Insert Into  MonitorJobs(OperatorID,TaskDate,StartTime,EndTime) " +
                    " Values(" + objOpr.OperatorID.ToString() +
                    " ," + objOpr.TaskDate.ToString() +
                    " ,'" + objOpr.StartTime.ToString() +"'"+
                    " ,'" + objOpr.EndTime.ToString() +"'"+
                    " )";
                DalHelper.ExcuteSqlWithTransaction(sql);

                DbCore dbCore = new DbCore("CMPW");
                sql = "select top 1 id from MonitorJobs order by id desc";
                DBCommandWrapper cmd = dbCore.GetSqlStringCommandWrapper(sql)
                    as AccessCommandWrapper;
                objOpr.ID = int.Parse(dbCore.ExecuteScalar(cmd).ToString());
            }
        }

        public void UpdateMonitorJob(MonitorJob objOpr)
        {
            if (objOpr != null)
            {
                DbCore dbCore = new DbCore("CMPW");
                string sql = "Update MonitorJobs Set " +
                    " OperatorID=" + objOpr.OperatorID.ToString() +
                    " ,TaskDate=" + objOpr.TaskDate.ToString() +
                    " ,StartTime='" + objOpr.StartTime.ToString()+"'"+
                    " ,EndTime='" + objOpr.EndTime.ToString() +"'"+
                    " Where  ID=" + objOpr.ID.ToString();
                //DalHelper.ExcuteSqlWithTransaction(sql); 
                DBCommandWrapper cmd = dbCore.GetSqlStringCommandWrapper(sql)
                    as AccessCommandWrapper;
                dbCore.ExecuteNonQuery(cmd);
            }
        }

        public void RemoveMonitorJobByOpratorID(int operatorID)
        {
            List<string> conditions = new List<string>();
            conditions.Add("OperatorID =" + operatorID.ToString());
            RemoveMonitorJodOnConditions(conditions);
        }

        public void RemoveMonitorJobByTaskDate(int taskDate)
        {
            List<string> conditions = new List<string>();
            conditions.Add("TaskDate= " + taskDate.ToString());
            RemoveMonitorJodOnConditions(conditions);
        }

        public void RemoveMonitorJob(int operatorID, int taskDate)
        {
            List<string> conditions = new List<string>();
            if (taskDate > 0)
                conditions.Add("TaskDate= " + taskDate.ToString());
            if (operatorID > 0)
                conditions.Add("OperatorID=" + operatorID.ToString());
            RemoveMonitorJodOnConditions(conditions);
        }

        private void RemoveMonitorJodOnConditions(List<string> conditions)
        {
            if(conditions.Count<0) return ;
           
            string sql = "Delete FROM MonitorJobs where " + conditions[0];
            for (int index = 1; index < conditions.Count;++index)
            {
                sql += " And " + conditions[index];
            }

            DalHelper.ExcuteSqlWithTransaction(sql); 

        }
        private MonitorJob GetMonitorJobByDataRow(DataRow dr)
        {
            MonitorJob o = new MonitorJob();
            o.ID = Int32.Parse(dr["ID"].ToString());
            o.OperatorID = Int32.Parse(dr["OperatorID"].ToString());

            o.TaskDate = Int32.Parse(dr["TaskDate"].ToString());
            o.StartTime = Convert.ToDateTime(dr["StartTime"]);
            o.EndTime = Convert.ToDateTime(dr["EndTime"]);
            return o;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JOYFULL.CMPW.Model;
using JOYFULL.CMPW.Data;
using JOYFULL.CMPW.Data.Access;
using System.Data;

namespace JOYFULL.CMPW.DAL
{
    public class ExEventDal
    {
        public ExEvent Add( ExEvent ex )
        {
            if (ex != null)
            {
                string sql = "Insert Into ExEvent([SystemID], [Description], [Found], [Solved]) " +
                    " Values(" + ex.SystemID.ToString() +
                    ",'" + ex.Description +
                    " ','" + ex.Found.ToString() +
                    " ','" + ex.Solved.ToString() +
                    "' )";
                //string sql = "Insert Into ExEvent([SystemID], [Description], [Found], [Solved]) Values( 1, '','','')";
                DalHelper.ExcuteSqlWithTransaction(sql);

                sql = "SELECT  Top 1 ID FROM ExEvent Order by id Desc";

                DbCore dbCore = new DbCore("CMPW");

                DBCommandWrapper cmd = 
                    dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
                ex.ID = int.Parse(dbCore.ExecuteScalar(cmd).ToString());
                return ex;
            }
            return null;
        }

        public void Update( ExEvent ex )
        {
            if (ex != null)
            {
                DbCore dbCore = new DbCore("CMPW");
                string sql = "Update ExEvent Set " +
                    " [SystemID]=" + ex.SystemID.ToString() +
                    " ,[Description]='" + ex.Description +
                    "' ,[Found]='" + ex.Found.ToString()+"'"+
                    " ,[Solved]='" + ex.Solved.ToString() +"'"+
                    " Where [ID]=" + ex.ID.ToString();
                DalHelper.ExcuteSqlWithTransaction(sql); 
            }
        }

        public ExEvent[] Get( int sysId, DateTime date )
        {
            string sql = "select * from ExEvent where [SystemID] = '" +
                sysId.ToString() + "' and [Found] > '" +
                date.ToString() + "' and [Found] < '" + 
                date.AddDays(1).ToString() + "'";
            DbCore dbCore = new DbCore("CMPW");
            DBCommandWrapper cmd = 
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;
            List<ExEvent> list = new List<ExEvent>();
            foreach( DataRow dr in ds.Tables[ 0 ].Rows )
            {
                ExEvent ex = new ExEvent();
                ex.ID = int.Parse( dr["ID"].ToString() );
                ex.Description = dr["Description"].ToString();
                ex.SystemID = int.Parse(dr["SystemID"].ToString());
                ex.Found = DateTime.Parse(dr["Found"].ToString());
                DateTime solved = DateTime.Now;
                if (dr["Solved"] != DBNull.Value &&
                    DateTime.TryParse(dr["Solved"].ToString(), out solved ))
                {
                    ex.Solved = solved;
                }
                list.Add( ex );
            }
            return list.ToArray();
        }
    }
}

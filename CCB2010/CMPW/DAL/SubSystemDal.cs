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
    public class SubSystemDal
    {
        public SubSystem GetSubSystemById(int id)
        {
            DbCore dbCore = new DbCore("CMPW");
            string sql = "SELECT * FROM SubSystems where id=" + id.ToString();
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetSubSystemByDataRow(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        public void RemoveSubSystemByName(string name)
        {
            string sql = "Delete FROM SubSystems where name='" + name + "'";
            DalHelper.ExcuteSqlWithTransaction(sql); 
        }

        public void UpdateSubSystem(SubSystem objOpr)
        {
            if (objOpr != null)
            {
                string sql = "Update SubSystems Set " +
                    " Name='" + objOpr.Name.ToString() + "'" +
                    " Description='" + objOpr.Description.ToString() +"'"+
                    "where  ID=" + objOpr.ID.ToString();

                DalHelper.ExcuteSqlWithTransaction(sql); 
            }
        }

        private SubSystem GetSubSystemByDataRow(DataRow dr)
        {
            SubSystem o = new SubSystem();
            o.ID = Int32.Parse(dr["ID"].ToString());
            o.Name = dr["Name"].ToString();
            o.Description = dr["Description"].ToString();
            return o;
        }
    }
}

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
    public class OperatorDal
    {
        public Operator Validate( string user, string pswd )
        {
            DbCore dbCore = new DbCore( "CMPW" );
            string sql = "SELECT * FROM OPERATORS WHERE NAME='" +
                user + "' AND PASSWORD='" + DalHelper.Encrypt(pswd) + "'";
            DataSet ds = dbCore.ExecuteDataSet( CommandType.Text, sql );
            if( ds.Tables.Count > 0 && ds.Tables[ 0 ].Rows.Count > 0 )
            {
                return GetOperatorByDataRow( ds.Tables[ 0 ].Rows[ 0 ] );
            }
            return null;
        }
        public Operator[] GetAll()
        {
            DbCore dbCore = new DbCore( "CMPW" );
            string sql = "SELECT * FROM OPERATORS ORDER BY ID";
            DataSet ds = dbCore.ExecuteDataSet( CommandType.Text, sql );
            List<Operator> all = new List<Operator>();
            if( ds.Tables.Count > 0 && ds.Tables[ 0 ].Rows.Count > 0 )
            {
                foreach( DataRow dr in ds.Tables[ 0 ].Rows )
                {
                    Operator op = GetOperatorByDataRow( dr );
                    all.Add( op );
                }
            }
            return all.ToArray();
        }

        public string GetAdminPswd()
        {
            DbCore dbCore = new DbCore( "CMPW" );
            string sql = "SELECT Top 1 [Password] FROM OPERATORS WHERE priority=1";
            string strTemp = "1";

            DBCommandWrapper cmd =
               dbCore.GetSqlStringCommandWrapper(sql) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet(cmd);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strTemp=ds.Tables[0].Rows[0]["Password"].ToString();
                strTemp = DalHelper.Decrypt(strTemp);
            }
            
            return strTemp;
        }
        public Operator GetOperatorById( int id )
        {
            DbCore dbCore = new DbCore( "CMPW" );
            string sql = "SELECT * FROM Operators where id=" + id.ToString();
            DBCommandWrapper cmd = 
                dbCore.GetSqlStringCommandWrapper( sql ) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet( cmd );
            if( ds.Tables.Count > 0 && ds.Tables[ 0 ].Rows.Count > 0 )
            {
                return GetOperatorByDataRow(ds.Tables[ 0 ].Rows[ 0 ]);
            }
            return null;
        }
        public Operator GetOperatorByName( string name )
        {
            DbCore dbCore = new DbCore( "CMPW" );
            string sql = "SELECT * FROM Operators where name='" + name + "'";
            DBCommandWrapper cmd = 
                dbCore.GetSqlStringCommandWrapper( sql ) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet( cmd );
            if( ds.Tables.Count > 0 && ds.Tables[ 0 ].Rows.Count > 0 )
            {
                return GetOperatorByDataRow(ds.Tables[ 0 ].Rows[ 0 ]);
            }
            return null;
        }
        public int GetOperatorCount( )
        {
            DbCore dbCore = new DbCore( "CMPW" );
            string sql = "SELECT COUNT(*) FROM OPERATORS";
            int count = 0;
            Int32.TryParse( dbCore.ExecuteScalar( CommandType.Text, sql ).ToString(), 
                out count );
            return count;
        }
        public void AddOperator(Operator objOpr)
        {
            if (objOpr != null)
            {
                string password=objOpr.Password;
                password = DalHelper.Encrypt(password);
                string sql = "Insert into Operators(Name,[Password],Priority) " +
                    " Values( '" + objOpr.Name.ToString() + "'" +
                    " ,'" + password.ToString() + "'" +
                    " ," + objOpr.Priority.ToString() +
                    " )";
                DalHelper.ExcuteSqlWithTransaction(sql); 
            }
        }
        public void UpdateOperator(Operator objOpr)
        {
            if (objOpr != null)
            {
                string password = objOpr.Password;
                password = DalHelper.Encrypt(password);

                string sql = "Update Operators Set " +
                    " Name='" + objOpr.Name.ToString() + "'" +
                    " ,[Password]='" + password.ToString() + "'" +
                    " ,Priority=" + objOpr.Priority.ToString() +
                    " Where  ID=" + objOpr.ID.ToString();

                DalHelper.ExcuteSqlWithTransaction(sql); 
            }
        }
        public void RemoveOperatorByName(string name)
        {
            string sql = "Delete FROM Operators where name='" + name + "'";
            DalHelper.ExcuteSqlWithTransaction(sql); 
        }
        private Operator GetOperatorByDataRow(DataRow dr)
        {
            Operator o = new Operator();
            o.ID = Int32.Parse(dr["ID"].ToString());
            o.Name = dr["Name"].ToString();
            string strTemp=dr["Password"].ToString();
            o.Password = DalHelper.Decrypt(strTemp);
            o.Priority = Convert.ToInt32(dr["Priority"]);
            return o;
        }
    }
}

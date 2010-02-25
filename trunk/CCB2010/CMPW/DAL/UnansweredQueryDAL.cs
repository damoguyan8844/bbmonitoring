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
    public class UnansweredQueryDAL
    {
        public void Add( UnansweredQuery item )
        {
            DbCore dbCore = new DbCore( "CMPW" );
            string sql = "INSERT INTO UnansweredQuery([Date],[Time],[Value]) values( " +
                item.Date.ToString() + ", '" +
                item.Time.ToString() + "'," +
                item.Value.ToString() + ")";
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper( sql ) as AccessCommandWrapper;
            dbCore.ExecuteNonQuery( cmd );
        }

        public IList< UnansweredQuery > GetAllByDate(int date )
        {
            DbCore dbCore = new DbCore( "CMPW" );
            string sql = "SELECT * from UnansweredQuery where Date=" + date.ToString();
            DBCommandWrapper cmd =
                dbCore.GetSqlStringCommandWrapper( sql ) as AccessCommandWrapper;
            DataSet ds = dbCore.ExecuteDataSet( cmd );
            List<UnansweredQuery> list = new List<UnansweredQuery>();
            if( ds.Tables.Count > 0 && ds.Tables[ 0 ].Rows.Count > 0 )
            {
                foreach( DataRow dr in ds.Tables[ 0 ].Rows )
                {
                    list.Add( GetByDataRow( dr ) );
                }
            }
            return list;
        }

        private UnansweredQuery GetByDataRow( DataRow dr )
        {
            UnansweredQuery u = new UnansweredQuery();
            u.ID = int.Parse( dr[ "ID" ].ToString() );
            u.Date = int.Parse( dr[ "Date" ].ToString() );
            u.Time = DateTime.Parse( dr[ "Time" ].ToString() );
            u.Value = int.Parse( dr[ "Value" ].ToString() );
            return u;
        }
    }
}

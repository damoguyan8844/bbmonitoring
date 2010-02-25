using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JOYFULL.CMPW.Data;
using JOYFULL.CMPW.Data.Access;

//////////////////////////////////////////////////////////////////////////
///Setting on App.config
//////////////////////////////////////////////////////////////////////////
//<?xml version="1.0" encoding="utf-8" ?>
//<configuration>
//  <configSections>
//    <section name="JOYFULL.CMPW.Data" type="JOYFULL.CMPW.Data.DataConfigHandler, JOYFULL.CMPW.Data, Version=1.0.*, Culture=neutral"/>
//  </configSections>
//  <JOYFULL.CMPW.Data>
//    <DatabaseSettings>
//      <DatabaseSetting DatabaseKey="CMPW" MaxConnectionCount="100" MaxDataReaderCount="100" ConnectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=test.accdb" DatabaseType="MSAccess"/>
//    </DatabaseSettings>
//  </JOYFULL.CMPW.Data>

//</configuration>

//namespace JYSoft.JBS.Data
//{
//    class AccessExample
//    {
//        private void button1_Click( object sender, EventArgs e )
//        {
//            DbCore dbCore = new DbCore( "CMPW" );
//            //string sql = "SELECT * FROM Users";
//            //DBCommandWrapper cmd = 
//            //    dbCore.GetSqlStringCommandWrapper( sql ) as AccessCommandWrapper;
//            //DataSet ds = dbCore.ExecuteDataSet( cmd );
//            //var o = dbCore.ExecuteScalar( cmd );
//            dbCore.BeginTransaction();
//            try
//            {
//                for( int i = 3; i < 10; ++i )
//                {
//                    string sql = "INSERT INTO USERS VALUES( " + i.ToString() +
//                        ", 'Shot', 'IBM', 'US' )";
//                    int num = dbCore.ExecuteNonQuery( CommandType.Text, sql );
//                    if( num != 1 )
//                    {
//                        dbCore.RollbackTransaction();
//                    }
//                }
//                dbCore.CommitTransaction();
//            }
//            catch
//            {
//                dbCore.RollbackTransaction();
//            }
//        }
//    }
//}

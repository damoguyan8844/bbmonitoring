using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using log4net;

using JOYFULL.CMPW.Data;
using JOYFULL.CMPW.Data.Access;
using JOYFULL.CMPW.Model;

namespace JOYFULL.CMPW.DAL
{
    public class DalHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DalHelper));

        public static void ExcuteSqlWithTransaction(string sql)
        {
            List<string> sqls = new List<string>();
            sqls.Add(sql);
            ExcuteSqlWithTransaction(ref sqls);
        }

        public static void ExcuteSqlWithTransaction( ref  List<string>  sqls)
        {
            DbCore dbCore = new DbCore("CMPW");
            dbCore.BeginTransaction();

            string currentsql="";
            try
            {
                foreach ( string sql in sqls)
                {
                    currentsql = sql;
                    int num = dbCore.ExecuteNonQuery(CommandType.Text, sql);
                    if (num != 1)
                    {
                        dbCore.RollbackTransaction();
                    }
                }
                dbCore.CommitTransaction();
            }
            catch( Exception e )
            {
                log.Error("Error Sql: "+currentsql);
                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
                dbCore.RollbackTransaction();
            }
        }

        public static string Encrypt( string str)
        {
            return EnDeCrypt.EnDeCryptMethod.Encode(str);
        }
        public static string Decrypt(string str)
        {
            return EnDeCrypt.EnDeCryptMethod.Decode(str);
        }
    }
}

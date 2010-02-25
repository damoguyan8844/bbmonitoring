using System;


using System.Xml;
using System.Configuration;
using System.Collections.Specialized;
using System.Reflection;

namespace JOYFULL.CMPW.Data
{
    public class DataConfig
    {

        static XmlNode configRoot;
        static DatabaseSettings settings = new DatabaseSettings();
        static DataConfig()
        {
            configRoot = ConfigurationManager.GetSection("JOYFULL.CMPW.Data") as XmlNode;
            if (configRoot == null)
                throw new ArgumentException( "配置文件中没有找到 JOYFULL.CMPW.Data 节点" );
            XmlNode dataParentNode = configRoot.SelectSingleNode("DatabaseSettings");
            if (dataParentNode == null)
                throw new ArgumentException( "配置文件的 JOYFULL.CMPW.Data 节点下没有找到 DatabaseSettings 节点" );
            XmlNodeList dataSubNodes = dataParentNode.SelectNodes("DatabaseSetting");
            if (dataSubNodes == null)
                return;
            for (int i = 0; i < dataSubNodes.Count; i++)
            {
                DatabaseSettingData data = new DatabaseSettingData();
                data.DatabaseKey = dataSubNodes[i].Attributes["DatabaseKey"].Value;
                data.MaxConnectionCount = Convert.ToInt32(dataSubNodes[i].Attributes["MaxConnectionCount"].Value);
                data.MaxDataReaderCount = Convert.ToInt32(dataSubNodes[i].Attributes["MaxDataReaderCount"].Value);
                //增加解密
                data.ConnectionString = dataSubNodes[i].Attributes["ConnectionString"].Value;//GetDecryptString(dataNode[i].Attributes["ConnectionString"].Value);
                data.DatabaseType = (DatabaseType)Enum.Parse(typeof(DatabaseType), dataSubNodes[i].Attributes["DatabaseType"].Value);
                data.EnabledLog = false;

                settings.Add(data);
            }
        }



        public static XmlNode GetDataConfigRoot()
        {

            return configRoot;

        }

        public static DatabaseSettingData GetDatabaseSettingData(string DatabaseKey)
        {

            return settings[DatabaseKey];

        }


        private string GetDecryptString(string ConnectionString)
        {

             //DESEncrypt.DESEncryptor des = new DESEncryptor();
             //des.InputString = ConnectionString;
             //des.DecryptKey = "20091001";
             //des.DesDecrypt();
            return string.Empty;

        }


    }

}

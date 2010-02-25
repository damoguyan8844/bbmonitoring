/******************************************
 * 
 * 模块名称：数据库资源配置实体类
 * 当前版本：1.0
 * 开发人员：
 * 完成时间：
 * 版本历史：
 * 
 ******************************************/

using System;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// 数据库资源配置实体类。
	/// </summary>
	
	[Serializable()]
	public class DatabaseSettingData
	{
		public DatabaseSettingData()
		{
		}

		#region "私有成员变量"

		private string databaseKey;
		private int    maxConnectionCount;
		private int    maxDataReaderCount;
		private string connectionString;
		private DatabaseType databaseType;
		private bool enabledLog;

		#endregion

		#region "公共属性"

		public string DatabaseKey
		{
			get
			{
				return this.databaseKey;
			}
			set
			{
				this.databaseKey = value;
			}
		}

		public int MaxConnectionCount
		{
			get
			{
				return this.maxConnectionCount;
			}
			set
			{
				this.maxConnectionCount = value;
			}
		}

		public int MaxDataReaderCount
		{
			get
			{
				return this.maxDataReaderCount;
			}
			set
			{
				this.maxDataReaderCount = value;
			}
		}

		public string ConnectionString
		{
			get
			{
				return this.connectionString;
			}
			set
			{
				this.connectionString = value;
			}
		}

		public DatabaseType DatabaseType
		{
			get
			{
				return this.databaseType;
			}
			set
			{
				this.databaseType = value;
			}
		}

		public bool EnabledLog
		{
			get
			{
				return this.enabledLog;
			}
			set
			{
				this.enabledLog = value;
			}
		}

		#endregion

		#region "成员方法"

		
		#endregion
	}
}

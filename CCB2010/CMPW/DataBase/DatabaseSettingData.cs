/******************************************
 * 
 * ģ�����ƣ����ݿ���Դ����ʵ����
 * ��ǰ�汾��1.0
 * ������Ա��
 * ���ʱ�䣺
 * �汾��ʷ��
 * 
 ******************************************/

using System;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// ���ݿ���Դ����ʵ���ࡣ
	/// </summary>
	
	[Serializable()]
	public class DatabaseSettingData
	{
		public DatabaseSettingData()
		{
		}

		#region "˽�г�Ա����"

		private string databaseKey;
		private int    maxConnectionCount;
		private int    maxDataReaderCount;
		private string connectionString;
		private DatabaseType databaseType;
		private bool enabledLog;

		#endregion

		#region "��������"

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

		#region "��Ա����"

		
		#endregion
	}
}

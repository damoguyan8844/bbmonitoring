/******************************************
 * ģ�����ƣ�DatabaseFactory ��
 * ��ǰ�汾��1.0
 * ������Ա��
 * ���ʱ�䣺
 * �汾��ʷ��
  ******************************************/

using System;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// ���������ļ��е����ô������ݿ����ӵĹ�����
	/// </summary>
	public sealed class DatabaseFactory
	{
		private DatabaseFactory()
		{
		}

#if UNIT_TEST
		/// <summary>
		/// �������ļ��ж�ȡĬ�����ݿ����ӵ����ã�����һ�����Ӳ����ء�
		/// </summary>
		/// <returns>һ�����ݿ����ӵ� Database</returns>
		internal static Database CreateTestDatabase()
		{
			//
			// TODO: �������ļ���ȡ��Ĭ�ϵ��������ͺ������ַ�������һ������
			//
			string connectionString = "User ID=dev;Password=dev;Data Source=liuft";
			Oracle.OracleDatabase db = new Oracle.OracleDatabase();
			db.SetConnectionString(connectionString);
			return db;
		}
#endif

		/// <summary>
		/// ��ָ���������ַ������������ͣ�����һ�����Ӳ����ء�
		/// </summary>
		/// <param name="connectionString">���ݿ������ַ�����</param>
		/// <param name="databaseType">���ݿ����� <see cref="DatabaseType"/> ֵ֮һ��</param>
		/// <returns>һ�����ݿ����ӵ� <see cref="Database"/>��</returns>
		internal static Database CreateDatabase( string connectionString, DatabaseType databaseType )
		{
			Database db = null;

			if( databaseType == DatabaseType.Oracle )
			{
				db = new Oracle.OracleDatabase();
			}
			else if( databaseType == DatabaseType.SqlServer )
			{
				db = new Sql.SqlDatabase();
			}
            else if( databaseType == DatabaseType.MSAccess )
            {
                db = new Access.AccessDatabase();
            }

			db.SetConnectionString( connectionString );
			return db;
		}
		/// <summary>
		/// �������ļ��ж�ȡָ�����Ƶ����ݿ��������ýڣ�����һ�����Ӳ����ء�
		/// </summary>
		/// <param name="databaseKey">���ݿ����Ӽ�ֵ</param>
		/// <returns>һ�����ݿ����ӵ� <see cref="Database"/>��</returns>
		internal static Database CreateDatabase( string databaseKey )
		{
			DatabaseSettingData setting = DataConfig.GetDatabaseSettingData( databaseKey );
			return CreateDatabase( setting.ConnectionString, setting.DatabaseType );
		}
	}
}

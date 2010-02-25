/******************************************
 * 模块名称：DatabaseFactory 类
 * 当前版本：1.0
 * 开发人员：
 * 完成时间：
 * 版本历史：
  ******************************************/

using System;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// 根据配置文件中的配置创建数据库连接的工厂。
	/// </summary>
	public sealed class DatabaseFactory
	{
		private DatabaseFactory()
		{
		}

#if UNIT_TEST
		/// <summary>
		/// 从配置文件中读取默认数据库连接的配置，创建一个连接并返回。
		/// </summary>
		/// <returns>一个数据库连接的 Database</returns>
		internal static Database CreateTestDatabase()
		{
			//
			// TODO: 从配置文件中取得默认的连接类型和连接字符串创建一个连接
			//
			string connectionString = "User ID=dev;Password=dev;Data Source=liuft";
			Oracle.OracleDatabase db = new Oracle.OracleDatabase();
			db.SetConnectionString(connectionString);
			return db;
		}
#endif

		/// <summary>
		/// 以指定的连接字符串和连接类型，创建一个连接并返回。
		/// </summary>
		/// <param name="connectionString">数据库连接字符串。</param>
		/// <param name="databaseType">数据库类型 <see cref="DatabaseType"/> 值之一。</param>
		/// <returns>一个数据库连接的 <see cref="Database"/>。</returns>
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
		/// 从配置文件中读取指定名称的数据库连接配置节，创建一个连接并返回。
		/// </summary>
		/// <param name="databaseKey">数据库连接键值</param>
		/// <returns>一个数据库连接的 <see cref="Database"/>。</returns>
		internal static Database CreateDatabase( string databaseKey )
		{
			DatabaseSettingData setting = DataConfig.GetDatabaseSettingData( databaseKey );
			return CreateDatabase( setting.ConnectionString, setting.DatabaseType );
		}
	}
}

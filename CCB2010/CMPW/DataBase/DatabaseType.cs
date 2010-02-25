using System;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// 数据库连接类型。
	/// </summary>
	public enum DatabaseType
	{
		/// <summary>
		/// 表示连接到 Oracle 数据库。
		/// </summary>
		Oracle,
		/// <summary>
		/// 表示连接到Sql Server数据库。
		/// </summary>
		SqlServer,
        /// <summary>
        /// MS Access
        /// </summary>
        MSAccess
	}
}

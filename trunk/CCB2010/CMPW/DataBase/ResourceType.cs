/******************************************
 * 
 * 模块名称：数据资源枚举
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
	/// 数据资源类型
	/// </summary>
	internal enum ResourceType
	{
		/// <summary>
		/// 数据库连接类型
		/// </summary>
		Connection,

		/// <summary>
		/// 数据读取器类型
		/// </summary>
		DataReader

//		//本地数据库事务
//		LocalTransaction,
//
//		//分布式事务
//		DTCTransaction,
	}
}

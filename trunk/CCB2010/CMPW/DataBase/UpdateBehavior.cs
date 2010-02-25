/******************************************
 * 模块名称：UpdateBehavior 枚举
 * 当前版本：1.0
 * 开发人员：
 * 完成时间：
 * 版本历史：
  ******************************************/

using System;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// 在 Database.UpdateDataSet 方法中使用，指定在 DataAdapter 的更新命令遇到失败时的操作。
	/// </summary>
	public enum UpdateBehavior
	{
		/// <summary>
		/// 不对 DataAdapter 的更新命令干预。如果遇到失败，更新会停止，DataTable中的剩余行不会更新。
		/// </summary>
		Standard,
		/// <summary>
		/// 如果 DataAdapter 的更新命令遇到失败，更新会继续，更新命令会尝试更新剩余的数据行。 
		/// </summary>
		Continue,
		/// <summary>
		/// 如果 DataAdapter 的更新命令遇到失败, 所有的更新操作会回滚。
		/// </summary>
		Transactional
	}
}

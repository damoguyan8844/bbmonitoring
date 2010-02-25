/******************************************
 * 模块名称：DBCommandWrapper 类
 * 当前版本：1.0
 * 开发人员：
 * 完成时间：
 * 版本历史：
  ******************************************/

using System;
using System.Data;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// <para>对 <see cref="IDbCommand"/> 类型的封装。此类是一个基类，不能实例化。</para>
	/// <seealso cref="IDbCommand"/>
	/// </summary>
	public abstract class DBCommandWrapper : MarshalByRefObject, IDisposable
	{
		/// <summary>
		/// <para>在继承类中实现时，获取被封装的 <see cref="IDbCommand"/>。</para>
		/// </summary>
		/// <value>
		/// <para>被封装的  <see cref="IDbCommand"/>。默认值是 <see langword="null"/>。</para>
		/// </value>
		internal abstract IDbCommand Command { get; }

		/// <summary>
		/// <para>在继承类中实现时，获取或设置命令影响的行数。</para>
		/// </summary>
		/// <value>
		/// <para>命令影响的行数。</para>
		/// </value>
		public abstract int RowsAffected { get; set; }

		/// <summary>
		/// <para>在继承类中实现时，获取或设置命令超时时间。</para>
		/// </summary>
		/// <value>
		/// <para>命令超时时间。</para>
		/// </value>
		public abstract int CommandTimeout { get; set; }

		/// <summary>
		/// <para>在继承类中实现时，为命令增加一个参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		/// <param name="size"><para>列的长度。</para></param>
		/// <param name="direction"><para>参数方向 <see cref="ParameterDirection"/> 值之一。</para></param>
		/// <param name="nullable"><para>参数是否接受空值。</para></param>
		/// <param name="precision"><para>用于表示 <paramref name="value"/> 的最大位数。</para></param>
		/// <param name="scale"><para>将 <paramref name="value"/> 解析为的小数位数。</para></param>
		/// <param name="sourceColumn"><para>对应 <paramref name="value"/> 的源列名称。</para></param>
		/// <param name="sourceVersion"><para><see cref="DataRowVersion"/> 值之一。</para></param>
		/// <param name="value"><para>参数的值。</para></param>
		public abstract void AddParameter(string name, DbType dbType, int size, ParameterDirection direction,
			bool nullable, byte precision, byte scale, string sourceColumn,
			DataRowVersion sourceVersion, object value);

		/// <summary>
		/// <para>在继承类中实现时，为命令增加一个参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		/// <param name="direction"><para>参数方向 <see cref="ParameterDirection"/> 值之一。</para></param>
		/// <param name="sourceColumn"><para>对应 <paramref name="value"/> 的源列名称。</para></param>
		/// <param name="sourceVersion"><para><see cref="DataRowVersion"/> 值之一。</para></param>
		/// <param name="value"><para>参数的值。</para></param>
		public abstract void AddParameter(string name, DbType dbType, ParameterDirection direction,
			string sourceColumn, DataRowVersion sourceVersion, object value);

		/// <summary>
		/// <para>在继承类中实现时，为命令增加一个输出类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		/// <param name="size"><para>列的长度。</para></param>
		public abstract void AddOutParameter(string name, DbType dbType, int size);

		/// <summary>
		/// <para>在继承类中实现时，为命令增加一个输入类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		public abstract void AddInParameter(string name, DbType dbType);

		/// <summary>
		/// <para>在继承类中实现时，为命令增加一个输入类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		/// <param name="value"><para>参数的值。</para></param>
		public abstract void AddInParameter(string name, DbType dbType, object value);

		/// <summary>
		/// <para>在继承类中实现时，为命令增加一个输入类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		/// <param name="sourceColumn"><para>对应 <paramref name="value"/> 的源列名称。</para></param>
		/// <param name="sourceVersion"><para><see cref="DataRowVersion"/> 值之一。</para></param>
		public abstract void AddInParameter(string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion);

		/// <summary>
		/// <para>在继承类中实现时，为命令增加一个输出的游标类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="CursorName"><para>游标类型参数的名称。</para></param>
		public abstract void AddCursorOutParameter(string CursorName);

		/// <summary>
		/// <para>在继承类中实现时，返回指定名称 <paramref name="name"/> 的参数的值。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <returns><para>参数的值。</para></returns>
		public abstract object GetParameterValue(string name);

		/// <summary>
		/// <para>在继承类中实现时，返回指定索引 <paramref name="index"/> 的参数名称。</para>
		/// </summary>
		/// <param name="index"><para>索引。</para></param>
		/// <returns><para>参数名称。</para></returns>
		public abstract string GetParameterName(int index);

		/// <summary>
		/// <para>在继承类中实现时，返回指定索引 <paramref name="index"/> 的参数的值。</para>
		/// </summary>
		/// <param name="index"><para>索引。</para></param>
		/// <returns><para>参数的值。</para></returns>
		public abstract object GetParameterValue(int index);

		/// <summary>
		/// <para>在继承类中实现时，设置指定名称 <paramref name="name"/> 的参数的值。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="value"><para>参数的新值。</para></param>
		public abstract void SetParameterValue(string name, object value);

		/// <summary>
		/// 从数据库或缓存中取得参数集合。
		/// </summary>
		internal void DiscoverParameters(char parameterToken)
		{
			DoDiscoverParameters(parameterToken);
		}

		/// <summary>
		/// 确定是否需要从数据库或缓存中取参数。
		/// </summary>
		/// <returns>如果需要取参数，返回 true 。</returns>
		internal bool IsFurtherPreparationNeeded()
		{
			return DoIsFurtherPreparationNeeded();
		}

		/// <summary>
		/// Assign values to parameters in positional orders
		/// </summary>
		internal void AssignParameterValues()
		{
			DoAssignParameterValues();
		}

		/// <summary>
		/// <para>在继承类中实现时，从数据库中取得存储过程的参数集合。</para>
		/// </summary>
		/// <param name="parameterToken"><para>命令的参数前导符。</para></param>
		protected abstract void DoDiscoverParameters(char parameterToken);

		/// <summary>
		/// <para>When overridden in a derived class, assign the values provided by a user to the command parameters discovered in positional order.</para>
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// <para>The number of parameters does not match number of values for stored procedure.</para>
		/// </exception>
		protected abstract void DoAssignParameterValues();

		/// <summary>
		/// <para>When overridden in a derived class, determine if a stored procedure is using parameter discovery.</para>
		/// </summary>
		/// <returns>
		/// <para><see langword="true"/> if further preparation is needed.</para>
		/// </returns>
		protected abstract bool DoIsFurtherPreparationNeeded();

		/// <summary>
		/// <para>在继承类中实现时，清除所占用的资源。</para>
		/// </summary>
		public abstract void Dispose();
	}
}

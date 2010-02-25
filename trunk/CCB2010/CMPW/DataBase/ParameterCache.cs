/******************************************
 * 模块名称：ParameterCache 类
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
	/// <para>
	/// 提供一组方法，对命令执行的存储过程参数进行缓存
	/// </para>
	/// </summary>
	public class ParameterCache
	{
		private CachingMechanism cache = new CachingMechanism();

		/// <summary>
		/// <para>为命令包装从缓存中取得参数集合，或从数据库中反射参数集合。</para>
		/// </summary>
		/// <param name="command"><para>待填充参数的命令包装 <see cref="DBCommandWrapper"/>。</para></param>
		/// <param name="parameterToken"><para>表示参数的前导符。</para></param>
		public void FillParameters(DBCommandWrapper command, char parameterToken)
		{
			if (AlreadyCached(command))
			{
				AddParametersFromCache(command);
			}
			else
			{
				command.DiscoverParameters(parameterToken);
				IDataParameter[] copyOfParameters = CreateParameterCopy(command);

				this.cache.AddParameterSetToCache(command.Command, copyOfParameters);
			}

			command.AssignParameterValues();
		}

		/// <summary>
		/// <para>清空参数缓存中的所有项。</para>
		/// </summary>
		internal void Clear()
		{
			this.cache.Clear();
		}

		/// <summary>
		/// <para>检查一个指定的命令封装是否已缓存其参数信息。</para>
		/// </summary>
		/// <param name="command"><para>待检查的命令封装。</para></param>
		/// <returns>如果指定的命令封装的参数信息已缓存，返回 true，否则返回 false。</returns>
		protected virtual bool AlreadyCached(DBCommandWrapper command)
		{
			return this.cache.IsParameterSetCached(command.Command);
		}

		/// <summary>
		/// <para>为命令包装从缓存中取得参数集合</para>
		/// </summary>
		/// <param name="command"><para>待填充参数的命令包装 <see cref="DBCommandWrapper"/>。</para></param>
		protected virtual void AddParametersFromCache(DBCommandWrapper command)
		{
			IDataParameter[] parameters = this.cache.GetCachedParameterSet(command.Command);

			foreach (IDataParameter p in parameters)
			{
				command.Command.Parameters.Add(p);
			}
		}

		private static IDataParameter[] CreateParameterCopy(DBCommandWrapper command)
		{
			IDataParameterCollection parameters = command.Command.Parameters;
			IDataParameter[] parameterArray = new IDataParameter[parameters.Count];
			parameters.CopyTo(parameterArray, 0);

			return CachingMechanism.CloneParameters(parameterArray);
		}
	}
}

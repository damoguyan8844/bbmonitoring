/******************************************
 * 模块名称：CachingMechanism 类
 * 当前版本：1.0
 * 开发人员：
 * 完成时间：
 * 版本历史：
  ******************************************/

using System;
using System.Collections;
using System.Data;

namespace JOYFULL.CMPW.Data
{
	/// <devdoc>
	/// 提供一组方法，支持存储过程参数的获取和缓存。
	/// </devdoc>
	internal class CachingMechanism
	{
		private Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

		/// <devdoc>
		/// 创建并返回一个 IDataParameter[] 的副本。
		/// </devdoc>        
		public static IDataParameter[] CloneParameters(IDataParameter[] originalParameters)
		{
			IDataParameter[] clonedParameters = new IDataParameter[originalParameters.Length];

			for (int i = 0, j = originalParameters.Length; i < j; i++)
			{
				clonedParameters[i] = (IDataParameter)((ICloneable)originalParameters[i]).Clone();
			}

			return clonedParameters;
		}

		/// <devdoc>
		/// 从缓存中清除所有项。
		/// </devdoc>
		public void Clear()
		{
			this.paramCache.Clear();
		}

		/// <devdoc>
		/// 为命令增加一个参数数组到缓存中。
		/// </devdoc>        
		public void AddParameterSetToCache(IDbCommand command, IDataParameter[] parameters)
		{
			string connectionString = command.Connection.ConnectionString;
			string storedProcedure = command.CommandText;
			string key = CreateHashKey(connectionString, storedProcedure);
			this.paramCache[key] = parameters;
		}

		/// <devdoc>
		/// 为命令从缓存中取得参数数组，如果没有找到对应的参数数组，则返回 null 。
		/// </devdoc>        
		public IDataParameter[] GetCachedParameterSet(IDbCommand command)
		{
			string connectionString = command.Connection.ConnectionString;
			string storedProcedure = command.CommandText;
			string key = CreateHashKey(connectionString, storedProcedure);
			IDataParameter[] cachedParameters = (IDataParameter[])(this.paramCache[key]);
			return CloneParameters(cachedParameters);
		}

		/// <devdoc>
		/// 确定指定命令是否在缓存中有缓存的参数集
		/// </devdoc>        
		public bool IsParameterSetCached(IDbCommand command)
		{
			string hashKey = CreateHashKey(
				command.Connection.ConnectionString,
				command.CommandText);
			return this.paramCache[hashKey] != null;
		}

		private static string CreateHashKey(string connectionString, string storedProcedure)
		{
			return connectionString + ":" + storedProcedure;
		}
	}
}

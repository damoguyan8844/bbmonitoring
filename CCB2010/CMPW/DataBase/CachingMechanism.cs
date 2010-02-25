/******************************************
 * ģ�����ƣ�CachingMechanism ��
 * ��ǰ�汾��1.0
 * ������Ա��
 * ���ʱ�䣺
 * �汾��ʷ��
  ******************************************/

using System;
using System.Collections;
using System.Data;

namespace JOYFULL.CMPW.Data
{
	/// <devdoc>
	/// �ṩһ�鷽����֧�ִ洢���̲����Ļ�ȡ�ͻ��档
	/// </devdoc>
	internal class CachingMechanism
	{
		private Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

		/// <devdoc>
		/// ����������һ�� IDataParameter[] �ĸ�����
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
		/// �ӻ�������������
		/// </devdoc>
		public void Clear()
		{
			this.paramCache.Clear();
		}

		/// <devdoc>
		/// Ϊ��������һ���������鵽�����С�
		/// </devdoc>        
		public void AddParameterSetToCache(IDbCommand command, IDataParameter[] parameters)
		{
			string connectionString = command.Connection.ConnectionString;
			string storedProcedure = command.CommandText;
			string key = CreateHashKey(connectionString, storedProcedure);
			this.paramCache[key] = parameters;
		}

		/// <devdoc>
		/// Ϊ����ӻ�����ȡ�ò������飬���û���ҵ���Ӧ�Ĳ������飬�򷵻� null ��
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
		/// ȷ��ָ�������Ƿ��ڻ������л���Ĳ�����
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

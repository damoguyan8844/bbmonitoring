/******************************************
 * ģ�����ƣ�ParameterCache ��
 * ��ǰ�汾��1.0
 * ������Ա��
 * ���ʱ�䣺
 * �汾��ʷ��
  ******************************************/

using System;
using System.Data;

namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// <para>
	/// �ṩһ�鷽����������ִ�еĴ洢���̲������л���
	/// </para>
	/// </summary>
	public class ParameterCache
	{
		private CachingMechanism cache = new CachingMechanism();

		/// <summary>
		/// <para>Ϊ�����װ�ӻ�����ȡ�ò������ϣ�������ݿ��з���������ϡ�</para>
		/// </summary>
		/// <param name="command"><para>���������������װ <see cref="DBCommandWrapper"/>��</para></param>
		/// <param name="parameterToken"><para>��ʾ������ǰ������</para></param>
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
		/// <para>��ղ��������е������</para>
		/// </summary>
		internal void Clear()
		{
			this.cache.Clear();
		}

		/// <summary>
		/// <para>���һ��ָ���������װ�Ƿ��ѻ����������Ϣ��</para>
		/// </summary>
		/// <param name="command"><para>�����������װ��</para></param>
		/// <returns>���ָ���������װ�Ĳ�����Ϣ�ѻ��棬���� true�����򷵻� false��</returns>
		protected virtual bool AlreadyCached(DBCommandWrapper command)
		{
			return this.cache.IsParameterSetCached(command.Command);
		}

		/// <summary>
		/// <para>Ϊ�����װ�ӻ�����ȡ�ò�������</para>
		/// </summary>
		/// <param name="command"><para>���������������װ <see cref="DBCommandWrapper"/>��</para></param>
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

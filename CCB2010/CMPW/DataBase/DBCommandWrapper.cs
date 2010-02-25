/******************************************
 * ģ�����ƣ�DBCommandWrapper ��
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
	/// <para>�� <see cref="IDbCommand"/> ���͵ķ�װ��������һ�����࣬����ʵ������</para>
	/// <seealso cref="IDbCommand"/>
	/// </summary>
	public abstract class DBCommandWrapper : MarshalByRefObject, IDisposable
	{
		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ����ȡ����װ�� <see cref="IDbCommand"/>��</para>
		/// </summary>
		/// <value>
		/// <para>����װ��  <see cref="IDbCommand"/>��Ĭ��ֵ�� <see langword="null"/>��</para>
		/// </value>
		internal abstract IDbCommand Command { get; }

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ����ȡ����������Ӱ���������</para>
		/// </summary>
		/// <value>
		/// <para>����Ӱ���������</para>
		/// </value>
		public abstract int RowsAffected { get; set; }

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ����ȡ���������ʱʱ�䡣</para>
		/// </summary>
		/// <value>
		/// <para>���ʱʱ�䡣</para>
		/// </value>
		public abstract int CommandTimeout { get; set; }

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ��Ϊ��������һ������ <see cref="IDataParameter"/>��</para>
		/// </summary>
		/// <param name="name"><para>�������ơ�</para></param>
		/// <param name="dbType"><para>�������� <see cref="DbType"/> ֵ֮һ��</para></param>
		/// <param name="size"><para>�еĳ��ȡ�</para></param>
		/// <param name="direction"><para>�������� <see cref="ParameterDirection"/> ֵ֮һ��</para></param>
		/// <param name="nullable"><para>�����Ƿ���ܿ�ֵ��</para></param>
		/// <param name="precision"><para>���ڱ�ʾ <paramref name="value"/> �����λ����</para></param>
		/// <param name="scale"><para>�� <paramref name="value"/> ����Ϊ��С��λ����</para></param>
		/// <param name="sourceColumn"><para>��Ӧ <paramref name="value"/> ��Դ�����ơ�</para></param>
		/// <param name="sourceVersion"><para><see cref="DataRowVersion"/> ֵ֮һ��</para></param>
		/// <param name="value"><para>������ֵ��</para></param>
		public abstract void AddParameter(string name, DbType dbType, int size, ParameterDirection direction,
			bool nullable, byte precision, byte scale, string sourceColumn,
			DataRowVersion sourceVersion, object value);

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ��Ϊ��������һ������ <see cref="IDataParameter"/>��</para>
		/// </summary>
		/// <param name="name"><para>�������ơ�</para></param>
		/// <param name="dbType"><para>�������� <see cref="DbType"/> ֵ֮һ��</para></param>
		/// <param name="direction"><para>�������� <see cref="ParameterDirection"/> ֵ֮һ��</para></param>
		/// <param name="sourceColumn"><para>��Ӧ <paramref name="value"/> ��Դ�����ơ�</para></param>
		/// <param name="sourceVersion"><para><see cref="DataRowVersion"/> ֵ֮һ��</para></param>
		/// <param name="value"><para>������ֵ��</para></param>
		public abstract void AddParameter(string name, DbType dbType, ParameterDirection direction,
			string sourceColumn, DataRowVersion sourceVersion, object value);

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ��Ϊ��������һ��������͵Ĳ��� <see cref="IDataParameter"/>��</para>
		/// </summary>
		/// <param name="name"><para>�������ơ�</para></param>
		/// <param name="dbType"><para>�������� <see cref="DbType"/> ֵ֮һ��</para></param>
		/// <param name="size"><para>�еĳ��ȡ�</para></param>
		public abstract void AddOutParameter(string name, DbType dbType, int size);

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ��Ϊ��������һ���������͵Ĳ��� <see cref="IDataParameter"/>��</para>
		/// </summary>
		/// <param name="name"><para>�������ơ�</para></param>
		/// <param name="dbType"><para>�������� <see cref="DbType"/> ֵ֮һ��</para></param>
		public abstract void AddInParameter(string name, DbType dbType);

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ��Ϊ��������һ���������͵Ĳ��� <see cref="IDataParameter"/>��</para>
		/// </summary>
		/// <param name="name"><para>�������ơ�</para></param>
		/// <param name="dbType"><para>�������� <see cref="DbType"/> ֵ֮һ��</para></param>
		/// <param name="value"><para>������ֵ��</para></param>
		public abstract void AddInParameter(string name, DbType dbType, object value);

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ��Ϊ��������һ���������͵Ĳ��� <see cref="IDataParameter"/>��</para>
		/// </summary>
		/// <param name="name"><para>�������ơ�</para></param>
		/// <param name="dbType"><para>�������� <see cref="DbType"/> ֵ֮һ��</para></param>
		/// <param name="sourceColumn"><para>��Ӧ <paramref name="value"/> ��Դ�����ơ�</para></param>
		/// <param name="sourceVersion"><para><see cref="DataRowVersion"/> ֵ֮һ��</para></param>
		public abstract void AddInParameter(string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion);

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ��Ϊ��������һ��������α����͵Ĳ��� <see cref="IDataParameter"/>��</para>
		/// </summary>
		/// <param name="CursorName"><para>�α����Ͳ��������ơ�</para></param>
		public abstract void AddCursorOutParameter(string CursorName);

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ������ָ������ <paramref name="name"/> �Ĳ�����ֵ��</para>
		/// </summary>
		/// <param name="name"><para>�������ơ�</para></param>
		/// <returns><para>������ֵ��</para></returns>
		public abstract object GetParameterValue(string name);

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ������ָ������ <paramref name="index"/> �Ĳ������ơ�</para>
		/// </summary>
		/// <param name="index"><para>������</para></param>
		/// <returns><para>�������ơ�</para></returns>
		public abstract string GetParameterName(int index);

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ������ָ������ <paramref name="index"/> �Ĳ�����ֵ��</para>
		/// </summary>
		/// <param name="index"><para>������</para></param>
		/// <returns><para>������ֵ��</para></returns>
		public abstract object GetParameterValue(int index);

		/// <summary>
		/// <para>�ڼ̳�����ʵ��ʱ������ָ������ <paramref name="name"/> �Ĳ�����ֵ��</para>
		/// </summary>
		/// <param name="name"><para>�������ơ�</para></param>
		/// <param name="value"><para>��������ֵ��</para></param>
		public abstract void SetParameterValue(string name, object value);

		/// <summary>
		/// �����ݿ�򻺴���ȡ�ò������ϡ�
		/// </summary>
		internal void DiscoverParameters(char parameterToken)
		{
			DoDiscoverParameters(parameterToken);
		}

		/// <summary>
		/// ȷ���Ƿ���Ҫ�����ݿ�򻺴���ȡ������
		/// </summary>
		/// <returns>�����Ҫȡ���������� true ��</returns>
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
		/// <para>�ڼ̳�����ʵ��ʱ�������ݿ���ȡ�ô洢���̵Ĳ������ϡ�</para>
		/// </summary>
		/// <param name="parameterToken"><para>����Ĳ���ǰ������</para></param>
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
		/// <para>�ڼ̳�����ʵ��ʱ�������ռ�õ���Դ��</para>
		/// </summary>
		public abstract void Dispose();
	}
}

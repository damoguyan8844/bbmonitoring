/******************************************
 * 模块名称：OracleCommandWrapper 类
 * 当前版本：1.0
 * 开发人员：
 * 完成时间：
 * 版本历史：
  ******************************************/

using System;
using System.Collections;
using System.Data;
using System.Data.OracleClient;
using System.Globalization;

namespace JOYFULL.CMPW.Data.Oracle
{
	/// <summary>
	/// <para>连接Oracle数据库的命令封装。</para>
	/// </summary>   
	public class OracleCommandWrapper : DBCommandWrapper
	{
		private OracleCommand command;
		private int rowsAffected;
		private Hashtable guidParameters;
		private object[] parameterValues;
		private bool parameterDiscoveryRequired;
		private bool isRefCursorAdded;
		private char parameterToken;

		/// <summary>
		/// <para>使用查询字符串和命令类型初使化 <see cref="OracleCommandWrapper"/> 类的实例。</para>
		/// </summary>
		/// <param name="commandText"><para>存储过程名称或者查询语句。</para></param>
		/// <param name="commandType"><para>命令类型，<see crer="CommandType"/> 值之一。</para></param>
		/// <param name="parameterToken"><para>命令的参数前导符。</para></param>
		internal OracleCommandWrapper(string commandText, CommandType commandType, char parameterToken)
		{
			this.parameterToken = parameterToken;
			this.command = CreateCommand(commandText, commandType);
			this.guidParameters = new Hashtable();
		}

		/// <summary>
		/// <para>使用查询字符串，命令类型和参数值初使化 <see cref="OracleCommandWrapper"/> 类的实例。</para>
		/// </summary>
		/// <param name="commandText"><para>存储过程名称或者查询语句。</para></param>
		/// <param name="commandType"><para>命令类型，<see crer="CommandType"/> 值之一。</para></param>
		/// <param name="parameterToken"><para>命令的参数前导符。</para></param>
		/// <param name="parameterValues"><para>参数值。</para></param>
		internal OracleCommandWrapper(string commandText, CommandType commandType, char parameterToken, object[] parameterValues) : this(commandText, commandType, parameterToken)
		{
			this.parameterValues = parameterValues;
			if (commandType == CommandType.StoredProcedure)
			{
				this.parameterDiscoveryRequired = true;
			}
		}

		/// <devdoc>
		/// 确定是否需要提取参数。
		/// </devdoc>
		internal bool ParameterDiscoveryRequired
		{
			get { return parameterDiscoveryRequired; }
		}

		/// <summary>
		/// <para>获取被封装的 <see cref="IDbCommand"/>.</para>
		/// </summary>
		/// <value>
		/// <para>被封装的 <see cref="IDbCommand"/>. 默认值是 <see langword="null"/>.</para>
		/// </value>
		/// <seealso cref="OracleCommand"/>
		internal override IDbCommand Command
		{
			get { return this.command; }
		}

		/// <summary>
		/// <para>获取或设置此命令所影响的行数。</para>
		/// </summary>
		/// <value>
		/// <para>此命令所影响的行数。</para>
		/// </value>
		public override int RowsAffected
		{
			get { return this.rowsAffected; }
			set { this.rowsAffected = value; }
		}

		/// <summary>
		/// <para>获取或设置命令超时时间。</para>
		/// </summary>
		/// <value>
		/// <para>命令超时时间。</para>
		/// </value>
		/// <remarks>
		/// <para>内部 <see cref="OracleCommand"/> 不实现命令超时。</para>
		/// </remarks>
		public override int CommandTimeout
		{
			get { return -1; }
			set
			{
			}
		}

		internal bool IsRefCursorAdded
		{
			get { return isRefCursorAdded; }
		}

		/// <summary>
		/// <para>为命令增加一个参数 <see cref="IDataParameter"/>。</para>
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
		public override void AddParameter(string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			this.command.Parameters.Add(CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value));
		}

		/// <summary>
		/// <para>为命令增加一个参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="oracleType"><para>参数类型 <see cref="OracleType"/> 值之一。</para></param>
		/// <param name="size"><para>列的长度。</para></param>
		/// <param name="direction"><para>参数方向 <see cref="ParameterDirection"/> 值之一。</para></param>
		/// <param name="nullable"><para>参数是否接受空值。</para></param>
		/// <param name="precision"><para>用于表示 <paramref name="value"/> 的最大位数。</para></param>
		/// <param name="scale"><para>将 <paramref name="value"/> 解析为的小数位数。</para></param>
		/// <param name="sourceColumn"><para>对应 <paramref name="value"/> 的源列名称。</para></param>
		/// <param name="sourceVersion"><para><see cref="DataRowVersion"/> 值之一。</para></param>
		/// <param name="value"><para>参数的值。</para></param>
		public void AddParameter(string name, OracleType oracleType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			OracleParameter param = CreateParameter(name, DbType.AnsiString, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
			if (oracleType == OracleType.Cursor)
			{
				isRefCursorAdded = true;
			}
			param.OracleType = oracleType;
			this.command.Parameters.Add(param);
		}

		/// <summary>
		/// <para>为命令增加一个参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="oracleType"><para>参数类型 <see cref="OracleType"/> 值之一。</para></param>
		/// <param name="size"><para>列的长度。</para></param>
		/// <param name="direction"><para>参数方向 <see cref="ParameterDirection"/> 值之一。</para></param>
		/// <param name="value"><para>参数的值。</para></param>
		public void AddParameter(string name, OracleType oracleType, int size, ParameterDirection direction, object value)
		{
			AddParameter(name, oracleType, size, direction, true, 0, 0, string.Empty, DataRowVersion.Default, value);
		}

		/// <summary>
		/// <para>为命令增加一个参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		/// <param name="direction"><para>参数方向 <see cref="ParameterDirection"/> 值之一。</para></param>
		/// <param name="sourceColumn"><para>对应 <paramref name="value"/> 的源列名称。</para></param>
		/// <param name="sourceVersion"><para><see cref="DataRowVersion"/> 值之一。</para></param>
		/// <param name="value"><para>参数的值。</para></param>
		public override void AddParameter(string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			this.command.Parameters.Add(CreateParameter(name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value));
		}

		/// <summary>
		/// <para>为命令增加一个输出类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		/// <param name="size"><para>列的长度。</para></param>
		public override void AddOutParameter(string name, DbType dbType, int size)
		{
			this.command.Parameters.Add(CreateParameter(name, dbType, size, ParameterDirection.Output, false, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value));
		}
		/// <summary>
		/// <para>为命令增加一个输出类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="oracleType"><para>参数类型 <see cref="OracleType"/> 值之一。</param>
		/// <param name="size"><para>列的长度。</para></param>
		public void AddOutParameter(string name, OracleType oracleType, int size)
		{
			AddParameter(name, oracleType, size, ParameterDirection.Output, true, 0, 0, string.Empty, DataRowVersion.Default, null);
		}

		/// <summary>
		/// <para>为命令增加一个输入类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		public override void AddInParameter(string name, DbType dbType)
		{
			this.command.Parameters.Add(CreateParameter(name, dbType, 0, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, null));
		}

		/// <summary>
		/// <para>为命令增加一个输入类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		/// <param name="value"><para>参数的值。</para></param>
		public override void AddInParameter(string name, DbType dbType, object value)
		{
			this.command.Parameters.Add(CreateParameter(name, dbType, 0, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, value));
		}

		/// <summary>
		/// <para>为命令增加一个输入类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="dbType"><para>参数类型 <see cref="DbType"/> 值之一。</para></param>
		/// <param name="sourceColumn"><para>对应 <paramref name="value"/> 的源列名称。</para></param>
		/// <param name="sourceVersion"><para><see cref="DataRowVersion"/> 值之一。</para></param>
		public override void AddInParameter(string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion)
		{
			this.command.Parameters.Add(CreateParameter(name, dbType, 0, ParameterDirection.Input, false, 0, 0, sourceColumn, sourceVersion, null));
		}
		/// <summary>
		/// <para>为命令增加一个输入类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="oracleType"><para>参数类型 <see cref="OracleType"/> 值之一。</para></param>
		/// <param name="size"><para>列的长度。</para></param>
		/// <param name="value"><para>参数的值。</para></param>
		public void AddInParameter( string name, OracleType oracleType, int size, object value )
		{
			AddParameter(name, oracleType, size, ParameterDirection.Input, true, 0, 0, string.Empty, DataRowVersion.Default, value);
		}

		/// <summary>
		/// <para>为命令增加一个输出的游标类型的参数 <see cref="IDataParameter"/>。</para>
		/// </summary>
		/// <param name="CursorName"><para>游标类型参数的名称。</para></param>
		public override void AddCursorOutParameter(string CursorName)
		{
			this.AddParameter(CursorName, OracleType.Cursor, 0, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull);
		}

		/// <summary>
		/// <para>返回指定名称 <paramref name="name"/> 的参数的值。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <returns><para>参数的值。</para></returns>
		public override object GetParameterValue(string name)
		{
			string parameterName = name;
			OracleParameter parameter = command.Parameters[parameterName];
			//check for DBNull
			if (parameter.Value is DBNull)
			{
				return DBNull.Value;
			}
			// cast the parameter as Guid if it is a guid parameter
			if (guidParameters.Contains(parameterName))
			{
				byte[] buffer = (byte[])parameter.Value;
				if (buffer.Length == 0)
				{
					return DBNull.Value;
				}
				else
				{
					return new Guid(buffer);
				}
			}
				// cast the parameter as Boolean if it is a boolean parameter
			else if (parameter.DbType == DbType.Boolean)
			{
				return Convert.ToBoolean(parameter.Value, CultureInfo.InvariantCulture);
			}
			return parameter.Value;
		}

		/// <summary>
		/// <para>返回指定索引 <paramref name="index"/> 的参数的值。</para>
		/// </summary>
		/// <param name="index"><para>索引。</para></param>
		/// <returns><para>参数名称。</para></returns>
		public override string GetParameterName(int index)
		{
			OracleParameter parameter = command.Parameters[index];
			return parameter.ParameterName;
		}

		/// <summary>
		/// <para>返回指定索引 <paramref name="index"/> 的参数的值。</para>
		/// </summary>
		/// <param name="index"><para>索引。</para></param>
		/// <returns><para>参数的值。</para></returns>
		public override object GetParameterValue(int index)
		{
			OracleParameter parameter = command.Parameters[index];
			//check for DBNull
			if (parameter.Value is DBNull)
			{
				return DBNull.Value;
			}
			// cast the parameter as Guid if it is a guid parameter
			if (guidParameters.Contains(parameter.ParameterName))
			{
				byte[] buffer = (byte[])parameter.Value;
				if (buffer.Length == 0)
				{
					return DBNull.Value;
				}
				else
				{
					return new Guid(buffer);
				}
			}
				// cast the parameter as Boolean if it is a boolean parameter
			else if (parameter.DbType == DbType.Boolean)
			{
				return Convert.ToBoolean(parameter.Value, CultureInfo.InvariantCulture);
			}
			return parameter.Value;
		}

		/// <summary>
		/// <para>设置指定名称 <paramref name="name"/> 的参数的值。</para>
		/// </summary>
		/// <param name="name"><para>参数名称。</para></param>
		/// <param name="value"><para>参数的新值。</para></param>
		public override void SetParameterValue(string name, object value)
		{
			OracleParameter parameter = command.Parameters[name];
			if (value is Guid)
			{
				parameter.Value = ((Guid)value).ToByteArray();
			}
			string tmpVal = value as string;
			if ((tmpVal != null) && (tmpVal.Length == 0))
			{
				parameter.Value = Convert.DBNull;
			}
			parameter.Value = (value == null) ? DBNull.Value : value;
		}

		/// <summary>
		/// <para>清除所占用的资源。</para>
		/// </summary>
		public override void Dispose()
		{
			this.command.Dispose();
		}

		/// <summary>
		/// <para>Dicover the parameters for a stored procedure using a separate connection and command.</para>
		/// </summary>
		/// <param name="parameterToken"><para>The parameter delimeter for database commands.</para></param>
		protected override void DoDiscoverParameters(char parameterToken)
		{
			try
			{
				this.parameterToken = parameterToken;
				using (OracleCommand newCommand = CreateNewCommandAndConnectionForDiscovery())
				{
					OracleCommandBuilder.DeriveParameters(newCommand);

					foreach (IDataParameter parameter in newCommand.Parameters)
					{
						IDataParameter cloneParameter = (IDataParameter)((ICloneable)parameter).Clone();
						cloneParameter.ParameterName = cloneParameter.ParameterName;
						this.command.Parameters.Add(cloneParameter);
					}

					newCommand.Connection.Close();
				}
			}
			catch( Exception ex )
			{
//				if( ex is System.Data.OracleClient.OracleException )
//				{
//					throw OracleException.Wrapper( ex as System.Data.OracleClient.OracleException );
//				}
				throw ex;
			}
		}

		/// <summary>
		/// <para>Assign the values provided by a user to the command parameters discovered in positional order.</para>
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// <para>The number of parameters does not match number of values for stored procedure.</para>
		/// </exception>
		protected override void DoAssignParameterValues()
		{
			if (SameNumberOfParametersAndValues() == false)
			{
				throw new InvalidOperationException("存储过程的参数数目和提供的值数目不匹配");
			}

			int returnParameter = 0;
			for (int i = 0; i < this.parameterValues.Length; i++)
			{
				IDataParameter parameter = this.command.Parameters[i + returnParameter];

				// There used to be code here that checked to see if the parameter was input or input/output
				// before assigning the value to it. We took it out because of an operational bug with
				// deriving parameters for a stored procedure. It turns out that output parameters are set
				// to input/output after discovery, so any direction checking was unneeded. Should it ever
				// be needed, it should go here, and check that a parameter is input or input/output before
				// assigning a value to it.
				SetParameterValue(parameter.ParameterName, this.parameterValues[i]);
			}
		}

		/// <summary>
		/// <para>Determine if a stored procedure is using parameter discovery.</para>
		/// </summary>
		/// <returns>
		/// <para><see langword="true"/> if further preparation is needed.</para>
		/// </returns>
		protected override bool DoIsFurtherPreparationNeeded()
		{
			return this.parameterDiscoveryRequired;
		}

		private OracleParameter CreateParameter(string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
		{
			OracleParameter param = this.command.CreateParameter();
			param.ParameterName = name;
			param.DbType = dbType;
			param.Size = size;
			param.Value = (value == null) ? DBNull.Value : value;
			// modify parameter type and value for special cases
			switch (dbType)
			{
					// for Guid, change to value to byte array
				case DbType.Guid:
					guidParameters.Add(param.ParameterName, "System.Guid");
					param.OracleType = OracleType.Raw;
					param.Size = 16;
					// convert Guid value to byte array only if not null
					if ((value is DBNull) || (value == null))
					{
						param.Value = Convert.DBNull;
					}
					else
					{
						param.Value = ((Guid)value).ToByteArray();
					}
					break;
					//                case DbType.AnsiString:
					//                case DbType.AnsiStringFixedLength:
					//                case DbType.String:
					//                case DbType.StringFixedLength:                
					//                    // for empty string, set it to DBNull
					//                    if ((value == null) || (!(value is DBNull)) && ((string)value).Length == 0) 
					//                    {
					//                        param.Value = Convert.DBNull;
					//                    }
					//                    break;
				default:
					break;
			}

			param.Direction = direction;
			param.IsNullable = nullable;
			param.Precision = precision;
			param.Scale = scale;
			param.SourceColumn = sourceColumn;
			param.SourceVersion = sourceVersion;
			return param;
		}

		private bool SameNumberOfParametersAndValues()
		{
			int returnParameterCount = 0;
			int numberOfParametersToStoredProcedure = this.command.Parameters.Count - returnParameterCount;
			int numberOfValuesProvidedForStoredProcedure = this.parameterValues.Length;
			return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
		}

		/// <devdoc>
		/// Discovery has to be done on its own connection to allow for the case of the
		/// connection being used being enrolled in a transaction. The OracleCommandBuilder.DeriveParameters
		/// method creates a new OracleCommand internally to communicate to the database, and it
		/// reuses the same connection that is passed in on the command object. If this command
		/// object has a connection that is enrolled in a transaction, the DeriveParameters method does not
		/// honor that transaction, and the call fails. To avoid this, create your own connection and
		/// command, and use them. 
		/// 
		/// You then have to clone each of the IDataParameter objects before it can be transferred to 
		/// the original command, or another exception is thrown.
		/// </devdoc>
		private OracleCommand CreateNewCommandAndConnectionForDiscovery()
		{
			try
			{
				OracleConnection clonedConnection = (OracleConnection)((ICloneable)this.command.Connection).Clone();
				clonedConnection.Open();
				OracleCommand newCommand = CreateCommand(this.command.CommandText, this.command.CommandType);
				newCommand.Connection = clonedConnection;
				return newCommand;
			}
			catch( Exception ex )
			{
//				if( ex is System.Data.OracleClient.OracleException )
//				{
//					throw OracleException.Wrapper( ex as System.Data.OracleClient.OracleException );
//				}
				throw ex;
			}
		}

		private static OracleCommand CreateCommand(string commandText, CommandType commandType)
		{
			OracleCommand newCommand = new OracleCommand();
			newCommand.CommandText = commandText;
			newCommand.CommandType = commandType;
			return newCommand;
		}
	}
}

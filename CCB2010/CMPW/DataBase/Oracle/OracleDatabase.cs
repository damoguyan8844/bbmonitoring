/******************************************
 * 模块名称：OracleDatabase 类
 * 当前版本：1.0
 * 开发人员：
 * 完成时间：
 * 版本历史：
  ******************************************/

using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Globalization;

namespace JOYFULL.CMPW.Data.Oracle
{
	/// <summary>
	/// <para>Represents an Oracle Database.</para>
	/// </summary>
	/// <remarks> 
	/// <para>
	/// Internally uses Oracle .NET Managed Provider from Microsoft (System.Data.OracleClient) to connect to Oracle 9i database.
	/// </para>  
	/// <para>
	/// When retrieving a result set, it will build the package name. The package name should be set based
	/// on the stored procedure prefix and this should be set via configuration. For 
	/// example, a package name should be set as prefix of "ENTLIB_" and package name of
	/// "pkgENTLIB_ARCHITECTURE". For your applications, this is required only if you are defining your stored procedures returning 
	/// ref cursors.
	/// </para>
	/// </remarks>
	public class OracleDatabase : Database
	{
		private const string RefCursorName = "cur_OUT";

		/// <summary>
		/// Default constructor
		/// </summary>
		internal OracleDatabase() : base()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionString"></param>
		internal OracleDatabase(string connectionString) : base()
		{
			base.SetConnectionString(connectionString);
		}

		/// <summary>
		/// <para>Gets the parameter token used to delimit parameters for the Oracle Database.</para>
		/// </summary>
		/// <value>
		/// <para>The ':' symbol.</para>
		/// </value>
		protected internal override char ParameterToken
		{
			get { return ':'; }
		}

		/// <summary>
		/// <para>Get the connection for this database.</para>
		/// <seealso cref="IDbConnection"/>
		/// <seealso cref="OracleConnection"/>
		/// </summary>
		/// <returns>
		/// <para>The <see cref="OracleConnection"/> for this database.</para>
		/// </returns>
		internal override IDbConnection GetConnection()
		{
			return new OracleConnection(base.ConnectionString);
		}

		/// <summary>
		/// <para>Create an <see cref="OracleCommandWrapper"/> for a stored procedure.</para>
		/// </summary>
		/// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
		/// <returns><para>The <see cref="OracleCommandWrapper"/> for the stored procedure.</para></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="storedProcedureName"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <para><paramref name="storedProcedureName"/> hast not been initialized.</para>
		/// </exception>
		internal override DBCommandWrapper GetStoredProcCommandWrapper(string storedProcedureName)
		{
			ArgumentValidation.CheckForNullReference(storedProcedureName, "storedProcedureName");
			ArgumentValidation.CheckForEmptyString(storedProcedureName, "storedProcedureName");

			OracleCommandWrapper wrapper = new OracleCommandWrapper(storedProcedureName, CommandType.StoredProcedure, ParameterToken);
			PreparePackageSchema(wrapper);
			return wrapper;
		}

		/// <summary>
		/// <para>Create an <see cref="OracleCommandWrapper"/> for a stored procedure.</para>
		/// </summary>
		/// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
		/// <param name="parameterValues"><para>The list of parameters for the procedure.</para></param>
		/// <returns><para>The <see cref="OracleCommandWrapper"/> for the stored procedure.</para></returns>
		/// <remarks>
		/// <para>The parameters for the stored procedure will be discovered and the values are assigned in positional order.</para>
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="storedProcedureName"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// <para>- or -</para>
		/// <para><paramref name="parameterValues"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <para><paramref name="storedProcedureName"/> hast not been initialized.</para>
		/// </exception>
		internal override DBCommandWrapper GetStoredProcCommandWrapper(string storedProcedureName, params object[] parameterValues)
		{
			ArgumentValidation.CheckForNullReference(storedProcedureName, "storedProcedureName");
			ArgumentValidation.CheckForEmptyString(storedProcedureName, "storedProcedureName");
			ArgumentValidation.CheckForNullReference(parameterValues, "parameterValues");

			OracleCommandWrapper wrapper = new OracleCommandWrapper(storedProcedureName, CommandType.StoredProcedure, ParameterToken, parameterValues);
			PreparePackageSchema(wrapper);
			return wrapper;
		}

		/// <summary>
		/// <para>Create an <see cref="OracleCommandWrapper"/> for a SQL query.</para>
		/// </summary>
		/// <param name="query"><para>The text of the query.</para></param>        
		/// <returns><para>The <see cref="OracleCommandWrapper"/> for the SQL query.</para></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="query"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <para><paramref name="query"/> hast not been initialized.</para>
		/// </exception>
		internal override DBCommandWrapper GetSqlStringCommandWrapper(string query)
		{
			ArgumentValidation.CheckForNullReference(query, "query");
			ArgumentValidation.CheckForEmptyString(query, "query");

			OracleCommandWrapper wrapper = new OracleCommandWrapper(query, CommandType.Text, ParameterToken);
			return wrapper;
		}

		/// <summary>
		/// <para>Create a <see cref="OracleDataAdapter"/> with the given update behavior and connection.</para>
		/// </summary>
		/// <param name="updateBehavior">
		/// <para>One of the <see cref="UpdateBehavior"/> values.</para>
		/// </param>
		/// <param name="connection">
		/// <para>The open connection to the database.</para>
		/// </param>
		/// <returns>An <see cref="OracleDataAdapter"/>.</returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="connection"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		protected internal override DbDataAdapter GetDataAdapter(UpdateBehavior updateBehavior, IDbConnection connection)
		{
			ArgumentValidation.CheckForNullReference(connection, "connection");

			OracleDataAdapter adapter = new OracleDataAdapter(String.Empty, (OracleConnection)connection);
			if (updateBehavior == UpdateBehavior.Continue)
			{
				adapter.RowUpdated += new OracleRowUpdatedEventHandler(OnOracleRowUpdated);
			}
			return adapter;
		}

		/// <summary>
		/// Creates and <see cref="OracleDataReader"/> based on the <paramref name="commandWrapper"/>.
		/// </summary>
		/// <param name="commandWrapper">The command wrapper to execute.</param>        
		/// <returns>An <see cref="OracleDataReader"/> object.</returns>        
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="commandWrapper"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		internal override IDataReader ExecuteReader(DBCommandWrapper commandWrapper)
		{
			PrepareCWRefCursor(commandWrapper);
			return new OracleDataReaderWrapper((OracleDataReader)base.ExecuteReader(commandWrapper));
		}

		/// <summary>
		/// <para>Creates and <see cref="OracleDataReader"/> based on the <paramref name="commandWrapper"/>.</para>
		/// </summary>        
		/// <param name="commandWrapper"><para>The command wrapper to execute.</para></param>        
		/// <param name="transaction"><para>The transaction to participate in when executing this reader.</para></param>        
		/// <returns><para>An <see cref="OracleDataReader"/> object.</para></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="commandWrapper"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// <para>- or -</para>
		/// <para><paramref name="transaction"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		internal override IDataReader ExecuteReader(DBCommandWrapper commandWrapper, IDbTransaction transaction)
		{
			PrepareCWRefCursor(commandWrapper);
			return new OracleDataReaderWrapper((OracleDataReader)base.ExecuteReader(commandWrapper, transaction));
		}

		/// <summary>
		/// <para>Execute a command and return the results in a new <see cref="DataSet"/>.</para>
		/// </summary>
		/// <param name="commandWrapper"><para>The command to execute to fill the <see cref="DataSet"/></para></param>
		/// <returns><para>A <see cref="DataSet"/> filed with records and, if necessary, schema.</para></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="commandWrapper"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		internal override DataSet ExecuteDataSet(DBCommandWrapper commandWrapper)
		{
			PrepareCWRefCursor(commandWrapper);
			return base.ExecuteDataSet(commandWrapper);
		}

		/// <summary>
		/// <para>Execute a command and return the results in a new <see cref="DataSet"/>.</para>
		/// </summary>
		/// <param name="commandWrapper"><para>The command to execute to fill the <see cref="DataSet"/></para></param>
		/// <param name="transaction"><para>The transaction to participate in when executing this reader.</para></param>        
		/// <returns><para>A <see cref="DataSet"/> filed with records and, if necessary, schema.</para></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="commandWrapper"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="commandWrapper"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// <para>- or -</para>
		/// <para><paramref name="transaction"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
		/// </exception>
		internal override DataSet ExecuteDataSet(DBCommandWrapper commandWrapper, IDbTransaction transaction)
		{
			PrepareCWRefCursor(commandWrapper);
			return base.ExecuteDataSet(commandWrapper, transaction);
		}

		/// <summary>
		/// <para>Load a <see cref="DataSet"/> from a <see cref="DBCommandWrapper"/>.</para>
		/// </summary>
		/// <param name="commandWrapper">
		/// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
		/// </param>
		/// <param name="dataSet">
		/// <para>The <see cref="DataSet"/> to fill.</para>
		/// </param>
		/// <param name="tableNames">
		/// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
		/// </param>
		internal override void LoadDataSet(DBCommandWrapper commandWrapper, DataSet dataSet, string[] tableNames)
		{
			PrepareCWRefCursor(commandWrapper);
			base.LoadDataSet(commandWrapper, dataSet, tableNames);
		}

		/// <summary>
		/// <para>Load a <see cref="DataSet"/> from a <see cref="DBCommandWrapper"/> in  a transaction.</para>
		/// </summary>
		/// <param name="commandWrapper">
		/// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
		/// </param>
		/// <param name="dataSet">
		/// <para>The <see cref="DataSet"/> to fill.</para>
		/// </param>
		/// <param name="tableNames">
		/// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
		/// </param>
		/// <param name="transaction">
		/// <para>The <see cref="IDbTransaction"/> to execute the command in.</para>
		/// </param>
		internal override void LoadDataSet(DBCommandWrapper commandWrapper, DataSet dataSet, string[] tableNames, IDbTransaction transaction)
		{
			PrepareCWRefCursor(commandWrapper);
			base.LoadDataSet(commandWrapper, dataSet, tableNames, transaction);
		}

		/// <summary>
		/// 获取一个 <see cref="OracleLob"/> 类型的数据。
		/// </summary>
		/// <param name="db">数据库访问对象 <see cref="DbCore"/></param>
		/// <param name="buf">数据缓冲区。</param>
		/// <returns>一个 <see cref="OracleLob"/> 类型的数据。</returns>
		public static OracleLob GetBlob( DbCore db, byte[] buf )
		{
			try
			{
				bool HaveTransaction = db.isTransactionCreated;
				//要保证下面的 command 在事务下执行。
				if( !HaveTransaction )
				{
					db.BeginTransaction();
				}
				OracleCommandWrapper command = db.GetSqlStringCommandWrapper( "declare xx blob; begin dbms_lob.createtemporary(xx, false, 0); :tempblob := xx; end;" ) as OracleCommandWrapper;
				command.AddParameter( "tempblob", OracleType.Blob, 0, ParameterDirection.Output, null );
				db.ExecuteNonQuery( command );
				OracleLob tempLob;
				tempLob = command.GetParameterValue( "tempblob" ) as OracleLob;
				tempLob.BeginBatch(OracleLobOpenMode.ReadWrite);
				tempLob.Write(buf, 0, buf.Length);
				tempLob.EndBatch();
				if( !HaveTransaction )
				{
					db.CommitTransaction();
				}
				return tempLob;
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
		/// 获取一个<see cref="OracleLob"/>类型的临时变量，用于传入存储过程参数。
		/// </summary>
		/// <param name="tran">数据库连接的会话。</param>
		/// <param name="buf">数据缓冲区</param>
		/// <returns>一个<see cref="OracleLob"/>类型的临时变量。</returns>
		public OracleLob GetBlob( IDbTransaction tran, byte[] buf )
		{
			try
			{
				OracleCommandWrapper command = this.GetSqlStringCommandWrapper( "declare xx blob; begin dbms_lob.createtemporary(xx, false, 0); :tempblob := xx; end;" ) as OracleCommandWrapper;
				command.AddParameter( "tempblob", OracleType.Blob, 0, ParameterDirection.Output, null );
				this.ExecuteNonQuery( command, tran );
				OracleLob tempLob;
				tempLob = command.GetParameterValue( "tempblob" ) as OracleLob;
				tempLob.BeginBatch(OracleLobOpenMode.ReadWrite);
				tempLob.Write(buf, 0, buf.Length);
				tempLob.EndBatch();
				return tempLob;
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
		/// 获取一个<see cref="OracleLob"/>类型的临时变量，用于传入存储过程参数。
		/// </summary>
		/// <param name="conn">数据库连接。</param>
		/// <param name="buf">数据缓冲区</param>
		/// <returns>一个<see cref="OracleLob"/>类型的临时变量。</returns>
		public OracleLob GetBlob( IDbConnection conn, byte[] buf )
		{
			try
			{
				IDbTransaction tran = conn.BeginTransaction();
				OracleLob tempLob = GetBlob( tran, buf );
				tran.Commit();
				return tempLob;
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

		/// <devdoc>
		/// This is a private method that will build the Oracle package name if your stored procedure
		/// has proper prefix and postfix. 
		/// This functionality is include for
		/// the portability of the architecture between SQL and Oracle datbase.
		/// This method will also add the reference cursor to the command writer if not added already. This
		/// is required for Oracle .NET managed data provider.
		/// </devdoc>        
		private static void PrepareCWRefCursor(DBCommandWrapper commandWrapper)
		{
			ArgumentValidation.CheckForNullReference(commandWrapper, "commandWrapper");

			OracleCommandWrapper oracleCommandWrapper = (OracleCommandWrapper)commandWrapper;
			if (!(oracleCommandWrapper.ParameterDiscoveryRequired))
			{
				if (CommandType.StoredProcedure == commandWrapper.Command.CommandType)
				{
					// Check for ref. cursor in the command writer, if it does not exist, add a know reference cursor out
					// of "cur_OUT"
					if (!oracleCommandWrapper.IsRefCursorAdded)
					{
						oracleCommandWrapper.AddParameter(RefCursorName, OracleType.Cursor, 0, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull);
					}
				}
			}
		}

		/// <devdoc>
		/// Look into configuration and get the information how the command wrapper should be updated if calling a package on this
		/// connection.
		/// </devdoc>        
		private void PreparePackageSchema(OracleCommandWrapper commandWrapper)
		{
//			const string allPrefix = "*";
//			string packageName = String.Empty;
//			string prefix = String.Empty;
//			string commandText = commandWrapper.Command.CommandText;
//
//			if (CommandType.StoredProcedure == commandWrapper.Command.CommandType)
//			{
//				OracleConnectionStringData oraConnectionString = this.DatabaseProviderData.ConnectionStringData as OracleConnectionStringData;
//				if (oraConnectionString != null)
//				{
//					foreach (OraclePackageData oraPackage in oraConnectionString.OraclePackages)
//					{
//						if ((oraPackage.Prefix == allPrefix) || (commandText.StartsWith(oraPackage.prefix)))
//						{
//							//use the package name for the matching prefix
//							packageName = oraPackage.Name;
//							break;
//						}
//					}
//				}
//				if (0 != packageName.Length)
//				{
//					commandWrapper.Command.CommandText = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", packageName, commandText);
//				}
//			}
		}

		/// <devdoc>
		/// Listens for the RowUpdate event on a data adapter to support UpdateBehavior.Continue
		/// </devdoc>
		private void OnOracleRowUpdated(object sender, OracleRowUpdatedEventArgs args)
		{
			if (args.RecordsAffected == 0)
			{
				if (args.Errors != null)
				{
					args.Row.RowError = "Failed to update row";
					args.Status = UpdateStatus.SkipCurrentRow;
				}
			}
		}
	}
}

using System;
using System.Data;
using System.Data.Common;

using System.Xml;
using System.Collections;
using System.Reflection;


namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// 数据访问核心类
	/// </summary>
	
	public class DbCore:IDisposable
	{
		//private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// 用指定的 <paramref name="databaseKey"/> 初使化 <see cref="DbCore"/> 的实例。
		/// </summary>
		/// <param name="databaseKey">此数据库对象的键值。</param>
		public DbCore( string databaseKey )
		{
            
            //log4net.Config.XmlConfigurator.Configure();
			this.databaseKey = databaseKey;
            //this.resourceIndex = -1;
			transactionStack = new Stack();
			this.isTransactionCreated = false;
            DatabaseSettingData data = DataConfig.GetDatabaseSettingData( databaseKey );
			if( data == null )
			{
				throw new ArgumentException("指定键值[" + databaseKey + "]的数据库连接设置不存在。","databaseKey");
			}
			this.database = DatabaseFactory.CreateDatabase( data.ConnectionString,data.DatabaseType );
			this.database.EnabledLog = data.EnabledLog;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="data"></param>
		public DbCore( DatabaseSettingData data)
		{
			if( data == null )
			{
				throw new ArgumentException("指定键值[" + databaseKey + "]的数据库连接设置不存在。","databaseKey");
			}
			this.databaseKey = data.DatabaseKey;		
			transactionStack = new Stack();
			this.isTransactionCreated = false;			
			this.database = DatabaseFactory.CreateDatabase( data.ConnectionString,data.DatabaseType );
			this.database.EnabledLog = data.EnabledLog;
		}
		#region "私有成员变量"

		private string databaseKey;
        //private int    resourceIndex;
		internal bool isTransactionCreated;
		internal Database database;
      
        private Stack transactionStack;
		
       
		#endregion

		#region "公共属性"
        /// <summary>
        /// 数据库事务堆栈
        /// </summary>
        internal Stack TransactionStack
        {
            get
            {
                return this.transactionStack;
            }
        }


		#endregion

		#region "成员方法"

     
		#region 事务相关

        /// <summary>
        /// 把参与本地事务的事务压入事务堆栈中
        /// </summary>
        /// <param name="train">本地事务</param>
        internal void PopulateLocalTrainResource(IDbTransaction train)
        {
            this.transactionStack.Push(train);
        }

       
		/// <summary>
		/// 创建一个本地数据库事务，并把事务压人该数据连接资源的事务堆栈中
		/// </summary>
		public void BeginTransaction()
		{
			try
			{
				IDbConnection connection = database.OpenConnection();
				IDbTransaction train = connection.BeginTransaction();
				PopulateLocalTrainResource( train );
				this.isTransactionCreated = true;
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据传入的事务隔离级别创建事务，并把事务压人事务堆栈中
		/// </summary>
		/// <param name="isoLevel">事务隔离级别枚举</param>
		public void BeginTransaction( IsolationLevel isoLevel )
		{
			try
			{
				IDbConnection connection = database.OpenConnection();
				IDbTransaction train = connection.BeginTransaction( isoLevel );
				PopulateLocalTrainResource( train );
				this.isTransactionCreated = true;
                
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		

		private IDbTransaction GetCurrentTransaction( )
		{
            return TransactionStack.Peek() as IDbTransaction;

		}

		private IDbTransaction PopCurrentTransaction( )
		{
           
			return TransactionStack.Pop() as IDbTransaction;
         
		}

		/// <summary>
		/// 提交事务，POP出Transaction提交
		/// </summary>
		public void CommitTransaction( )
		{
			try
			{
                
				IDbTransaction tran = PopCurrentTransaction();
				if( tran != null )
				{
					tran.Commit();
				}               
				ReleaseDbResource();
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 回滚事务，POP出Transaction回滚
		/// </summary>
		public void RollbackTransaction()
		{
			try
			{
                IDbTransaction tran = PopCurrentTransaction();
				if( tran != null )
				{
						tran.Rollback();
				}
				ReleaseDbResource();
			}
			catch( Exception ex )
			{

				throw ex;
			}
		}

		#endregion

		#region 获得一个命令 DBCommandWrapper 的方法

		/// <summary>
		/// 为一个存储过程获得一个命令封装。
		/// </summary>
		/// <param name="storedProcedureName">存储过程名称。</param>
		/// <returns>为存储过程封装的 <see cref="DBCommandWrapper"/>。</returns>
		public DBCommandWrapper GetStoredProcCommandWrapper(string storedProcedureName)
		{
			try
			{
				return database.GetStoredProcCommandWrapper( storedProcedureName );
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 为一个存储过程获得一个命令封装。
		/// </summary>
		/// <param name="storedProcedureName">存储过程名称。</param>
		/// <param name="parameterValues">存储过程的参数值</param>
		/// <returns>为存储过程封装的 <see cref="DBCommandWrapper"/>。</returns>
		/// <remarks>
		/// <para>存储过程的参数将会从数据库反射得到，值会按提供的顺序对应到参数上，要求提供的值与参数数目相同。</para>
		/// </remarks>        
		public DBCommandWrapper GetStoredProcCommandWrapper(string storedProcedureName, params object[] parameterValues)
		{
			try
			{
				return database.GetStoredProcCommandWrapper( storedProcedureName, parameterValues );
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 为一个查询语句获得一个命令封装。
		/// </summary>
		/// <param name="query">查询语句。</param>
		/// <returns>为查询语句封装的 <see cref="DBCommandWrapper"/>。</returns>
		public DBCommandWrapper GetSqlStringCommandWrapper(string query)
		{
			try
			{
				return database.GetSqlStringCommandWrapper( query );
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		#endregion

		#region LoadDataSet方法

		/// <summary>
		/// 执行一个命令，并把结果填充到 <paramref name="dataSet" />
		/// </summary>
		/// <param name="command">待执行的命令。</param>
		/// <param name="dataSet">待填充的<see cref="DataSet"/></param>
		/// <param name="tableName"><paramref name="dataSet" />中的表名。</param>
		public void LoadDataSet(DBCommandWrapper command, DataSet dataSet, string tableName)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					database.LoadDataSet( command, dataSet, tableName, tran );
				}
				else
				{
					database.LoadDataSet( command, dataSet, tableName );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 执行一个命令，并把结果填充到 <paramref name="dataSet" />
		/// </summary>
		/// <param name="command">待执行的命令。</param>
		/// <param name="dataSet">待填充的<see cref="DataSet"/></param>
		/// <param name="tableNames"><paramref name="dataSet" />中的表名。</param>
		public void LoadDataSet(DBCommandWrapper command, DataSet dataSet, string[] tableNames)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					database.LoadDataSet( command, dataSet, tableNames, tran );
				}
				else
				{
					database.LoadDataSet( command, dataSet, tableNames );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据存储过程名称和值，执行一个存储过程，并把结果填充到 <paramref name="dataSet" />
		/// </summary>
		/// <param name="storedProcedureName">存储过程名称。</param>
		/// <param name="dataSet">待填充的<see cref="DataSet"/></param>
		/// <param name="tableNames"><paramref name="dataSet" />中的表名。</param>
		/// <param name="parameterValues">存储过程参数值。</param>
		public void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					database.LoadDataSet( tran, storedProcedureName, dataSet, tableNames, parameterValues );
				}
				else
				{
					database.LoadDataSet( storedProcedureName, dataSet, tableNames, parameterValues );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据命令类型和命令文本，执行一个查询，并把结果填充到 <paramref name="dataSet" />
		/// </summary>
		/// <param name="commandType">命令类型。</param>
		/// <param name="commandText">命令文本。</param>
		/// <param name="dataSet">待填充的<see cref="DataSet"/></param>
		/// <param name="tableNames"><paramref name="dataSet" />中的表名。</param>
		public void LoadDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					database.LoadDataSet( tran, commandType, commandText, dataSet, tableNames );
				}
				else
				{
					database.LoadDataSet( commandType, commandText, dataSet, tableNames );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		#endregion

		#region ExecuteDataSet方法

		/// <summary>
		/// 执行一个命令，并返回其结果集。
		/// </summary>
		/// <param name="command">待执行的命令。</param>
		/// <returns>命令的执行结果集，一个 <see cref="DataSet"/>。</returns>
		public DataSet ExecuteDataSet(DBCommandWrapper command)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					return database.ExecuteDataSet( command, tran );
				}
				else
				{
					return database.ExecuteDataSet( command );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据存储过程名称和值。执行一个存储过程，并返回一个数据集。
		/// </summary>
		/// <param name="storedProcedureName">存储过程名称。</param>
		/// <param name="parameterValues">存储过程的值。</param>
		/// <returns>存储过程的执行结果集，一个 <see cref="DataSet"/>。</returns>
		public DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					return database.ExecuteDataSet( tran, storedProcedureName, parameterValues );
				}
				else
				{
					return database.ExecuteDataSet( storedProcedureName, parameterValues );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据命令类型和命令文本执行一个查询，并返回一个结果集。
		/// </summary>
		/// <param name="commandType">命令类型。<see cref="CommandType"/>值之一。</param>
		/// <param name="commandText">命令文本。</param>
		/// <returns>命令的执行结果。一个<see cref="DataSet"/>。</returns>
		public DataSet ExecuteDataSet(CommandType commandType, string commandText)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					return database.ExecuteDataSet( tran, commandType, commandText );
				}
				else
				{
					return database.ExecuteDataSet( commandType, commandText );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		#endregion

		#region ExecuteScalar方法

		/// <summary>
		/// 执行指定的命令，并返回结果的第一行第一列的值。
		/// </summary>
		/// <param name="command">待执行的命令。</param>
		/// <returns>命令执行结果的第一行第一列的值。</returns>
		public object ExecuteScalar(DBCommandWrapper command)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					return database.ExecuteScalar( command, tran );
				}
				else
				{
					return database.ExecuteScalar( command );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据存储过程名称和值，执行一个存储过程并返回第一行第一列的值。
		/// </summary>
		/// <param name="storedProcedureName">存储过程名称。</param>
		/// <param name="parameterValues">存储过程的值。</param>
		/// <returns>存储过程的执行结果的第一行第一列的值。</returns>
		public object ExecuteScalar(string storedProcedureName, params Object[] parameterValues)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					return database.ExecuteScalar( tran, storedProcedureName, parameterValues );
				}
				else
				{
					return database.ExecuteScalar( storedProcedureName, parameterValues );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据命令类型和命令文本，执行一个命令并返回第一行第一列的值。
		/// </summary>
		/// <param name="commandType">命令类型。<see cref="CommandType"/> 值之一。</param>
		/// <param name="commandText">命令文本。</param>
		/// <returns>命令执行结果的第一行第一列的值。</returns>
		public object ExecuteScalar(CommandType commandType, string commandText)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					return database.ExecuteScalar( tran, commandType, commandText );
				}
				else
				{
					return database.ExecuteScalar( commandType, commandText );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		#endregion

		#region ExecuteNoQuery方法

		/// <summary>
		/// 执行一个指定的命令封装。
		/// </summary>
		/// <param name="command">一个命令封装。</param>
		public void ExecuteNonQuery(DBCommandWrapper command)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					database.ExecuteNonQuery( command, tran );
				}
				else
				{
					database.ExecuteNonQuery( command );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据存储过程名称和值执行对应的存储过程。
		/// </summary>
		/// <param name="storedProcedureName">存储过程名称。</param>
		/// <param name="parameterValues">存储过程值。</param>
		/// <returns>存储教程执行的返回值。</returns>
		public int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					return database.ExecuteNonQuery( tran, storedProcedureName, parameterValues );
				}
				else
				{
					return database.ExecuteNonQuery( storedProcedureName, parameterValues );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据命令类型和命令文本执行命令。
		/// </summary>
		/// <param name="commandType">命令类型。<see cref="CommandType"/> 值之一。</param>
		/// <param name="commandText">命令文本。类型为存储过程时，表示存储过程名称。</param>
		/// <returns>命令的执行结果。</returns>
		public int ExecuteNonQuery(CommandType commandType, string commandText)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					return database.ExecuteNonQuery( tran,commandType,commandText );
				}
				else
				{
					return database.ExecuteNonQuery( commandType, commandText );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		#endregion

		#region ExecuteReader方法

		/// <summary>
		/// 执行指定命令，并返回一个 <see cref="IDataReader"/>
		/// </summary>
		/// <param name="command">待执行的命令。</param>
		/// <returns>命令的执行结果。一个 <see cref="IDataReader"/></returns>
		public IDataReader ExecuteReader(DBCommandWrapper command)
		{
			try
			{
				IDataReader reader;
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					reader = database.ExecuteReader( command, tran );
				}
				else
				{
					reader = database.ExecuteReader( command );
				}
				return reader;
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据存储过程名称和值集合，执行存储过程，并返回一个 <see cref="IDataReader"/>
		/// </summary>
		/// <param name="storedProcedureName">存储过程名称。</param>
		/// <param name="parameterValues">存储过程的值。</param>
		/// <returns>存储过程的执行结果。一个 <see cref="IDataReader"/></returns>
		public IDataReader ExecuteReader(string storedProcedureName, params Object[] parameterValues)
		{
			try
			{
                
				IDataReader reader;
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					reader = database.ExecuteReader( tran, storedProcedureName, parameterValues );
				}
				else
				{
					reader = database.ExecuteReader( storedProcedureName, parameterValues );
				}

				return reader;
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 根据命令类型和命令文本执行命令。并返回一个 <see cref="IDataReader"/>
		/// </summary>
		/// <param name="commandType">命令类型。<see cref="CommandType"/> 值之一。</param>
		/// <param name="commandText">命令文本。</param>
		/// <returns>执行结果。一个<see cref="IDataReader"/></returns>
		public IDataReader ExecuteReader(CommandType commandType, string commandText)
		{
			try
			{
				IDataReader reader;
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					reader = database.ExecuteReader( tran, commandType, commandText );
				}
				else
				{
					reader = database.ExecuteReader( commandType, commandText );
				}
				return reader;
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		#endregion

		#region UpdateDataSet方法

		// TODO:未完成
		/// <summary>
		/// 用于更新一个<see cref="DataSet"/>中变动内容的方法。
		/// </summary>
		/// <param name="dataSet">待更新的<see cref="DataSet"/>。</param>
		/// <param name="tableName">表名。</param>
		/// <param name="insertCommand">Insert操作对应的命令。</param>
		/// <param name="updateCommand">Update操作对应的命令。</param>
		/// <param name="deleteCommand">Delete操作对应的命令。</param>
		/// <param name="updateBehavior">更新命令遇到失败时的操作。<see cref="UpdateBehavior"/>值之一。</param>
		/// <returns>更新成功的行数。</returns>
		public int UpdateDataSet(DataSet dataSet, string tableName,
			DBCommandWrapper insertCommand, DBCommandWrapper updateCommand,
			DBCommandWrapper deleteCommand, UpdateBehavior updateBehavior)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					return database.UpdateDataSet( dataSet, tableName, insertCommand, updateCommand, deleteCommand, tran );
				}
				else
				{
					return database.UpdateDataSet( dataSet, tableName, insertCommand, updateCommand, deleteCommand, updateBehavior );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		/// <summary>
		/// 用于更新一个<see cref="DataSet"/>中变动内容的方法。
		/// </summary>
		/// <param name="dataSet">待更新的<see cref="DataSet"/>。</param>
		/// <param name="tableName">表名。</param>
		/// <param name="insertCommand">Insert操作对应的命令。</param>
		/// <param name="updateCommand">Update操作对应的命令。</param>
		/// <param name="deleteCommand">Delete操作对应的命令。</param>
		/// <returns>更新成功的行数。</returns>
		public int UpdateDataSet(DataSet dataSet, string tableName,
			DBCommandWrapper insertCommand, DBCommandWrapper updateCommand,
			DBCommandWrapper deleteCommand)
		{
			try
			{
				if( this.isTransactionCreated )
				{
					IDbTransaction tran = GetCurrentTransaction();
					return database.UpdateDataSet( dataSet, tableName, insertCommand, updateCommand, deleteCommand, tran );
				}
				else
				{
					return database.UpdateDataSet( dataSet, tableName, insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard );
				}
			}
			catch( Exception ex )
			{
				throw ex;
			}
		}

		#endregion

		/// <summary>
		/// Info
		/// </summary>
		/// <param name="message"></param>
        //public void LogInfo(string message)
        //{
        //    logger.Info(message);
        //}

		/// <summary>
		/// Debug
		/// </summary>
		/// <param name="message"></param>
        //public void LogDebug(string message)
        //{
        //    logger.Debug(message);
        //}

		/// <summary>
		/// Error
		/// </summary>
		/// <param name="message"></param>
        //public void LogError(string message)
        //{
        //    logger.Error(message);
        //}

		/// <summary>
		/// Fatal
		/// </summary>
		/// <param name="message"></param>
        //public void LogFatal(string message)
        //{
        //    logger.Fatal(message);
        //}

		/// <summary>
		/// Warn
		/// </summary>
		/// <param name="message"></param>
        //public void LogWarn(string message)
        //{
        //    logger.Warn(message);
        //}

		/// <summary>
		/// 清除参数缓存中的所有缓存项。
		/// </summary>
        //public void ClearParameterCache()
        //{
        //    database.ClearParameterCache();
        //}

		#region 资源释放

        private void ReleaseDbResource()
        {
            this.isTransactionCreated = false;
        }

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
            this.isTransactionCreated = false;
			this.databaseKey = string.Empty;
			this.database = null;
		}

		/// <summary>
		/// 
		/// </summary>
		~ DbCore()
		{
			
		}

		#endregion

		#endregion
	}
}

using System;
using System.Data;
using System.Data.Common;

using System.Xml;
using System.Collections;
using System.Reflection;


namespace JOYFULL.CMPW.Data
{
	/// <summary>
	/// ���ݷ��ʺ�����
	/// </summary>
	
	public class DbCore:IDisposable
	{
		//private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		/// <summary>
		/// ��ָ���� <paramref name="databaseKey"/> ��ʹ�� <see cref="DbCore"/> ��ʵ����
		/// </summary>
		/// <param name="databaseKey">�����ݿ����ļ�ֵ��</param>
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
				throw new ArgumentException("ָ����ֵ[" + databaseKey + "]�����ݿ��������ò����ڡ�","databaseKey");
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
				throw new ArgumentException("ָ����ֵ[" + databaseKey + "]�����ݿ��������ò����ڡ�","databaseKey");
			}
			this.databaseKey = data.DatabaseKey;		
			transactionStack = new Stack();
			this.isTransactionCreated = false;			
			this.database = DatabaseFactory.CreateDatabase( data.ConnectionString,data.DatabaseType );
			this.database.EnabledLog = data.EnabledLog;
		}
		#region "˽�г�Ա����"

		private string databaseKey;
        //private int    resourceIndex;
		internal bool isTransactionCreated;
		internal Database database;
      
        private Stack transactionStack;
		
       
		#endregion

		#region "��������"
        /// <summary>
        /// ���ݿ������ջ
        /// </summary>
        internal Stack TransactionStack
        {
            get
            {
                return this.transactionStack;
            }
        }


		#endregion

		#region "��Ա����"

     
		#region �������

        /// <summary>
        /// �Ѳ��뱾�����������ѹ�������ջ��
        /// </summary>
        /// <param name="train">��������</param>
        internal void PopulateLocalTrainResource(IDbTransaction train)
        {
            this.transactionStack.Push(train);
        }

       
		/// <summary>
		/// ����һ���������ݿ����񣬲�������ѹ�˸�����������Դ�������ջ��
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
		/// ���ݴ����������뼶�𴴽����񣬲�������ѹ�������ջ��
		/// </summary>
		/// <param name="isoLevel">������뼶��ö��</param>
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
		/// �ύ����POP��Transaction�ύ
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
		/// �ع�����POP��Transaction�ع�
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

		#region ���һ������ DBCommandWrapper �ķ���

		/// <summary>
		/// Ϊһ���洢���̻��һ�������װ��
		/// </summary>
		/// <param name="storedProcedureName">�洢�������ơ�</param>
		/// <returns>Ϊ�洢���̷�װ�� <see cref="DBCommandWrapper"/>��</returns>
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
		/// Ϊһ���洢���̻��һ�������װ��
		/// </summary>
		/// <param name="storedProcedureName">�洢�������ơ�</param>
		/// <param name="parameterValues">�洢���̵Ĳ���ֵ</param>
		/// <returns>Ϊ�洢���̷�װ�� <see cref="DBCommandWrapper"/>��</returns>
		/// <remarks>
		/// <para>�洢���̵Ĳ�����������ݿⷴ��õ���ֵ�ᰴ�ṩ��˳���Ӧ�������ϣ�Ҫ���ṩ��ֵ�������Ŀ��ͬ��</para>
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
		/// Ϊһ����ѯ�����һ�������װ��
		/// </summary>
		/// <param name="query">��ѯ��䡣</param>
		/// <returns>Ϊ��ѯ����װ�� <see cref="DBCommandWrapper"/>��</returns>
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

		#region LoadDataSet����

		/// <summary>
		/// ִ��һ��������ѽ����䵽 <paramref name="dataSet" />
		/// </summary>
		/// <param name="command">��ִ�е����</param>
		/// <param name="dataSet">������<see cref="DataSet"/></param>
		/// <param name="tableName"><paramref name="dataSet" />�еı�����</param>
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
		/// ִ��һ��������ѽ����䵽 <paramref name="dataSet" />
		/// </summary>
		/// <param name="command">��ִ�е����</param>
		/// <param name="dataSet">������<see cref="DataSet"/></param>
		/// <param name="tableNames"><paramref name="dataSet" />�еı�����</param>
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
		/// ���ݴ洢�������ƺ�ֵ��ִ��һ���洢���̣����ѽ����䵽 <paramref name="dataSet" />
		/// </summary>
		/// <param name="storedProcedureName">�洢�������ơ�</param>
		/// <param name="dataSet">������<see cref="DataSet"/></param>
		/// <param name="tableNames"><paramref name="dataSet" />�еı�����</param>
		/// <param name="parameterValues">�洢���̲���ֵ��</param>
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
		/// �����������ͺ������ı���ִ��һ����ѯ�����ѽ����䵽 <paramref name="dataSet" />
		/// </summary>
		/// <param name="commandType">�������͡�</param>
		/// <param name="commandText">�����ı���</param>
		/// <param name="dataSet">������<see cref="DataSet"/></param>
		/// <param name="tableNames"><paramref name="dataSet" />�еı�����</param>
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

		#region ExecuteDataSet����

		/// <summary>
		/// ִ��һ�������������������
		/// </summary>
		/// <param name="command">��ִ�е����</param>
		/// <returns>�����ִ�н������һ�� <see cref="DataSet"/>��</returns>
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
		/// ���ݴ洢�������ƺ�ֵ��ִ��һ���洢���̣�������һ�����ݼ���
		/// </summary>
		/// <param name="storedProcedureName">�洢�������ơ�</param>
		/// <param name="parameterValues">�洢���̵�ֵ��</param>
		/// <returns>�洢���̵�ִ�н������һ�� <see cref="DataSet"/>��</returns>
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
		/// �����������ͺ������ı�ִ��һ����ѯ��������һ���������
		/// </summary>
		/// <param name="commandType">�������͡�<see cref="CommandType"/>ֵ֮һ��</param>
		/// <param name="commandText">�����ı���</param>
		/// <returns>�����ִ�н����һ��<see cref="DataSet"/>��</returns>
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

		#region ExecuteScalar����

		/// <summary>
		/// ִ��ָ������������ؽ���ĵ�һ�е�һ�е�ֵ��
		/// </summary>
		/// <param name="command">��ִ�е����</param>
		/// <returns>����ִ�н���ĵ�һ�е�һ�е�ֵ��</returns>
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
		/// ���ݴ洢�������ƺ�ֵ��ִ��һ���洢���̲����ص�һ�е�һ�е�ֵ��
		/// </summary>
		/// <param name="storedProcedureName">�洢�������ơ�</param>
		/// <param name="parameterValues">�洢���̵�ֵ��</param>
		/// <returns>�洢���̵�ִ�н���ĵ�һ�е�һ�е�ֵ��</returns>
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
		/// �����������ͺ������ı���ִ��һ��������ص�һ�е�һ�е�ֵ��
		/// </summary>
		/// <param name="commandType">�������͡�<see cref="CommandType"/> ֵ֮һ��</param>
		/// <param name="commandText">�����ı���</param>
		/// <returns>����ִ�н���ĵ�һ�е�һ�е�ֵ��</returns>
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

		#region ExecuteNoQuery����

		/// <summary>
		/// ִ��һ��ָ���������װ��
		/// </summary>
		/// <param name="command">һ�������װ��</param>
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
		/// ���ݴ洢�������ƺ�ִֵ�ж�Ӧ�Ĵ洢���̡�
		/// </summary>
		/// <param name="storedProcedureName">�洢�������ơ�</param>
		/// <param name="parameterValues">�洢����ֵ��</param>
		/// <returns>�洢�̳�ִ�еķ���ֵ��</returns>
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
		/// �����������ͺ������ı�ִ�����
		/// </summary>
		/// <param name="commandType">�������͡�<see cref="CommandType"/> ֵ֮һ��</param>
		/// <param name="commandText">�����ı�������Ϊ�洢����ʱ����ʾ�洢�������ơ�</param>
		/// <returns>�����ִ�н����</returns>
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

		#region ExecuteReader����

		/// <summary>
		/// ִ��ָ�����������һ�� <see cref="IDataReader"/>
		/// </summary>
		/// <param name="command">��ִ�е����</param>
		/// <returns>�����ִ�н����һ�� <see cref="IDataReader"/></returns>
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
		/// ���ݴ洢�������ƺ�ֵ���ϣ�ִ�д洢���̣�������һ�� <see cref="IDataReader"/>
		/// </summary>
		/// <param name="storedProcedureName">�洢�������ơ�</param>
		/// <param name="parameterValues">�洢���̵�ֵ��</param>
		/// <returns>�洢���̵�ִ�н����һ�� <see cref="IDataReader"/></returns>
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
		/// �����������ͺ������ı�ִ�����������һ�� <see cref="IDataReader"/>
		/// </summary>
		/// <param name="commandType">�������͡�<see cref="CommandType"/> ֵ֮һ��</param>
		/// <param name="commandText">�����ı���</param>
		/// <returns>ִ�н����һ��<see cref="IDataReader"/></returns>
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

		#region UpdateDataSet����

		// TODO:δ���
		/// <summary>
		/// ���ڸ���һ��<see cref="DataSet"/>�б䶯���ݵķ�����
		/// </summary>
		/// <param name="dataSet">�����µ�<see cref="DataSet"/>��</param>
		/// <param name="tableName">������</param>
		/// <param name="insertCommand">Insert������Ӧ�����</param>
		/// <param name="updateCommand">Update������Ӧ�����</param>
		/// <param name="deleteCommand">Delete������Ӧ�����</param>
		/// <param name="updateBehavior">������������ʧ��ʱ�Ĳ�����<see cref="UpdateBehavior"/>ֵ֮һ��</param>
		/// <returns>���³ɹ���������</returns>
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
		/// ���ڸ���һ��<see cref="DataSet"/>�б䶯���ݵķ�����
		/// </summary>
		/// <param name="dataSet">�����µ�<see cref="DataSet"/>��</param>
		/// <param name="tableName">������</param>
		/// <param name="insertCommand">Insert������Ӧ�����</param>
		/// <param name="updateCommand">Update������Ӧ�����</param>
		/// <param name="deleteCommand">Delete������Ӧ�����</param>
		/// <returns>���³ɹ���������</returns>
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
		/// ������������е����л����
		/// </summary>
        //public void ClearParameterCache()
        //{
        //    database.ClearParameterCache();
        //}

		#region ��Դ�ͷ�

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

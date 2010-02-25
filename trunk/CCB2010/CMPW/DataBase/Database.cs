/******************************************
 * 模块名称：Database 类
 * 当前版本：1.0
 * 开发人员：
 * 完成时间：
 * 版本历史：
  ******************************************/

using System;
using System.Data;
using System.Data.Common;
using System.Globalization;
//using log4net;
using System.Reflection;

namespace JOYFULL.CMPW.Data
{
    /// <summary>
    /// 作为各种类型的数据库对象的基类。
    /// </summary>
    public abstract class Database
    {
        private static ParameterCache parameterCache = new ParameterCache();
        private string connectionString = string.Empty;
        private bool enabledLog;
        //private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 默认构造函数。
        /// </summary>
        protected internal Database()
        {
        }

        /// <summary>
        /// 获取或设置一个值，用于表示是否记录数据访问日志。
        /// </summary>
        internal bool EnabledLog
        {
            get
            {
                return this.enabledLog;
            }
            set
            {
                this.enabledLog = value;
            }
        }
        /// <summary>
        /// <para>获取连接使用的连接字符串。</para>
        /// <seealso cref="IDbConnection.ConnectionString"/>
        /// </summary>
        /// <value>
        /// <para>用于打开连接的字符串。</para>
        /// </value>
        protected internal string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
        }

        /// <summary>
        /// 设置连接使用的连接字符串。
        /// </summary>
        /// <param name="connectionString">连接字符串。</param>
        protected internal void SetConnectionString(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 在继承类中实现时，获取表示参数的前导符。
        /// </summary>
        /// <value>
        /// <para>用于此数据库对象的表示参数的前导符。</para>
        /// </value>
        protected internal abstract char ParameterToken { get; }

        /// <summary>
        /// <para>获取用于创建此数据库对象的键值。</para>
        /// </summary>
        /// <value>
        /// <para>用于创建此数据库对象的键值。</para>
        /// </value>
        protected internal string ServiceKey
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// <para>在继承类中实现时，获得一个数据库连接。</para>
        /// <seealso cref="IDbConnection"/>        
        /// </summary>
        /// <returns>
        /// <para>用于此数据库对象的 <see cref="IDbConnection"/> 。</para>
        /// </returns>
        internal abstract IDbConnection GetConnection();

        /// <summary>
        /// 在继承类中实现时，为一个存储过程获得一个命令封装。
        /// </summary>
        /// <param name="storedProcedureName">存储过程名称。</param>
        /// <returns>为存储过程封装的 <see cref="DBCommandWrapper"/>。</returns>
        internal abstract DBCommandWrapper GetStoredProcCommandWrapper(string storedProcedureName);

        /// <summary>
        /// 在继承类中实现时，为一个存储过程获得一个命令封装。
        /// </summary>
        /// <param name="storedProcedureName">存储过程名称。</param>
        /// <param name="parameterValues">存储过程的参数值</param>
        /// <returns>为存储过程封装的 <see cref="DBCommandWrapper"/>。</returns>
        /// <remarks>
        /// <para>存储过程的参数将会从数据库反射得到，值会按提供的顺序对应到参数上，要求提供的值与参数数目相同。</para>
        /// </remarks>        
        internal abstract DBCommandWrapper GetStoredProcCommandWrapper(string storedProcedureName, params object[] parameterValues);

        /// <summary>
        /// 在继承类中实现时，为一个查询语句获得一个命令封装。
        /// </summary>
        /// <param name="query">查询语句。</param>
        /// <returns>为查询语句封装的 <see cref="DBCommandWrapper"/>。</returns>
        internal abstract DBCommandWrapper GetSqlStringCommandWrapper(string query);

        /// <summary>
        /// <para>When overridden in a derived class, creates a <see cref="DbDataAdapter"/> with the given update behavior and connection.</para>        
        /// </summary>
        /// <param name="behavior">
        /// <para>One of the <see cref="UpdateBehavior"/> values.</para>
        /// </param>
        /// <param name="connection">
        /// <para>The open connection to the database.</para>
        /// </param>
        /// <returns>An <see cref="DbDataAdapter"/>.</returns>
        /// <seealso cref="DbDataAdapter"/>
        protected internal abstract DbDataAdapter GetDataAdapter(UpdateBehavior behavior, IDbConnection connection);

        /// <summary>
        /// <para>Execute the <paramref name="command"/> and add a new <see cref="DataTable"></see> to the existing <see cref="DataSet"></see></para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="DBCommandWrapper"/> to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to load.</para>
        /// </param>
        /// <param name="tableName">
        /// <para>The name for the new <see cref="DataTable"/> to add to the <see cref="DataSet"/>.</para>
        /// </param>
        /// <seealso cref="DbDataAdapter.Fill"/>
        /// <exception cref="System.ArgumentNullException">Any input parameter was null</exception>
        /// <exception cref="System.ArgumentException">tableName was an empty string</exception>
        internal virtual void LoadDataSet(DBCommandWrapper command, DataSet dataSet, string tableName)
        {
            LoadDataSet(command, dataSet, new string[] { tableName });
        }

        /// <summary>
        /// <para>Execute the <paramref name="command"/> within the given <paramref name="transaction" /> and add a new <see cref="DataTable"></see> to the existing <see cref="DataSet"></see></para>
        /// </summary>
        /// <param name="command">
        /// <para>The <see cref="DBCommandWrapper"/> to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to load.</para>
        /// </param>
        /// <param name="tableName">
        /// <para>The name for the new <see cref="DataTable"/> to add to the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <seealso cref="DbDataAdapter.Fill"/>
        /// <exception cref="System.ArgumentNullException">Any input parameter was null</exception>
        /// <exception cref="System.ArgumentException">tableName was an empty string</exception>
        internal virtual void LoadDataSet(DBCommandWrapper command, DataSet dataSet, string tableName, IDbTransaction transaction)
        {
            LoadDataSet(command, dataSet, new string[] { tableName }, transaction);
        }

        /// <summary>
        /// <para>Load a <see cref="DataSet"/> from a <see cref="DBCommandWrapper"/>.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        internal virtual void LoadDataSet(DBCommandWrapper command, DataSet dataSet, string[] tableNames)
        {
            using (IDbConnection connection = GetConnection())
            {
                PrepareCommand(command, connection);
                DoLoadDataSet(command, dataSet, tableNames);
            }
        }

        /// <summary>
        /// <para>Load a <see cref="DataSet"/> from a <see cref="DBCommandWrapper"/> in  a transaction.</para>
        /// </summary>
        /// <param name="command">
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
        internal virtual void LoadDataSet(DBCommandWrapper command, DataSet dataSet, string[] tableNames, IDbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            DoLoadDataSet(command, dataSet, tableNames);
        }

        /// <summary>
        /// <para>Load a <see cref="DataSet"/> from a <see cref="DBCommandWrapper"/></para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure name to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        internal virtual void LoadDataSet(string storedProcedureName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            using (DBCommandWrapper wrapper = GetStoredProcCommandWrapper(storedProcedureName, parameterValues))
            {
                LoadDataSet(wrapper, dataSet, tableNames);
            }
        }

        /// <summary>
        /// <para>Load a <see cref="DataSet"/> from a stored procedure in  a transaction.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the stored procedure in.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure name to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        internal virtual void LoadDataSet(IDbTransaction transaction, string storedProcedureName, DataSet dataSet, string[] tableNames, object[] parameterValues)
        {
            using (DBCommandWrapper wrapper = GetStoredProcCommandWrapper(storedProcedureName, parameterValues))
            {
                LoadDataSet(wrapper, dataSet, tableNames, transaction);
            }
        }

        /// <summary>
        /// <para>Load a <see cref="DataSet"/> from command text.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        internal virtual void LoadDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            using (DBCommandWrapper wrapper = CreateCommandWrapperByCommandType(commandType, commandText))
            {
                LoadDataSet(wrapper, dataSet, tableNames);
            }
        }

        /// <summary>
        /// <para>Load a <see cref="DataSet"/> from command text in a transaction.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command in.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        internal virtual void LoadDataSet(IDbTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            using (DBCommandWrapper wrapper = CreateCommandWrapperByCommandType(commandType, commandText))
            {
                LoadDataSet(wrapper, dataSet, tableNames, transaction);
            }
        }

        /// <summary>
        /// <para>Execute the <paramref name="command"/> and return the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The <see cref="DBCommandWrapper"/> to execute.</para></param>
        /// <returns>A <see cref="DataSet"/> with the results of the <paramref name="command"/>.</returns>
        /// <seealso cref="DbDataAdapter.Fill"/>
        internal virtual DataSet ExecuteDataSet(DBCommandWrapper command)
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = CultureInfo.InvariantCulture;
            LoadDataSet(command, dataSet, "Table");
            return dataSet;
        }

        /// <summary>
        /// <para>Execute the <paramref name="command"/> as part of the <paramref name="transaction" /> and return the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="command"><para>The <see cref="DBCommandWrapper"/> to execute.</para></param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>A <see cref="DataSet"/> with the results of the <paramref name="command"/>.</returns>
        /// <seealso cref="DbDataAdapter.Fill"/>
        internal virtual DataSet ExecuteDataSet(DBCommandWrapper command, IDbTransaction transaction)
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = CultureInfo.InvariantCulture;
            LoadDataSet(command, dataSet, "Table", transaction);
            return dataSet;
        }

        /// <summary>
        /// <para>Execute the <paramref name="storedProcedureName"/> with <paramref name="parameterValues" /> and return the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="DataSet"/> with the results of the <paramref name="storedProcedureName"/>.</para>
        /// </returns>
        internal virtual DataSet ExecuteDataSet(string storedProcedureName, params object[] parameterValues)
        {
            using (DBCommandWrapper wrapper = GetStoredProcCommandWrapper(storedProcedureName, parameterValues))
            {
                return ExecuteDataSet(wrapper);
            }
        }

        /// <summary>
        /// <para>Execute the <paramref name="storedProcedureName"/> ith <paramref name="parameterValues" /> as part of the <paramref name="transaction" /> and return the results in a new <see cref="DataSet"/> within a transaction.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="DataSet"/> with the results of the <paramref name="storedProcedureName"/>.</para>
        /// </returns>
        internal virtual DataSet ExecuteDataSet(IDbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            using (DBCommandWrapper wrapper = GetStoredProcCommandWrapper(storedProcedureName, parameterValues))
            {
                return ExecuteDataSet(wrapper, transaction);
            }
        }

        /// <summary>
        /// <para>Execute the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> and return the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="DataSet"/> with the results of the <paramref name="commandText"/>.</para>
        /// </returns>
        internal virtual DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            using (DBCommandWrapper wrapper = CreateCommandWrapperByCommandType(commandType, commandText))
            {
                return ExecuteDataSet(wrapper);
            }
        }

        /// <summary>
        /// <para>Execute the <paramref name="commandText"/> as part of the given <paramref name="transaction" /> and return the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="DataSet"/> with the results of the <paramref name="commandText"/>.</para>
        /// </returns>
        internal virtual DataSet ExecuteDataSet(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DBCommandWrapper wrapper = CreateCommandWrapperByCommandType(commandType, commandText))
            {
                return ExecuteDataSet(wrapper, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the resultset.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual object ExecuteScalar(DBCommandWrapper command)
        {
            using (IDbConnection connection = OpenConnection())
            {
                PrepareCommand(command, connection);
                return DoExecuteScalar(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> within a <paramref name="transaction" />, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the resultset.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual object ExecuteScalar(DBCommandWrapper command, IDbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteScalar(command);
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> with the given <paramref name="parameterValues" /> and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the resultset.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual object ExecuteScalar(string storedProcedureName, params Object[] parameterValues)
        {
            using (DBCommandWrapper wrapper = GetStoredProcCommandWrapper(storedProcedureName, parameterValues))
            {
                return ExecuteScalar(wrapper);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> with the given <paramref name="parameterValues" /> within a 
        /// <paramref name="transaction" /> and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The stored procedure to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the resultset.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual object ExecuteScalar(IDbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            using (DBCommandWrapper wrapper = GetStoredProcCommandWrapper(storedProcedureName, parameterValues))
            {
                return ExecuteScalar(wrapper, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" />  and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the resultset.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual object ExecuteScalar(CommandType commandType, string commandText)
        {
            using (DBCommandWrapper wrapper = CreateCommandWrapperByCommandType(commandType, commandText))
            {
                return ExecuteScalar(wrapper);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> 
        /// within the given <paramref name="transaction" /> and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The first column of the first row in the resultset.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DBCommandWrapper wrapper = CreateCommandWrapperByCommandType(commandType, commandText))
            {
                return ExecuteScalar(wrapper, transaction);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>       
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual void ExecuteNonQuery(DBCommandWrapper command)
        {
            using (IDbConnection connection = OpenConnection())
            {
                PrepareCommand(command, connection);
                DoExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> within the given <paramref name="transaction" />, and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual void ExecuteNonQuery(DBCommandWrapper command, IDbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            DoExecuteNonQuery(command);
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> using the given <paramref name="parameterValues" /> and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="storedProcedureName">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>The number of rows affected</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual int ExecuteNonQuery(string storedProcedureName, params object[] parameterValues)
        {
            using (DBCommandWrapper wrapper = GetStoredProcCommandWrapper(storedProcedureName, parameterValues))
            {
                ExecuteNonQuery(wrapper);
                return wrapper.RowsAffected;
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> using the given <paramref name="parameterValues" /> within a transaction and returns the number of rows affected.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>The number of rows affected</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual int ExecuteNonQuery(IDbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            using (DBCommandWrapper wrapper = GetStoredProcCommandWrapper(storedProcedureName, parameterValues))
            {
                ExecuteNonQuery(wrapper, transaction);

                return wrapper.RowsAffected;
            }
        }

        /// <summary>
        /// <para>Execute the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> and return the number of rows affected.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The number of rows affected</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            using (DBCommandWrapper wrapper = CreateCommandWrapperByCommandType(commandType, commandText))
            {
                ExecuteNonQuery(wrapper);

                return wrapper.RowsAffected;
            }
        }

        /// <summary>
        /// <para>Execute the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> as part of the given <paramref name="transaction" /> and return the number of rows affected.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>The number of rows affected</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteScalar"/>
        internal virtual int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DBCommandWrapper wrapper = CreateCommandWrapperByCommandType(commandType, commandText))
            {
                ExecuteNonQuery(wrapper, transaction);

                return wrapper.RowsAffected;
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteReader"/>
        internal virtual IDataReader ExecuteReader(DBCommandWrapper command)
        {
            IDbConnection connection = OpenConnection();
            PrepareCommand(command, connection);

            try
            {
                return DoExecuteReader(command.Command, CommandBehavior.CloseConnection);
            }
            catch
            {
                connection.Close();
                throw;
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="command"/> within a transaction and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="command">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteReader"/>
        internal virtual IDataReader ExecuteReader(DBCommandWrapper command, IDbTransaction transaction)
        {
            PrepareCommand(command, transaction);
            return DoExecuteReader(command.Command, CommandBehavior.Default);
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> with the given <paramref name="parameterValues" /> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>        
        /// <param name="storedProcedureName">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteReader"/>
        internal IDataReader ExecuteReader(string storedProcedureName, params object[] parameterValues)
        {
            using (DBCommandWrapper wrapper = GetStoredProcCommandWrapper(storedProcedureName, parameterValues))
            {
                return ExecuteReader(wrapper);
            }
        }

        /// <summary>
        /// <para>Executes the <paramref name="storedProcedureName"/> with the given <paramref name="parameterValues" /> within the given <paramref name="transaction" /> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="storedProcedureName">
        /// <para>The command that contains the query to execute.</para>
        /// </param>
        /// <param name="parameterValues">
        /// <para>An array of paramters to pass to the stored procedure. The parameter values must be in call order as they appear in the stored procedure.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteReader"/>
        internal IDataReader ExecuteReader(IDbTransaction transaction, string storedProcedureName, params object[] parameterValues)
        {
            using (DBCommandWrapper wrapper = GetStoredProcCommandWrapper(storedProcedureName, parameterValues))
            {
                return ExecuteReader(wrapper, transaction);
            }
        }

        /// <summary>
        /// <para>Execute the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteReader"/>
        internal IDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            using (DBCommandWrapper wrapper = CreateCommandWrapperByCommandType(commandType, commandText))
            {
                return ExecuteReader(wrapper);
            }
        }

        /// <summary>
        /// <para>Execute the <paramref name="commandText"/> interpreted as specified by the <paramref name="commandType" /> within the given 
        /// <paramref name="transaction" /> and returns an <see cref="IDataReader"></see> through which the result can be read.
        /// It is the responsibility of the caller to close the connection and reader when finished.</para>
        /// </summary>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        /// </param>
        /// <param name="commandType">
        /// <para>One of the <see cref="CommandType"/> values.</para>
        /// </param>
        /// <param name="commandText">
        /// <para>The command text to execute.</para>
        /// </param>
        /// <returns>
        /// <para>An <see cref="IDataReader"/> object.</para>
        /// </returns>
        /// <seealso cref="IDbCommand.ExecuteReader"/>
        internal IDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            using (DBCommandWrapper wrapper = CreateCommandWrapperByCommandType(commandType, commandText))
            {
                return ExecuteReader(wrapper, transaction);
            }
        }

        /// <summary>
        /// <para>Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the <see cref="DataSet"/>.</para>
        /// </summary>        
        /// <param name="dataSet"><para>The <see cref="DataSet"/> used to update the data source.</para></param>
        /// <param name="tableName"><para>The name of the source table to use for table mapping.</para></param>
        /// <param name="insertCommand"><para>The <see cref="DBCommandWrapper"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Added"/></para></param>
        /// <param name="updateCommand"><para>The <see cref="DBCommandWrapper"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Modified"/></para></param>        
        /// <param name="deleteCommand"><para>The <see cref="DBCommandWrapper"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Deleted"/></para></param>        
        /// <param name="updateBehavior"><para>One of the <see cref="UpdateBehavior"/> values.</para></param>
        /// <returns>number of records affected</returns>
        /// <seealso cref="DbDataAdapter.Update"/>
        internal virtual int UpdateDataSet(DataSet dataSet, string tableName,
            DBCommandWrapper insertCommand, DBCommandWrapper updateCommand,
            DBCommandWrapper deleteCommand, UpdateBehavior updateBehavior)
        {
            using (IDbConnection connection = OpenConnection())
            {
                if (updateBehavior == UpdateBehavior.Transactional)
                {
                    IDbTransaction trans = BeginTransaction(connection);
                    try
                    {
                        int rowsAffected = UpdateDataSet(dataSet, tableName, insertCommand, updateCommand, deleteCommand, trans);
                        CommitTransaction(trans);
                        return rowsAffected;
                    }
                    catch
                    {
                        RollbackTransaction(trans);
                        throw;
                    }
                }
                else
                {
                    if (insertCommand != null)
                    {
                        PrepareCommand(insertCommand, connection);
                    }
                    if (updateCommand != null)
                    {
                        PrepareCommand(updateCommand, connection);
                    }
                    if (deleteCommand != null)
                    {
                        PrepareCommand(deleteCommand, connection);
                    }

                    return DoUpdateDataSet(updateBehavior, connection, dataSet, tableName,
                        insertCommand, updateCommand, deleteCommand);
                }
            }
        }

        /// <summary>
        /// <para>Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the <see cref="DataSet"/> within a transaction.</para>
        /// </summary>        
        /// <param name="dataSet"><para>The <see cref="DataSet"/> used to update the data source.</para></param>
        /// <param name="tableName"><para>The name of the source table to use for table mapping.</para></param>
        /// <param name="insertCommand"><para>The <see cref="DBCommandWrapper"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Added"/></para></param>
        /// <param name="updateCommand"><para>The <see cref="DBCommandWrapper"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Modified"/></para></param>        
        /// <param name="deleteCommand"><para>The <see cref="DBCommandWrapper"/> executed when <see cref="DataRowState"/> is <seealso cref="DataRowState.Deleted"/></para></param>        
        /// <param name="transaction"><para>The <see cref="IDbTransaction"/> to use.</para></param>
        /// <returns>number of records affected</returns>
        /// <seealso cref="DbDataAdapter.Update"/>
        internal virtual int UpdateDataSet(DataSet dataSet, string tableName,
            DBCommandWrapper insertCommand, DBCommandWrapper updateCommand,
            DBCommandWrapper deleteCommand, IDbTransaction transaction)
        {
            if (insertCommand != null)
            {
                PrepareCommand(insertCommand, transaction);
            }
            if (updateCommand != null)
            {
                PrepareCommand(updateCommand, transaction);
            }
            if (deleteCommand != null)
            {
                PrepareCommand(deleteCommand, transaction);
            }

            return DoUpdateDataSet(UpdateBehavior.Transactional, transaction.Connection,
                dataSet, tableName, insertCommand, updateCommand, deleteCommand);
        }

        /// <summary>
        /// Clears the parameter cache. Since there is only one parameter cache that is shared by all instances
        /// of this class, this clears all parameters cached for all databases.
        /// </summary>
        internal void ClearParameterCache()
        {
            Database.parameterCache.Clear();
        }

        /// <summary>
        /// <para>Assigns a <paramref name="connection"/> to the <paramref name="command"/> and discovers parameters if needed.</para>
        /// </summary>
        /// <param name="command"><para>The command that contains the query to prepare.</para></param>
        /// <param name="connection">The connection to assign to the command.</param>
        protected internal void PrepareCommand(DBCommandWrapper command, IDbConnection connection)
        {
            ArgumentValidation.CheckForNullReference(command, "command");
            ArgumentValidation.CheckForNullReference(connection, "connection");

            command.Command.Connection = connection;
            if (command.IsFurtherPreparationNeeded())
            {
                parameterCache.FillParameters(command, ParameterToken);
            }
        }

        /// <summary>
        /// <para>Assigns a <paramref name="transaction"/> to the <paramref name="command"/> and discovers parameters if needed.</para>
        /// </summary>
        /// <param name="command"><para>The command that contains the query to prepare.</para></param>
        /// <param name="transaction">The transaction to assign to the command.</param>
        protected internal void PrepareCommand(DBCommandWrapper command, IDbTransaction transaction)
        {
            ArgumentValidation.CheckForNullReference(command, "command");
            ArgumentValidation.CheckForNullReference(transaction, "transaction");

            command.Command.Transaction = transaction;
            PrepareCommand(command, transaction.Connection);
        }

        /// <summary>
        /// 创建并返回一个打开的连接。
        /// </summary>
        /// <returns>一个打开的连接 <see cref="IDbConnection"/>。</returns>
        internal IDbConnection OpenConnection()
        {
            IDbConnection connection = GetConnection();
            try
            {
                connection.Open();
                return connection;
            }
            catch
            {
                throw;
            }
        }

        private DBCommandWrapper CreateCommandWrapperByCommandType(CommandType commandType, string commandText)
        {
            DBCommandWrapper wrapper = null;
            switch (commandType)
            {
                case CommandType.StoredProcedure:
                    wrapper = GetStoredProcCommandWrapper(commandText);
                    break;

                case CommandType.Text:
                    wrapper = GetSqlStringCommandWrapper(commandText);
                    break;
            }

            if (wrapper == null)
            {
                throw new ArgumentException("不合法的参数值", "commandType");
            }
            return wrapper;
        }

        private int DoUpdateDataSet(UpdateBehavior behavior, IDbConnection connection,
            DataSet dataSet, string tableName, DBCommandWrapper insertCommand,
            DBCommandWrapper updateCommand, DBCommandWrapper deleteCommand)
        {
            ArgumentValidation.CheckForNullReference(dataSet, "dataSet");
            ArgumentValidation.CheckForNullReference(tableName, "tableName");
            ArgumentValidation.CheckForEmptyString(tableName, "tableName");

            if (insertCommand == null && updateCommand == null && deleteCommand == null)
            {
                throw new ArgumentException("At least one command must be initialized");
            }

            using (DbDataAdapter adapter = GetDataAdapter(behavior, connection))
            {
                IDbDataAdapter explicitAdapter = (IDbDataAdapter)adapter;
                if (insertCommand != null)
                {
                    explicitAdapter.InsertCommand = insertCommand.Command;
                }
                if (updateCommand != null)
                {
                    explicitAdapter.UpdateCommand = updateCommand.Command;
                }
                if (deleteCommand != null)
                {
                    explicitAdapter.DeleteCommand = deleteCommand.Command;
                }

                try
                {
                    DateTime startTime = DateTime.Now;
                    int rows = adapter.Update(dataSet.Tables[tableName]);
                    return rows;
                }
                catch
                {
                    throw;
                }
            }
        }

        private void DoLoadDataSet(DBCommandWrapper command, DataSet dataSet, string[] tableNames)
        {
            ArgumentValidation.CheckForNullReference(tableNames, "tableNames");

            if (tableNames.Length == 0)
            {
                throw new ArgumentException("The table name array used to map results to user-specified table names cannot be empty.", "tableNames");
            }

            for (int i = 0; i < tableNames.Length; i++)
            {
                ArgumentValidation.CheckForNullReference(tableNames[i],
                    string.Concat("tableNames[", i, "]"));
                ArgumentValidation.CheckForEmptyString(tableNames[i], 
                    string.Concat("tableNames[", i, "]"));
            }

            using (DbDataAdapter adapter = 
                GetDataAdapter(UpdateBehavior.Standard, command.Command.Connection))
            {
                ((IDbDataAdapter)adapter).SelectCommand = command.Command;

                try
                {
                    DateTime startTime = DateTime.Now;
                    //this.LogWrite( command.Command, startTime );

                    string systemCreatedTableNameRoot = "Table";
                    for (int i = 0; i < tableNames.Length; i++)
                    {
                        string systemCreatedTableName = (i == 0)
                            ? systemCreatedTableNameRoot
                            : systemCreatedTableNameRoot + i;

                        adapter.TableMappings.Add(systemCreatedTableName, tableNames[i]);
                    }

                    adapter.Fill(dataSet);
                }
                catch
                {
                    throw;
                }
            }
        }

        private object DoExecuteScalar(DBCommandWrapper command)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                //this.LogWrite( command.Command, startTime );

                object returnValue = command.Command.ExecuteScalar();
                return returnValue;
            }
            catch
            {
                throw;
            }
        }

        private void DoExecuteNonQuery(DBCommandWrapper command)
        {
            DateTime startTime = DateTime.MinValue;
            string strParmInfo = string.Empty;
            try
            {
                if (command.Command.Parameters.Count > 0)//参数
                {
                    for (int i = 0; i < command.Command.Parameters.Count; i++)
                    {
                        strParmInfo += Environment.NewLine + "参数：" + command.GetParameterName(i) + "；值：" + command.GetParameterValue(i);
                    }
                }
                startTime = DateTime.Now;
                command.RowsAffected = command.Command.ExecuteNonQuery();

            }
            catch (Exception ex)//记录日志,记录SQL语句、参数、执行开始时间、执行结束时间
            {
                //logger.Error(startTime.ToString()+"开始执行："+command.Command.CommandText+strParmInfo+Environment.NewLine+ex.Message);
                throw ex;
            }
        }

        private IDataReader DoExecuteReader(IDbCommand command, CommandBehavior cmdBehavior)
        {
            try
            {
                DateTime startTime = DateTime.Now;
                //this.LogWrite( command, startTime );

                IDataReader reader = command.ExecuteReader(cmdBehavior);
                return reader;
            }
            catch
            {
                throw;
            }
        }

        private IDbTransaction BeginTransaction(IDbConnection connection)
        {
            try
            {
                IDbTransaction tran = connection.BeginTransaction();
                return tran;
            }
            catch
            {
                throw;
            }
        }

        private void RollbackTransaction(IDbTransaction tran)
        {
            tran.Rollback();
        }

        private void CommitTransaction(IDbTransaction tran)
        {
            tran.Commit();
        }

        ///// <summary>
        ///// 2006年7月3号修改，
        ///// 日志记录如果产生异常不会影响其他方法的执行，
        ///// 将该方法的所有调用位置前移，置于数据库操作之前
        ///// </summary>
        ///// <param name="command"></param>
        ///// <param name="startTime"></param>
        //private void LogWrite(IDbCommand command, DateTime startTime)
        //{

        //    try
        //    {
        //        //陈文彬2005-10-28新加入的代码，增加审计日志部分的内容
        //        int securityLevel = -1;
        //        if(ClientContext.Current["ACM_AUDITLOG_MODULE_SECURITYLEVEL"] != null)
        //        {
        //            securityLevel = (int)ClientContext.Current["ACM_AUDITLOG_MODULE_SECURITYLEVEL"];
        //        }

        //        if( this.enabledLog || securityLevel == 4) //陈文彬2005-10-28新加入的代码，增加安全等级的判断，如果安全等级达到4级则必须记录数据访问日志
        //        {
        //            IDataParameter[] param = new IDataParameter[command.Parameters.Count];
        //            command.Parameters.CopyTo(param,0);
        //            Logging.DataAccessEntry dae = new Logging.DataAccessEntry( this.connectionString,startTime,DateTime.Now,command.CommandText,param);
        //            Logging.LogCore.Write(dae);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        string msg = ex.Message;
        //    }
        //}

    }
}

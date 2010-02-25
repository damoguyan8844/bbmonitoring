using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using JOYFULL.CMPW.Data;
using System.Xml;
using System.Data.Common;

namespace JOYFULL.CMPW.Data.Access
{
    /// <summary>
    /// <para>Represents a Sql Server Database.</para>
    /// </summary>
    /// <remarks> 
    /// <para>
    /// Internally uses Sql Server .NET Managed Provider from Microsoft (System.Data.SqlClient) to connect to the database.
    /// </para>  
    /// </remarks>
    internal class AccessDatabase : Database
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="AccessDatabase"/> class.
        /// </summary>
        public AccessDatabase( )
            : base()
        {
        }

        /// <summary>
        /// <para>Gets the parameter token used to delimit parameters for the Sql Database.</para>
        /// </summary>
        /// <value>
        /// <para>The '@' symbol.</para>
        /// </value>
        protected internal override char ParameterToken
        {
            get { return '@'; }
        }

        /// <summary>
        /// <para>Get the connection for this database.</para>
        /// <seealso cref="IDbConnection"/>
        /// <seealso cref="SqlConnection"/>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="SqlConnection"/> for this database.</para>
        /// </returns>
        internal override IDbConnection GetConnection( )
        {
            return new OleDbConnection( ConnectionString );
        }

        /// <summary>
        /// <para>Create a <see cref="SqlCommandWrapper"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <returns><para>The <see cref="SqlCommandWrapper"/> for the stored procedure.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="storedProcedureName"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="storedProcedureName"/> hast not been initialized.</para>
        /// </exception>
        internal override DBCommandWrapper GetStoredProcCommandWrapper( string storedProcedureName )
        {
            ArgumentValidation.CheckForNullReference( storedProcedureName, "storedProcedureName" );
            ArgumentValidation.CheckForEmptyString( storedProcedureName, "storedProcedureName" );

            return new AccessCommandWrapper( storedProcedureName, CommandType.StoredProcedure, ParameterToken );
        }

        /// <summary>
        /// <para>Create an <see cref="SqlCommandWrapper"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <param name="parameterValues"><para>The list of parameters for the procedure.</para></param>
        /// <returns><para>The <see cref="SqlCommandWrapper"/> for the stored procedure.</para></returns>
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
        internal override DBCommandWrapper GetStoredProcCommandWrapper( string storedProcedureName, params object[] parameterValues )
        {
            ArgumentValidation.CheckForNullReference( storedProcedureName, "storedProcedureName" );
            ArgumentValidation.CheckForEmptyString( storedProcedureName, "storedProcedureName" );
            ArgumentValidation.CheckForNullReference( parameterValues, "parameterValues" );

            return new AccessCommandWrapper( storedProcedureName, CommandType.StoredProcedure, ParameterToken, parameterValues );
        }

        /// <summary>
        /// <para>Create an <see cref="SqlCommandWrapper"/> for a SQL query.</para>
        /// </summary>
        /// <param name="query"><para>The text of the query.</para></param>        
        /// <returns><para>The <see cref="SqlCommandWrapper"/> for the SQL query.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="query"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="query"/> hast not been initialized.</para>
        /// </exception>
        internal override DBCommandWrapper GetSqlStringCommandWrapper( string query )
        {
            ArgumentValidation.CheckForNullReference( query, "query" );
            ArgumentValidation.CheckForEmptyString( query, "query" );

            return new AccessCommandWrapper( query, CommandType.Text, ParameterToken );
        }

        /// <summary>
        /// <para>Create a <see cref="SqlDataAdapter"/> with the given update behavior and connection.</para>
        /// </summary>
        /// <param name="updateBehavior">
        /// <para>One of the <see cref="UpdateBehavior"/> values.</para>
        /// </param>
        /// <param name="connection">
        /// <para>The open connection to the database.</para>
        /// </param>
        /// <returns>An <see cref="SqlDataAdapter"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="connection"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        protected internal override DbDataAdapter GetDataAdapter( UpdateBehavior updateBehavior, IDbConnection connection )
        {
            string queryStringToBeFilledInLater = String.Empty;
            OleDbDataAdapter adapter = new OleDbDataAdapter( queryStringToBeFilledInLater, 
                ( OleDbConnection )connection );

            if( updateBehavior == UpdateBehavior.Continue )
            {
                adapter.RowUpdated += new OleDbRowUpdatedEventHandler( OnAccessRowUpdated );
            }
            return adapter;
        }

        # region unsupported XmlReader
        ///// <summary>
        ///// <para>Executes the <see cref="SqlCommandWrapper"/> and returns an <see cref="XmlReader"/>.</para>
        ///// </summary>
        ///// <param name="command">
        ///// <para>The <see cref="SqlCommandWrapper"/> to execute.</para>
        ///// </param>
        ///// <returns>
        ///// <para>An <see cref="XmlReader"/> object.</para>
        ///// </returns>
        //internal XmlReader ExecuteXmlReader( AccessCommandWrapper command )
        //{
        //    IDbConnection connection = OpenConnection();
        //    PrepareCommand( command, connection );
        //    OleDbCommand accessCommand = command.Command as OleDbCommand;
        //    return DoExecuteXmlReader( accessCommand );
        //}

        ///// <summary>
        ///// <para>Executes the <see cref="SqlCommandWrapper"/> in a transaction and returns an <see cref="XmlReader"/>.</para>
        ///// </summary>
        ///// <param name="command">
        ///// <para>The <see cref="SqlCommandWrapper"/> to execute.</para>
        ///// </param>
        ///// <param name="transaction">
        ///// <para>The <see cref="IDbTransaction"/> to execute the command within.</para>
        ///// </param>
        ///// <returns>
        ///// <para>An <see cref="XmlReader"/> object.</para>
        ///// </returns>
        //internal XmlReader ExecuteXmlReader( AccessCommandWrapper command, IDbTransaction transaction )
        //{
        //    PrepareCommand( command, transaction );

        //    OleDbCommand accessCommand = command.Command as OleDbCommand;
        //    return DoExecuteXmlReader( accessCommand );
        //}

        ///// <devdoc>
        ///// Execute the actual Xml Reader call.
        ///// </devdoc>        
        //private XmlReader DoExecuteXmlReader( OleDbCommand oleCommand )
        //{
        //    try
        //    {
        //        DateTime startTime = DateTime.Now;
        //        XmlReader reader = oleCommand.ExecuteXmlReader();
        //        return reader;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        # endregion

        /// <devdoc>
        /// Listens for the RowUpdate event on a dataadapter to support UpdateBehavior.Continue
        /// </devdoc>
        private void OnAccessRowUpdated( object sender, OleDbRowUpdatedEventArgs rowThatCouldNotBeWritten )
        {
            if( rowThatCouldNotBeWritten.RecordsAffected == 0 )
            {
                if( rowThatCouldNotBeWritten.Errors != null )
                {
                    rowThatCouldNotBeWritten.Row.RowError = "Failed to update row";
                    rowThatCouldNotBeWritten.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }
    }
}

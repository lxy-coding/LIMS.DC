using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using LIMS.DC.Common.LOG;

namespace LIMS.DC.DAL.Helper
{
    /********************* Copyright （c）*************************
    ** 湖南物联千盟
    ********************* File info*******************************
    ** Create by ： 张乐凡
    ** Create Date： 2019-09-10
    ** Version： 1.0
    ** Descriptions： 数据库操作类
    ********************* File info*******************************
    ** Modify by ： 周张智
    ** Modify Date： 2019-09-18
    ** Version： 1.1
    ** Descriptions： 数据库操作类添加NHibernate操作方法
    *************************************************************/
    /// <summary>
    /// ORACLE 数据库操作类
    /// 作者：张乐凡\周张智
    /// 日期： 2019-09-10
    /// </summary>
    public static class OracleDataHelper
    {
        #region ORACLE连接字符
        /// <summary>
        /// Oracle.ManagedDataAccess 使用
        /// </summary>
        static readonly string connstr = ConfigurationManager.AppSettings["ConnectionString"];
        #endregion

        /// <summary>
        /// 日志
        /// </summary>
        static SystemLog log = new SystemLog();

        #region ExecuteNonQuery
        /// <summary>
        /// 执行SQL语句增删改
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <returns>-1：执行失败</returns>
        public static int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, (OracleParameter[])null);
        }

        /// <summary>
        /// 执行SQL语句增删改
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>-1：执行失败</returns>
        public static int ExecuteNonQuery(string commandText, OracleParameter[] commandParameters)
        {
            // 初始化Oracle连接，执行数据库操作
            using (OracleConnection connection = new OracleConnection(connstr))//using可以保证用完之后直接销毁
            {
                try
                {
                    connection.Open();
                    return ExecuteNonQuery(commandText, commandParameters, connection);
                }
                catch (Exception exExecuteNonQuery)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exExecuteNonQuery.Message + commandText, "OracleCommand");

                    throw exExecuteNonQuery;
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        /// <summary>
        /// 执行SQL语句增删改
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandParameters">参数</param>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string commandText, OracleParameter[] commandParameters, OracleConnection connection)
        {
            // 初始化SqlCommand
            bool mustCloseConnection = false;
            OracleCommand command = new OracleCommand();
            using (command = PrepareCommand(command, connection, (OracleTransaction)null, CommandType.Text, commandText, commandParameters, out mustCloseConnection))
            {
                // 初始化执行结果
                int retval = -1;
                try
                {
                    // 执行
                    retval = command.ExecuteNonQuery();
                }
                catch (Exception exExecuteNonQuery)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exExecuteNonQuery.Message + commandText, "ExecuteNonQuery");

                    throw exExecuteNonQuery;
                }
                finally
                {
                    // 清空参数列表
                    command.Parameters.Clear();

                    // 关闭连接
                    if (mustCloseConnection)
                    {
                        connection.Close();
                    }
                }
                return retval;
            }
        }
        #endregion

        #region DatabaseIsOpen
        /// <summary>
        /// 判断数据库是否打开
        /// </summary>
        /// <returns>bool</returns>
        public static bool DatabaseIsOpen()
        {
            using (OracleConnection connection = new OracleConnection(connstr))//using可以保证用完之后直接销毁
            {
                try
                {
                    connection.Open();
                    connection.Close();
                    return true;
                }
                catch (Exception exConnection)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exConnection.Message, "DatabaseIsOpen");
                    return false;
                }
            }
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 执行SQL语句查询,返回结果集第一行的第一列值
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <returns></returns>
        public static object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, (OracleParameter[])null);
        }

        /// <summary>
        /// 执行SQL语句查询,返回结果集第一行的第一列值
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandParameters">参数</param>
        /// <returns></returns>
        public static object ExecuteScalar(string commandText, OracleParameter[] commandParameters )
        {
            // 初始化Oracle连接，执行数据库操作
            using (OracleConnection connection = new OracleConnection(connstr))//using可以保证用完之后直接销毁
            {
                try
                {
                    connection.Open();
                    return ExecuteScalar(commandText, commandParameters, connection);
                }
                catch (Exception exExecuteScalar)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exExecuteScalar.Message + commandText, "OracleCommand");

                    throw exExecuteScalar;
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        /// <summary>
        /// 执行SQL语句查询,返回结果集第一行的第一列值
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="commandParameters">参数</param>
        /// <param name="connection">connection</param>
        /// <returns></returns>
        public static object ExecuteScalar(string commandText, OracleParameter[] commandParameters, OracleConnection connection)
        {
            // 初始化SqlCommand
            bool mustCloseConnection = false;
            OracleCommand command = new OracleCommand();
            using (command = PrepareCommand(command, connection, (OracleTransaction)null, CommandType.Text, commandText, commandParameters, out mustCloseConnection))
            {
                // 初始化执行结果
                object retval = new object();
                try
                {
                    // 执行
                    retval = command.ExecuteScalar();
                }
                catch (Exception exExecuteScalar)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exExecuteScalar.Message + commandText, "ExecuteScalar");

                    throw exExecuteScalar;
                }
                finally
                {
                    // 清空参数列表
                    command.Parameters.Clear();

                    // 关闭连接
                    if (mustCloseConnection)
                    {
                        connection.Close();
                    }
                }
                return retval;
            }
        }
        #endregion

        #region ExecuteDataset
        /// <summary>
        /// 执行SQL获取DataSet对象
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string commandText)
        {
            return ExecuteDataset(commandText, (OracleParameter[])null);
        }

        /// <summary>
        /// 执行SQL获取DataSet对象
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <param name="commandParameters">参数列表</param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(string commandText, OracleParameter[] commandParameters)
        {
            DataSet retDataSet = new DataSet();

            // 初始化Oracle连接，执行数据库操作
            using (OracleConnection connection = new OracleConnection(connstr))//using可以保证用完之后直接销毁
            {
                try
                {
                    connection.Open();
                    return ExecuteDataset(connection, commandText, commandParameters);
                }
                catch (Exception exExecuteDataset)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exExecuteDataset.Message + commandText, "ExecuteDataset");

                    throw exExecuteDataset;
                }
            }
        }

        /// <summary>
        /// 执行SQL获取DataSet对象
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="connection">Oracle连接</param>
        /// <param name="commandText">Sql语句</param>
        /// <param name="commandParameters">参数列表</param>
        /// <returns></returns>
        public static DataSet ExecuteDataset(OracleConnection connection, string commandText, OracleParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            // 初始化并赋值command
            bool mustCloseConnection = false;
            OracleCommand cmd = new OracleCommand();
            using (cmd = PrepareCommand(cmd, connection, (OracleTransaction)null, CommandType.Text, commandText, commandParameters, out mustCloseConnection))
            {
                // 初始化DataSet
                DataSet ds = new DataSet();

                try
                {
                    // 用DataAdapter填充dataset
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        // 填充DataSet
                        da.Fill(ds);
                    }
                }
                catch (Exception exOracleDataAdapter)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exOracleDataAdapter.Message + commandText, "ExecuteDataset");

                    throw exOracleDataAdapter;
                }
                finally
                {
                    // 清空参数列表
                    cmd.Parameters.Clear();
                    // 关闭连接
                    if (mustCloseConnection)
                    {
                        connection.Close();
                    }
                }
                return ds;// 返回
            }
        }
        #endregion

        #region ExecuteDataTable
        /// <summary>
        /// 执行SQL获取DataTable对象
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, (OracleParameter[])null);
        }

        /// <summary>
        /// 执行SQL获取DataTable对象
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <param name="commandParameters">参数列表</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText, OracleParameter[] commandParameters)
        {
            DataTable retDataSet = new DataTable();

            // 初始化Oracle连接，执行数据库操作
            using (OracleConnection connection = new OracleConnection(connstr))//using可以保证用完之后直接销毁
            {
                try
                {
                    connection.Open();
                    return ExecuteDataTable(commandText, commandParameters, connection);
                }
                catch (Exception exConnection)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exConnection.Message + commandText, "ExecuteDataTable");

                    throw exConnection;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行SQL获取DataTable对象
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <param name="commandParameters">参数列表</param>
        /// <param name="connection">Oracle连接</param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string commandText, OracleParameter[] commandParameters, OracleConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            // 初始化并赋值command
            bool mustCloseConnection = true;
            OracleCommand cmd = new OracleCommand();
            using (cmd = PrepareCommand(cmd, connection, (OracleTransaction)null, CommandType.Text, commandText, commandParameters, out mustCloseConnection))
            {
                DataTable table = new DataTable();
                // 初始化DataTable
                try
                {
                    // 用DataAdapter填充dataset
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        da.Fill(table);// 填充DataSet
                    }
                }
                catch (Exception exOracleDataAdapter)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exOracleDataAdapter.Message + commandText, "ExecuteDataTable");

                    throw exOracleDataAdapter;
                }
                finally
                {
                    // 清空参数列表
                    cmd.Parameters.Clear();
                    // 关闭连接
                    if (mustCloseConnection)
                    {
                        connection.Close();
                    }
                }

                return table;// 返回
            }

        }
        #endregion



        public static int UpdateDataTable(DataTable table, string selectText)
        {
            using (OracleConnection connection = new OracleConnection(connstr))
            {
                connection.Open();
                OracleDataAdapter adapter = new OracleDataAdapter(selectText, connection);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                return adapter.Update(table);
            }
        }

        public static int UpdateDataRows(DataRow[] rows, string selectText)
        {
            using (OracleConnection connection = new OracleConnection(connstr))
            {
                connection.Open();
                OracleDataAdapter adapter = new OracleDataAdapter(selectText, connection);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                return adapter.Update(rows);
            }
        }


        #region ExecuteReader
        /// <summary>
        /// 执行SQL获取OracleDataReader对象
        /// 作者：li
        /// 日期：2019-11-27
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <returns></returns>
        public static OracleDataReader ExecuteReader(string commandText)
        {
            OracleConnection connection = new OracleConnection(connstr);
            OracleCommand cmd = new OracleCommand(commandText, connection);
            try
            {
                connection.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                connection.Close();
                log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", ex.Message + commandText, "ExecuteReader");
                throw ex;
            }
        }
        #endregion

        #region ExecuteCount
        /// <summary>
        /// 执行SQL获取数量
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <returns></returns>
        public static int ExecuteCount(string commandText)
        {
            return ExecuteCount(commandText, (OracleParameter[])null);
        }

        /// <summary>
        /// 执行SQL获取数量
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="commandParameters">参数列表</param>
        /// <returns></returns>
        public static int ExecuteCount(string commandText, OracleParameter[] commandParameters)
        {
            // 初始化Oracle连接，执行数据库操作
            using (OracleConnection connection = new OracleConnection(connstr))//using可以保证用完之后直接销毁
            {
                try
                {
                    connection.Open();
                    return ExecuteCount(commandText, commandParameters, connection);
                }
                catch (Exception exOracleCommand)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exOracleCommand.Message + commandText, "OracleCommand");

                    throw exOracleCommand;
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        /// <summary>
        /// 执行SQL获取数量
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="commandParameters">参数列表</param>
        /// <param name="connection">Oracle连接</param>
        /// <returns></returns>
        public static int ExecuteCount(string commandText, OracleParameter[] commandParameters, OracleConnection connection)
        {
            try
            {
                using (DataTable dt = ExecuteDataTable(commandText, commandParameters, connection))
                {
                    // 通过调用DataTable方法，获取table中的总行数
                    if (dt != null)
                    {
                        return dt.Rows.Count;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception exExecuteCount)
            {
                log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exExecuteCount.Message + commandText, "ExecuteCount");

                throw exExecuteCount;
            }
        }
        #endregion

        #region ExecuteTransaction

        /// <summary>
        /// 执行SQL语句事务
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="commandText">SQL语句集合</param>
        /// <returns>成功的条数, -1：执行失败</returns>
        public static int ExecuteTransaction(ArrayList listSql)
        {
            return ExecuteTransaction(listSql, connstr);
        }

        /// <summary>
        /// 执行SQL语句事务
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="listSql">SQL语句集合</param>
        /// <param name="connection">connection</param>
        /// <returns>成功的条数, -1：执行失败</returns>
        public static int ExecuteTransaction(ArrayList listSql, string connstr)
        {
            int inum = 0;

            try
            {
                using (OracleConnection connection = new OracleConnection(connstr))//using可以保证用完之后直接销毁
                {
                    connection.Open();

                    OracleCommand cmd = connection.CreateCommand();

                    OracleTransaction transaction = connection.BeginTransaction();

                    cmd.Transaction = transaction;
                    try
                    {

                        foreach (string s in listSql)
                        {
                            if (s.Trim().Length > 1)
                            {
                                cmd.CommandText = s;

                                inum += cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception exExecuteNonQuery)
                    {
                        transaction.Rollback();
                        inum = -1;
                        string strSql = "";
                        foreach (string s in listSql)
                        {
                            strSql += s;
                        }

                        log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exExecuteNonQuery.Message+ strSql, "ExecuteNonQuery");

                        throw exExecuteNonQuery;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
            catch (Exception exExecuteTransaction)
            {
                inum = -1;

                log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exExecuteTransaction.Message, "ExecuteTransaction");

                throw exExecuteTransaction;
            }

            return inum;
        }

        #endregion

        #region ExecuteProcedure

        /// <summary>
        /// 存储过程.查询
        /// 作者：周张智
        /// 日期：2019-10-10
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandParameters">参数</param>
        /// <returns>DataTable</returns>
        public static DataTable QueryProcedure(string commandText, OracleParameter[] commandParameters = null)
        {
            // 初始化Oracle连接，执行数据库操作
            using (OracleConnection connection = new OracleConnection(connstr))//using可以保证用完之后直接销毁
            {
                try
                {
                    connection.Open();
                    return QueryProcedure(commandText, commandParameters, connection);
                }
                catch (Exception exExecuteScalar)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exExecuteScalar.Message, "OracleCommand");

                    throw exExecuteScalar;
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        /// <summary>
        /// 存储过程.查询
        /// 作者：周张智
        /// 日期：2019-10-10
        /// </summary>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandParameters">参数</param>
        /// <param name="connection">connection</param>
        /// <returns>DataTable</returns>
        public static DataTable QueryProcedure(string commandText, OracleParameter[] commandParameters, OracleConnection connection)
        {
            DataSet ds = new DataSet();
            // 初始化SqlCommand
            bool mustCloseConnection = false;
            OracleCommand command = new OracleCommand();
            using (command = PrepareCommand(command, connection, (OracleTransaction)null, CommandType.StoredProcedure, commandText, commandParameters, out mustCloseConnection))
            {
                try
                {
                    // 执行
                    OracleDataAdapter ad = new OracleDataAdapter(command);
                    ad.Fill(ds, "report_build");
                }
                catch (Exception exExecuteScalar)
                {
                    log.WriteLog(E_ProcessLogType.Error, E_LogType.DataDeal, "数据库操作类", exExecuteScalar.Message, "ExecuteScalar");

                    throw exExecuteScalar;
                }
                finally
                {
                    // 清空参数列表
                    command.Parameters.Clear();
                    // 关闭连接
                    if (mustCloseConnection)
                    {
                        connection.Close();
                    }
                }
                return ds.Tables[0];
            }
        }
        #endregion 

        #region 私有方法
        /// <summary>
        /// 准备command
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="connection">连接</param>
        /// <param name="transaction">回退机制</param>
        /// <param name="commandType">command类型</param>
        /// <param name="commandText">sql语句</param>
        /// <param name="commandParameters">参数列表</param>
        /// <param name="mustCloseConnection">是否需要关闭 out</param>
        private static OracleCommand PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, out bool mustCloseConnection)
        {
            //using (OracleCommand command = new OracleCommand())
            //{
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // 如果连接状态不为打开，则持续打开，否则需要关闭
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            command.Connection = connection;// 注册连接

            command.CommandText = commandText;// 注册Sql语句

            // 如果有回退机制，注册回退机制
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            command.CommandType = commandType;//注册命令类型

            // 如果有参数列表，将参数依次注册进参数列表
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            return command;
            //}
        }

        /// <summary>
        /// 将参数加入command中
        /// 作者：张乐凡
        /// 日期：2019-09-10
        /// </summary>
        /// <param name="command">空参数列表的command</param>
        /// <param name="commandParameters">参数列表</param>
        private static void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (OracleParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 给为null的Value转换值
                        if (p.Value == null)
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
        #endregion
    }
}

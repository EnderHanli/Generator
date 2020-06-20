using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using ClassGenerator.Extension.Enums;
using ClassGenerator.Extension.Model;
using  Dapper;

namespace ClassGenerator.Extension.Helper
{
    public class DatabaseHelper
    {
        private readonly DbProviderFactory _dbProviderFactory;
        public DbConnection Connection { get; private set; }
        public const string ParameterMarker = "@";

        private readonly string _providerName;

        private readonly string _connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public DatabaseHelper(string path)
        {
            var configFile = System.IO.Path.Combine(path, "App.Config");
            var configFileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configFile
            };
            var config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            _connectionString = config.ConnectionStrings.ConnectionStrings["db"].ConnectionString;
            _providerName = config.ConnectionStrings.ConnectionStrings["db"].ProviderName;
            _dbProviderFactory = DbProviderFactories.GetFactory(_providerName);
            CreateConnection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DbCommand CreateCommand(string commandText, CommandType commandType)
        {
            try
            {
                var dbCommand = _dbProviderFactory.CreateCommand();
                // ReSharper disable once PossibleNullReferenceException
                dbCommand.CommandText = commandText;
                dbCommand.CommandType = commandType;
                dbCommand.Connection = Connection;
                return dbCommand;
            }
            catch (Exception)
            {
                throw new Exception("Geçersiz parametre.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateConnection()
        {
            try
            {
                Connection = _dbProviderFactory.CreateConnection();
                // ReSharper disable once PossibleNullReferenceException
                Connection.ConnectionString = _connectionString;
                Connection.Open();
            }
            catch (Exception)
            {
                throw new Exception("Bağlantı oluşturulurken hata oluştu. Lütfen bağlantı dizesini ve sağlayıcı adını kontrol edin.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetServerName()
        {
            var builder = _dbProviderFactory.CreateConnectionStringBuilder();
            // ReSharper disable once PossibleNullReferenceException
            builder.ConnectionString = _connectionString;
            builder.TryGetValue("Data Source", out var serverName);
            return serverName.ToString();
        }

        public DatabaseType GetDatabaseType()
        {
            if (_providerName.Contains(nameof(DatabaseType.SqlServer)))
                return DatabaseType.SqlServer;
            if (_providerName.Contains(nameof(DatabaseType.PostgreSql)))
                return DatabaseType.PostgreSql;
            if (_providerName.Contains(nameof(DatabaseType.Oracle)))
                return DatabaseType.Oracle;
            throw new ArgumentOutOfRangeException();
        }

        public List<DbModel> LoadTable()
        {
            string sql;

            if (Connection.State != ConnectionState.Open)
                Connection.Open();

            switch (GetDatabaseType())
            {
                case DatabaseType.SqlServer:
                    sql = SqlServerHelper.TableSql;
                    break;
                case DatabaseType.PostgreSql:
                    sql = PostgreSqlHelper.TableSql;
                    break;
                case DatabaseType.Oracle:
                    sql = OracleHelper.TableSql;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DbCommand command = CreateCommand(sql, CommandType.Text);

            List<DbModel> results = Connection.Query<DbModel>(command.CommandText, commandType: command.CommandType = CommandType.Text).ToList();
            command.Dispose();
            Connection.Close();
            return results;
        }

        public List<DbModel> LoadViews()
        {
            string sql;

            if (Connection.State != ConnectionState.Open)
                Connection.Open();

            switch (GetDatabaseType())
            {
                case DatabaseType.SqlServer:
                    sql = SqlServerHelper.ViewSql;
                    break;
                case DatabaseType.PostgreSql:
                    sql = PostgreSqlHelper.ViewSql;
                    break;
                case DatabaseType.Oracle:
                    sql = OracleHelper.ViewSql;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            DbCommand command = CreateCommand(sql, CommandType.Text);

            List<DbModel> results = Connection.Query<DbModel>(command.CommandText, commandType: command.CommandType = CommandType.Text).ToList();
            command.Dispose();
            Connection.Close();
            return results;
        }
    }
}
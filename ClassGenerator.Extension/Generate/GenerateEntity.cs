using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using ClassGenerator.Extension.Enums;
using ClassGenerator.Extension.Helper;
using Dapper;
using DbColumn = ClassGenerator.Extension.Model.DbColumn;

namespace ClassGenerator.Extension.Generate
{
    public class GenerateEntity
    {
        private readonly DatabaseHelper _databaseHelper;

        public GenerateEntity(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }
        public List<DbColumn> Generate(long objectId)
        {
            string sql;
            var parameterMarker = "";
            var databaseType = _databaseHelper.GetDatabaseType();

            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    sql = SqlServerHelper.TableColumnSql;
                    parameterMarker = SqlServerHelper.ParameterMarker;
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

            if(_databaseHelper.Connection.State!=ConnectionState.Open)
                _databaseHelper.Connection.Open();

            DynamicParameters dynamicParameters=new DynamicParameters();
            dynamicParameters.Add(parameterMarker + "ObjectId", objectId, DbType.String);
            var columns = _databaseHelper.Connection.Query<DbColumn>(sql,dynamicParameters).ToList();
            columns.ConvertSqlTypeToDbType(databaseType);
            return columns;
        }
    }
}
using System;
using System.Collections.Generic;
using ClassGenerator.Extension.Enums;
using ClassGenerator.Extension.Model;

namespace ClassGenerator.Extension.Helper
{
    public static class TypeHelper
    {
        public static void ConvertSqlTypeToDbType(this List<DbColumn> columns, DatabaseType databaseType)
        {
            foreach (var column in columns)
            {
                column.ColumnName = ProjectHelper.GetPascalCase(column.ColumnName);
                switch (databaseType)
                {
                    case DatabaseType.SqlServer:
                        column.CsType = SqlServerHelper.GetClrType(column.SqlType, column.IsNullable);
                        column.DbType = SqlServerHelper.GetDbType(column.SqlType);
                        break;
                    case DatabaseType.PostgreSql:
                        column.CsType = PostgreSqlHelper.GetClrType(column.SqlType, column.IsNullable);
                        break;
                    case DatabaseType.Oracle:
                        column.CsType = OracleHelper.GetClrType(column.SqlType, column.IsNullable);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
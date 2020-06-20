using ClassGenerator.Extension.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGenerator.Extension.Generate
{
    public static class GenerateDal
    {
        private const string ParameterMarker = "@";

        public static string GetEntitiesSql(string schemaName, string tableName)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("select * from {0}.{1} where 1=1 ", schemaName, tableName);
            return sb.ToString();
        }

        public static string GetInsertSql(string schemaName, string tableName, List<DbColumn> columns)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("insert into {0}.{1} ({2}) values ({3})", schemaName, tableName, string.Join(", ", columns.Select(p => p.ColumnName)),
                string.Join(", ", columns.Select(p => ParameterMarker + p.ColumnName)));
            sb.Append(";");
            return sb.ToString();
        }

        public static string GetUpdateSql(string schemaName, string tableName, List<DbColumn> columns)
        {
            var sb = new StringBuilder();
            string whereClause = string.Empty;
            List<DbColumn> pkColumns = columns.FindAll(p => p.IsPrimaryKey == true);

            if (pkColumns.Count > 0)
            {
                whereClause = string.Join(" AND ",
                    pkColumns.Select(p => string.Format("{0} = " + ParameterMarker + "{0}", p.ColumnName)));
            }
            sb.AppendFormat("update {0}.{1} set {2} where {3}", schemaName, tableName,
                string.Join(", ",
                    columns.FindAll(p => p.IsPrimaryKey == false)
                        .Select(p => string.Format("{0} = " + ParameterMarker + "{0}", p.ColumnName))), whereClause);
            sb.Append(";");
            return sb.ToString();
        }

        public static string GetDeleteSql(string schemaName, string tableName, List<DbColumn> columns)
        {
            var sb = new StringBuilder();
            string whereClause = string.Empty;
            List<DbColumn> pkColumns = columns.FindAll(p => p.IsPrimaryKey == true);

            if (pkColumns.Count > 0)
            {
                whereClause = string.Join(" AND ",
                    pkColumns.Select(p => string.Format("{0} = " + ParameterMarker + "{0}", p.ColumnName)));
            }

            sb.AppendFormat("delete from {0}.{1} where {2}", schemaName, tableName, whereClause);
            sb.Append(";");
            return sb.ToString();
        }
    }
}
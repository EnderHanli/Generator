using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassGenerator.Extension.Helper
{
    public static class OracleHelper
    {
        public const string TableSql = "select s.name as SchemaName, t.name as TableName from sys.schemas s join sys.tables t  on t.schema_id=s.schema_id order by s.name, t.name";
        public const string ViewSql = "select s.name as SchemaName, v.name as ViewName from sys.schemas s join sys.views v  on v.schema_id=s.schema_id order by s.name, v.name";

        private static SqlDbType GetSqlDbType(string sqlTypeName)
        {
            sqlTypeName = sqlTypeName.ToLower(System.Globalization.CultureInfo.InvariantCulture);

            if (Enum.TryParse(sqlTypeName, true, out SqlDbType sqlType))
            {
                return sqlType;
            }
            else
            {
                switch (sqlTypeName)
                {
                    case "sql_variant":
                        return SqlDbType.Variant;
                    case "numeric":
                        return SqlDbType.Decimal;
                    case "rowversion":
                        return SqlDbType.Timestamp;
                    case "smalldatetime":
                        return SqlDbType.DateTime;
                    case "sysname":
                        return SqlDbType.VarChar;

                    default:
                        throw new InvalidCastException($"Unable to cast '{sqlTypeName}' to appropriate SqlDbType enum.");
                }
            }
        }

        public static string GetClrType(string sqlTypeName, bool isNullable)
        {
            Type type;
            SqlDbType sqlType = GetSqlDbType(sqlTypeName);
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    type = isNullable ? typeof(long?) : typeof(long);
                    break;
                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    type = typeof(byte[]);
                    break;
                case SqlDbType.Bit:
                    type = isNullable ? typeof(bool?) : typeof(bool);
                    break;
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    type = typeof(string);
                    break;
                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    type = isNullable ? typeof(DateTime?) : typeof(DateTime);
                    break;
                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    type = isNullable ? typeof(decimal?) : typeof(decimal);
                    break;
                case SqlDbType.Float:
                    type = isNullable ? typeof(double?) : typeof(double);
                    break;
                case SqlDbType.Int:
                    type = isNullable ? typeof(int?) : typeof(int);
                    break;
                case SqlDbType.Real:
                    type = isNullable ? typeof(float?) : typeof(float);
                    break;
                case SqlDbType.UniqueIdentifier:
                    type = typeof(Guid);
                    break;
                case SqlDbType.SmallInt:
                    type = isNullable ? typeof(short?) : typeof(short);
                    break;
                case SqlDbType.TinyInt:
                    type = isNullable ? typeof(byte?) : typeof(byte);
                    break;
                case SqlDbType.Variant:
                case SqlDbType.Udt:
                    type = typeof(object);
                    break;
                case SqlDbType.Structured:
                    type = typeof(DataTable);
                    break;
                case SqlDbType.DateTimeOffset:
                    type = isNullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset);
                    break;
                default:
                    throw new ArgumentException("Invalid SqlType", sqlType.ToString());
            }

            return GetDisplayName(type);
        }

        private static string GetDisplayName(this Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return string.Format("{0}?", GetDisplayName(t.GetGenericArguments()[0]));
            }

            if (t.IsGenericType)
            {
                return string.Format("{0}<{1}>",
                                     t.Name.Remove(t.Name.IndexOf('`')),
                                     string.Join(",", t.GetGenericArguments().Select(at => at.GetDisplayName())));
            }

            if (t.IsArray)
            {
                return string.Format("{0}[{1}]",
                                     GetDisplayName(t.GetElementType()),
                                     new string(',', t.GetArrayRank() - 1));
            }

            return t.Name;
        }
    }
}

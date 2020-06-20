using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ClassGenerator.Extension.Helper
{
    public static class SqlServerHelper
    {
        public const string ParameterMarker = "@";
        public const string TableSql = "select s.name as SchemaName, t.name  as Name,t.object_id as ObjectId from sys.tables t inner join sys.schemas s on s.schema_id =t.schema_id";
        public const string ViewSql = "select s.name as SchemaName, v.name as Name from sys.schemas s join sys.views v  on v.schema_id=s.schema_id order by s.name, v.name";
        public const string TableColumnSql = @"SELECT 
                                                c.name as ColumnName,
                                                t.name as SqlType,
                                                case when i.is_primary_key = 1 then i.is_primary_key  else 0 end as IsPrimaryKey,
                                                c.is_nullable as IsNullable,
                                                c.max_length as MaxLength
                                                from sys.columns c
    	                                             inner join sys.types t on t.user_type_id =c.user_type_id
    	                                             left join  sys.index_columns ic ON ic.object_id = c.object_id AND c.column_id = ic.column_id
		                                             left join sys.indexes i  ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    	                                             where c.object_id = " + ParameterMarker + "ObjectId";

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
            var type = ConvertClrType(sqlTypeName, isNullable);
            return GetDisplayName(type);

            
        }

        private static Type ConvertClrType(string sqlTypeName, bool isNullable = false)
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

            return type;
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

            string typeName;
            using (var provider = new CSharpCodeProvider())
            {
                var typeRef = new CodeTypeReference(t);
                typeName = provider.GetTypeOutput(typeRef);
            }

            return typeName.Replace("System.","");
        }

        public static DbType GetDbType(string sqlTypeName)
        {
            Dictionary<Type, DbType> typeMap = new Dictionary<Type, DbType>
            {
                [typeof(byte)] = DbType.Byte,
                [typeof(sbyte)] = DbType.SByte,
                [typeof(short)] = DbType.Int16,
                [typeof(ushort)] = DbType.UInt16,
                [typeof(int)] = DbType.Int32,
                [typeof(uint)] = DbType.UInt32,
                [typeof(long)] = DbType.Int64,
                [typeof(ulong)] = DbType.UInt64,
                [typeof(float)] = DbType.Single,
                [typeof(double)] = DbType.Double,
                [typeof(decimal)] = DbType.Decimal,
                [typeof(bool)] = DbType.Boolean,
                [typeof(string)] = DbType.String,
                [typeof(char)] = DbType.StringFixedLength,
                [typeof(Guid)] = DbType.Guid,
                [typeof(DateTime)] = DbType.DateTime,
                [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                [typeof(byte[])] = DbType.Binary,
                [typeof(byte?)] = DbType.Byte,
                [typeof(sbyte?)] = DbType.SByte,
                [typeof(short?)] = DbType.Int16,
                [typeof(ushort?)] = DbType.UInt16,
                [typeof(int?)] = DbType.Int32,
                [typeof(uint?)] = DbType.UInt32,
                [typeof(long?)] = DbType.Int64,
                [typeof(ulong?)] = DbType.UInt64,
                [typeof(float?)] = DbType.Single,
                [typeof(double?)] = DbType.Double,
                [typeof(decimal?)] = DbType.Decimal,
                [typeof(bool?)] = DbType.Boolean,
                [typeof(char?)] = DbType.StringFixedLength,
                [typeof(Guid?)] = DbType.Guid,
                [typeof(DateTime?)] = DbType.DateTime,
                [typeof(DateTimeOffset?)] = DbType.DateTimeOffset
            };
            Type type = ConvertClrType(sqlTypeName);
            return typeMap[type];
        }
    }
}

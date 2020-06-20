using System.Data;

namespace ClassGenerator.Extension.Model
{
    public class DbColumn
    {
        public string ColumnName { get; set; }
        public string CsType { get; set; }
        public DbType DbType { get; set; }
        public string SqlType { get; set; }
        public bool IsNullable { get; set; }
        public bool IsPrimaryKey { get; set; }
        public int MaxLength { get; set; }
    }
}
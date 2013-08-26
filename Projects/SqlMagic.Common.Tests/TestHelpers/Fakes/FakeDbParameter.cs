using System;
using System.Data;

namespace SqlMagic.Common.Tests.TestHelpers.Fakes
{
    public class FakeDbParameter : IDbDataParameter
    {
        public byte Precision { get; set; }
        public byte Scale { get; set; }
        public int Size { get; set; }
        public DbType DbType { get; set; }
        public ParameterDirection Direction{ get; set; }
        public bool IsNullable{ get; private set; }
        public string ParameterName{ get; set; }
        public string SourceColumn{ get; set; }
        public DataRowVersion SourceVersion{ get; set; }
        public object Value{ get; set; }
    }
}

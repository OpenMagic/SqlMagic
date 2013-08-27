using System.Data.Linq.Mapping;

namespace SqlMagic.Tests.TestHelpers
{
    [Table(Name = "TestEntities")]
    public class TestEntity
    {
        public int Id { get; set; }

        public int RequiredInt { get; set; }
        public int? NullableInt { get; set; }

        [Column(CanBeNull = false)]
        public string RequiredString { get; set; }
        public string NullableString { get; set; }
    }
}

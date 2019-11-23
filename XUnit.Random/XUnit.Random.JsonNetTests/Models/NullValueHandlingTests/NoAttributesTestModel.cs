using System;
using XUnit.Random.JsonNetTests.Models.Abstractions;

namespace XUnit.Random.JsonNetTests.Models.NullValueHandlingTests
{
    public class NoAttributesTestModel : INullValueHandlingTestModel
    {
        public string Id { get; set; }

        public string NameId { get; set; }

        public string TextValueId { get; set; }

        public int ValueId { get; set; }

        public int? NullableValueId { get; set; }

        public DateTime DateValueId { get; set; }

        public DateTime? NullableDateValueId { get; set; }
    }
}
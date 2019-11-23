using System;

namespace XUnit.Random.JsonNetTests.Models.Abstractions
{
    public interface INullValueHandlingTestModel
    {
        string Id { get; set; }

        string NameId { get; set; }

        string TextValueId { get; set; }

        int ValueId { get; set; }

        int? NullableValueId { get; set; }

        DateTime DateValueId { get; set; }

        DateTime? NullableDateValueId { get; set; }
    }
}
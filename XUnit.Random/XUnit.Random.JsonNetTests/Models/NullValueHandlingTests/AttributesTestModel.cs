using System;
using Newtonsoft.Json;
using XUnit.Random.JsonNetTests.Models.Abstractions;

namespace XUnit.Random.JsonNetTests.Models.NullValueHandlingTests
{
    public class AttributesTestModel : INullValueHandlingTestModel
    {
        [JsonProperty(PropertyName = "ID")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nameID")]
        public string NameId { get; set; }

        [JsonProperty(PropertyName = "textValueID")]
        public string TextValueId { get; set; }

        [JsonProperty(PropertyName = "valueID")]
        public int ValueId { get; set; }

        [JsonProperty(PropertyName = "nullableValueID")]
        public int? NullableValueId { get; set; }

        [JsonProperty(PropertyName = "dateValueID")]
        public DateTime DateValueId { get; set; }

        [JsonProperty(PropertyName = "nullableDateValueID")]
        public DateTime? NullableDateValueId { get; set; }
    }
}
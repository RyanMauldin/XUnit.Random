using System;
using Newtonsoft.Json;
using XUnit.Random.JsonNetTests.Models.Abstractions;

namespace XUnit.Random.JsonNetTests.Models.NullValueHandlingTests
{
    public class IgnoreAttributesTestModel : INullValueHandlingTestModel
    {
        [JsonProperty(PropertyName = "ID", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nameID", NullValueHandling = NullValueHandling.Ignore)]
        public string NameId { get; set; }

        [JsonProperty(PropertyName = "textValueID", NullValueHandling = NullValueHandling.Ignore)]
        public string TextValueId { get; set; }

        [JsonProperty(PropertyName = "valueID", NullValueHandling = NullValueHandling.Ignore)]
        public int ValueId { get; set; }

        [JsonProperty(PropertyName = "nullableValueID", NullValueHandling = NullValueHandling.Ignore)]
        public int? NullableValueId { get; set; }

        [JsonProperty(PropertyName = "dateValueID", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateValueId { get; set; }

        [JsonProperty(PropertyName = "nullableDateValueID", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? NullableDateValueId { get; set; }
    }
}
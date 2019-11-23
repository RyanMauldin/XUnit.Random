using System;
using Newtonsoft.Json;
using XUnit.Random.JsonNetTests.Models.Abstractions;

namespace XUnit.Random.JsonNetTests.Models.NullValueHandlingTests
{
    public class MixedAttributesTestModel : INullValueHandlingTestModel
    {
        [JsonProperty(PropertyName = "ID")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nameID", NullValueHandling = NullValueHandling.Include)]
        public string NameId { get; set; }

        [JsonProperty(PropertyName = "textValueID", NullValueHandling = NullValueHandling.Ignore)]
        public string TextValueId { get; set; }

        [JsonProperty(PropertyName = "valueID", NullValueHandling = NullValueHandling.Include)]
        public int ValueId { get; set; }

        [JsonProperty(PropertyName = "nullableValueID", NullValueHandling = NullValueHandling.Ignore)]
        public int? NullableValueId { get; set; }

        [JsonProperty(PropertyName = "dateValueID", NullValueHandling = NullValueHandling.Include)]
        public DateTime DateValueId { get; set; }

        [JsonProperty(PropertyName = "nullableDateValueID", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? NullableDateValueId { get; set; }
    }
}
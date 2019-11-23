using System;
using Newtonsoft.Json;
using XUnit.Random.JsonNetTests.Models.Abstractions;


namespace XUnit.Random.JsonNetTests.Models.NullValueHandlingTests
{
    public class MixedUpAttributesTestModel : INullValueHandlingTestModel
    {
        [JsonProperty(PropertyName = "ID")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "nameID", NullValueHandling = NullValueHandling.Ignore)]
        public string NameId { get; set; }

        [JsonProperty(PropertyName = "textValueID", NullValueHandling = NullValueHandling.Include)]
        public string TextValueId { get; set; }

        [JsonProperty(PropertyName = "valueID", NullValueHandling = NullValueHandling.Ignore)]
        public int ValueId { get; set; }

        [JsonProperty(PropertyName = "nullableValueID", NullValueHandling = NullValueHandling.Include)]
        public int? NullableValueId { get; set; }

        [JsonProperty(PropertyName = "dateValueID", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateValueId { get; set; }

        [JsonProperty(PropertyName = "nullableDateValueID", NullValueHandling = NullValueHandling.Include)]
        public DateTime? NullableDateValueId { get; set; }
    }
}
using Newtonsoft.Json;

namespace XUnit.Random.JsonNetTests.Models.NullValueHandlingTests
{
    public class BrokenTargetWithNullHandlingModel
    {
        private string _memberCode;
        private long? _memberId = BrokenConstants.DefaultMemberId;

        /// <summary>
        /// Specific constructor
        /// </summary>
        public BrokenTargetWithNullHandlingModel(string memberCode, long? memberId)
        {
            MemberCode = memberCode;
            MemberId = memberId;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MemberCode
        {
            get => _memberCode;
            set => _memberCode = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        [JsonProperty(PropertyName = "MemberID", NullValueHandling = NullValueHandling.Ignore)]
        public long? MemberId
        {
            get => _memberId;
            set => _memberId = value.HasValue && value.Value > 0 ? value : BrokenConstants.DefaultMemberId;
        }

        [JsonProperty(PropertyName = "helpID", NullValueHandling = NullValueHandling.Ignore)]
        public long HelpId { get; set; }
    }
}

using FluentAssertions;
using Xunit;
using XUnit.Random.Extensions;
using XUnit.Random.Extensions.Types;
using XUnit.Random.JsonNetTests.Models.NullValueHandlingTests;

namespace XUnit.Random.JsonNetTests
{
    public class NamingExtensionTests
    {
        private const string CamelNullableDateValueId = "nullableDateValueId";
        private const string LowerNullableDateValueId = "nullabledatevalueid";
        private const string PascalNullableDateValueId = "NullableDateValueId";
        private const string SnakeNullableDateValueId = "nullable_date_value_id";
        private const string TitleNullableDateValueId = "Nullabledatevalueid";
        private const string UpperNullableDateValueId = "NULLABLEDATEVALUEID";
        
        [Fact]
        [Trait("Category", "Mocked")]
        public void NamingExtensionTest()
        {
            const string nullableDateValueId = nameof(AttributesTestModel.NullableDateValueId);

            var camelNullableDateValueId = nullableDateValueId.ToCamelConvention();
            camelNullableDateValueId.Should().NotBeNullOrWhiteSpace();
            camelNullableDateValueId.Should().BeEquivalentTo(CamelNullableDateValueId);
            var camelNullableDateValueIdByConvention = nullableDateValueId.ToConvention(Convention.Camel);
            camelNullableDateValueIdByConvention.Should().NotBeNullOrWhiteSpace();
            camelNullableDateValueIdByConvention.Should().BeEquivalentTo(CamelNullableDateValueId);
            camelNullableDateValueId.Should().BeEquivalentTo(camelNullableDateValueIdByConvention);

            var lowerNullableDateValueId = nullableDateValueId.ToLowerConvention();
            lowerNullableDateValueId.Should().NotBeNullOrWhiteSpace();
            lowerNullableDateValueId.Should().BeEquivalentTo(LowerNullableDateValueId);
            var lowerNullableDateValueIdByConvention = nullableDateValueId.ToConvention(Convention.Lower);
            lowerNullableDateValueIdByConvention.Should().NotBeNullOrWhiteSpace();
            lowerNullableDateValueIdByConvention.Should().BeEquivalentTo(LowerNullableDateValueId);
            lowerNullableDateValueId.Should().BeEquivalentTo(lowerNullableDateValueIdByConvention);

            var pascalNullableDateValueId = nullableDateValueId.ToPascalConvention();
            pascalNullableDateValueId.Should().NotBeNullOrWhiteSpace();
            pascalNullableDateValueId.Should().BeEquivalentTo(PascalNullableDateValueId);
            var pascalNullableDateValueIdByConvention = nullableDateValueId.ToConvention(Convention.Pascal);
            pascalNullableDateValueIdByConvention.Should().NotBeNullOrWhiteSpace();
            pascalNullableDateValueIdByConvention.Should().BeEquivalentTo(PascalNullableDateValueId);
            pascalNullableDateValueId.Should().BeEquivalentTo(pascalNullableDateValueIdByConvention);

            var snakeNullableDateValueId = nullableDateValueId.ToSnakeConvention();
            snakeNullableDateValueId.Should().NotBeNullOrWhiteSpace();
            snakeNullableDateValueId.Should().BeEquivalentTo(SnakeNullableDateValueId);
            var snakeNullableDateValueIdByConvention = nullableDateValueId.ToConvention(Convention.Snake);
            snakeNullableDateValueIdByConvention.Should().NotBeNullOrWhiteSpace();
            snakeNullableDateValueIdByConvention.Should().BeEquivalentTo(SnakeNullableDateValueId);
            snakeNullableDateValueId.Should().BeEquivalentTo(snakeNullableDateValueIdByConvention);

            var titleNullableDateValueId = nullableDateValueId.ToTitleConvention();
            titleNullableDateValueId.Should().NotBeNullOrWhiteSpace();
            titleNullableDateValueId.Should().BeEquivalentTo(TitleNullableDateValueId);
            var titleNullableDateValueIdByConvention = nullableDateValueId.ToConvention(Convention.Title);
            titleNullableDateValueIdByConvention.Should().NotBeNullOrWhiteSpace();
            titleNullableDateValueIdByConvention.Should().BeEquivalentTo(TitleNullableDateValueId);
            titleNullableDateValueId.Should().BeEquivalentTo(titleNullableDateValueIdByConvention);

            var upperNullableDateValueId = nullableDateValueId.ToUpperConvention();
            upperNullableDateValueId.Should().NotBeNullOrWhiteSpace();
            upperNullableDateValueId.Should().BeEquivalentTo(UpperNullableDateValueId);
            var upperNullableDateValueIdByConvention = nullableDateValueId.ToConvention(Convention.Upper);
            upperNullableDateValueIdByConvention.Should().NotBeNullOrWhiteSpace();
            upperNullableDateValueIdByConvention.Should().BeEquivalentTo(UpperNullableDateValueId);
            upperNullableDateValueId.Should().BeEquivalentTo(upperNullableDateValueIdByConvention);
        }
    }
}

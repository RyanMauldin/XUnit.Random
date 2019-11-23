using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using XUnit.Random.Extensions;
using XUnit.Random.JsonNetTests.Models.Abstractions;
using XUnit.Random.JsonNetTests.Models.NullValueHandlingTests;

namespace XUnit.Random.JsonNetTests
{
    public class NullValueHandlingTests
    {
        [Fact]
        [Trait("Category", "Mocked")]
        public void AttributesTestModelSerializationTests()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.None };
            var model = new AttributesTestModel { NameId = "Omg", NullableDateValueId = null };
            TestSerializationOutput(model);
            TestSerializationOutput(model, settings);
            settings.Formatting = Formatting.Indented;
            TestSerializationOutput(model, settings);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void IgnoreAttributesTestModelSerializationTests()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.None };
            var model = new IgnoreAttributesTestModel { NameId = "Omg", NullableDateValueId = null };
            TestSerializationOutput(model);
            TestSerializationOutput(model, settings);
            settings.Formatting = Formatting.Indented;
            TestSerializationOutput(model, settings);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MixedAttributesTestModelSerializationTests()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.None };
            var model = new MixedAttributesTestModel { NameId = "Omg", NullableDateValueId = null };
            TestSerializationOutput(model);
            TestSerializationOutput(model, settings);
            settings.Formatting = Formatting.Indented;
            TestSerializationOutput(model, settings);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void MixedUpAttributesTestModelSerializationTests()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.None };
            var model = new MixedUpAttributesTestModel { NameId = "Omg", NullableDateValueId = null };
            TestSerializationOutput(model);
            TestSerializationOutput(model, settings);
            settings.Formatting = Formatting.Indented;
            TestSerializationOutput(model, settings);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void NoAttributesTestModelSerializationTests()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.None };
            var model = new NoAttributesTestModel { NameId = "Omg", NullableDateValueId = null };
            TestSerializationOutput(model);
            TestSerializationOutput(model, settings);
            settings.Formatting = Formatting.Indented;
            TestSerializationOutput(model, settings);
        }

        private static void TestSerializationOutput(INullValueHandlingTestModel model, JsonSerializerSettings settings = null)
        {
            var serializedModel = JsonConvert.SerializeObject(model);
            var serializedModelWithSettings = JsonConvert.SerializeObject(model, settings);
            var modelDefinition = model.ToJsonDefinition();
            var modelDefinitionWithSettings = model.ToJsonDefinition(settings);
            serializedModel.Should().NotBeNullOrWhiteSpace();
            serializedModelWithSettings.Should().NotBeNullOrWhiteSpace();
            modelDefinition.Should().NotBeNullOrWhiteSpace();
            modelDefinitionWithSettings.Should().NotBeNullOrWhiteSpace();
            serializedModel.Should().BeEquivalentTo(modelDefinition);
            serializedModelWithSettings.Should().BeEquivalentTo(modelDefinitionWithSettings);
        }
    }
}
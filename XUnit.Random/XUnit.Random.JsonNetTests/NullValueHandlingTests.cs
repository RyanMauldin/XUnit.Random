using System;
using System.Globalization;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using XUnit.Random.Extensions;
using XUnit.Random.Extensions.Types;
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
            TestSerializationOutput(model, null, Convention.Pascal);
            TestSerializationOutput(model, settings, Convention.Pascal);
            settings.Formatting = Formatting.Indented;
            TestSerializationOutput(model, settings, Convention.Pascal);
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void BrokenTargetModelSerializationTests()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            const string memberCode = "HelloMemberCode";
            long? memberId = 34234234;
            var model = new BrokenTargetModel(memberCode, memberId);
            var serializedModel = JsonConvert.SerializeObject(model, settings);
            var modelDefinition = model.ToJsonDefinition(settings, Convention.Pascal);
            serializedModel.Should().NotBeNullOrWhiteSpace();
            modelDefinition.Should().NotBeNullOrWhiteSpace();
            serializedModel.Should().BeEquivalentTo(modelDefinition);
            StringComparer.Ordinal.Equals(serializedModel, modelDefinition).Should().BeTrue();
        }

        [Fact]
        [Trait("Category", "Mocked")]
        public void BrokenTargetWithNullHandlingModelSerializationTests()
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            const string memberCode = "HelloMemberCode";
            long? memberId = 34234234;
            var model = new BrokenTargetWithNullHandlingModel(memberCode, memberId);
            var serializedModel = JsonConvert.SerializeObject(model, settings);
            var modelDefinition = model.ToJsonDefinition(settings, Convention.Pascal);
            serializedModel.Should().NotBeNullOrWhiteSpace();
            modelDefinition.Should().NotBeNullOrWhiteSpace();
            serializedModel.Should().BeEquivalentTo(modelDefinition);
            StringComparer.Ordinal.Equals(serializedModel, modelDefinition).Should().BeTrue();
        }

        private static void TestSerializationOutput(INullValueHandlingTestModel model, JsonSerializerSettings settings = null,
            Convention convention = Convention.Camel, CultureInfo cultureInfo = null)
        {
            var serializedModel = JsonConvert.SerializeObject(model);
            var serializedModelWithSettings = JsonConvert.SerializeObject(model, settings);
            var modelDefinition = model.ToJsonDefinition(null, convention, cultureInfo);
            var modelDefinitionWithSettings = model.ToJsonDefinition(settings, convention, cultureInfo);
            serializedModel.Should().NotBeNullOrWhiteSpace();
            serializedModelWithSettings.Should().NotBeNullOrWhiteSpace();
            modelDefinition.Should().NotBeNullOrWhiteSpace();
            modelDefinitionWithSettings.Should().NotBeNullOrWhiteSpace();
            serializedModel.Should().BeEquivalentTo(modelDefinition);
            StringComparer.Ordinal.Equals(serializedModel, modelDefinition).Should().BeTrue();
            serializedModelWithSettings.Should().BeEquivalentTo(modelDefinitionWithSettings);
            StringComparer.Ordinal.Equals(serializedModelWithSettings, modelDefinitionWithSettings).Should().BeTrue();
        }
    }
}
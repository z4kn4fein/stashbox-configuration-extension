using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stashbox;
using Stashbox.Configuration;
using Stashbox.Configuration.Tests;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace stashbox.configuration.tests
{
    [TestClass]
    public class StashboxConfigTests
    {
        private IStashboxContainer stashboxContainer;

        [TestInitialize]
        public void Init()
        {
            this.stashboxContainer = new StashboxContainer();
        }

        [TestMethod]
        public void WireUpConfigurationUnityExtensionTests_WithRegisteredSettingsManager()
        {
            this.stashboxContainer.RegisterExtension(new AutoConfigurationExtension(settingReader: key => ConfigurationManager.AppSettings[key]));
            this.stashboxContainer.RegisterType<FakeClass1>();

            var fake = this.stashboxContainer.Resolve<FakeClass1>();

            Assert.AreEqual("fakeValue1", fake.FakeValue1);
            Assert.AreEqual("fakeValue2", fake.FakeValue2);
            Assert.AreEqual("fakeValue3", fake.FakeValue3);
        }

        [TestMethod]
        public void WireUpConfigurationUnityExtensionTests_WithPrefixAttribute()
        {
            this.stashboxContainer.RegisterExtension(new AutoConfigurationExtension());
            this.stashboxContainer.RegisterType<FakeClass2>();

            var fake = this.stashboxContainer.Resolve<FakeClass2>();

            Assert.AreEqual("fakeValue4", fake.FakeValue4);
            Assert.AreEqual("fakeValue5", fake.FakeValue5);
        }

        [TestMethod]
        public void WireUpConfigurationUnityExtensionTests_WithPrefix_WithoutAttribute()
        {
            this.stashboxContainer.RegisterExtension(new AutoConfigurationExtension());
            this.stashboxContainer.RegisterType<FakeClass3>();

            var fake = this.stashboxContainer.Resolve<FakeClass3>();

            Assert.AreEqual("fakeValue6", fake.FakeValue6);
            Assert.AreEqual("fakeValue7", fake.FakeValue7);
        }

        [TestMethod]
        public void WireUpConfigurationUnityExtensionTests_WithPrefixAttribute_WithCustomSeparator()
        {
            this.stashboxContainer.RegisterExtension(new AutoConfigurationExtension(separator: "-"));
            this.stashboxContainer.RegisterType<FakeClass4>();

            var fake = this.stashboxContainer.Resolve<FakeClass4>();

            Assert.AreEqual("fakeValue8", fake.FakeValue8);
            Assert.AreEqual("fakeValue9", fake.FakeValue9);
        }

        [TestMethod]
        public void WireUpConfigurationUnityExtensionTests_WithCustomTypes()
        {
            this.stashboxContainer.RegisterExtension(new AutoConfigurationExtension());
            this.stashboxContainer.RegisterType<FakeClass5>();

            var fake = this.stashboxContainer.Resolve<FakeClass5>();

            Assert.AreEqual(true, fake.FakeValue10);
            Assert.AreEqual(34, fake.FakeValue11);
            Assert.AreEqual(54.425, fake.FakeValue12);
            Assert.AreEqual(TimeSpan.FromSeconds(84), fake.FakeValue13);
            Assert.AreEqual(new DateTime(2015, 4, 3), fake.FakeValue14);
            Assert.AreEqual("fakeConnectionString", fake.FakeConnectionString);
        }

        [TestMethod]
        public void WireUpConfigurationUnityExtensionTests_WithCustomConverter()
        {
            this.stashboxContainer.RegisterExtension(new AutoConfigurationExtension());
            this.stashboxContainer.RegisterType<FakeClass6>();

            var fake = this.stashboxContainer.Resolve<FakeClass6>();

            CollectionAssert.AllItemsAreInstancesOfType(fake.FakeValue15.ToList(), typeof(string));
            CollectionAssert.AreEquivalent(new List<string> { "test1", "test2" }, fake.FakeValue15.ToList());
        }
    }
}

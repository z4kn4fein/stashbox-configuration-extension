using System;
using System.Collections.Generic;
using Stashbox.Configuration.Attributes;

namespace Stashbox.Configuration.Tests
{
    public class FakeClass1
    {
        [Setting("fakeKey1")]
        public string FakeValue1 { get; set; }

        [Setting("fakeKey2")]
        public string FakeValue2 { get; set; }

        [Setting("fakeKey3")]
        public string FakeValue3 { get; set; }
    }

    [SettingPrefix("fakePrefix1")]
    public class FakeClass2
    {
        [Setting("fakeKey4")]
        public string FakeValue4 { get; set; }

        [Setting("fakeKey5")]
        public string FakeValue5 { get; set; }
    }

    public class FakeClass3
    {
        [Setting("fakePrefix2:fakeKey6")]
        public string FakeValue6 { get; set; }

        [Setting("fakePrefix2:fakeKey7")]
        public string FakeValue7 { get; set; }
    }

    [SettingPrefix("fakePrefix3")]
    public class FakeClass4
    {
        [Setting("fakeKey8")]
        public string FakeValue8 { get; set; }

        [Setting("fakeKey9")]
        public string FakeValue9 { get; set; }
    }

    public class FakeClass5
    {
        [Setting("fakeKey10")]
        public bool FakeValue10 { get; set; }

        [Setting("fakeKey11")]
        public int FakeValue11 { get; set; }

        [Setting("fakeKey12")]
        public double FakeValue12 { get; set; }

        [Setting("fakeKey13")]
        public TimeSpan FakeValue13 { get; set; }

        [Setting("fakeKey14")]
        public DateTime FakeValue14 { get; set; }

        [ConnectionString("fakeName")]
        public string FakeConnectionString { get; set; }
    }

    public class FakeClass6
    {
        [Setting("FakeValue15", typeof(FakeValue15Converter))]
        public IEnumerable<string> FakeValue15 { get; set; }
    }

    public class FakeValue15Converter
    {
        public IEnumerable<string> Convert(string stringValue)
        {
            return new List<string> { "test1", "test2" };
        }
    }
}

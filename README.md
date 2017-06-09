# stashbox-configuration-extension [![Build status](https://ci.appveyor.com/api/projects/status/4uopj58p4dw4wh4p/branch/master?svg=true)](https://ci.appveyor.com/project/pcsajtai/stashbox-configuration-extension/branch/master) [![NuGet Version](https://buildstats.info/nuget/Stashbox.Configuration)](https://www.nuget.org/packages/Stashbox.Configuration/)
Auto application configuration parser extension for [Stashbox](https://github.com/z4kn4fein/stashbox)

## Usage
Example settings file:
```xml
<configuration>
    <appSettings>
        <add key="foo:foo1" value="true" />
        <add key="foo:foo2" value="34" />
        <add key="foo:foo3" value="54.425" />
        <add key="foo4" value="00:01:24" />
        <add key="foo5" value="2015-04-03" />
    </appSettings>
    <connectionStrings>
        <add name="fooConnection" connectionString="connection-string"/>
    </connectionStrings>
</configuration>
```
The object models:
```c#
[SettingPrefix("foo")]
public class Foo
{
    [Setting("foo1")]
    public bool Foo1 { get; set; }

    [Setting("foo2")]
    public int Foo2 { get; set; }

    [Setting("foo3")]
    public double Foo3 { get; set; }
}

public class Bar
{
    [Setting("foo4")]
    public TimeSpan Foo4 { get; set; }

    [Setting("foo5")]
    public DateTime Foo5 { get; set; }
    
    [ConnectionString("fooConnection")]
    public string FooConnectionString { get; set; }
}
```
Wire things up:
```c#
var stashboxContainer = new StashboxContainer(config => config.WithUnknownTypeResolution());

stashboxContainer.RegisterExtension(new AutoConfigurationExtension());

var foo = stashboxContainer.Resolve<Foo>();
var bar = stashboxContainer.Resolve<Bar>();
```
## Additional options
The extension uses the `ConfigurationManager` for reading from the config file, but if you'd like you can specify custom configuration reader expressions e.g. for Azure Cloud Services:
```c#
stashboxContainer.RegisterExtension(new AutoConfigurationExtension(settingReader: key => CloudConfiguration.GetSetting(key)));
```
You can also replace the default `:` separator which splits the setting keys from the prefix value:
```c#
stashboxContainer.RegisterExtension(new AutoConfigurationExtension(separator: "-"));
```
You can also specify a custom converter to parse your setting e.g. from Json:
```c#
public class FooConverter
{
    public FooObject Convert(string stringValue)
    {
        return JsonConvert.DeserializeObject<FooObject>(stringValue);
    }
}

public class Foo
{
    [Setting("foo", typeof(FooConverter))]
    public FooObject FooObj { get; set; }
}
```

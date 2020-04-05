# webBeta NSerializer

[![Build Status](https://travis-ci.org/webbeta/NSerializer.svg?branch=master)](https://travis-ci.org/webbeta/NSerializer)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/c9db06803de94cc6aaea9e2fdcbd494e)](https://www.codacy.com/gh/webbeta/NSerializer?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=webbeta/NSerializer&amp;utm_campaign=Badge_Grade)
[![GitHub license](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![NuGet](https://buildstats.info/nuget/webBeta.NSerializer)](http://www.nuget.org/packages/webBeta.NSerializer)

* [What is webBeta NSerializer?](#what-is-webbeta-nserializer?)
* [Quick examples](#quick-examples)
* [License](#license)

## What is webBeta NSerializer?

YML based serializer for .Net Core. Based on JMS serializer https://jmsyst.com/libs/serializer/master/reference/yml_reference

It is a fork of our [YML based serializer for Java](https://github.com/webbeta/Serializer).

Steps to create a NSerializer instance.

1. Create a cache provider.

```csharp
public class MyCache : ICache
{
    public string Get(string key)
    {
        return null;
    }

    public void Set(string key, string content)
    {
    }

    public void Remove(string key)
    {
    }
}
```

2. Create a configuration provider.

```csharp
public class MyConfigurationProvider : IConfigurationProvider
{
    private readonly Dictionary<string, object> _conf;

    public MyConfigurationProvider(Dictionary<string, object> conf)
    {
        _conf = conf;
    }

    public bool GetBoolean(string key, bool defaultValue)
    {
        return !_conf.ContainsKey(key) ? defaultValue : bool.Parse(_conf[key].ToString());
    }

    public string GetString(string key, string defaultValue)
    {
        return !_conf.ContainsKey(key) ? defaultValue : _conf[key].ToString();
    }
}
```

3. Create an environment provider.

```csharp
public class MyEnvironment : IEnvironment
{    
    public bool IsProd()
    {
        return true;
    }
}
```

4. Then build the NSerializer.

```csharp
NSerializer serializer = NSerializerBuilder.Build()
        .WithCache(cache)
        .WithConfigurationProvider(configurationProvider)
        .WithEnvironment(environment)
        .Get();
```

5. Optionally, add a logger instance:

```csharp
public class MyLogger : ILogger
{    
    public Level GetLevel()
    {
        return Level.ERROR;
    }

    public void SetLevel(Level level) 
    {
    }

    public void Error(string message)
    {
    }
}

serializer.SetLogger(logger);
```

## Quick examples

Class Foo:

```csharp
namespace Foo.Bar
{
    public class Foo {
        
        private string bar = "foobar";

        public string AutoPropertyString { get; set; } = "Foo Autoproperty!";
        
        public string GetBar() {
            return bar;
        }
        
        public long GetCalc() {
            return 2 + 2;
        }
        
    }
}

```

Yml file (named ```Foo.Bar.Foo.yml```):

```yaml
Foo.Bar.Foo:
  virtual_properties:
    GetCalc:
      serialized_name: calc
      groups: [barGroup]
  properties:
    bar:
      groups: [fooGroup]
    AutoPropertyString:
      groups: [autoGroup]

```

Usage:

```csharp
serializer.Serialize(fooInstance, "fooGroup", "barGroup", "autoGroup");

// it will return
// {"bar": "foobar", "calc": 4, "autoPropertyString": "Foo Autoproperty!"}
```

## License

[MIT](LICENSE)

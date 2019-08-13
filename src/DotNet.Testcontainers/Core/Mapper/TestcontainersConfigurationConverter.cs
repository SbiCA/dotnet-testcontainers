using System.Collections.Generic;
using System.IO;
using System.Linq;
using Docker.DotNet.Models;
using DotNet.Testcontainers.Core.Models;

namespace DotNet.Testcontainers.Core.Mapper
{
  internal class TestcontainersConfigurationConverter
  {
    public IList<string> Entrypoint
    {
      get
      {
        return new ToList().Convert(this.Config.Container.Entrypoint);
      }
    }

    public IList<string> Command
    {
      get
      {
        return new ToList().Convert(this.Config.Container.Command);
      }
    }

    public IList<string> Environments
    {
      get
      {
        return new ToMappedList().Convert(this.Config.Container.Environments);
      }
    }

    public IDictionary<string, string> Labels
    {
      get
      {
        return new ToDictionary().Convert(this.Config.Container.Labels);
      }
    }

    public IDictionary<string, EmptyStruct> ExposedPorts
    {
      get
      {
        return new ToExposedPorts().Convert(this.Config.Container.ExposedPorts);
      }
    }

    public IDictionary<string, IList<PortBinding>> PortBindings
    {
      get
      {
        return new ToPortBindings().Convert(this.Config.Host.PortBindings);
      }
    }

    public IList<Mount> Mounts
    {
      get
      {
        return new ToMounts().Convert(this.Config.Host.Mounts);
      }
    }

    private TestcontainersConfiguration Config { get; }

    public TestcontainersConfigurationConverter(TestcontainersConfiguration config)
    {
      this.Config = config;
    }

    private class ToList : CollectionConverter<IList<string>>
    {
      public override IList<string> Convert(IReadOnlyCollection<string> source)
      {
        return source?.ToList();
      }
    }

    private class ToDictionary : DictionaryConverter<IDictionary<string, string>>
    {
      public override IDictionary<string, string> Convert(IReadOnlyDictionary<string, string> source)
      {
        return source?.ToDictionary(item => item.Key, item => item.Value);
      }
    }

    private class ToMappedList : DictionaryConverter<IList<string>>
    {
      public override IList<string> Convert(IReadOnlyDictionary<string, string> source)
      {
        return source?.Select(item => $"{item.Key}={item.Value}").ToList();
      }
    }

    private class ToExposedPorts : DictionaryConverter<IDictionary<string, EmptyStruct>>
    {
      public ToExposedPorts() : base("ExposedPorts")
      {
      }

      public override IDictionary<string, EmptyStruct> Convert(IReadOnlyDictionary<string, string> source)
      {
        return source?.ToDictionary(exposedPort => $"{exposedPort.Key}/tcp", exposedPort => default(EmptyStruct));
      }
    }

    private class ToPortBindings : DictionaryConverter<IDictionary<string, IList<PortBinding>>>
    {
      public ToPortBindings() : base("PortBindings")
      {
      }

      public override IDictionary<string, IList<PortBinding>> Convert(IReadOnlyDictionary<string, string> source)
      {
        return source?.ToDictionary(binding => $"{binding.Key}/tcp", binding =>
        {
          // TODO shouldn't this be the value?
          var portBinding = string.IsNullOrWhiteSpace(binding.Value)
            ? new PortBinding()
            : new PortBinding {HostPort = binding.Key};

          return new List<PortBinding> {portBinding} as IList<PortBinding>;
        });
      }
    }

    private class ToMounts : DictionaryConverter<IList<Mount>>
    {
      public ToMounts() : base("Mounts")
      {
      }

      public override IList<Mount> Convert(IReadOnlyDictionary<string, string> source)
      {
        return source?.Select(mount => new Mount {Source = Path.GetFullPath(mount.Key), Target = mount.Value, Type = "bind"}).ToList();
      }
    }
  }
}

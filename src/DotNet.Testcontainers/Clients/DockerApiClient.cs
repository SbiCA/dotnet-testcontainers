namespace DotNet.Testcontainers.Clients
{
  using System;
  using System.Runtime.InteropServices;
  using Docker.DotNet;
  using DotNet.Testcontainers.Core;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;

  internal class DockerApiClient
  {
#pragma warning disable S1075

    protected static readonly Uri Endpoint = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new Uri("npipe://./pipe/docker_engine") : new Uri("unix:/var/run/docker.sock");

#pragma warning restore S1075

    protected static readonly DockerClient Docker = new DockerClientConfiguration(Endpoint).CreateClient();

    static DockerApiClient()
    {
      new HostBuilder()
        .ConfigureServices((hostContext, services) => services.Configure<HostOptions>(option => option.ShutdownTimeout = TimeSpan.FromSeconds(30)))
        .ConfigureServices((hostContext, services) => services.Configure<ConsoleLifetimeOptions>(option => option.SuppressStatusMessages = true))
        .ConfigureServices((hostContext, services) => services.AddHostedService<PurgeOrphanedTestcontainers>())
        .Build()
        .RunAsync();
    }

    protected DockerApiClient()
    {
    }
  }
}

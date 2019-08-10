namespace DotNet.Testcontainers.Core
{
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;
  using System.Linq;
  using DotNet.Testcontainers.Core.Containers;
  using Microsoft.Extensions.Hosting;
  using System;

  internal class PurgeOrphanedTestcontainers : IHostedService
  {
    private static readonly CancellationTokenSource Shutdown = new CancellationTokenSource();

    private static readonly ICollection<TestcontainersContainer> Containers = new List<TestcontainersContainer>();

    public static CancellationToken ShutdownToken { get; } = Shutdown.Token;

    public Task StartAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      Shutdown.Cancel();

      Containers.ToList().ForEach(container =>
      {
        container.Dispose();
      });

      Console.WriteLine($"PURGED: {Containers.Count} left.");

      return Task.CompletedTask;
    }

    public static void RegisterTestcontainer(TestcontainersContainer container)
    {
      if (container != null)
      {
        lock (Containers)
        {
          Containers.Add(container);
        }
      }
    }

    public static void UnregisterTestcontainer(TestcontainersContainer container)
    {
      if (container != null)
      {
        lock (Containers)
        {
          Containers.Remove(container);
        }
      }
    }
  }
}

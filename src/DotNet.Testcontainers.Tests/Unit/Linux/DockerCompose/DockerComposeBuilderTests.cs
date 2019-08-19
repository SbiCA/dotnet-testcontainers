using DotNet.Testcontainers.Core.Builder;

namespace DotNet.Testcontainers.Tests.Unit.Linux.DockerCompose
{
  public class DockerComposeBuilderTests
  {
    public async Task CanCreateServices()
    {
      var composeBuilder = new DockerComposeBuilder()
        .WithCompose();

      using (var compose = composeBuilder.Build())
      {
         // TODO ping
      }
    }
  }
}

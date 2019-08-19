using System;
using System.Collections.Generic;

namespace DotNet.Testcontainers.Core.Builder
{
  public class DockerComposeBuilder
  {
    public List<string> ComposeFiles { get; } = new List<string>();

    public DockerComposeBuilder WithCompose(string fileName)
    {
      this.ComposeFiles.Add(fileName);
      return this;
    }

    public ComposeServices Build()
    {
      // https://github.com/testcontainers/testcontainers-java/blob/4df20c2d1d090d8f2d3129ec24b0988faecbe896/core/src/main/java/org/testcontainers/containers/DockerComposeContainer.java#L642
      // TODO call docker-compose -f ... -f ... -d
      
    }
  }
}


// TODO do we need another abstraction?
public class ComposeServices : IDisposable
{
  public void Dispose()
  {
  }
}

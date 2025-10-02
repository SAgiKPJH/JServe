
namespace Container.Docker;

public class DockerContainer : IContainer
{
    Task<bool> IContainer.DownAsync()
    {
        throw new NotImplementedException();
    }

    Task<bool> IContainer.ResumeAsync()
    {
        throw new NotImplementedException();
    }

    Task<bool> IContainer.StopAsync()
    {
        throw new NotImplementedException();
    }

    Task<bool> IContainer.UpAsync()
    {
        throw new NotImplementedException();
    }
}
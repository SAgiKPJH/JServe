namespace Container;

internal interface IContainer
{
    Task<bool> UpAsync();
    Task<bool> DownAsync();
    Task<bool> StopAsync();
    Task<bool> ResumeAsync();
}
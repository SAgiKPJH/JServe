namespace Container;

public interface IContainerService
{
    Task<IEnumerable<string>> GetContainerList();
    Task<bool> Up(string name, string image);
    Task<bool> Down(string name);
    Task<bool> Stop(string name);
    Task<bool> Resume(string name);

    Task<IEnumerable<string>> GetImageList();
    Task<bool> AddImage(string image);
    Task<bool> AddImageLocal(string imagePath);
    Task<bool> RemoveImage(string image);
    Task<bool> RenameImage(string oldName, string newName);
}
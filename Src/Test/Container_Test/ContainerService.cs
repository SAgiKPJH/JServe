using Container;
using Moq;

namespace Container_Test;

public class ContainerService
{
    #region Container Tests
    [Fact]
    public async Task GetEmptyCotainerList()
    {
        // Arange
        IContainerService containerService = Mock.Of<IContainerService>(service => service.GetContainerList() == Task.FromResult(Enumerable.Empty<string>()));

        // Act
        var result = await containerService.GetContainerList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Up()
    {
        // Arange
        IContainerService containerService = Mock.Of<IContainerService>
        (
            service => service.GetContainerList() == Task.FromResult(new List<string> { "nginx" } as IEnumerable<string>)
                    && service.AddImage(It.IsAny<string>()) == Task.FromResult(true)
                    && service.Up(It.IsAny<string>(), It.IsAny<string>()) == Task.FromResult(true)
        );
        await containerService.AddImage("nginx:latest");

        // Act
        var success = await containerService.Up("nginx", "nginx:latest");

        // Assert
        var result = await containerService.GetContainerList();
        Assert.True(success);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(result.Count(), 1);
    }

    [Fact]
    public async Task Down()
    {
        // Arange
        IContainerService containerService = Mock.Of<IContainerService>
        (
            service => service.GetContainerList() == Task.FromResult(Enumerable.Empty<string>())
                    && service.AddImage(It.IsAny<string>()) == Task.FromResult(true)
                    && service.Up(It.IsAny<string>(), It.IsAny<string>()) == Task.FromResult(true)
                    && service.Down(It.IsAny<string>()) == Task.FromResult(true)
        );
        await containerService.AddImage("nginx:latest");
        await containerService.Up("nginx", "nginx:latest");

        // Act
        var success = await containerService.Down("nginx");

        // Assert
        var result = await containerService.GetContainerList();
        Assert.True(success);
        Assert.Empty(result);
        Assert.Equal(result.Count(), 0);
    }
    #endregion

    #region Image Tests
    [Fact]
    public async Task GetEmptyImageList()
    {
        // Arange
        IContainerService containerService = Mock.Of<IContainerService>(service => service.GetImageList() == Task.FromResult(Enumerable.Empty<string>()));

        // Act
        var result = await containerService.GetImageList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AddImage()
    {
        // Arange
        IContainerService containerService = Mock.Of<IContainerService>
        (
            service => service.GetImageList() == Task.FromResult( new List<string> { "nginx:latest" } as IEnumerable<string>)
                    && service.AddImage(It.IsAny<string>()) == Task.FromResult(true)
        );

        // Act
        var success = await containerService.AddImage("nginx:latest");

        // Assert
        var result = await containerService.GetImageList();
        Assert.True(success);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(result.Count(), 1);
    }

    [Fact]
    public async Task RemoveImage()
    {
        // Arange
        IContainerService containerService = Mock.Of<IContainerService>
        (
            service => service.GetImageList() == Task.FromResult(Enumerable.Empty<string>())
                    && service.AddImage(It.IsAny<string>()) == Task.FromResult(true)
                    && service.RemoveImage(It.IsAny<string>()) == Task.FromResult(true)
        );
        await containerService.AddImage("nginx:latest");

        // Act
        var success = await containerService.RemoveImage("nginx:latest");

        // Assert
        var result = await containerService.GetImageList();
        Assert.True(success);
        Assert.Empty(result);
        Assert.Equal(result.Count(), 0);
    }

    [Fact]
    public async Task RenameImage()
    {
        // Arange
        IContainerService containerService = Mock.Of<IContainerService>
        (
            service => service.GetImageList() == Task.FromResult( new List<string> { "nginx:latest_test" } as IEnumerable<string>)
                    && service.AddImage(It.IsAny<string>()) == Task.FromResult(true)
                    && service.RenameImage(It.IsAny<string>(), It.IsAny<string>()) == Task.FromResult(true)
        );
        await containerService.AddImage("nginx:latest");

        // Act
        var success = await containerService.RenameImage("nginx:latest", "nginx:latest" + "_test");

        // Assert
        var result = await containerService.GetImageList();
        Assert.True(success);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(result.Count(), 1);
    }
    #endregion
}
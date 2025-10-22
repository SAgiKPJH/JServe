using ProcessService.VolueObject;

namespace ProcessServiceTest.VoluObject;
public class NameTests
{
    [Fact]
    public void Create_WithValidInput_ShouldCreateName()
    {
        // Arrange & Act
        var name = Name.Create("test");

        // Assert
        Assert.Equal("test", name);
        Assert.Equal("test", name.ToString());
    }

    [Fact]
    public void Create_WithUpperCase_ShouldConvertToLowerCase()
    {
        // Arrange & Act
        var name = Name.Create("TEST");

        // Assert
        Assert.Equal("test", name);
    }

    [Fact]
    public void Create_WithAllowedSpecialChars_ShouldCreateName()
    {
        // Arrange & Act
        var names = new[]
        {
            Name.Create("test-name"),
            Name.Create("test_name"),
            Name.Create("test.name"),
            Name.Create("test:name"),
            Name.Create("test/name")
        };

        // Assert
        Assert.Equal("test-name", names[0]);
        Assert.Equal("test_name", names[1]);
        Assert.Equal("test.name", names[2]);
        Assert.Equal("test:name", names[3]);
        Assert.Equal("test/name", names[4]);
    }

    [Fact]
    public void Create_WithNullInput_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => Name.Create(null));
    }

    [Fact]
    public void Create_WithEmptyInput_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => Name.Create(""));
    }

    [Fact]
    public void Create_WithWhitespaceOnly_ShouldThrowArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => Name.Create("   "));
    }

    [Fact]
    public void Create_WithLengthGreaterThan10_ShouldThrowArgumentException()
    {
        // Arrange
        var longInput = "verylongname";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => Name.Create(longInput));
    }

    [Fact]
    public void Create_WithInvalidSpecialChars_ShouldThrowArgumentException()
    {
        // Arrange
        var invalidInputs = new[] { "test@name", "test#name", "test$name", "test%name", "test name" };

        // Act & Assert
        foreach (var input in invalidInputs)
            Assert.Throws<ArgumentException>(() => Name.Create(input));
    }

    [Fact]
    public void Create_WithMaximumLength_ShouldCreateName()
    {
        // Arrange
        var maxLengthInput = "1234567890"; // Exactly 10 characters

        // Act
        var name = Name.Create(maxLengthInput);

        // Assert
        Assert.Equal(maxLengthInput, name.Value);
    }

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var name1 = Name.Create("test");
        var name2 = Name.Create("test");

        // Act & Assert
        Assert.True(name1.Equals(name2));
        Assert.True(name1 == name2);
        Assert.False(name1 != name2);
    }

    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var name1 = Name.Create("test1");
        var name2 = Name.Create("test2");

        // Act & Assert
        Assert.False(name1.Equals(name2));
        Assert.False(name1 == name2);
        Assert.True(name1 != name2);
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var name = Name.Create("test");

        // Act & Assert
        Assert.False(name.Equals(null));
        Assert.False(name is null);
        Assert.True(name is not null);
        Assert.False(null == name);
        Assert.True(null != name);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
    {
        // Arrange
        var name1 = Name.Create("test");
        var name2 = Name.Create("test");

        // Act & Assert
        Assert.Equal(name1.GetHashCode(), name2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCode()
    {
        // Arrange
        var name1 = Name.Create("test1");
        var name2 = Name.Create("test2");

        // Act & Assert
        Assert.NotEqual(name1.GetHashCode(), name2.GetHashCode());
    }
}

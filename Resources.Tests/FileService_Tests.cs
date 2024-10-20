using Moq;
using Resources.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resources.Interfaces;

namespace Resources.Tests;

public class FileService_Tests
{
    private readonly Mock<IFileService> _fileServiceMock = new();

    private string testText = "Testing";
    [Fact]

    public void SaveToFile__Should__SaveToFileIfStringIsntEmpty__ReturnTrue()
    {
        //Arrange

        _fileServiceMock.Setup(FileService => FileService.SaveToFile(testText)).Returns(true);
        var fileService = _fileServiceMock.Object;

        //Act

        var result = fileService.SaveToFile(testText);

        //Assert

        Assert.True(result);
    }

    [Fact]

    public void GetFromFile__Should__GetFromTextFile__ReturnString()
    {
        //Arrange

        _fileServiceMock.Setup(FileService => FileService.GetFromFile()).Returns(testText);
        var fileService = _fileServiceMock.Object;

        //Act

        var result = fileService.GetFromFile();

        //Assert

        Assert.Equal(testText, result);
    }
}

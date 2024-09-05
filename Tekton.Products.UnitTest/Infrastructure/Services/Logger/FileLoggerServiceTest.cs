using System;
using System.IO;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using Tekton.Products.Infrastructure.Services;

public class FileLoggerServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly string _testFilePath;
    private readonly FileLoggerService _loggerService;

    public FileLoggerServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();

        // Use a temporary file path for testing
        _testFilePath = Path.Combine(Path.GetTempPath(), "TestLogFile.txt");
        _mockConfiguration
            .Setup(config => config["Logging:LogFilePath"])
            .Returns(_testFilePath);

        _loggerService = new FileLoggerService(_mockConfiguration.Object);
    }

    [Fact]
    public void Log_ShouldWriteMessageToFile()
    {
        // Arrange
        var message = "This is a test log message.";
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }

        // Act
        _loggerService.Log(message);

        // Assert
        Assert.True(File.Exists(_testFilePath), "Log file was not created.");
        var logEntries = File.ReadAllLines(_testFilePath);
        Assert.NotEmpty(logEntries);
        Assert.Contains(message, logEntries[0]);
    }

    [Fact]
    public void Log_ShouldNotThrowException()
    {
        // Arrange
        var message = "This is a test log message.";

        // Act & Assert
        try
        {
            _loggerService.Log(message);
        }
        catch (Exception ex)
        {
            Assert.Fail($"Logging threw an exception: {ex.Message}");
        }
    }

    [Fact]
    public void Log_ShouldHandleExceptionsGracefully()
    {
        // Arrange
        _mockConfiguration
            .Setup(config => config["Logging:LogFilePath"])
            .Returns(Path.Combine(Path.GetTempPath(), "InvalidPath", "TestLogFile.txt"));

        var loggerService = new FileLoggerService(_mockConfiguration.Object);

        // Act & Assert
        try
        {
            loggerService.Log("This should handle the exception.");
        }
        catch (Exception ex)
        {
            Assert.Fail($"Logging threw an exception: {ex.Message}");
        }
    }

    // Clean up test file
    ~FileLoggerServiceTests()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }
}

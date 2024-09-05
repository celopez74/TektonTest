using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Tekton.Products.Infrastructure.Services
{
    public class FileLoggerService : ILoggerService
    {
        private readonly string _filePath;

        public FileLoggerService(IConfiguration configuration)
        {
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName + "//ApiLogs";
            _filePath = Path.Combine(projectRoot, configuration["Logging:LogFilePath"] ?? "ApiLog.txt");
        }

        public void Log(string message)
        {
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            try
            {
                using (var writer = new StreamWriter(_filePath, true))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones de logging si es necesario
                // Por ejemplo, podrías registrar en otro lugar o ignorar
            }
        }
    }
}
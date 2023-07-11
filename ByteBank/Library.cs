using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace ByteBank
{
  public class Library
  {
    public async void GenerateLogs()
    {
      string logsFolderPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Logs");

      // Verifica se a pasta "Logs" existe, caso contrário, cria-a
      if (!Directory.Exists(logsFolderPath))
      {
        Directory.CreateDirectory(logsFolderPath);
      }

      string logFilePath = Path.Combine(logsFolderPath, "log.txt");

      using (StreamWriter writer = File.CreateText(logFilePath))
      {
        for (int i = 0; i < 10; i++)
        {
          string logLine = $"{DateTime.Now.ToString()} - Click event #{i + 1}";
          writer.WriteLine(logLine);
        }
      }
    }

    public async void DeleteLogs()
    {
    }
  }
}

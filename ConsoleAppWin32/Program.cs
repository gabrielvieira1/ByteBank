using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWin32
{
  internal class Program
  {
    [DllImport("User32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBoxW(
      IntPtr hWnd,
      [param: MarshalAs(UnmanagedType.LPWStr)] string lpText,
      [param: MarshalAs(UnmanagedType.LPWStr)] string lpCaption,
      UInt32 utype);

    [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern bool RemoveDirectoryW(string lpPathName);

    // Definições da API Win32

    private const int FO_DELETE = 3;
    private const int FOF_SILENT = 0x0004;
    private const int FOF_NOCONFIRMATION = 0x0010;
    private const int FOF_NOERRORUI = 0x0400;
    private const int FOF_WANTNUKEWARNING = 0x4000;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFILEOPSTRUCT
    {
      public IntPtr hwnd;
      public int wFunc;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string pFrom;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string pTo;
      public short fFlags;
      public bool fAnyOperationsAborted;
      public IntPtr hNameMappings;
      [MarshalAs(UnmanagedType.LPWStr)]
      public string lpszProgressTitle;
    }

    [DllImport("shell32.dll", CharSet = CharSet.Auto)]
    private static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

    static void Main(string[] args)
    {
      // int result = MessageBoxW(IntPtr.Zero, "Hello World", "This is window title", 0);

      //string folderPath = message["FolderPath"] as string;
      string folderPath = "C:\\Users\\gabri\\AppData\\Local\\Packages\\c6ee4a46-c5c7-42ae-bec1-6449e0db18a7_03w1eqkrn0fpt\\LocalState\\Logs";


      if (!string.IsNullOrEmpty(folderPath))
      {
        if (DeleteFolder(folderPath))
        {
          MessageBoxW(IntPtr.Zero, "Folder deleted successfully.", "This is window title", 0);
        }
        else
        {
          MessageBoxW(IntPtr.Zero, "Failed to delete the folder.", "This is window title", 0);
        }
      }
      else
      {
        MessageBoxW(IntPtr.Zero, "Failed to delete the folder.", "This is window title", 0);
      }

      //string folderPath = "C:\\Users\\gv.santos\\Documents\\TesteFolderAppService";

      //Directory.Delete(folderPath);
    }

    //private static bool DeleteFolder(string folderPath)
    //{
    //  try
    //  {
    //    bool res = RemoveDirectoryW(folderPath);
    //    return res;
    //  }
    //  catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException || ex is NotSupportedException || ex is Win32Exception)
    //  {
    //    return false;
    //  }
    //}

    private static bool DeleteFolder(string folderPath)
    {
      SHFILEOPSTRUCT fileOp = new SHFILEOPSTRUCT
      {
        wFunc = FO_DELETE,
        pFrom = folderPath + "\0\0", // Duas terminações nulas para indicar o final da lista de arquivos/pastas
        fFlags = FOF_SILENT | FOF_NOCONFIRMATION | FOF_NOERRORUI | FOF_WANTNUKEWARNING
      };

      int result = SHFileOperation(ref fileOp);
      bool success = (result == 0);
      return success;
    }
  }
}

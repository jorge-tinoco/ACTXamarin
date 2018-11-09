// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.Logger
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using Android.OS;
using Android.Util;
using AndroidSecureHTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SecureHttpShared
{
  public class Logger
  {
    public static bool LOG_TO_FILE = false;
    public static bool ENCRYPT_LOG = false;
    private static readonly object syncLock = new object();
    private const string TAG = "AndroidSecureHTTP";
    private const int OSCode = 1;
    private const string OSValue = "android";
    private const string LOG_FOLDER = "Logs";
    private const string DOCUMENTS_FOLDER = "Documents";
    private const int MAX_LOG_FILE_LENGTH = 1048576;
    private const int LOG_EXPIRATION_DAYS = 30;
    private const string LOG_DATE_FORMAT = "yyyyMMdd";
    private const string LOG_FILE_EXTENSION = ".txt";
    private static string _applicationName;

    private static string GetMessage(string appName, string appVersion, string function, string message)
    {
      string str = Assembly.GetExecutingAssembly().GetName().Version.ToString();
      if (appName == null || appName.Length == 0)
      {
        appName = "AndroidSecureHTTP";
        appVersion = str;
      }
      return string.Format("[{0}_{1}][{2}][{3}] {4} - {5}", (object) appName, (object) appVersion, (object) str, (object) DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), (object) function, (object) message);
    }

    private static string GetTLSMessage(string appName, string appVersion, string user, string function, string message)
    {
      string str = Assembly.GetExecutingAssembly().GetName().Version.ToString();
      return string.Format("[{0}_{1}][{2}][{3}] {4} - {5}", (object) appName, (object) appVersion, (object) str, (object) user, (object) function, (object) message);
    }

    private static string GetFolder()
    {
      string path1 = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "Documents");
      return Path.Combine(string.IsNullOrEmpty(Logger._applicationName) ? path1 : Path.Combine(path1, Logger._applicationName), "Logs");
    }

    public static string CurrentLogFile
    {
      get
      {
        return Logger.GetLogFileName(Logger.GetFolder());
      }
    }

    private static void WriteLog(string message)
    {
      if (!Logger.LOG_TO_FILE)
        return;
      if (Logger.ENCRYPT_LOG && message != null && message.Length > 0)
        message = Encryptor.EncryptToHexa(message);
      lock (Logger.syncLock)
      {
        string folder = Logger.GetFolder();
        string logFileName = Logger.GetLogFileName(folder);
        try
        {
          Directory.CreateDirectory(folder);
          using (StreamWriter streamWriter = File.AppendText(logFileName))
          {
            streamWriter.WriteLine(message);
            streamWriter.Close();
          }
        }
        catch
        {
        }
      }
    }

    public static void InitializeLogger(string applicationName)
    {
      lock (Logger.syncLock)
      {
        Logger._applicationName = applicationName;
        Logger.DeleteOldLogFiles();
      }
    }

    private static void DeleteOldLogFiles()
    {
      DateTime dateTime = DateTime.Now;
      dateTime = dateTime.AddDays(-30.0);
      string strB = dateTime.ToString("yyyyMMdd");
      string folder = Logger.GetFolder();
      if (!Directory.Exists(folder))
        return;
      string[] files = Directory.GetFiles(folder);
      if (!((IEnumerable<string>) files).Any<string>())
        return;
      foreach (string path in (IEnumerable<string>) ((IEnumerable<string>) files).OrderBy<string, string>((Func<string, string>) (x => x)))
      {
        if (string.Compare(Path.GetFileNameWithoutExtension(path), strB) > 0)
          break;
        File.Delete(path);
      }
    }

    private static string GetLogFileName(string folderPath)
    {
      string str1 = DateTime.Now.ToString("yyyyMMdd");
      string str2 = Path.Combine(folderPath, string.Format("{0}{1}", (object) str1, (object) ".txt"));
      try
      {
        if (Directory.Exists(folderPath))
        {
          string[] files = Directory.GetFiles(folderPath, string.Format("{0}*", (object) str1));
          if (((IEnumerable<string>) files).Any<string>())
          {
            string str3 = ((IEnumerable<string>) files).OrderByDescending<string, string>((Func<string, string>) (x => x)).First<string>();
            if (new FileInfo(str3).Length >= 1048576L)
            {
              string[] strArray = Path.GetFileNameWithoutExtension(str3).Split('_');
              string str4 = strArray[0];
              int num = 1;
              if (strArray.Length > 1)
                num = int.Parse(strArray[1]) + 1;
              str2 = Path.Combine(folderPath, string.Format("{0}_{1}{2}", (object) str4, (object) num, (object) ".txt"));
            }
            else
              str2 = str3;
          }
        }
      }
      catch (Exception ex)
      {
        return str2 + ex.ToString();
      }
      return str2;
    }

    public static void LogError(string function, string message)
    {
      Logger.LogError("", "", function, message);
    }

    public static void LogError(string appName, string appVersion, string function, string message)
    {
      string message1 = Logger.GetMessage(appName, appVersion, function, message);
      Log.Error("AndroidSecureHTTP", message1);
      Logger.WriteLog(message1);
    }

    public static void LogInfo(string function, string message)
    {
      Logger.LogInfo("", "", function, message);
    }

    public static void LogInfo(string appName, string appVersion, string function, string message)
    {
      string message1 = Logger.GetMessage(appName, appVersion, function, message);
      Log.Info("AndroidSecureHTTP", message1);
      Logger.WriteLog(message1);
    }

    public static void LogWarning(string function, string message)
    {
      Logger.LogWarning("", "", function, message);
    }

    public static void LogWarning(string appName, string appVersion, string function, string message)
    {
      string message1 = Logger.GetMessage(appName, appVersion, function, message);
      Log.Warn("AndroidSecureHTTP", message1);
      Logger.WriteLog(message1);
    }

    public static void LogDebug(string function, string message)
    {
      Logger.LogDebug("", "", function, message);
    }

    public static void LogDebug(string appName, string appVersion, string function, string message)
    {
      string message1 = Logger.GetMessage(appName, appVersion, function, message);
      Log.Debug("AndroidSecureHTTP", message1);
      Logger.WriteLog(message1);
    }

    public static string GetLog()
    {
      lock (Logger.syncLock)
      {
        string str = string.Empty;
        string folder = Logger.GetFolder();
        if (Directory.Exists(folder))
        {
          string[] files = Directory.GetFiles(folder);
          if (((IEnumerable<string>) files).Any<string>())
            str = File.ReadAllText(((IEnumerable<string>) files).OrderByDescending<string, string>((Func<string, string>) (x => x)).First<string>());
        }
        return str;
      }
    }

    public static void ClearLog()
    {
      lock (Logger.syncLock)
      {
        string folder = Logger.GetFolder();
        if (!Directory.Exists(folder))
          return;
        foreach (FileSystemInfo file in new DirectoryInfo(folder).GetFiles())
          file.Delete();
      }
    }

    public static void LogExceptionToTLS(SecureHttp secureHttp, string function, string message)
    {
      Logger.LogToTLS(secureHttp, function, message, 0);
    }

    public static void LogInfoToTLS(SecureHttp secureHttp, string function, string message)
    {
      Logger.LogToTLS(secureHttp, function, message, 1);
    }

    public static void LogWarningToTLS(SecureHttp secureHttp, string function, string message)
    {
      Logger.LogToTLS(secureHttp, function, message, 2);
    }

    public static void LogCriticalToTLS(SecureHttp secureHttp, string function, string message)
    {
      Logger.LogToTLS(secureHttp, function, message, 3);
    }

    private static void LogToTLS(SecureHttp secureHttp, string function, string message, int logType)
    {
      SecureHttp anonymousHttpRequester = SecureHttpFactory.GetAnonymousAuthenticationRequester(secureHttp.GetContext());
      string appName = secureHttp.GetLogApplicationName();
      string appVersion = secureHttp.GetLogApplicationVersion();
      string logUrl = secureHttp.GetLogUrl();
      string user = secureHttp.GetUser();
      string certificateSN = secureHttp.GetCertificateSerialNumber();
      if (anonymousHttpRequester == null)
        Logger.LogError(appName, appVersion, "LogErrorToTLS", "Error obtaining AnonymousHttp from factory");
      else
        anonymousHttpRequester.Init(logUrl, appName, appVersion, (Action<InitResponse>) (initResponse => anonymousHttpRequester.Post(Logger.GetLogUrl(logUrl, appName, appVersion, logType), JsonConvert.SerializeObject((object) new
        {
          message = Logger.GetTLSMessage(appName, appVersion, user, function, message),
          username = user,
          snCertificate = certificateSN,
          logType = logType,
          mobileApp = "AndroidSecureHTTP",
          os = 1
        }, Formatting.None), (Action<string>) (response => Logger.LogInfo(appName, appVersion, nameof (LogToTLS), "Log successfully sent to TLS, logType=" + (object) logType)), (Action<string>) (response => Logger.LogError(appName, appVersion, nameof (LogToTLS), "Error sending log to TLS, logType=" + (object) logType + ": " + response)))), (Action<InitResponse>) (initResponse => Logger.LogError(appName, appVersion, nameof (LogToTLS), "Error initializing AnonymousHttp, logType=" + (object) logType + ": " + initResponse.ToString())));
    }

    private static string GetLogUrl(string logUrl, string appName, string appVersion, int logCode)
    {
      string str = "";
      switch (logCode)
      {
        case 0:
          str = "exception";
          break;
        case 1:
          str = "info";
          break;
        case 2:
          str = "warning";
          break;
        case 3:
          str = "critical";
          break;
      }
      return string.Format("{0}?os={1}&app={2}&log={3}", new object[4]
      {
        (object) logUrl,
        (object) "android",
        (object) appName.ToLower(),
        (object) str
      });
    }
  }
}

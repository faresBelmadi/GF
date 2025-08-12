using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public static class FileLogger
{
    private static readonly object _fileLock = new object();
    private static string _logsDir;
    private static string _sessionFilePath;
    private static bool _initialized;

    private const int MaxFiles = 15;

    public static void Init(string logFilePrefix = "session")
    {
        if (_initialized)
            return;

        _logsDir = Path.Combine(Application.persistentDataPath, "Logs");
        Directory.CreateDirectory(_logsDir);

        var dirInfo = new DirectoryInfo(_logsDir);
        var files = dirInfo.GetFiles("*.txt").OrderBy(f => f.CreationTimeUtc).ToList();
        while (files.Count >= MaxFiles)
        {
            try
            {
                files[0].Delete();
            }
            catch
            {
                /* ignore */
            }
            files.RemoveAt(0);
        }

        var ts = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        _sessionFilePath = Path.Combine(_logsDir, $"{logFilePrefix}_{ts}.txt");

        using (var sw = new StreamWriter(_sessionFilePath, append: false, Encoding.UTF8))
        {
            sw.WriteLine($"# Session start: {DateTime.Now:O} (local)");
            sw.WriteLine($"# Unity {Application.unityVersion}, Platform {Application.platform}");
            sw.WriteLine();
        }

        Application.logMessageReceivedThreaded += HandleUnityLog;

        _initialized = true;
    }

    public static void Shutdown()
    {
        if (!_initialized)
            return;
        Application.logMessageReceivedThreaded -= HandleUnityLog;
        _initialized = false;
    }

    private static void HandleUnityLog(string condition, string stackTrace, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
            case LogType.Assert:
            case LogType.Exception:
                Error(string.IsNullOrEmpty(stackTrace) ? condition : $"{condition}\n{stackTrace}");
                break;
            case LogType.Warning:
                Info("[Warning] " + condition);
                break;
            default:
                Debug(condition);
                break;
        }
    }

    private static void Write(LogSeverity severity, string message, LogSystem logSystem)
    {
        if (!_initialized)
            Init();

        var time = DateTime.Now.ToString("HH:mm:ss.fff");
        string line;
        string logSystemString;
        if (logSystem == LogSystem.None)
            logSystemString = "";
        else
            logSystemString = $"[{logSystem}]";

        switch (severity)
        {
            case LogSeverity.Info:
                line = $"*[{time}] [INFO]* {logSystemString} {message}";
                break;
            case LogSeverity.Error:
                line = $"**[{time}] [ERROR]** {logSystemString} {message}";
                break;
            default:
                line = $"[{time}] [DEBUG] {logSystemString} {message}";
                break;
        }

        lock (_fileLock)
        {
            using (var sw = new StreamWriter(_sessionFilePath, append: true, Encoding.UTF8))
                sw.WriteLine(line);
        }
    }
    public static void Debug(string msg, LogSystem logSystem = LogSystem.None) => Write(LogSeverity.Debug, msg, logSystem);
    public static void Info(string msg, LogSystem logSystem = LogSystem.None) => Write(LogSeverity.Info, msg, logSystem);
    public static void Error(string msg, LogSystem logSystem = LogSystem.None) => Write(LogSeverity.Error, msg, logSystem);
}

public enum LogSeverity 
{
    Debug,
    Info,
    Error 
}

public enum LogSystem 
{
    None,
    Dialogue,
    Stats,
    Effet
}

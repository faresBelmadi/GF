using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;


public enum LogSeverity { Debug, Info, Error }

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

    private static void Write(LogSeverity severity, string message)
    {
        if (!_initialized) 
            Init(); // fail-safe

        var time = DateTime.Now.ToString("HH:mm:ss.fff");
        string line;

        switch (severity)
        {
            case LogSeverity.Info:
                line = $"*[{time}] [INFO]* {message}";
                break;
            case LogSeverity.Error:
                line = $"**[{time}] [ERROR]** {message}";
                break;
            default:
                line = $"[{time}] [DEBUG] {message}";
                break;
        }

        lock (_fileLock)
        {
            using (var sw = new StreamWriter(_sessionFilePath, append: true, Encoding.UTF8))
                sw.WriteLine(line);
        }
    }
    public static void Debug(string msg) => Write(LogSeverity.Debug, msg);
    public static void Info(string msg) => Write(LogSeverity.Info, msg);
    public static void Error(string msg) => Write(LogSeverity.Error, msg);
}
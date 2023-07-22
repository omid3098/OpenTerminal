using System;
using System.Collections.Generic;
using UnityEngine;

public class LogStack
{
    private readonly TerminalConfig config;
    public List<LogType> logTypes { get; private set; }
    public List<string> logTitles { get; private set; }
    public List<string> logStacks { get; private set; }
    public static string Clipboard
    {
        get { return GUIUtility.systemCopyBuffer; }
        set { GUIUtility.systemCopyBuffer = value; }
    }
    public LogStack(TerminalConfig config)
    {
        this.config = config;
        logTypes = new List<LogType>();
        logTitles = new List<string>();
        logStacks = new List<string>();
    }

    public void AddLog(string logString, string stackTrace, LogType type)
    {
        if (logTitles.Count > config.LogCount)
        {
            logTitles.RemoveAt(0);
            logStacks.RemoveAt(0);
            logTypes.RemoveAt(0);
        }
        logTitles.Add(logString);
        logStacks.Add(stackTrace);
        logTypes.Add(type);
    }
    public void Share()
    {
        // Debug.Log("Sharing Log...");
        string _result = "";
        for (int i = 0; i < logTypes.Count; i++)
        {
            _result += logTypes[i] + " : ";
            _result += logTitles[i] + "\n";
            switch (logTypes[i])
            {
                case LogType.Log:
                    if (config.infoStacks)
                        _result += logStacks[i] + "\n";
                    break;
                case LogType.Warning:
                    if (config.warningStacks)
                        _result += logStacks[i] + "\n";
                    break;
                case LogType.Error:
                    if (config.errorStacks)
                        _result += logStacks[i] + "\n";
                    break;
                case LogType.Assert:
                    if (config.assertStacks)
                        _result += logStacks[i] + "\n";
                    break;
                case LogType.Exception:
                    if (config.exceptionStacks)
                        _result += logStacks[i] + "\n";
                    break;
                default:
                    break;
            }
        }
        Clipboard = _result;
        Application.OpenURL("mailto:" + config.supportEmail + "?subject=" + config.EmailTitle + "&body=" + MyEscapeURL(_result));
    }

    public void Clear()
    {
        logTitles.Clear();
        logStacks.Clear();
        logTypes.Clear();
    }

    string MyEscapeURL(string url)
    {
        // use UnityWebRequest.EscapeURL instead of WWW.EscapeURL
        return UnityEngine.Networking.UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}

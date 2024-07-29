using UnityEngine;

public static class GameDebug
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogMessage(string message)
    {
        Debug.Log(message);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarning(string warning)
    {
        Debug.LogWarning(warning);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogError(string error)
    {
        Debug.LogError(error);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, params object[] args)
    {
        Debug.LogFormat(format, args);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        Debug.LogWarningFormat(format, args);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        Debug.LogErrorFormat(format, args);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition)
    {
        Debug.Assert(condition);
    }
}

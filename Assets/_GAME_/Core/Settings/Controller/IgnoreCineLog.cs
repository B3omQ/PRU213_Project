#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class IgnoreCineLog
{
    static IgnoreCineLog()
    {
        Application.logMessageReceived += (condition, stackTrace, type) =>
        {
            if (condition.Contains("Screen position out of view frustum"))
            {
                return;
            }
        };
    }
}
#endif

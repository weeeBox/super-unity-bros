using UnityEngine;

using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[assembly: InternalsVisibleTo("Test")]

namespace LunarAssertsInternal
{
    struct SourceFileInfo
    {
        public readonly string path;
        public readonly int line;

        public SourceFileInfo(string path, int line)
        {
            this.path = path;
            this.line = line;
        }
    }

    static class assertUtils
    {
        private static Regex sourceFileRegex;

        #region Stack Trace

        public static string ExtractStackTrace(int numFrames)
        {
            string stackTrace = StackTraceUtility.ExtractStackTrace();
            return ExtractStackTrace(stackTrace, numFrames);
        }

        public static string ExtractStackTrace(string stackTrace, int numFrames)
        {
            int startPos = 0;
            for (int frameIndex = 0; frameIndex < numFrames && startPos < stackTrace.Length; ++frameIndex)
            {
                int pos = stackTrace.IndexOf('\n', startPos);
                if (pos == -1)
                {
                    return "";
                }

                startPos = pos + 1;
            }

            return stackTrace.Substring(startPos);
        }

        public static bool TryExtractSourceFileInfo(string stackTrace, out SourceFileInfo info)
        {
            int index = stackTrace.IndexOf("\n");
            string frameString = index != -1 ? stackTrace.Substring(0, index) : stackTrace;

            Match match;
            if ((match = SourceFileRegex.Match(frameString)).Success)
            {
                string path = match.Groups[1].ToString();
                string line = match.Groups[2].ToString();

                int lineNumber;
                if (int.TryParse(line, out lineNumber))
                {
                    info = new SourceFileInfo(path, lineNumber);
                    return true;
                }
            }

            info = default(SourceFileInfo);
            return false;
        }

        #endregion

        #region String representation

        public static string TryFormat(string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(format) && args != null && args.Length > 0)
            {
                try
                {
                    return string.Format(format, args);
                }
                catch (Exception e)
                {
                    Debug.LogError("Error while formatting string: " + e.Message);
                }
            }
            
            return format;
        }
        
        public static string ToString(object obj)
        {
            return obj != null ? obj.ToString() : "null";
        }

        #endregion

        #region Dialogs

        #if UNITY_EDITOR

        public static void DisplayDialog(string title, string message, 
                           string ok = null, Action actionOk = null, 
                           string cancel = null, Action actionCancel = null, 
                           string alt = null, Action actionAlt = null)
        {
            if (alt != null)
            {
                int result = EditorUtility.DisplayDialogComplex(title, message, ok, cancel, alt);
                Action[] actionLookup = {
                    actionOk,
                    actionCancel,
                    actionAlt
                };
                Action action = actionLookup[result];
                if (action != null)
                {
                    action();
                }
            }
            else if (cancel != null)
            {
                Action action = EditorUtility.DisplayDialog(title, message, ok, cancel) ? actionOk : actionCancel;
                if (action != null)
                {
                    action();
                }
            }
            else if (EditorUtility.DisplayDialog(title, message, ok != null ? ok : "OK") && actionOk != null)
            {
                actionOk();
            }
        }

        #endif // UNITY_EDITOR

        #endregion

        #region Files

        #if UNITY_EDITOR

        public static bool OpenFileAtLineExternal(string path, int line)
        {
            return UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(path, line);
        }

        public static int FindLineOfToken(string currentFile, string token)
        {
            string[] lines = File.ReadAllLines(currentFile);
            for (int i = 0; i < lines.Length; ++i)
            {
                if (lines[i].Contains(token))
                {
                    return i;
                }
            }

            return -1;
        }

        public static string AssetsRelativePath(string currentFile)
        {
            return FileUtil.GetProjectRelativePath(currentFile);
        }

        #endif // UNITY_EDITOR

        #endregion

        #region Properties

        static Regex SourceFileRegex
        {
            get
            {
                if (sourceFileRegex == null)
                {
                    sourceFileRegex = new Regex("\\(at (Assets\\/.*?):(\\d+)\\)");
                }
                return sourceFileRegex;
            }
        }

        #endregion
    }
}

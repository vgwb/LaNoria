using UnityEngine;

namespace vgwb.framework
{
    public static class Log
    {
        public static void Info(string txt)
        {
            Debug.Log(txt);
        }

        public static void Dbg(string txt)
        {
            Debug.Log(txt);
        }

        public static void Warn(string txt)
        {
            Debug.LogWarning(txt);
        }

        public static void Err(string txt)
        {
            Debug.LogError(txt);
        }
    }
}

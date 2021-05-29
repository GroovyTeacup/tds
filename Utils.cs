using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TDS.Patches;

namespace TDS
{
    public static class Utils
    {
        public static void HarmonyPatch(this MethodInfo method, MethodInfo prefix = null, MethodInfo postfix = null)
        {
            try
            {
                TDSPlugin.HarmonyInstance.Patch(method, prefix != null ? new HarmonyMethod(prefix) : null, postfix != null ? new HarmonyMethod(postfix) : null);
                UnityEngine.Debug.Log($"Patched {method.DeclaringType}.{method.Name}");
            }
            catch (Exception ex)
            {
                string prefixStr = prefix != null ? prefix.Name : "<none>";
                string postfixStr = postfix != null ? postfix.Name : "<none>";
                UnityEngine.Debug.LogError($"Exception occurred while patching method {method.Name}. prefix: {prefixStr} postfix: {postfixStr}\n {ex.ToString()}");
            }
        }

        public static void DisableMethod(this MethodInfo method)
        {
            try
            {
                TDSPlugin.HarmonyInstance.Patch(method, new HarmonyMethod(typeof(DisablePatch), "DisableMe"));
                UnityEngine.Debug.Log($"Patched (Disable) {method.DeclaringType}.{method.Name}");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"Exception occurred disabling method by patch {method.Name}. prefix: DisableMe\n {ex.ToString()}");
            }
        }

        public static Type[] TypeArray<T1>()
        {
            return new Type[] { typeof(T1) };
        }

        public static Type[] TypeArray<T1, T2>()
        {
            return new Type[] { typeof(T1), typeof(T2) };
        }

        public static Type[] TypeArray<T1, T2, T3>()
        {
            return new Type[] { typeof(T1), typeof(T2), typeof(T3) };
        }

        public static Type[] TypeArray<T1, T2, T3, T4>()
        {
            return new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
        }

        public static Type[] TypeArray<T1, T2, T3, T4, T5>()
        {
            return new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
        }

        public static Type[] TypeArray<T1, T2, T3, T4, T5, T6>()
        {
            return new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) };
        }
        public static Type[] TypeArray<T1, T2, T3, T4, T5, T6, T7>()
        {
            return new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) };
        }

        public static Type[] TypeArray<T1, T2, T3, T4, T5, T6, T7, T8>()
        {
            return new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) };
        }

        public static Type[] TypeArray<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
        {
            return new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9) };
        }

        public static Type[] TypeArray<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
        {
            return new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10) };
        }
    }
}

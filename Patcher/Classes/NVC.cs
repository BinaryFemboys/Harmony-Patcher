using HarmonyLib;
using System;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Patch.Loader
{
    internal class NVC
    {
        public static bool signature = false;
        static int rate = 0;

        [HarmonyPatch(typeof(NameValueCollection), nameof(NameValueCollection.Get), new[] { typeof(int) })]
        public class NameValueCollectionPatch
        {
            static bool Prefix(int index)
            {
                if (index == 0)
                {
                    signature = true;
                    rate = 0;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ProcessStartInfo), MethodType.Constructor, new[] { typeof(string), typeof(string) })]
        public class ProcessStartInfoPatch
        {
            static bool Prefix(ref string fileName, ref string arguments)
            {
                if (fileName == "cmd.exe" && arguments.Contains("Signature checksum failed"))
                {
                    arguments = "/c start cmd /C";
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Environment), nameof(Environment.Exit), new[] { typeof(int) })]
        public class EnvironmentExitPatch
        {
            static bool Prefix(int exitCode)
            {
                if (exitCode == 0 && signature)
                {
                    if (rate < 2)
                    {
                        rate++;
                        return false;
                    }
                    signature = false;
                    rate = 0;
                }
                return true;
            }
        }
    }
}

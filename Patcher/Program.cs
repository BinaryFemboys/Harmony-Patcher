using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using System.Threading;


namespace Patch.Loader
{
    internal class Program
    {
        public static string appname = "The Binfbs";
        public static string ownerid = "zBeMqtlnx1";
        public static string secret = "8740596e131687947c66c8c7ee26254dff3148a6d82be068b0087a8c8c6ed773";

        public static Harmony patch = new Harmony("The BinFbs");

        public static string originalname = "";
        public static string originalownerid = "";
        public static string originalsecret = "";

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Please provide the path to the assembly.");
                    Thread.Sleep(5000);
                    return;
                }

                var assembly = Assembly.LoadFile(Path.GetFullPath(args[0]));
                patch.PatchAll(Assembly.GetExecutingAssembly());

                var entryPoint = assembly.EntryPoint;
                var paramInfo = entryPoint.GetParameters();
                object[] parameters = new object[paramInfo.Length];

                entryPoint.Invoke(null, parameters);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Stack Trace: ");
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();

                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}

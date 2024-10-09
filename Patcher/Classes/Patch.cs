using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using HarmonyLib;

namespace Patch.Loader
{

    internal class Patch
    {

        [HarmonyPatch(typeof(Encoding), nameof(Encoding.Default.GetBytes), new[] { typeof(string) })]

        public class patch
        {

            #region utils

            public static bool Format(string inputString)
            {
                byte[] hashBytes;
                bool correctformat = false;
                try
                {
                    hashBytes = HexStringToByteArray(inputString);
                }
                catch (FormatException)
                {
                    return false;
                }
                if (hashBytes.Length == 32)
                {
                    correctformat = true;
                }

                return correctformat;
            }

            private static byte[] HexStringToByteArray(string input)
            {
                int numberChars = input.Length;
                byte[] bytes = new byte[numberChars / 2];
                for (int i = 0; i < numberChars; i += 2)
                {
                    bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
                }
                return bytes;
            }

            #endregion


            static List<string> all = new List<string>();
            static int nameindex = 0;
            static int ownerindex = 0;
            static bool secret_original = false;
            static bool name_original = false;
            static bool owner_original = false;


            public static bool Prefix(ref string s)
            {
                all.Add(s);


                for (int i = 0; i < all.Count; i++)
                {


                    if (all[i].Length == 64 && Format(all[i]) == true && secret_original == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        MessageBox.Show("Made with <3 by Deltrix for The BinFbs");
                        Console.ResetColor();
                        Program.originalsecret = all[i];

                        secret_original = true;
                    }





                    if (name_original == false)
                    {
                        if (all[i] == "name")
                        {
                            nameindex = i;
                        }


                        if (i == nameindex + 1 && nameindex != 0)
                        {
                            if (Format(all[i]) == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.ResetColor();
                                Program.originalname = all[i];
                                name_original = true;
                            }

                        }
                    }






                    if (owner_original == false)
                    {
                        if (all[i] == "ownerid")
                        {
                            ownerindex = i;
                        }


                        if (i == ownerindex + 1 && ownerindex != 0)
                        {
                            if (Format(all[i]) == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.ResetColor();
                                Program.originalownerid = all[i];
                                owner_original = true;
                            }
                        }
                    }



                }

                if (s == Program.originalname)
                {
                    s = Program.appname;
                }
                if (s == Program.originalownerid)
                {
                    s = Program.ownerid;
                }
                if (s == Program.originalsecret)
                {
                    s = Program.secret;
                }

                return true;
            }

        }
    }

}
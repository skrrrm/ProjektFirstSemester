using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ProjektFirstSemester
{
    class Menu
    {
        public static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Network Info");
            Console.WriteLine("2) Ping");
            Console.WriteLine("3) TraceRoute");
            //Console.WriteLine("4) NS-Lookup");
            Console.WriteLine("5) Exit");
            Console.WriteLine("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    NetworkInfoMenu();
                    return true;
                case "2":
                    DingMenu();
                    return true;
                case "3":
                    TracertMenu();
                    return true;
                //case "4":
                case "5":
                    return false;
                default:
                    return true;
            }
        }
        static void DingMenu()
        {
            Console.Clear();
            Console.WriteLine("Which URL or IP do you want to trace? ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Example: \n\twww.google.com\n\t1.1.1.1");
            Console.ResetColor();
            Console.WriteLine("\n");

            Echo.Ding();

            Console.WriteLine("Going back to main Menu.");
            Console.ReadLine();
            MainMenu();
        }
        static void TracertMenu()
        {
            Console.Clear();
            Console.WriteLine("Which URL or IP do you want to trace? ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Example: \n\twww.google.com\n\t1.1.1.1");
            Console.ResetColor();
            Console.WriteLine();

            Echo.Tracert();

            Console.WriteLine("Going back to main Menu.");
            MainMenu();
        }
        static void NetworkInfoMenu()
        {
            Echo.NetworkInfo();
            Console.WriteLine("Going back to mainmenu.");
            Console.ReadLine();
            MainMenu();
        }
    }
}

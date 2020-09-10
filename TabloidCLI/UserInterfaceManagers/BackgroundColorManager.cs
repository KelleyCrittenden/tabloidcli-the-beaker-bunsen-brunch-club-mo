using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class BackgroundColorManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;

        public BackgroundColorManager(IUserInterfaceManager parentUI)
        {
            _parentUI = parentUI;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("");
            Console.WriteLine("Available background colors:");
            Console.WriteLine(" 1) Blue");
            Console.WriteLine(" 2) Magenta");
            Console.WriteLine(" 3) Gray");
            Console.WriteLine(" 4) Cyan");
            Console.WriteLine(" 5) Red");
            Console.WriteLine(" 6) Black");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Clear();
                    return this;
                case "2":
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    Console.Clear();
                    return this;
                case "3":
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Clear();
                    return this;
                case "4":
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.Clear();
                    return this;
                case "5":
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Clear();
                    return this;
                case "6":
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine();
                    Console.WriteLine("Invalid Selection. Please choose a color.");
                    return this;
            }
        }

    }
}
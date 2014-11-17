using System;
using System.Collections.Generic;
using System.Threading;

namespace laboratory_work
{
    public class Menu
        //gives the main options to work with menu 
    {
        public static List<string> mainMenu = new List<string>
        {
            "Menu",
            ">  1. View all contacts",
            "   2. Search",
            "   3. New Contact",
            "   4. Exit[Escape]"
        };

        // Main menu
        public static List<string> searchMenu = new List<string>
        {
            "Search by",
            ">  1. Name",
            "   2. Surname",
            "   3. Name and Surname",
            "   4. Phone",
            "   5. E-mail"
        };

        // Menu of searching person
        public static int Pos = 1; //position of current choice
        private static List<string> _current = new List<string>(); //current menu

        public static int MainMenu()
            //method which worked with main menu
        {
            Pos = 1;
            _current = mainMenu;
            var current = _current;
            Show();
            var c = ConsoleKey.X;
            while (c != ConsoleKey.Enter)
            {
                c = Console.ReadKey().Key;
                switch (c)
                {
                    case (ConsoleKey.DownArrow):
                    {
                        Pos++;
                        if (Pos >= current.Count)
                            Pos = 1;
                        break;
                    }
                    case (ConsoleKey.UpArrow):
                    {
                        Pos--;
                        if (Pos <= 0)
                            Pos = current.Count - 1;
                        break;
                    }
                    case (ConsoleKey.Escape):
                    {
                        ExitShowDialog();
                        break;
                    }
                }
                Console.Clear();
                Done();
                Show();
            }
            return Pos;
        }

        public static int SearchMenu()
            //method which worked with search menu
        {
            Pos = 1;
            _current = searchMenu;
            List<string> current = _current;
            Show();
            var c = ConsoleKey.X;
            while (c != ConsoleKey.Enter)
            {
                c = Console.ReadKey().Key;
                switch (c)
                {
                    case (ConsoleKey.DownArrow):
                    {
                        Pos++;
                        if (Pos >= current.Count)
                            Pos = 1;
                        break;
                    }
                    case (ConsoleKey.UpArrow):
                    {
                        Pos--;
                        if (Pos <= 0)
                            Pos = current.Count - 1;
                        break;
                    }
                    case (ConsoleKey.Escape):
                    {
                        return 0;
                    }
                }
                Console.Clear();
                Done();
                Show();
            }
            return Pos;
        }


        public static void ExitShowDialog()
        {
            Console.Clear();
            const string quest = "Do you really want to exit?";
            const string yes = "yes";
            const string no = "no";
            const string YES = "YES!!!";
            const string NO = "NO!!!";
            foreach (var t in quest)
            {
                Console.Write(t);
                Thread.Sleep(15);
            }
            Console.WriteLine();
            Console.WriteLine("{0}\t{1}", yes, NO);

            var poss = 1;
            var c = ConsoleKey.X;
            while (c != ConsoleKey.Enter)
            {
                c = Console.ReadKey().Key;
                switch (c)
                {
                    case (ConsoleKey.RightArrow):
                    {
                        poss = 1;
                        Console.Clear();
                        Console.WriteLine(quest);
                        Console.Write("{0}\t{1}", yes, NO);
                        break;
                    }
                    case (ConsoleKey.LeftArrow):
                    {
                        poss = 0;
                        Console.Clear();
                        Console.WriteLine(quest);
                        Console.Write("{0}\t{1}", YES, no);
                        break;
                    }
                }
            }
            if (poss == 0)
                Environment.Exit(0);
        }

        //showing current menu
        public static void Show()
        {
            foreach (string c in _current)
                Console.WriteLine(c);
        }

        //method called when user choose the current position
        private static void Done()
        {
            for (var i = 1; i < _current.Count; ++i)
                _current[i] = " " + _current[i].Substring(1);
            _current[Pos] = ">" + _current[Pos].Substring(1);
        }
    }
}
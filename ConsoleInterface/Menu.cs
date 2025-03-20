using Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public class Menu
    {
        private string[] _menuItems;
        private string _menuName;
        public Menu(string menuName, string[] values)
        {
            _menuItems = values;
            _menuName = menuName;
        }
        /// <summary>
        /// This method prints the menu items of interactive menu.
        /// </summary>
        private void PrintMenu(string[] elems, int row, int column, int idx)
        {
            Console.SetCursorPosition(column, row);

            for (int i = 0; i < elems.Length; i++)
            {
                // Setting color of current menu item.
                if (i + 1 == idx)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(elems[i]);
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        /// <summary>
        /// This menu implements all menu actions.
        /// </summary>
        /// <param name="tableValues"></param>
        /// <param name="columnNames"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int ActMenu()
        {
            int idx;

            // Cycle of repeating to process errors.
            while (true)
            {
                MainInterface.PrintColor(_menuName, ConsoleColor.DarkYellow, ConsoleColor.DarkCyan);

                // Cursor of current place.
                int row = Console.CursorTop;
                int column = Console.CursorLeft;
                // Default index of menu.
                idx = 1;

                // Cycle of repeating to show interactive menu.
                while (true)
                {
                    // The variable to break the cycle after choosing menu item.
                    bool isExit = true;

                    PrintMenu(_menuItems, row, column, idx);

                    switch (Console.ReadKey(true).Key)
                    {
                        // When user enters down key.
                        case ConsoleKey.DownArrow:
                            // Checking if it is the last element.
                            if (idx < _menuItems.Length)
                                // Moving to the next item.
                                idx++;
                            break;
                        // When user enters up key.
                        case ConsoleKey.UpArrow:
                            // Checking if it is the first and minimal element.
                            if (idx > 1)
                                // Moving to the previous item.
                                idx--;
                            break;
                        // When user chooses the item.
                        case ConsoleKey.Enter:
                            // Exiting interactive menu.
                            isExit = false;
                            break;

                    }
                    if (!isExit)
                        break;
                }
                return idx;
            }
        }
    }
}

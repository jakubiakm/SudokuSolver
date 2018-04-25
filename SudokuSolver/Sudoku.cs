using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Sudoku
    {
        /// <summary>
        /// Rozmiar kolumny, wiersza i podkwadratu Sudoku.
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Elementy Sudoku.
        /// </summary>
        public int[,] Board { get; set; }

        public Sudoku()
        {
            Size = 9;
            Board = new int[Size, Size];
        }

        public Sudoku(string path)
        {
            var lines = File.ReadLines(path);
            Size = lines.Count();
            if ((int)Math.Sqrt(Size) != Math.Sqrt(Size))
                throw new InvalidDataException("Sudoku musi być rozmiaru n x n, gdzie n to kwadrat liczby naturalnej.");
            Board = new int[Size, Size];
            int y = 0;
            foreach (var line in lines)
            {
                int x = 0;
                var numbers = line.Split(' ').Select(Int32.Parse).ToList();
                if (numbers.Count != Size)
                    throw new InvalidDataException("Sudoku musi być rozmiaru n x n, gdzie n to kwadrat liczby naturalnej.");
                foreach (var number in numbers)
                    Board[x++, y] = number;
                y++;
            }
        }

        /// <summary>
        /// Wyświetla aktualnie wypełnione sudoku w konsoli. Jeśli paramatery x i y są różne od -1, to podświetla na zielono element (x, y) w tablicy.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void PrintToConsole(int x = -1, int y = -1)
        {
            int maxNumberLength = Size.ToString().Length;
            int squareEdgeSize = (int)(Math.Sqrt(Size));

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i != Size; i++)
            {
                for (int j = 0; j != Size; j++)
                {
                    string element = Board[j, i] == 0 ? " " : Board[j, i].ToString();
                    if (x == j && y == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    if (element == " ")
                        Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write(element);
                    Console.Write(new string(' ', maxNumberLength - element.Length));
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" ");
                    if (j % squareEdgeSize == squareEdgeSize - 1)
                        Console.Write(" ");
                }
                Console.WriteLine();
                if (i % squareEdgeSize == squareEdgeSize - 1)
                    Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        public static readonly string[] SudokuBoardPaths =
        {
            "veryeasy_9x9_1.txt",
            "easy_9x9_1.txt",
            "easy_9x9_2.txt",
            "easy_9x9_3.txt",
            "medium_9x9_1.txt",
            "medium_9x9_2.txt",
            "medium_9x9_3.txt",
            "hard_9x9_1.txt",
            "veryhard_9x9_1.txt",

            "medium_16x16_1.txt",
            "hard_16x16_1.txt",

            "medium_25x25_1.txt",
            "hard_25x25_1.txt",
            /////////////////////
            "hard_25x25_6.txt",
            "hard_25x25_4.txt",
            "veryhard_25x25_1.txt",
            "veryhard_25x25_2.txt",
        };

        static void Main(string[] args)
        {
            foreach (var path in SudokuBoardPaths)
            {
                Console.WriteLine($"Solve sudoku {path}");
                Console.WriteLine();
                string sudokuFilePath = $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/SudokuBoards/{path}";
                Sudoku sudoku = new Sudoku(sudokuFilePath);
                SudokuSolver solver = new SudokuSolver(false);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                sudoku.PrintToConsole();
                Console.WriteLine($"Solved: {solver.Solve(sudoku)}");
                Console.WriteLine($"Time: {sw.ElapsedMilliseconds} miliseconds");
            }
        }
    }
}

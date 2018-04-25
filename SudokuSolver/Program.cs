using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        public static readonly string[] SudokuBoardPaths = { "easy_9x9_1.txt" };

        static void Main(string[] args)
        {
            foreach (var path in SudokuBoardPaths)
            {
                Console.WriteLine($"Solve sudoku {path}");
                Console.WriteLine();
                string sudokuFilePath = $"{Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))}/SudokuBoards/{path}";
                Sudoku sudoku = new Sudoku(sudokuFilePath);
                sudoku.PrintToConsole();
                SudokuSolver solver = new SudokuSolver(sudoku);
                Console.WriteLine($"Solved: {solver.Solve()}");
            }
        }
    }
}

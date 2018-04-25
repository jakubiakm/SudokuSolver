using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Sudoku sudoku = new Sudoku();
            sudoku.PrintToConsole();
            SudokuSolver solver = new SudokuSolver(sudoku);
            Console.WriteLine($"Solved: {solver.Solve()}");
        }
    }
}

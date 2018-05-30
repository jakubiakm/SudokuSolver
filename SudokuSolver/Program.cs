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
        public static String[] SudokuBoardPaths;

        static void Main(string[] args)
        {
            SudokuBoardPaths = readFromFile();
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
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("-------------------------");
            }
        }
        static String[] readFromFile()
        {
            List<String> files = new List<String>();
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("../../SudokuBoards.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line[0] == '!')
                        {
                            continue;
                        }
                        files.Add(line);
                        Console.WriteLine(line);                      
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            return files.ToArray();
        }
    }
}

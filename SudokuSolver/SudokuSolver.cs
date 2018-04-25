using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuSolver
    {
        public Sudoku sudoku { get; private set; }

        public SudokuSolver(Sudoku sudoku) => this.sudoku = sudoku;

        public bool Solve()
        {
            return true;
        }
    }
}

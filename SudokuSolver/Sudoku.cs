using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class Sudoku
    {
        /// <summary>
        /// Rozmiar kolumny, wiersza i podkwadratu Sudoku
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Elementy Sudoku
        /// </summary>
        public int[,] Board { get; set; }
    }
}

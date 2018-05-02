using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuPossibility
    {
        public SudokuPossibility(int value, double probability)
        {
            Value = value;
            Probability = probability;
        }

        public int Value { get; set; }

        public double Probability { get; set; }
    }

}

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
            sudoku.PrintToConsole();
            bool changed = false;
            while (!sudoku.IsFull())
            {
                if (!sudoku.IsCorrect())
                    return false;
                changed = FillBlankElement();
            }
            return sudoku.IsCorrect();
        }

        public bool FillBlankElement()
        {
            bool changed = false;
            var possibilities = GetBoardPossibilities();
            for (int i = 0; i != sudoku.Size; i++)
            {
                for (int j = 0; j != sudoku.Size; j++)
                {
                    if (sudoku.Board[i, j] == 0)
                    {
                        if (possibilities[i, j].Count == 1)
                        {
                            sudoku.Board[i, j] = possibilities[i, j][0];
                            sudoku.PrintToConsole(i, j);
                            changed = true;
                        }
                    }
                }
            }
            if(!changed)
            {
                for(int i = 0; i != sudoku.Size; i++)
                {
                    List<int> allPosibilities = new List<int>();
                    int subsquareLength = (int)Math.Sqrt(sudoku.Size);
                    int startx = (i % subsquareLength) * subsquareLength;
                    int starty = (i / subsquareLength) * subsquareLength;
                    for (int p = startx; p != startx + subsquareLength; p++)
                    {
                        for (int q = starty; q != starty + subsquareLength; q++)
                        {
                            if (sudoku.Board[p, q] == 0)
                            {
                                allPosibilities.AddRange(possibilities[p, q]);
                            }
                        }
                    }
                    var groups = allPosibilities.GroupBy(x => x).OrderBy(x => x.Count()).ToList();
                    if (groups.Count == 0)
                        continue;
                    var group = groups[0];
                    if(group.Count() == 1)
                    {
                        var key = group.Key;
                        for (int p = startx; p != startx + subsquareLength; p++)
                        {
                            for (int q = starty; q != starty + subsquareLength; q++)
                            {
                                if (sudoku.Board[p, q] == 0)
                                {
                                    if (possibilities[p, q].Contains(key))
                                    {
                                        sudoku.Board[p, q] = key;
                                        sudoku.PrintToConsole(p, q);
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return changed;
        }

        public List<int>[,] GetBoardPossibilities()
        {
            List<int>[,] possibilites = new List<int>[sudoku.Size, sudoku.Size];
            for (int i = 0; i != sudoku.Size; i++)
            {
                for (int j = 0; j != sudoku.Size; j++)
                {
                    if (sudoku.Board[i, j] == 0)
                    {
                        possibilites[i, j] = GetPossibilities(i, j);
                    }
                }
            }
            return possibilites;
        }

        public List<int> GetPossibilities(int x, int y)
        {
            List<int> possibilities = new List<int>();
            bool[] found = new bool[sudoku.Size];
            for (int i = 0; i != sudoku.Size; i++)
            {
                if (sudoku.Board[x, i] != 0 && i != y)
                    found[sudoku.Board[x, i] - 1] = true;
                if (sudoku.Board[i, y] != 0 && i != x)
                    found[sudoku.Board[i, y] - 1] = true;
            }
            int subsquareLength = (int)Math.Sqrt(sudoku.Size);
            int startx = (x / subsquareLength) * subsquareLength;
            int starty = (y / subsquareLength) * subsquareLength;
            for (int p = startx; p != startx + subsquareLength; p++)
                for (int q = starty; q != starty + subsquareLength; q++)
                    if (sudoku.Board[p, q] != 0 && !(p == x && q == y))
                        found[sudoku.Board[p, q] - 1] = true;
            for (int i = 0; i != found.Length; i++)
                if (!found[i])
                    possibilities.Add(i + 1);
            return possibilities;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    public class SudokuSolver
    {
        public bool ShowProgress { get; set; }

        public SudokuSolver(bool showProgress = true)
        {
            ShowProgress = showProgress;
        }

        public bool Solve(Sudoku sudoku)
        {
            if (ShowProgress)
                sudoku.PrintToConsole();
            bool changed = false;
            while (!sudoku.IsFull())
            {
                if (!sudoku.IsCorrect())
                    return false;
                changed = FillBlankElement(sudoku);
                if (changed == false)
                {
                    foreach (var s in GetPossibleBoards(sudoku))
                    {
                        if (Solve(s) == true)
                            return true;
                    }
                    return false;
                }
            }
            sudoku.PrintToConsole();
            return sudoku.IsCorrect();
        }

        public bool FillBlankElement(Sudoku sudoku)
        {
            bool changed = false;
            var possibilities = GetSudokuPossibilities(sudoku);
            for (int i = 0; i != sudoku.Size; i++)
            {
                for (int j = 0; j != sudoku.Size; j++)
                {
                    if (sudoku.Board[i, j] == 0)
                    {
                        if (possibilities[i, j].Count == 1)
                        {
                            sudoku.Board[i, j] = possibilities[i, j][0].Value;
                            if (ShowProgress)
                                sudoku.PrintToConsole(i, j);
                            changed = true;
                        }
                    }
                }
            }
            if (!changed)
            {
                for (int i = 0; i != sudoku.Size; i++)
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
                                allPosibilities.AddRange(possibilities[p, q].Select(l => l.Value));
                            }
                        }
                    }
                    var groups = allPosibilities.GroupBy(x => x).OrderBy(x => x.Count()).ToList();
                    if (groups.Count == 0)
                        continue;
                    var group = groups[0];
                    if (group.Count() == 1)
                    {
                        var key = group.Key;
                        for (int p = startx; p != startx + subsquareLength; p++)
                        {
                            for (int q = starty; q != starty + subsquareLength; q++)
                            {
                                if (sudoku.Board[p, q] == 0)
                                {
                                    if (possibilities[p, q].Select(l => l.Value).Contains(key))
                                    {
                                        sudoku.Board[p, q] = key;
                                        if (ShowProgress)
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

        public List<Sudoku> GetPossibleBoards(Sudoku sudoku)
        {
            List<Sudoku> sudokus = new List<Sudoku>();
            var probabilities = GetSudokuProbabilities(sudoku);
            int min = int.MaxValue;
            double minProbability = double.MaxValue;
            for (int i = 0; i != sudoku.Size; i++)
                for (int j = 0; j != sudoku.Size; j++)
                    if (probabilities[i, j] != null && probabilities[i, j].Count != 0 && probabilities[i, j].Count <= min)
                    {
                        if (probabilities[i, j].Count < min)
                        {
                            min = probabilities[i, j].Count;
                            minProbability = probabilities[i, j].Min(x => x.Probability);
                        }
                        else
                        {
                            if (probabilities[i, j].Min(x => x.Probability) < minProbability)
                            {
                                min = probabilities[i, j].Count;
                                minProbability = probabilities[i, j].Min(x => x.Probability);
                            }
                        }
                    }
            for (int i = 0; i != sudoku.Size; i++)
                for (int j = 0; j != sudoku.Size; j++)
                {
                    if (probabilities[i, j] != null && probabilities[i, j].Count == min && probabilities[i, j].Min(x => x.Probability) == minProbability)
                    {
                        foreach (var possibility in probabilities[i, j].OrderBy(x => x.Probability))
                        {
                            int[,] newBoard = (int[,])sudoku.Board.Clone();
                            newBoard[i, j] = possibility.Value;
                            sudokus.Add(new Sudoku(newBoard));
                        }
                        return sudokus;
                    }
                }
            return sudokus;
        }

        public List<SudokuPossibility>[,] GetSudokuProbabilities(Sudoku sudoku)
        {
            var possibilities = GetSudokuPossibilities(sudoku);
            for (int i = 0; i != sudoku.Size; i++)
            {
                for (int j = 0; j != sudoku.Size; j++)
                {
                    if (possibilities[i, j] != null)
                    {
                        foreach (var possibility in possibilities[i, j])
                        {
                            possibility.Probability = GetSubsquareOccurences(sudoku, possibilities, i, j, possibility.Value);
                        }
                    }
                }
            }
            return possibilities;
        }

        public int GetSubsquareOccurences(Sudoku sudoku, List<SudokuPossibility>[,] possibilities, int x, int y, int value)
        {
            int subsquareLength = (int)Math.Sqrt(sudoku.Size);
            int occurences = 0;
            int startx = (x / subsquareLength) * subsquareLength;
            int starty = (y / subsquareLength) * subsquareLength;
            for (int p = startx; p != startx + subsquareLength; p++)
            {
                for (int q = starty; q != starty + subsquareLength; q++)
                {
                    if (x != p && y != q && possibilities[p, q] != null && possibilities[p,q].Where(l => l.Value == value).Count() > 0)
                    {
                        occurences++;
                    }
                }
            }
            return occurences;
        }

        public List<SudokuPossibility>[,] GetSudokuPossibilities(Sudoku sudoku)
        {
            List<SudokuPossibility>[,] possibilites = new List<SudokuPossibility>[sudoku.Size, sudoku.Size];
            for (int i = 0; i != sudoku.Size; i++)
            {
                for (int j = 0; j != sudoku.Size; j++)
                {
                    if (sudoku.Board[i, j] == 0)
                    {
                        possibilites[i, j] = GetPossibilities(sudoku, i, j);
                    }
                }
            }
            return possibilites;
        }

        public List<SudokuPossibility> GetPossibilities(Sudoku sudoku, int x, int y)
        {
            List<SudokuPossibility> possibilities = new List<SudokuPossibility>();
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
                    possibilities.Add(new SudokuPossibility(i + 1, 0));
            return possibilities;
        }
    }
}

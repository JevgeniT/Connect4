using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Connect4
{
    public class GameEngine
    {
        private int _player;
        private int Score { get; set; }
        private static char[,] Field { get; set; }

        public GameEngine(Settings settings)
        {
            Field = settings.Field;
            Score = settings.Score;
        }


        private static int GetRow()
        {
            return Field.GetUpperBound(0) + 1;
        }

        private static int GetCol()
        {
            return Field.Length / GetRow();
        }

        private void DrawField()
        {
            Console.Clear();
            for (int j = 0; j < GetCol(); j++)
            {
                Console.Write(_player == j ? "x\t" : " \t");
            }

            Console.WriteLine();
            for (int i = 0; i < GetRow(); i++)
            {
                Console.Write("|");
                for (int j = 0; j < GetCol(); j++)
                {
                    Console.Write($"{Field[i, j]} \t");
                }

                Console.Write("|");

                Console.WriteLine();
            }
        }

        private void MakeMove(int pos, bool isHuman)
        {
            char playerChar = isHuman ? 'x' : 'o';

            for (int i = GetRow() - 1; i >= 0; i--)
            {
                if (!Char.IsLetterOrDigit(Field[i, pos]))
                {
                    Field[i, pos] = playerChar;
                    Score++;
                    break;
                }
            }

            DrawField();
        }

        public bool Check(char toCheck, int row, int col, bool checkRow, bool checkCol)
        {
            int[] rx = checkRow ? new[] {1, 2, 3} : !checkCol && !checkRow ? new[] {1, 2, 3} : new[] {0, 0, 0};
            int[] cx = checkCol ? new[] {1, 2, 3} : !checkCol && !checkRow ? new[] {-1, -2, -3} : new[] {0, 0, 0};


            return toCheck == Field[row + rx[0], col + cx[0]] &&
                   toCheck == Field[row + rx[1], col + cx[1]] &&
                   toCheck == Field[row + rx[2], col + cx[2]];
        }

        private bool IsWin()
        {
            for (int row = 0; row < GetRow(); row++)
            {
                char check;
                for (int col = 0; col < GetCol(); col++)
                {
                    if (!char.IsLetter(Field[row, col]))
                    {
                        continue;
                    }

                    check = Field[row, col];
                    if (col < GetCol() - 3 && Check(check, row, col, false, true)
                        || row < GetRow() - 3 && Check(check, row, col, true, false)
                        || (row < GetRow() - 3 && col < GetCol() - 3 && Check(check, row, col, true, true))
                        || row < GetRow() - 3 && col >= GetCol() - 4 && Check(check, row, col, false, false))
                    {
                        Console.WriteLine(check == 'x' ? "You win!" : "You Lose");
                        Console.ReadKey();
                        return true;
                    }
                }
            }

            return false;
        }

        private void ComputerMove()
        {
            Random random = new Random();
            List<int> unique = new List<int>();

            for (int i = 0; i < GetRow(); i++)
            {
                if (!Char.IsLetterOrDigit(Field[0, i]))
                {
                    unique.Add(i);
                }
            }

            var computer = unique.OrderBy(x => random.Next()).Take(3);
            MakeMove(computer.ElementAt(0), false);
        }

        public void Run()
        {
            DrawField();

            Console.WriteLine("---Press A to move left");
            Console.WriteLine("---Press D to move right");
            Console.WriteLine("---Press R to make move");
            Console.WriteLine("---Press V to save current game");

            do
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey();
                switch (pressedKey.Key.ToString())
                {
                    case "A" when _player > 0:
                        _player--;
                        DrawField();
                        break;
                    case "D" when _player < GetCol() - 1:
                        _player++;
                        DrawField();
                        break;
                    case "R":
                        MakeMove(_player, true);
                        ComputerMove();
                        break;
                    case "G":
                    {
                        Menu menu = new Menu();
                        menu.Run();
                        break;
                    }
                    case "V":
                    {
                        Config config = new Config
                        {
                            Score = Score,
                            Field = Field
                        };
                        config.Save(JsonConvert.SerializeObject(config));
                        break;
                    }
                    default:
                        DrawField();
                        break;
                }
            } while (Score >= Field.Length || !IsWin());

            Console.WriteLine("Field is full");
        } //end Run
    }
}
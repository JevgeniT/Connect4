using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Connect4
{
    public class GameEngine
    {
        private int _player ;
        private int Score { get; set; }
        private static char[,] Field { get; set; }

        public GameEngine(Settings settings)
        {
            Field = settings.Field;
            Score = settings.Score;
        }


        private static int GetRow()
        {
         return GameField().GetUpperBound(0) + 1;
        }

        private static int GetCol()
        {
            return GameField().Length / GetRow();
        }

        private static char[,] GameField()
        {    
            return Field;
        }
        private void DrawField()
        {    
           Console.Clear();
            for (int j = 0; j < GetCol(); j++)
            {
                
                Console.Write(_player==j?"x\t":" \t");
            }

            Console.WriteLine();
            for (int i = 0; i < GetRow(); i++)
            {
                Console.Write("|");
                for (int j = 0; j < GetCol(); j++)
                {
                    Console.Write($"{GameField()[i, j]} \t");
                }
                Console.Write("|");

                Console.WriteLine();
            }
        }

        private void MakeMove(int pos, bool isHuman)
        {
            char playerChar = isHuman?'x':'o';
            
            for (int i = GetRow()-1; i >=0; i--)
            {
                if (!Char.IsLetterOrDigit(GameField()[i,pos]))
                {
                    GameField()[i,pos] = playerChar;
                    Score++;
                    break;
                }
            }
            DrawField();
        }

        private bool IsWin()
        {
            for (int row = 0; row < GetRow(); row++)
            {
                char check;
                for (int col = 0; col < GetCol(); col++)
                {
                    if (char.IsLetter(GameField()[row, col]))
                    {
                        check = GameField()[row, col];
                    }
                    else
                    {
                        continue;
                    }
                    if (col <= GetCol() - 4 &&
                        check == GameField()[row, col + 1] &&
                        check == GameField()[row, col + 2] &&
                        check == GameField()[row, col + 3])
                    {
                        return true;
                    }
                    if (row < GetRow() - 3 &&
                        check == GameField()[row + 1, col] &&
                        check == GameField()[row + 2, col] &&
                        check == GameField()[row + 3, col])
                    {
                        return true;
                    }

                    if (row <= GetRow() - 3 &&
                        col <= GetCol() - 3 &&
                        check == GameField()[row + 1, col + 1] &&
                        check == GameField()[row + 2, col + 2] &&
                        check == GameField()[row + 3, col + 3])
                    {
                        return true;
                    }
                }

                for (int i = GetCol() - 1; i >= 0; i--)
                {
                    check = GameField()[row, i];
                    if (row <= GetRow() - 3 &&
                        i >  GetCol() - 4 &&
                        char.IsLetter(check)&&
                        check == GameField()[row + 1, i - 1] &&
                        check == GameField()[row + 2, i - 2] &&
                        check == GameField()[row + 3, i - 3])
                    {
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
            for (int i = 0; i <= GetRow() ; i++)
            {
                if (!Char.IsLetterOrDigit(GameField()[0,i]))
                {
                    unique.Add(i);
                }
            }

            var computer = unique.OrderBy(x => random.Next()).Take(3);
            
             MakeMove(computer.ElementAt(0),false);
            
          
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
                    case "A" when _player>0:
                        _player--;
                        DrawField();
                        break;
                    case "D" when _player < GetCol()-1:
                        _player++;
                        DrawField();
                        break;
                    case "R":
                        MakeMove(_player,true);
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
                        Config config = new Config()
                        {
                            Score = Score,
                            NewField = GameField()
                        };
                        config.Save(JsonConvert.SerializeObject(config));
                        break;
                    }
                    default:
                        DrawField();
                        break;
                    
                }

                if (IsWin())
                {
                    Console.WriteLine("win");
                    Console.ReadKey();
                }
            
            } while (Score>=GameField().Length ||  !IsWin());
            Console.WriteLine("Field is full");
        }//end Run
    
    }
}
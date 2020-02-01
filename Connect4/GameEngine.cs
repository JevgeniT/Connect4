using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                Console.Write(_player==j?"x\t":" \t");
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
            char playerChar = isHuman?'x':'o';
            
            for (int i = GetRow()-1; i >=0; i--)
            {
                if (!Char.IsLetterOrDigit(Field[i,pos]))
                {
                    Field[i,pos] = playerChar;
                    Score++;
                    break;
                }
            }
            DrawField();
        }

        private bool CheckColumn(char toCheck, int row, int col)
        {
            return toCheck == Field[row+1, col ] && toCheck == Field[row+2, col ] && toCheck == Field[row+3, col ];
        }

        private bool CheckRow(char toCheck, int row, int col)
        { 
            return toCheck == Field[row, col+1 ] && toCheck == Field[row, col +2] && toCheck == Field[row, col+3 ];
        }

        private bool CheckTopDownDiagonal(char toCheck,int row, int col)
        {
            return toCheck == Field[row+1, col + 1] && toCheck == Field[row+2, col + 2] && toCheck == Field[row+3, col + 3];
        }

        private bool CheckBottomUpDiagonal(char toCheck,int row, int col)
        {
            return toCheck == Field[row+1, col - 1] && toCheck == Field[row+2, col - 2] && toCheck == Field[row+3, col - 3];
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
                 
                    if (col <= GetCol() - 4 && CheckRow(check, row , col))
                    {
                        return true;
                    }
                    if (row < GetRow() - 3 && CheckColumn(check, row, col))
                    {
                        return true;
                    }

                    if (row <= GetRow() - 3 &&
                        col <= GetCol() - 3 && CheckTopDownDiagonal(check, row, col))
                    {
                        return true;
                    }
                }

                for (int col = GetCol() - 1; col >= 0; col--)
                {
                    check = Field[row, col];
                    if (row <= GetRow() - 3 &&
                        col >  GetCol() - 4 &&
                        char.IsLetter(check)&&
                        CheckBottomUpDiagonal(check,row,col)
                        )
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
                if (!Char.IsLetterOrDigit(Field[0,i]))
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

                if (IsWin())
                {
                    Console.WriteLine("win");
                    Console.ReadKey();
                }
            
            } while (Score>=Field.Length ||  !IsWin());
            Console.WriteLine("Field is full");
        }//end Run
    
    }
}
using System;

namespace Connect4
{
    public class Settings
    {
         public char[,] Field { get; set; } = new char[Length,Width];
        
                public static int Length { get; set; } = 6;
                
                public static int Width { get; set; } = 7;
        
               
                public int Score { get; set; }
        
                
                
                public void GetLength()
                {
                    Console.Write("Enter field length: ");
                    var length = Console.ReadLine();
                    int x;
                    if (length != null && int.TryParse(length, out x) && x>Length)
                    {
                     Length = x;
                     Console.WriteLine("Length has been changed to {0}",Length);
                    }
                    else
                    {
                      Console.WriteLine("Cannot be less than 6,empty or non-integer");
                    }
        
                    Console.ReadKey();
                }
                public void GetWidth()
                {
                    Console.Write("Enter field width: ");
                    var width = Console.ReadLine();
                    int x;
                    if (width != null && int.TryParse(width, out x) && x>=Width)
                    {
                        Width = x;
                        Console.WriteLine("Width has been changed to {0}",Width);
                    }
                    else
                    {
                        Console.WriteLine("Cannot be less than 7,empty or non-integer");
                    }
                    Console.ReadKey();
                }
    }
}
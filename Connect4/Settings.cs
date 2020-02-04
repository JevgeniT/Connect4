using System;

namespace Connect4
{
    public class Settings
    {
        public char[,] Field { get; set; } = new char[Length, Width];

        private static int Length { get; set; } = 6;

        private static int Width { get; set; } = 7;

        public int Score { get; set; }


        public void SetLength()
        {
            Console.Write("Enter field length: ");
            var length = Console.ReadLine();
            if (length != null && int.TryParse(length, out var x) && x < 12 && x>6)
            {
                Length = x;
                Console.WriteLine("Length has been changed to {0}", Length);
            }
            else
            {
                Console.WriteLine("Invalid input! Must be integer between 6-12");
            }

            Console.ReadKey();
        }

        public void SetWidth()
        {
            Console.Write("Enter field width: ");
            var width = Console.ReadLine();
            if (width != null && int.TryParse(width, out var x) && x < 12 && x>7)
            {
                Width = x;
                Console.WriteLine("Width has been changed to {0}", Width);
            }
            else
            {
                Console.WriteLine("Invalid input! Must be integer between 7-12");
            }
            Console.ReadKey();
        }
    }
}
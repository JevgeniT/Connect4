using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Connect4
{
    public class Config
    {
        public char[,] NewField { get; set; }
        public int Score { get; set; }

        private string Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)+"/";

        private bool ValidInput(string fileName)
        {
            if (fileName.Length < 3)
            {
                Console.WriteLine("Size has to be at least 3");
                return false;
            }

            return true;
        }
        
        public void Save(string game)
        {
            
            Console.Write("ENTER FILE NAME: ");
            var fileName = Console.ReadLine().ToLower();
           
            if (File.Exists(Path+fileName+".json") && ValidInput(fileName))
            {
                Console.WriteLine("The file already exists. Do you want to overwrite it? [y/n]");
                if (Console.ReadLine() == "y")
                {
                    InnerSave(fileName, game);
                }
                else
                {
                    Console.WriteLine("DENIED");
                }
            }
            else if (!File.Exists(Path+fileName+".json") && ValidInput(fileName))
            {
                InnerSave(fileName, game);
            }
        }

        private void InnerSave(string fileName, string game)
        {
            File.WriteAllText(Path+fileName+".json", game);
            Console.WriteLine("GAME SAVED");
        }
        public Settings LoadGame(string name)
        {    
            var loadedSettings = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path+name));
            return new Settings {Field = loadedSettings.NewField, Score = loadedSettings.Score};
        }

       
        public List<string> ListAll()
        {    
            return new DirectoryInfo(Path).GetFiles("*.json").Select(o => o.Name).ToList();
        }


    }
}
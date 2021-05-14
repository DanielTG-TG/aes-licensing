using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licesing;

namespace Licesing
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Title = "Licesing system.";
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Welcome to the licesing system.");

                if (File.Exists("License.txt"))
                {
                    byte[] lic = File.ReadAllBytes("License.txt");
                    Console.WriteLine("Current license: " + SymmetricEncryptor.DecryptToString(lic, "k3bTn5dQ4DD8NHh6MBgGusuRAGNorsUH"));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    Environment.Exit(1);
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Please input your license key.");
                Console.Write("> ");

                string input = Console.ReadLine();
                byte[] enc = SymmetricEncryptor.EncryptString(input, "k3bTn5dQ4DD8NHh6MBgGusuRAGNorsUH");

                File.WriteAllBytes("License.txt", enc);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully saved the license.");
                Console.WriteLine("Press any key to exit.");
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There was an error saving the license.");
                Console.WriteLine("Press any key to exit.");
            }
            

            Console.ReadKey();
        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

namespace OC_Lab_1;

class Program
{
    public static void Main(string[] args)
    {
        string action;

        while (true)
        {
            Console.Clear();
            Functions.ShowFunctions();
            Console.WriteLine("Выберите действие, которое хотите совершить: ");
            action = Console.ReadLine();
            switch (action)
            {
                case "1":
                    Functions.ShowDriveInfo();
                    Functions.FinishOfTheFunction();
                    break;
                case "2":
                    Functions.WorkWithFile();
                    System.Threading.Thread.Sleep(100);
                    Functions.FinishOfTheFunction();
                    break;
                case "3":
                    Functions.WorkWithJSON();
                    Functions.FinishOfTheFunction();
                    break;
                case "4":
                    Functions.WorkWithXML();
                    Functions.FinishOfTheFunction();
                    break;
                case "5":
                    Functions.WorkWithZIP();
                    Functions.FinishOfTheFunction();
                    break;
                case "6":
                    Console.WriteLine("Выход из программы...");
                    return;
                default:
                    Console.WriteLine("Неизвестный номер меню!");
                    Console.ReadLine();
                    break;
            }
        }
    }
}

using System;
using System.Globalization;
using System.Text;

namespace MusicCollection
{
    internal class Program
    {
        const int Capacity = 100;

        static int[] ids = new int[Capacity];
        static string[] artists = new string[Capacity];
        static string[] titles = new string[Capacity];
        static int[] years = new int[Capacity];
        static int[] durations = new int[Capacity];
        static DateTime[] addedDates = new DateTime[Capacity];
        static bool[] isFavorites = new bool[Capacity];

        static int count = 0;
        static int nextId = 1;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Seed();

            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("=== Музична колекція: облік треків ===");
                Console.WriteLine("1. Додати трек");
                Console.WriteLine("2. Показати всі треки");
                Console.WriteLine("3. Знайти трек за номером");
                Console.WriteLine("4. Видалити трек за номером");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");

                string choice = (Console.ReadLine() ?? "").Trim();
                switch (choice)
                {
                    case "1": AddTrack(); break;
                    case "2": PrintAll(); break;
                    case "3": FindById(); break;
                    case "4": DeleteById(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Немає такого пункту меню."); break;
                }
            }

            Console.WriteLine("Роботу завершено.");
        }
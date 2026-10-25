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

        static void Store(string artist, string title, int year, int duration, DateTime addedDate, bool isFavorite)
        {
            ids[count] = nextId;
            artists[count] = artist;
            titles[count] = title;
            years[count] = year;
            durations[count] = duration;
            addedDates[count] = addedDate;
            isFavorites[count] = isFavorite;
            count++;
            nextId++;
        }

        static void Seed()
        {
            Store("Queen", "Bohemian Rhapsody", 1975, 354, new DateTime(2025, 10, 10), true);
            Store("The Beatles", "Hey Jude", 1968, 431, new DateTime(2025, 10, 15), true);
            Store("Daft Punk", "Get Lucky", 2013, 248, new DateTime(2025, 11, 1), false);
        }

        static int IndexOfId(int id)
        {
            for (int i = 0; i < count; i++)
            {
                if (ids[i] == id) return i;
            }
            return -1;
        }

        static void AddTrack()
        {
            if (count >= Capacity)
            {
                Console.WriteLine($"Сховище заповнене ({Capacity}).");
                return;
            }

            Console.WriteLine("--- Новий трек ---");

            Console.Write("Виконавець: ");
            string artist = (Console.ReadLine() ?? "").Trim();
            if (artist.Length == 0) { Console.WriteLine("Порожнє поле."); return; }

            Console.Write("Назва треку: ");
            string title = (Console.ReadLine() ?? "").Trim();
            if (title.Length == 0) { Console.WriteLine("Порожнє поле."); return; }

            Console.Write("Рік випуску: ");
            if (!int.TryParse(Console.ReadLine(), out int year) || year < 1000 || year > DateTime.Now.Year)
            {
                Console.WriteLine("Некоректний рік."); return;
            }

            Console.Write("Тривалість у секундах: ");
            if (!int.TryParse(Console.ReadLine(), out int duration) || duration <= 0)
            {
                Console.WriteLine("Некоректна тривалість."); return;
            }

            Console.Write("Дата додавання (дд.мм.рррр): ");
            bool dateOk = DateTime.TryParseExact(
                Console.ReadLine(), "dd.MM.yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None,
                out DateTime addedDate
            );
            if (!dateOk) { Console.WriteLine("Некоректна дата."); return; }

            Console.Write("Чи в улюблених (так/ні): ");
            string favInput = (Console.ReadLine() ?? "").Trim().ToLower();
            bool isFavorite = (favInput == "так" || favInput == "yes" || favInput == "1" || favInput == "true");

            Store(artist, title, year, duration, addedDate, isFavorite);
            Console.WriteLine($"Додано трек №{ids[count - 1]} у позицію {count - 1}. Зайнято: {count}/{Capacity}.");
        }

        static void PrintAll()
        {
            if (count == 0)
            {
                Console.WriteLine("Колекція порожня.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine(" № | Виконавець   | Назва        | Рік  | Тривалість | Дата додавання | Улюблений");
            Console.WriteLine("---+--------------+--------------+------+------------+----------------+----------");

            int totalDuration = 0;
            for (int i = 0; i < count; i++)
            {
                string favStr = isFavorites[i] ? "так" : "ні";
                Console.WriteLine($"{ids[i],3} | {artists[i],-12} | {titles[i],-12} | {years[i],4} | {durations[i],6} сек  | {addedDates[i]:dd.MM.yyyy}     | {favStr,-9}");
                totalDuration += durations[i];
            }

            Console.WriteLine("---+--------------+--------------+------+------------+----------------+----------");
            Console.WriteLine($"Усього треків: {count}, Загальна тривалість: {totalDuration} сек ({totalDuration / 60} хв {totalDuration % 60} сек).");
        }

        static void FindById()
        {
            Console.Write("Номер треку для пошуку: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Потрібно ввести число."); return;
            }

            int index = IndexOfId(id);
            if (index == -1)
            {
                Console.WriteLine($"Треку №{id} немає."); return;
            }

            Console.WriteLine($"Знайдено в позиції {index}:");
            Console.WriteLine($"  виконавець: {artists[index]}");
            Console.WriteLine($"  назва: {titles[index]}");
            Console.WriteLine($"  рік: {years[index]}");
            Console.WriteLine($"  тривалість: {durations[index]} сек");
            Console.WriteLine($"  дата додавання: {addedDates[index]:dd.MM.yyyy}");
            Console.WriteLine($"  улюблений: {(isFavorites[index] ? "так" : "ні")}");
        }

        static void DeleteById()
        {
            Console.Write("Номер треку для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Потрібно ввести число."); return;
            }

            int index = IndexOfId(id);
            if (index == -1)
            {
                Console.WriteLine($"Треку №{id} немає."); return;
            }

            Console.WriteLine($"Видалено трек №{ids[index]} ({artists[index]} - {titles[index]}).");

            for (int i = index; i < count - 1; i++)
            {
                ids[i] = ids[i + 1];
                artists[i] = artists[i + 1];
                titles[i] = titles[i + 1];
                years[i] = years[i + 1];
                durations[i] = durations[i + 1];
                addedDates[i] = addedDates[i + 1];
                isFavorites[i] = isFavorites[i + 1];
            }

            count--;

            ids[count] = 0;
            artists[count] = "";
            titles[count] = "";
            years[count] = 0;
            durations[count] = 0;
            addedDates[count] = default;
            isFavorites[count] = false;

            Console.WriteLine($"Зайнято комірок: {count}/{Capacity}.");
        }
    }
}
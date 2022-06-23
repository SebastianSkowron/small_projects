using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotto
{
    class Program
    {

        static int cumulation;
        static int start = 40;
        static Random rnd = new Random();
        static void Main(string[] args)
        {

            int money = start;
            int day = 0;

            do
            {
                money = start;
                day = 0;
                ConsoleKey choice;

                do
                {
                    cumulation = rnd.Next(2, 37) * 1000000;
                    day++;
                    int tickets = 0;
                    List<int[]> ticket = new List<int[]>();

                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Dzien: {0}", day);
                        Console.WriteLine("Witaj w grze LOTTO, dzis do wygrania jest {0} zl.", cumulation);
                        Console.WriteLine("\nStan konta: {0} zl", money);
                        ShowTicket(ticket);

                        // MENU START
                        if (money >= 3 && tickets < 8)
                        {
                            Console.WriteLine("\n1 - Postaw los -3zl [{0}/8]", tickets + 1);
                        }
                        Console.WriteLine("2 - Sprawdz kupon - losowanie");
                        Console.WriteLine("3 - Zakoncz gre");
                        // MENU END

                        choice = Console.ReadKey().Key;

                        if (choice == ConsoleKey.D1 && money >= 3 && tickets < 8)
                        {
                            ticket.Add(Bet());
                            money -= 3;
                            tickets++;
                        }

                    } while (choice == ConsoleKey.D1);

                    Console.Clear();

                    if (ticket.Count > 0)
                    {
                        int win = Check(ticket);

                        if (win > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nBrawo wygrales {0} zl w tym losowaniu!", win);
                            Console.ResetColor();
                            money += win;
                        }

                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nNiestety, nic nie wygrales.");
                            Console.ResetColor();
                        }
                    }

                    else
                    {
                        Console.WriteLine("Nie miales losow w tym losowaniu.");
                    }

                    Console.WriteLine("Enter - kontynuuj.");
                    Console.ReadKey();

                } while (money >= 3 && choice != ConsoleKey.D3);

                Console.Clear();
                Console.WriteLine("Dzien {0}.\nKoniec gry, twoj wynik to: {1} zl.", day, money - start);
                Console.WriteLine("Enter - zacznij od nowa.");

            } while (Console.ReadKey().Key == ConsoleKey.Enter);

        }

        private static int Check(List<int[]> ticket)
        {
            int win = 0;
            int[] drawn = new int[6];
            for (int i = 0; i < drawn.Length; i++)
            {
                int coupon = rnd.Next(1, 50);
                if (!drawn.Contains(coupon))
                {
                    drawn[i] = coupon;
                }
                else
                {
                    i--;
                }
            }
            Array.Sort(drawn);
            Console.WriteLine("Wylosowane liczby to: ");
            foreach (int number in drawn)
            {
                Console.Write(number + ", ");
            }
            int[] gotcha = CheckCoupon(ticket, drawn);
            int value = 0;

            Console.WriteLine();
            if (gotcha[0] > 0)
            {
                value = gotcha[0] * 24;
                Console.WriteLine("3 trafienia: {0} + {1} zl", gotcha[0], value);
                win += value;
            }

            if (gotcha[1] > 0)
            {
                value = gotcha[1] * rnd.Next(100, 301);
                Console.WriteLine("4 trafienia: {0} + {1} zl", gotcha[1], value);
                win += value;
            }

            if (gotcha[2] > 0)
            {
                value = gotcha[2] * rnd.Next(4000, 8001);
                Console.WriteLine("5 trafien: {0} + {1} zl", gotcha[2], value);
                win += value;
            }

            if (gotcha[3] > 0)
            {
                value = (gotcha[3] * cumulation) / (gotcha[3] + rnd.Next(0, 5));
                Console.WriteLine("6 trafien: {0} + {1} zl", gotcha[3], value);
                win += value;
            }
            return win;
        }

        private static int[] CheckCoupon(List<int[]> ticket, int[] drawn)
        {
            int[] wins = new int[4];
            int i = 0;

            Console.WriteLine("\n\nTwoj kupon: ");
            foreach (int[] draw in ticket)
            {
                i++;
                Console.Write(i + ": ");
                int hits = 0;
                foreach (int number in draw)
                {
                    if (drawn.Contains(number))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(number + ", ");
                        Console.ResetColor();
                        hits++;
                    }
                    else
                    {
                        Console.Write(number + ", ");
                    }
                }
                switch (hits)
                {
                    case 3:
                        wins[0]++;
                        break;
                    case 4:
                        wins[1]++;
                        break;
                    case 5:
                        wins[2]++;
                        break;
                    case 6:
                        wins[3]++;
                        break;
                }
                Console.WriteLine(" - Trafiono [{0}/6]", hits);
            }
            return wins;
        }

        private static int[] Bet()
        {
            int[] numbers = new int[6];
            int number = -1;
            for (int i = 0; i < numbers.Length; i++)
            {
                number = -1;
                Console.Clear();
                Console.Write("Postawione liczby: ");
                foreach (int n in numbers)
                {
                    if (n > 0)
                    {
                        Console.Write(n + ", ");
                    }
                }
                Console.WriteLine("\n\nWybierz liczbe od 1 do 49: ");
                Console.Write("[{0}/6] ", i + 1);
                bool correct = int.TryParse(Console.ReadLine(), out number);
                if (correct && number >= 1 && number <= 49 && !numbers.Contains(number))
                {
                    numbers[i] = number;
                }
                else
                {
                    Console.WriteLine("Niestety, bledna liczba.");
                    i--;
                    Console.ReadKey();
                }
            }
            Array.Sort(numbers);
            return numbers;
        }

        private static void ShowTicket(List<int[]> ticket)
        {
            if (ticket.Count == 0)
            {
                Console.WriteLine("Nie postawiles jeszcze zadnych losow.");
            }
            else
            {
                int i = 0;
                Console.WriteLine("\nTwoj kupon: ");

                foreach (int[] ticketinhio in ticket)
                {
                    i++;
                    Console.Write(i + ": ");

                    foreach (int number in ticketinhio)
                    {
                        Console.Write(number + ", ");
                    }
                    Console.WriteLine();
                }
            }
        }

    }
}

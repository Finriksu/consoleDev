using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FinalProject
{
    struct Article
    {
        public string title;
        public int price;

        public override string ToString()
        {
            string display = (title + " " + price + "€");
            return display;
        }
    }
    class Program
    {
        /// <summary>
        /// Sets the price goal for sales
        /// </summary>
        /// <returns>The price goal</returns>
        static int SetGoal()
        {
            string input = "";
            int goal;
            
            Console.WriteLine("Syötä myyntitavoite:");
            while(int.TryParse(input, out goal) == false)
            {
                input = Console.ReadLine();
                int.TryParse(input, out goal);
                if(int.TryParse(input, out goal) == false)
                {
                    Console.WriteLine("Virhe! Syötä arvo numeroina");
                }
            }

            return goal;
        }
        /// <summary>
        /// Add new sold articles to array and calculates the change in the price goal.
        /// </summary>
        /// <param name="goal">Price goal</param>
        /// <param name="sum">Sum of sold article prices</param>
        /// <returns>Returns array of sold articles</returns>
        static List<Article> AddArticles(int goal, out int sum)
        {
            Article newArticle = new Article();
            List<Article> soldArticles = new List<Article>();

            string input = "";
            int price = 0, goalOver = 0, n = 1;
            sum = 0;

            while(input != "exit")
            {
                Console.WriteLine("Syötä tuotteen nimi:           Syötä " + "exit" + " lopettaaksesi");
                input = Console.ReadLine();
                if(input == "exit")
                {
                    break;
                }
                else
                {
                    newArticle.title = input;
                }

                Console.WriteLine("Syötä tuotteen hinta:");
                input = Console.ReadLine();
                if (int.TryParse(input, out price))
                {
                    newArticle.price = price;
                    soldArticles.Add(newArticle);
                    sum = sum + price;
                }
                else
                {
                    Console.WriteLine("Virhe! Tuotetta ei lisätty.");
                }
                CheckGoal(ref goal, ref goalOver, ref n, price);
            }
            
            return soldArticles;
        }
        /// <summary>
        /// Checks and calculates how much is left of the goal or by how much it has been passed
        /// </summary>
        /// <param name="goal">Price goal</param>
        /// <param name="goalOver">By how much the goal has been passed</param>
        /// <param name="n">Variable to check if it is the first time that goal is less than a zero</param>
        /// <param name="price">Price of the sold article</param>
        static void CheckGoal(ref int goal, ref int goalOver, ref int n, int price)
        {
            goal = goal - price;

            if (goal > 0)
            {
                Console.WriteLine("Tavoitteeseen: {0}€", goal);
            }
            else if (goal < 0)
            {
                goalOver = goal * -1;
                Console.WriteLine("Tavoite ylitetty {0}€:lla", goalOver);
            }
            else if (goal == 0)
            {
                Console.WriteLine("Tavoite saavutettu!");
            }
        }
        /// <summary>
        /// Saves sold articles, total income and goal performance to a textfile.
        /// </summary>
        /// <param name="soldArticles">List of sold articles</param>
        /// <param name="sum">Sum of sold article prices</param>
        /// <param name="goal">Price goal</param>
        static void SaveToFile(List<Article> soldArticles, int sum, int goal)
        {
            StreamWriter soldFile = null;
            try
            {
                soldFile = new StreamWriter(@"D:\\Temp\\soldarticles.txt");

                foreach (Article value in soldArticles)
                {
                    soldFile.WriteLine(value);
                }

                soldFile.WriteLine("\nYhteensä: {0}€", sum);

                soldFile.WriteLine("\nMyyntitavoite: {0}€", goal);

                if (sum > goal)
                {
                    soldFile.WriteLine("\nTavoite ylitetty {0}€:lla", (sum - goal));
                }
                else if (sum == goal)
                {
                    soldFile.WriteLine("\nTavoite saavutettu!");
                }
                else
                {
                    soldFile.WriteLine("\nTavoitteeseen: {0}€", (goal - sum));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                soldFile.Close();
            }
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            List<Article> soldArticles;

            int goal;

            goal = SetGoal();
            soldArticles = AddArticles(goal, out int sum);
            SaveToFile(soldArticles, sum, goal);
        }
    }
}

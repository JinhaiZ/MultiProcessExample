using System;
using System.Collections.Generic;
using System.Threading;

namespace Premier
{
    class NombrePremier
    {
        [STAThread]
        static void Main(string[] args)
        {
            // un programme simple du critère d'Eratosthène pour la 
            // recherche de nombres premiers suoérieur à 1
            // On ne s'arrête pas à la racine carrée

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetWindowSize(7, 20);  // la taille de la fenêtre


            for (int p = 1; p < 1000000; p++)
            {
                int i = 2;
                while ((p % i) != 0 && i < p)
                {
                    i++;
                }
                if (i == p)
                    Console.WriteLine(p.ToString());
                Thread.Sleep(50);

            }
        }


    }
}

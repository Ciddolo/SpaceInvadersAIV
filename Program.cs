using Aiv.Draw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareInvadersB
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.Play();

            Console.WriteLine("Score: " + Game.GetScore());
            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alura.Loja.Testes.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using ( var contexto = new LojaContext())
            {
                var produtos = contexto.Produtos.ToList();

                foreach (var p in produtos) 
                {
                    Console.WriteLine(p);
                }

                //var p1 = produtos.First();
                //p1.Nome = "Harry Potter E O Prisioneiro de Askaban";

                //contexto.SaveChanges();

                Console.WriteLine("=================");

                foreach(var e in contexto.ChangeTracker.Entries())
                {
                    Console.WriteLine(e.State);
                }

                var p1 = produtos.Last();
                p1.Nome = "Harr Potter e as Reliquias da Morte";

                Console.WriteLine("=================");

                foreach (var e in contexto.ChangeTracker.Entries())
                {
                    Console.WriteLine(e.State);
                }

            }
        }
    }
}

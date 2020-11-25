using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

            var p1 = new Produto() { Nome = "Panettone", Categoria = "Comida", PrecoUnidade = 15.00, Unidade ="Gramas"};
            var p2 = new Produto() { Nome = "Refrigerante", Categoria = "Bebidas", PrecoUnidade = 5.00, Unidade = "Litros" };
            var p3 = new Produto() { Nome = "Arroz", Categoria = "Comida", PrecoUnidade = 35.00, Unidade = "Gramas" };

            var promocaoNatal = new Promocao();
            promocaoNatal.Descricao = "Natal Mais Feliz";
            promocaoNatal.DataInicio = DateTime.Now;
            promocaoNatal.DataTermino = DateTime.Now.AddMonths(2);

            promocaoNatal.IncluiProduto(p1);
            promocaoNatal.IncluiProduto(p2);
            promocaoNatal.IncluiProduto(p3);

            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                //add produto na promocao

                //contexto.Promocaos.Add(promocaoNatal);
                //ExibeEntries(contexto.ChangeTracker.Entries());

               //remove promocao por cascata
                var promocao = contexto.Promocaos.Find(1);
                contexto.Promocaos.Remove(promocao);

                contexto.SaveChanges();

            }     
        }

        private static void ExibeEntries(IEnumerable<EntityEntry> entries)
        {
            foreach (var e in entries)
            {
                Console.WriteLine(e.Entity.ToString() + " - " + e.State);
            }
        }
    }
}

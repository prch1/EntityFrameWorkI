using Microsoft.EntityFrameworkCore;
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
            using (var contexto = new LojaContext())
            {

                {
                    var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                    var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                    loggerFactory.AddProvider(SqlLoggerProvider.Create());

                    var cliente = contexto
                        .Clientes
                        .Include(c => c.EnderecoDeEntrega)
                        .FirstOrDefault();

                    Console.WriteLine($"Endereco de Entrega : {cliente.EnderecoDeEntrega.Logradouro}");

                    var produto = contexto
                        .Produtos
                        .Where(p => p.Id == 8)
                        .FirstOrDefault();

                    contexto.Entry(produto)
                    .Collection(p => p.Compras)
                    .Query()
                    .Where(c => c.Preco > 1)
                    .Load();


                    Console.WriteLine($"Mostrando as compras do produto {produto.Nome}");

                    foreach (var item in produto.Compras)
                    {
                        Console.WriteLine(item.Produto.Nome);
                    }

                }
            }

        }

        private static void ExibeProdutoPromocao()
        {
            using (var contexto2 = new LojaContext())
            {
                var serviceProvider = contexto2.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var promocao = contexto2
                              .Promocaos
                              .Include(p => p.Produtos)
                              .ThenInclude(pp => pp.Produto)
                              .FirstOrDefault();

                Console.WriteLine("\nMostrando os produtos da promoção...");

                foreach (var item in promocao.Produtos)
                {
                    Console.WriteLine(item.Produto);
                }

            }
        }

        private static void IncluirPromocao()
        {
            using (var contexto = new LojaContext())
            {
               
                
                
                var promocao = new Promocao();
                promocao.Descricao = "Queima Total 2020";
                promocao.DataInicio = new DateTime(2020, 12, 01);
                promocao.DataTermino = new DateTime(2020, 12, 31);

                var produtos = contexto
                              .Produtos
                              .Where(p => p.Categoria == "Bebidas")
                              .ToList();

                foreach (var item in produtos)
                {
                    promocao.IncluiProduto(item);
                }

                contexto.Promocaos.Add(promocao);

                ExibeEntries(contexto.ChangeTracker.Entries());
                contexto.SaveChanges();
            }
        }

        private static void UmParaUm()
        {
            var cli = new Cliente();
            cli.Nome = "Nick Jonas";
            cli.EnderecoDeEntrega = new Endereco()
            {
                Numero = 25,
                Logradouro = "Rua X",
                Complemento = "AAA",
                Bairro = "Rural",
                Cidade = "Ocean"
            };

            using (var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                contexto.Clientes.Add(cli);
                contexto.SaveChanges();
            }
        }

        private static void MuitosParaMuitos()
        {

            var p1 = new Produto() { Nome = "Panettone", Categoria = "Comida", PrecoUnidade = 15.00, Unidade = "Gramas" };
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

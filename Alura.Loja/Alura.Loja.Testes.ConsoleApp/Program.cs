﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            using ( var contexto = new LojaContext())
            {
                var serviceProvider = contexto.GetInfrastructure<IServiceProvider>();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(SqlLoggerProvider.Create());

                var produtos = contexto.Produtos.ToList();

                ExibeEntries(contexto.ChangeTracker.Entries());

                //ADDed - Unchamged
                //var novoProduto = new Produto()
                //{
                //    Nome = "Desinfetante",
                //    Categoria = "Limpeza",
                //    Preco = 2.99
                //};

                //contexto.Produtos.Add(novoProduto);

                //ExibeEntries(contexto.ChangeTracker.Entries());

                //contexto.SaveChanges();

                //ExibeEntries(contexto.ChangeTracker.Entries());

               
                //UPDATE-Modified
                //var p1 = produtos.Last();
                //p1.Nome = "Percy Jackson o Ladrão de Raios";

                //Console.WriteLine("=================");
                //foreach (var e in contexto.ChangeTracker.Entries())
                //{
                //    Console.WriteLine(e);
                //}

                //contexto.SaveChanges();


                //DELETED
                var p1 = produtos.First();
                contexto.Produtos.Remove(p1);

                ExibeEntries(contexto.ChangeTracker.Entries());

                //DETACHED
                contexto.SaveChanges();

                ExibeEntries(contexto.ChangeTracker.Entries());

                var entry = contexto.Entry(p1);
                Console.WriteLine("\n\n" + entry.Entity.ToString() + " - " + entry.State);

            }
        }

        private static void ExibeEntries(IEnumerable<EntityEntry> entries)
        {
            Console.WriteLine("=================");
            foreach (var e in entries)
            {
                Console.WriteLine(e.Entity.ToString() + " - " + e.State);
            }
        }
    }
}

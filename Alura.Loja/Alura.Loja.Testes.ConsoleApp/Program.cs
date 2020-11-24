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
            //GravarUsandoAdoNet();
            // GravarUsandoEntity();
            //RecuperarProdutosEntity();
            //ExcluirProdutosEntity();
           // RecuperarProdutosEntity();

            AtualizarProdutosEntity();

            Console.ReadKey();
        }

        private static void AtualizarProdutosEntity()
        {
            GravarUsandoEntity();     
            RecuperarProdutosEntity();

            using(var repo = new ProdutoDAOEntity())
            {
                Produto primeiro = repo.Produtos().First();
                primeiro.Nome = "Harry Potter e o Calice de Fogo v2 - Edit";
                repo.Atualizar(primeiro);
            }
            RecuperarProdutosEntity();
        }

        private static void ExcluirProdutosEntity()
        {
            using(var repo = new ProdutoDAOEntity())
            {
                IList<Produto> produtos = repo.Produtos();

                foreach(var item in produtos)
                {
                    repo.Remover(item);
                }
            }
        }

        private static void RecuperarProdutosEntity()
        {
            using(var repo = new ProdutoDAOEntity())
            {
                IList<Produto> produtos = repo.Produtos();
                Console.WriteLine("Encontrados {0} Produto(s)", produtos.Count);
                
                foreach(var item in produtos)
                {
                    Console.WriteLine(item.Nome);
                }

            }
        }

        private static void GravarUsandoEntity()
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e o Cálise de Fogo";
            p.Categoria = "Livros";
            p.Preco = 20.99;

            using (var contexto = new ProdutoDAOEntity())
            {
                contexto.Adicionar(p);
            }

        }

        private static void GravarUsandoAdoNet()
        {
            Produto p = new Produto();
            p.Nome = "Harry Potter e a Ordem da Fênix";
            p.Categoria = "Livros";
            p.Preco = 19.89;

            using (var repo = new ProdutoDAO())
            {
                repo.Adicionar(p);
            }
        }
    }
}

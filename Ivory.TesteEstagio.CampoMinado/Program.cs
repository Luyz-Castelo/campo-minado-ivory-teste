using System;

namespace Ivory.TesteEstagio.CampoMinado
{
    class Program
    {
        static int TAMANHO = 9;

        static void Main(string[] args)
        {
            var campoMinado = new CampoMinado();
            Console.WriteLine("Início do jogo\n=========");
            Console.WriteLine(campoMinado.Tabuleiro);

            // Realize sua codificação a partir deste ponto, boa sorte!

            while (campoMinado.JogoStatus == 0) {
                string[][] tabuleiroMatriz = ConstruirMatriz(campoMinado.Tabuleiro);
                EscreverMatriz(tabuleiroMatriz);

                Jogar(tabuleiroMatriz, campoMinado);
            }

            if (campoMinado.JogoStatus == 1) {
                EscreverMatriz(ConstruirMatriz(campoMinado.Tabuleiro));
                Console.WriteLine("Parabéns, você ganhou o jogo!");
            } else {
                EscreverMatriz(ConstruirMatriz(campoMinado.Tabuleiro));
                Console.WriteLine("Game over!");
            }
        }

        static void Jogar(string[][] tabuleiroMatriz, CampoMinado campoMinado) {
            for (int i = 0; i < TAMANHO; i++)
            {
                for (int j = 0; j < TAMANHO; j++)
                {
                    if (EhDesconhecido(tabuleiroMatriz[i][j]) && EhSeguro(i, j, tabuleiroMatriz)) {
                        Console.WriteLine($"Abrindo posição {i + 1} {j + 1}");
                        campoMinado.Abrir(i + 1, j + 1);
                        return;
                    }
                }
            }
        }

        static bool EhSeguro(int linha, int coluna, string[][] tabuleiroMatriz) {
            return true;
        }

        static bool EhDesconhecido(string tabuleiroCelula) {
            return tabuleiroCelula == "-";
        }

        static string[][] ConstruirMatriz(string tabuleiroTexto) {
            string[][] tabuleiroMatriz = new string[TAMANHO][];
            string[] linhas = tabuleiroTexto.Split("\r\n");
            for (int i = 0; i < TAMANHO; i++)
            {
                tabuleiroMatriz[i] = new string[TAMANHO];
                for (int j = 0; j < TAMANHO; j++)
                {
                    tabuleiroMatriz[i][j] = linhas[i][j].ToString();
                }
            }

            return tabuleiroMatriz;
        }

        static void EscreverMatriz(string[][] tabuleiroMatriz) {
            Console.WriteLine("=================");
            foreach (var linha in tabuleiroMatriz)
            {
                Console.WriteLine(String.Join(" ", linha));
            }
            Console.WriteLine("=================");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

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

            List<int[]> posicoesBomba = new List<int[]>();
            int jogada = 0;

            while (campoMinado.JogoStatus == 0) {
                Console.WriteLine($"\r\n>>> Jogada: {jogada + 1}");

                Jogar(campoMinado, posicoesBomba);

                jogada++;
            }

            if(campoMinado.JogoStatus == 1) {
                Console.WriteLine("\r\nParabéns, você ganhou o jogo!");
            } else {
                Console.WriteLine("\r\nGame over!");
            }
        }

        static void Jogar(CampoMinado campoMinado, List<int[]> posicoesBomba) {
            string[][] tabuleiroMatriz = ConstruirMatriz(campoMinado.Tabuleiro, posicoesBomba);

            for (int i = 0; i < TAMANHO; i++)
            {
                for (int j = 0; j < TAMANHO; j++)
                {
                    if(EhNumeroDiferenteDeZero(tabuleiroMatriz[i][j])) {
                        int numeroBombasCelulaAtual = int.Parse(tabuleiroMatriz[i][j]);

                        List<int[]> posicoesVizinhosDesconhecidos = BuscarVizinhosDesconhecidos(i, j, tabuleiroMatriz);
                        List<int[]> posicoesVizinhosBomba = BuscarVizinhosBomba(i, j, tabuleiroMatriz);

                        int quantidadeVizinhosBomba = posicoesVizinhosBomba.Count;
                        int quantidadeVizinhosDesconhecidos = posicoesVizinhosDesconhecidos.Count;

                        if(numeroBombasCelulaAtual == quantidadeVizinhosBomba) {
                            if (quantidadeVizinhosDesconhecidos > 0) {
                                int[] primeiroVizinho = posicoesVizinhosDesconhecidos[0];

                                EscreverMatriz(tabuleiroMatriz);

                                AbrirPosicao(campoMinado, primeiroVizinho[0], primeiroVizinho[1]);

                                EscreverMatriz(tabuleiroMatriz);
                                return;
                            }
                        } else if(numeroBombasCelulaAtual == (quantidadeVizinhosDesconhecidos + quantidadeVizinhosBomba)) {
                            EscreverMatriz(tabuleiroMatriz);

                            MarcarBombas(posicoesVizinhosDesconhecidos, tabuleiroMatriz, posicoesBomba);

                            EscreverMatriz(tabuleiroMatriz);
                            return;
                        }
                    }
                }
            }
        }

        static void AbrirPosicao(CampoMinado campoMinado, int linha, int coluna) {
            Console.WriteLine($"Abrindo ({linha + 1}, {coluna + 1})");

            campoMinado.Abrir(linha + 1, coluna + 1);
        }

        static void MarcarBombas(List<int[]> posicoesVizinhosDesconhecidos, string[][] tabuleiroMatriz, List<int[]> posicoesBomba) {
            Console.WriteLine("Marcando desconhecidos como bomba:");
            Console.WriteLine(String.Join("\n", posicoesVizinhosDesconhecidos.Select(posicao => $"({posicao[0] + 1}, {posicao[1] + 1})")));

            posicoesBomba.AddRange(posicoesVizinhosDesconhecidos);
            foreach (var vizinhoDesconhecido in posicoesVizinhosDesconhecidos) {
                tabuleiroMatriz[vizinhoDesconhecido[0]][vizinhoDesconhecido[1]] = "+";
            }
        }

        static List<int[]> BuscarVizinhosDesconhecidos(int linha, int coluna, string[][] tabuleiroMatriz) {
            return BuscarVizinhosPorTipo(linha, coluna, tabuleiroMatriz, "-");
        }

        static List<int[]> BuscarVizinhosBomba(int linha, int coluna, string[][] tabuleiroMatriz) {
            return BuscarVizinhosPorTipo(linha, coluna, tabuleiroMatriz, "+");
        }

        static List<int[]> BuscarVizinhosPorTipo(int linha, int coluna, string[][] tabuleiroMatriz, string caracter) {
            List<int[]> vizinhos = new List<int[]>();

            int emCima = linha - 1;
            int embaixo = linha + 1;
            int naEsquerda = coluna - 1;
            int naDireita = coluna + 1;

            if(EhPosicaoValida(emCima)) {
                if(tabuleiroMatriz[emCima][coluna] == caracter) {
                    AdicionarPosicao(vizinhos, emCima, coluna);
                }

                if(EhPosicaoValida(naEsquerda)) {
                    if(tabuleiroMatriz[emCima][naEsquerda] == caracter) {
                        AdicionarPosicao(vizinhos, emCima, naEsquerda);
                    }
                }

                if(EhPosicaoValida(naDireita)) {
                    if(tabuleiroMatriz[emCima][naDireita] == caracter) {
                        AdicionarPosicao(vizinhos, emCima, naDireita);
                    }
                }
            }

            if(EhPosicaoValida(embaixo)) {
                if(tabuleiroMatriz[embaixo][coluna] == caracter) {
                        AdicionarPosicao(vizinhos, embaixo, coluna);
                }

                if(EhPosicaoValida(naEsquerda)) {
                    if(tabuleiroMatriz[embaixo][naEsquerda] == caracter) {
                        AdicionarPosicao(vizinhos, embaixo, naEsquerda);
                    }
                }

                if(EhPosicaoValida(naDireita)) {
                    if(tabuleiroMatriz[embaixo][naDireita] == caracter) {
                        AdicionarPosicao(vizinhos, embaixo, naDireita);
                    }
                }
            }

            if(EhPosicaoValida(naEsquerda)) {
                if(tabuleiroMatriz[linha][naEsquerda] == caracter) {
                    AdicionarPosicao(vizinhos, linha, naEsquerda);
                }
            }

            if(EhPosicaoValida(naDireita)) {
                if(tabuleiroMatriz[linha][naDireita] == caracter) {
                    AdicionarPosicao(vizinhos, linha, naDireita);
                }
            }

            return vizinhos;
        }

        static bool EhPosicaoValida(int posicao) {
            return posicao >= 0 && posicao < TAMANHO;
        }

        static void AdicionarPosicao(List<int[]> posicoes, int linha, int coluna) {
            posicoes.Add(new int[2] { linha, coluna });
        }

        static bool EhNumeroDiferenteDeZero(string tabuleiroCelula) {
            return (tabuleiroCelula != "-" && tabuleiroCelula != "+") && int.Parse(tabuleiroCelula) > 0;
        }

        static string[][] ConstruirMatriz(string tabuleiroTexto, List<int[]> posicoesBomba) {
            string[][] tabuleiroMatriz = new string[TAMANHO][];
            string[] linhas = tabuleiroTexto.Split("\r\n");
            for (int i = 0; i < TAMANHO; i++)
            {
                tabuleiroMatriz[i] = new string[TAMANHO];
                for (int j = 0; j < TAMANHO; j++)
                {
                    if(posicoesBomba.Exists(posicao => posicao[0] == i && posicao[1] == j)) {
                        tabuleiroMatriz[i][j] = "+";
                    } else {
                        tabuleiroMatriz[i][j] = linhas[i][j].ToString();
                    }
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

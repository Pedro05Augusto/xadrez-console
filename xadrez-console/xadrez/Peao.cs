using System.Runtime.CompilerServices;
using tabuleiro;

namespace xadrez
{
    class Peao : Peca
    {
        private PartidaDeXadrez Partida;
        private int Direcao;
        public Peao(Tabuleiro tab, Cor cor, PartidaDeXadrez partida) : base(tab, cor)
        {
            Partida = partida;
        }

        public override string ToString()
        {
            return "P";
        }

        private bool PodeMover(Posicao pos)
        {
            Peca p = Tab.Peca(pos);
            return p == null || p.Cor != Cor;
        }

        private bool ExisteInimigo(Posicao pos) {
            Peca p = Tab.Peca(pos);
            return p != null && p.Cor != Cor;
        }
        private bool Livre(Posicao pos) {
            return Tab.Peca(pos) == null;
        }

        public int LadoDirecao() {
            return Cor == Cor.Branca ? -1 : 1;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            pos.DefinirValores(Posicao.Linha + (1 * LadoDirecao()), Posicao.Coluna);
            if (Tab.PosicaoValida(pos) && Livre(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(Posicao.Linha + (2 * LadoDirecao()), Posicao.Coluna);
            if (Tab.PosicaoValida(pos) && Livre(pos) && QteMovimentos == 0)
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(Posicao.Linha + (1 * LadoDirecao()), Posicao.Coluna - 1);
            if (Tab.PosicaoValida(pos) && ExisteInimigo(pos)) {
                mat[pos.Linha, pos.Coluna] = true;
            }
            pos.DefinirValores(Posicao.Linha + (1 * LadoDirecao()), Posicao.Coluna + 1);
            if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
            }
            if (Cor == Cor.Branca && Posicao.Linha == 3)
            {
                Posicao posicaoEsquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(posicaoEsquerda) && ExisteInimigo(posicaoEsquerda) && Tab.Peca(posicaoEsquerda) == Partida.vulneravelEnPassant) {
                    mat[posicaoEsquerda.Linha -1, posicaoEsquerda.Coluna] = true;
                }

                Posicao posicaoDireita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(posicaoDireita) && ExisteInimigo(posicaoDireita) && Tab.Peca(posicaoDireita) == Partida.vulneravelEnPassant)
                {
                    mat[posicaoDireita.Linha - 1, posicaoDireita.Coluna] = true;
                }
            }
            else if (Cor == Cor.Preta && Posicao.Linha == 4) {
                Posicao posicaoEsquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                if (Tab.PosicaoValida(posicaoEsquerda) && ExisteInimigo(posicaoEsquerda) && Tab.Peca(posicaoEsquerda) == Partida.vulneravelEnPassant)
                {
                    mat[posicaoEsquerda.Linha + 1, posicaoEsquerda.Coluna] = true;
                }

                Posicao posicaoDireita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                if (Tab.PosicaoValida(posicaoDireita) && ExisteInimigo(posicaoDireita) && Tab.Peca(posicaoDireita) == Partida.vulneravelEnPassant)
                {
                    mat[posicaoDireita.Linha + 1, posicaoDireita.Coluna] = true;
                }
            }

            return mat;
        }
    }
}

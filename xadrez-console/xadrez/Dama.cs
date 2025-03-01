using tabuleiro;

namespace xadrez
{
    class Dama : Peca
    {
        public Dama(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "D";
        }

        private bool PodeMover(Posicao pos)
        {
            Peca p = Tab.Peca(pos);
            return p == null || p.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            //acima
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.ExistePeca(pos) &&  Tab.Peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Linha--;
            }

            //nordeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.ExistePeca(pos) && Tab.Peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Linha--;
                pos.Coluna++;
            }
            //leste
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.ExistePeca(pos) && Tab.Peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Coluna++;
            }
            //sudeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.ExistePeca(pos) && Tab.Peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Linha++;
                pos.Coluna++;
            }
            //sul
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.ExistePeca(pos) && Tab.Peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Linha++;
            }
            //sudoeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.ExistePeca(pos) && Tab.Peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Linha++;
                pos.Coluna--;
            }
            //oeste
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.ExistePeca(pos) && Tab.Peca(pos).Cor != Cor)
                {
                    break;
                }
                pos.Coluna--;
            }
            //noroeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            while (Tab.PosicaoValida(pos) && PodeMover(pos))
            {
                mat[pos.Linha, pos.Coluna] = true;
                if (Tab.ExistePeca(pos) && Tab.Peca(pos).Cor != Cor) {
                    break;
                }
                pos.Linha--;
                pos.Coluna--;
            }
            return mat;
        }
    }
}

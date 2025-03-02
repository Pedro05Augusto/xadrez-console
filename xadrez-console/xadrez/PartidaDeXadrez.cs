using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization.Metadata;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> Capturadas;
        public bool xeque { get; private set; }
        public Peca vulneravelEnPassant { get; private set; }

        public PartidaDeXadrez()
        {
            vulneravelEnPassant = null;
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            Pecas = new HashSet<Peca>();
            Capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.RetirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = tab.RetirarPeca(destino);
            tab.ColocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                Capturadas.Add(pecaCapturada);
            }

            //#Jogada Especial ROQUE PEQUENO
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQteMovimentos();
                tab.ColocarPeca(T, destinoT);
            }

            //#Jogada Especial ROQUE GRANDE
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = tab.RetirarPeca(origemT);
                T.IncrementarQteMovimentos();
                tab.ColocarPeca(T, destinoT);
            }

            // #Jogada En Passant
            if (p is Peao) {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null) {
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else {
                        posP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }
                    pecaCapturada = tab.RetirarPeca(posP);
                    Capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.RetirarPeca(destino);
            p.DecrementarQteMovimentos();
            if (pecaCapturada != null)
            {
                tab.ColocarPeca(pecaCapturada, destino);
                Capturadas.Remove(pecaCapturada);
            }
            tab.ColocarPeca(p, origem);

            //#Jogada Especial ROQUE PEQUENO
            if (p is Rei && destino.Coluna == origem.Coluna + 2){
                Posicao origemT = new Posicao(p.Posicao.Linha, p.Posicao.Coluna + 3);
                Posicao destinoT = new Posicao(p.Posicao.Linha, p.Posicao.Coluna + 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQteMovimentos();
                tab.ColocarPeca(T, origemT);
            }

            //#Jogada Especial ROQUE GRANDE
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(p.Posicao.Linha, p.Posicao.Coluna - 4);
                Posicao destinoT = new Posicao(p.Posicao.Linha, p.Posicao.Coluna - 1);
                Peca T = tab.RetirarPeca(destinoT);
                T.DecrementarQteMovimentos();
                tab.ColocarPeca(T, origemT);
            }

            // #Jogada En Passant
            if (p is Peao) {
                if (origem.Coluna != destino.Coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tab.RetirarPeca(destino);
                    p.DecrementarQteMovimentos();
                    Posicao posP;
                    if (p.Cor == Cor.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    tab.ColocarPeca(peao, posP);
                }
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if (EmXeque(jogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = tab.Peca(destino);

            // #jogada especial Promocao
            if (p is Peao) {
                if ((p.Cor == Cor.Branca && destino.Linha == 0) || (p.Cor == Cor.Preta && destino.Linha == 7)) {
                    p = tab.RetirarPeca(destino);
                    Pecas.Remove(p);
                    Peca dama = new Dama(tab, p.Cor);
                    tab.ColocarPeca(dama, destino);
                    Pecas.Add(dama);
                }
            }

            if (EmXeque(Adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }
            if (TesteXequeMate(Adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                MudaJogador();
            }
            

            //#Jogada especial En Passant
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                vulneravelEnPassant = p;
            }
            else {
                vulneravelEnPassant = null;
            }
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.Peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (jogadorAtual != tab.Peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if (!tab.Peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possiveis para a peça de origem!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.Peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void MudaJogador()
        {
            if (jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Capturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(PecasCapturadas(cor));
            return aux;
        }

        private Cor Adversaria(Cor cor)
        {
            return cor == Cor.Branca ? Cor.Preta : Cor.Branca;
        }

        private Peca Rei(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EmXeque(Cor cor)
        {
            foreach (Peca x in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if (mat[Rei(cor).Posicao.Linha, Rei(cor).Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TesteXequeMate(Cor cor)
        {
            if (!EmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for (int i = 0; i < tab.Linhas; i++)
                {
                    for (int j = 0; j < tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocarNovaPeca(Peca peca, char coluna, int linha)
        {
            tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocarPecas()
        {
            ColocarNovaPeca(new Torre(tab, Cor.Branca), 'a', 1);
            ColocarNovaPeca(new Cavalo(tab, Cor.Branca), 'b', 1);
            ColocarNovaPeca(new Bispo(tab, Cor.Branca), 'c', 1);
            ColocarNovaPeca(new Dama(tab, Cor.Branca), 'd', 1);
            ColocarNovaPeca(new Rei(tab, Cor.Branca, this), 'e', 1);
            ColocarNovaPeca(new Bispo(tab, Cor.Branca), 'f', 1);
            ColocarNovaPeca(new Cavalo(tab, Cor.Branca), 'g', 1);
            ColocarNovaPeca(new Torre(tab, Cor.Branca), 'h', 1);
            ColocarNovaPeca(new Peao(tab, Cor.Branca, this), 'a', 2);
            ColocarNovaPeca(new Peao(tab, Cor.Branca, this), 'b', 2);
            ColocarNovaPeca(new Peao(tab, Cor.Branca, this), 'c', 2);
            ColocarNovaPeca(new Peao(tab, Cor.Branca, this), 'd', 2);
            ColocarNovaPeca(new Peao(tab, Cor.Branca, this), 'e', 2);
            ColocarNovaPeca(new Peao(tab, Cor.Branca, this), 'f', 2);
            ColocarNovaPeca(new Peao(tab, Cor.Branca, this), 'g', 2);
            ColocarNovaPeca(new Peao(tab, Cor.Branca, this), 'h', 2);

            ColocarNovaPeca(new Torre(tab, Cor.Preta), 'a', 8);
            ColocarNovaPeca(new Cavalo(tab, Cor.Preta), 'b', 8);
            ColocarNovaPeca(new Bispo(tab, Cor.Preta), 'c', 8);
            ColocarNovaPeca(new Dama(tab, Cor.Preta), 'd', 8);
            ColocarNovaPeca(new Rei(tab, Cor.Preta, this), 'e', 8);
            ColocarNovaPeca(new Bispo(tab, Cor.Preta), 'f', 8);
            ColocarNovaPeca(new Cavalo(tab, Cor.Preta), 'g', 8);
            ColocarNovaPeca(new Torre(tab, Cor.Preta), 'h', 8);
            ColocarNovaPeca(new Peao(tab, Cor.Preta, this), 'a', 7);
            ColocarNovaPeca(new Peao(tab, Cor.Preta, this), 'b', 7);
            ColocarNovaPeca(new Peao(tab, Cor.Preta, this), 'c', 7);
            ColocarNovaPeca(new Peao(tab, Cor.Preta, this), 'd', 7);
            ColocarNovaPeca(new Peao(tab, Cor.Preta, this), 'e', 7);
            ColocarNovaPeca(new Peao(tab, Cor.Preta, this), 'f', 7);
            ColocarNovaPeca(new Peao(tab, Cor.Preta, this), 'g', 7);
            ColocarNovaPeca(new Peao(tab, Cor.Preta, this), 'h', 7);
        }
    }
}

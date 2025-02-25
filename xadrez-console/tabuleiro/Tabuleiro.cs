using xadrez;

namespace tabuleiro
{
    class Tabuleiro
    {
        //Quantidade de linhas que o tabuleiro terá
        public int Linhas { get; set; }
        //Quantidade de colunas que o tabuleiro terá
        public int Colunas { get; set; }
        //Multi-Array(Matriz) Pecas que é do tipo Peca, permite que eu posicione uma Peca em determinada linha e coluna
        private Peca[,] Pecas;

        //Construtor do tabuleiro, onde insiro quantas linhas e colunas o tabuleiro terá
        public Tabuleiro(int linhas, int colunas)
        {
            Linhas = linhas;
            Colunas = colunas;
            //Defino qual é o tamanho do Multi-Array Pecas, sendo o mesmo do tamanho do tabuleiro
            Pecas = new Peca[linhas, colunas];
        }

        //Função chamada Peca, que retorna o objeto Peca que está posicionado na linha e coluna repassada nos parâmetros
        public Peca Peca(int linha, int coluna) {
            return Pecas[linha, coluna];
        }

        //Função chamada Peca também, porém, ao invés de receber a linha e coluna, ele recebe um objeto Posicao,
        //retornando o objeto Peca que está posicionado na linha e coluna que está no objeto Posicao
        public Peca Peca (Posicao pos)
        {
            return Pecas[pos.Linha, pos.Coluna];
        }

        //Verifica se há uma Peca em determinada posição (utiliza da funcao ValidarPosicao, caso a posição não seja válida, retorna uma Exceção)
        public bool ExistePeca(Posicao pos)
        {
            ValidarPosicao(pos);
            return Peca(pos) != null;
        }

        //Coloca a Peca do parâmetro, na posição do parâmetro
        public void ColocarPeca(Peca p, Posicao pos)
        {
            //Verifica se já existe uma peça na posição, antes de colocar uma nova
            if (ExistePeca(pos))
            {
                throw new TabuleiroException("Já existe uma peça nessa posição!");
            }

            //Array Pecas recebe o a nova Peca na posição recebida na função
            Pecas[pos.Linha, pos.Coluna] = p;
            p.Posicao = pos;
        }

        //Valida se a posição existe no tabuleiro
        public bool PosicaoValida(Posicao pos)
        {
            if (pos.Linha < 0 || pos.Linha >= Linhas || pos.Coluna < 0 || pos.Coluna >= Colunas)
            {
                return false;
            }
            return true;
        }

        //Retorna uma exceção caso a função PosicaoValida seja falsa
        public void ValidarPosicao(Posicao pos)
        {
            if (!PosicaoValida(pos))
            {
                throw new TabuleiroException("Posição inválida!");
            }
        }
    }
}

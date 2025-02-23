using tabuleiro;

namespace xadrez_console {
    class Program {
        public static void Main(string[] args) {

            Tabuleiro tab = new Tabuleiro(8, 8);
            Tela.ImprimirTabuleiro(tab);

        }
    }        
} 
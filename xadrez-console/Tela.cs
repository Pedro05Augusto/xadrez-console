using System.Net.NetworkInformation;
using tabuleiro;

namespace xadrez_console
{
    class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < tab.Linhas; i++){
                Console.Write((tab.Linhas - i) + " ");
                for (int j = 0; j < tab.Colunas; j++){
                    if (tab.Peca(i, j) == null){
                        Console.Write("- ");
                    }
                    else{
                        ImprimirPeca(tab.Peca(i, j));
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
            Console.Write(" ");
            for (int i = 0; i < tab.Colunas; i++) {
                Console.Write(" " + (char)('a' + i));
            }
        }

        public static void ImprimirPeca(Peca peca) {
            if (peca.Cor == Cor.Branca) {
                Console.Write(peca);
            }
            else {
                ConsoleColor aux = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(peca);
                Console.ForegroundColor = aux;
            }
        }
    }
}

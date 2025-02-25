using System.Diagnostics.CodeAnalysis;
using tabuleiro;
using xadrez;

namespace xadrez_console {
    class Program {
        public static void Main(string[] args) {

            PosicaoXadrez pos = new PosicaoXadrez('c', 7);
            Console.WriteLine(pos.ToPosicao());
        }
    }        
} 
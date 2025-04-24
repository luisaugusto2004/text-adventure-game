using Core;
using System;
using Util;

namespace Scripts
{
    class ScriptManager
    {
        public static void ScriptedIntroScene()
        {
            TextPrinter.Print($"Essa é a história da sua morte", 25);
            Console.ReadLine();
            Console.Clear();
            TextPrinter.Print("Depois de ver essa merda, não tem mais volta.", 25);
            Console.ReadLine();
            Console.Clear();
            TextPrinter.Print("Você vai encontrar coisas tão fodidas que a situação global atual vai parecer perfeitamente sensata em comparação.", 25);
            Console.ReadLine();
            Console.Clear();
            if (Game.currentPlayer.Name == "")
                TextPrinter.Print("Viver ou morrer, a escolha é sua, estranho.", 100);
            else
                TextPrinter.Print($"Viver ou morrer, a escolha é sua, {Game.currentPlayer.Name}", 100);
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Você ouve seu coração batendo mais que tudo");
            Console.ReadLine();
            Console.WriteLine("A sua visão atrapalhada pelo sol começa a centralizar naquilo");
            Console.WriteLine("E você finalmente se lembra do que está na sua frente");
            Console.ReadLine();
            Console.WriteLine("Um ser encapuzado envolto em névoa e trevas vivas.");
            Console.WriteLine("O céu começa a choviscar");
            Console.WriteLine("Você começa a sentir o seu corpo ficar bambo diante do seu fim");
            Console.WriteLine("Frente a frente com a sua morte.");
            Console.ReadLine();
            TextPrinter.Print("Viver ou morrer, a escolha é sua.", 100);
            Console.Clear();
        }
    }
}

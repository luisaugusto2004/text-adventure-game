using EntityPlayer;
using Util;
using Scripts;
using System;

namespace Core{
    class Game {

        public static Player currentPlayer = new Player();

        public static void Start() {
            Console.ForegroundColor = ConsoleColor.Green;
            TextPrinter.Print("Insira seu nome: ", 50);
            currentPlayer.Name = Console.ReadLine();

            Console.Clear();
            ScriptManager.ScriptedIntroScene();
        }
    }
}

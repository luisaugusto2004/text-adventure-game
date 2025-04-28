using EntityPlayer;
using Util;
using Scripts;
using System;
using Battle;

namespace Core{
    class Game {

        public static Player currentPlayer = new Player();
        public static Random GlobalRandom = new Random();
        public static bool ProfeciaAtivada = false;
        public static int Turns = 1;
        public static bool InTheWilds = true;


        public static void Start() {
            Console.ForegroundColor = ConsoleColor.Green;
            TextPrinter.Print("Insira seu nome: ", 50);
            currentPlayer = new Player(Console.ReadLine(), 30, 10);
            Console.Clear();
            ScriptManager.ScriptedIntroScene();
            Encounter.FirstEncounter();

            while (InTheWilds)
                Encounter.RandomEncounter(GlobalRandom);           
            Console.Clear();
        }
    }
}

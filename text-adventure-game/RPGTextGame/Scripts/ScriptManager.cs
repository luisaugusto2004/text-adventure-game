using Core;
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
            Console.ReadLine();
            Console.Clear();
        }

        public static void HandleSecretEnding()
        {
            Console.Clear();
            TextPrinter.Print("O mundo para.", 50);
            Console.ReadLine();
            TextPrinter.Print("O tempo se dobra como um papel queimando pelas bordas.", 50);
            Console.ReadLine();
            TextPrinter.Print("A névoa ao redor do encapuzado hesita. Pela primeira vez, ele recua um passo.", 50);
            Console.ReadLine();
            TextPrinter.Print("Sua pele arrepia. O coração, antes hesitante, bate com a força de mil trovões.", 50);
            Console.ReadLine();
            TextPrinter.Print("Você não sabe por quê, mas sabe que essa palavra... era a chave.", 50);
            Console.ReadLine();
            Console.Clear();
            TextPrinter.Print("A profecia desperta. Você foi escolhido. O portador da centelha que recusa o fim.", 50);
            TextPrinter.Print("Seu corpo se ilumina com uma chama antiga, esquecida pelas eras.", 50);
            Console.ReadLine();
            TextPrinter.Print("O ser encapuzado fala pela primeira vez:\r\n“...impossível.”", 50);
            TextPrinter.Print("A verdadeira batalha começa!", 50);
            Console.ReadLine();
        }

        public static void HandleSecretEndingWhenDefeated()
        {
            Console.Clear();
            TextPrinter.Print("Com um grito final, o Encapuzado cai, derrotado pelo poder da profecia.", 50);
            Console.ReadLine();
            TextPrinter.Print("Sua forma escura desintegra-se em sombras, deixando para trás apenas um vazio frio.", 50);
            Console.ReadLine();
            TextPrinter.Print("O mundo ao seu redor parece suspenso no tempo, a névoa se dissipa lentamente.", 50);
            Console.ReadLine();
            TextPrinter.Print("Você se sente diferente. O peso da profecia pesa sobre seus ombros, mas você é mais forte.", 50);
            TextPrinter.Print("Você se levantou contra o destino e venceu. A verdadeira batalha, agora, é sua.", 50);
        }

        public static void ScriptedScenePostBattle()
        {
            Console.Clear();
            TextPrinter.Print("A última coisa que você sente é o frio... um frio que invade seus ossos.", 40);
            Console.ReadLine();
            TextPrinter.Print("Sua visão se apaga. O som do mundo se cala.", 40);
            Console.ReadLine();
            TextPrinter.Print("Você morreu.", 60);
            Console.ReadLine();
            Console.Clear();
            TextPrinter.Print("...", 500);
            Console.ReadLine();
            TextPrinter.Print("Algo distante... gotas caindo...", 50);
            Console.ReadLine();
            TextPrinter.Print("Um cheiro de terra úmida enche seus pulmões. Você respira calmamente, não é como se importasse.", 40);
            Console.ReadLine();
            TextPrinter.Print("Seus dedos tocam madeira áspera acima de você.", 40);
            Console.ReadLine();
            TextPrinter.Print("Está escuro. Muito escuro.", 40);
            Console.ReadLine();
            TextPrinter.Print("Você percebe: está dentro de um caixão.", 50);
            Console.ReadLine();
            Console.Clear();
            TextPrinter.Print("Calmo como se fosse só mais uma segunda-feira. Você empurra... a madeira range, cede...", 40);
            Console.ReadLine();
            TextPrinter.Print("Com facilidade, você rompe a tampa e a terra cede.", 40);
            Console.ReadLine();
            Console.Clear();
            TextPrinter.Print("Você emerge de uma cova rasa, em um cemitério esquecido sob a chuva e exclama: ", 40);
            TextPrinter.Print("Eu vivo, de novo!", 70); // Sangue referência :O
            Console.ReadLine();
            TextPrinter.Print("O mundo que você conhecia... não existe mais.", 50);
            Console.ReadLine();
            TextPrinter.Print("Este é o tempo da vingança, e nenhuma vida merece ser salva", 50);
            TextPrinter.Print("Sua verdadeira jornada começa agora.", 60);
            Console.ReadLine();
            //   _____
            //  /     \\
            // /_______\\
            //  | . . |
            //  |  ^  |
            // /| --- |\\
            /// |     | \\
            //  |     |
            // /       \\
            ///_________\\
            //You like my big code?
        }
    }
}

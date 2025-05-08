namespace Util
{
    static class TextPrinter
    {
        public static void Print(string text, int speed)
        {

            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(text[i]);

                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    Console.Write(text.Substring(i + 1));
                    break;
                }
                Thread.Sleep(speed);
            }
            Console.WriteLine();
        }
    }
}

using System;

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        string playAgain;

        do
        {
            int magicNumber = random.Next(1, 101);
            int guess;
            int attempts = 0;

            Console.WriteLine("Guess My Number Game!");

            do
            {
                Console.Write("What is your guess? ");
                guess = int.Parse(Console.ReadLine());
                attempts++;

                if (guess < magicNumber)
                {
                    Console.WriteLine("Higher");
                }
                else if (guess > magicNumber)
                {
                    Console.WriteLine("Lower");
                }
                else
                {
                    Console.WriteLine($"You guessed it in {attempts} attempts!");
                }
            } while (guess != magicNumber);

            Console.Write("Do you want to play again? (yes/no): ");
            playAgain = Console.ReadLine().ToLower();

        } while (playAgain == "yes");

        Console.WriteLine("Thanks for playing!");
    }
}
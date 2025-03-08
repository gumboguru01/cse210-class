using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("What is Your  first name ? ");
        string firstname = Console.ReadLine();

        Console.Write("What is Your last name ? ");
        string lastname = Console.ReadLine();
        Console.WriteLine($"Your Name is {lastname}, {firstname} {lastname} ");


    }
}
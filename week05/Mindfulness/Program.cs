using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        MindfulnessApp app = new MindfulnessApp();
        app.Run();
    }
}

public class MindfulnessApp
{
    private int breathingCount = 0;
    private int reflectionCount = 0;
    private int listingCount = 0;

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("1. Start Breathing Activity");
            Console.WriteLine("2. Start Reflection Activity");
            Console.WriteLine("3. Start Listing Activity");
            Console.WriteLine("4. View Stats");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            string input = Console.ReadLine();

            if (input == "1")
            {
                Activity activity = new BreathingActivity();
                activity.Run();
                breathingCount++;
            }
            else if (input == "2")
            {
                Activity activity = new ReflectionActivity();
                activity.Run();
                reflectionCount++;
            }
            else if (input == "3")
            {
                Activity activity = new ListingActivity();
                activity.Run();
                listingCount++;
            }
            else if (input == "4")
            {
                ShowStats();
            }
            else if (input == "5")
            {
                Console.WriteLine("Thank you for using the Mindfulness App!");
                ShowStats();
                break;
            }
            else
            {
                Console.WriteLine("Invalid option. Please try again.");
                Thread.Sleep(2000);
            }
        }
    }

    private void ShowStats()
    {
        Console.WriteLine("\nActivity Summary:");
        Console.WriteLine($"Breathing Activities Completed: {breathingCount}");
        Console.WriteLine($"Reflection Activities Completed: {reflectionCount}");
        Console.WriteLine($"Listing Activities Completed: {listingCount}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

// Base class
public abstract class Activity
{
    private string _name;
    private string _description;
    private int _duration;

    public void Run()
    {
        DisplayStartingMessage();
        PerformActivity();
        DisplayEndingMessage();
    }

    protected abstract void PerformActivity();

    public void DisplayStartingMessage()
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {_name}.");
        Console.WriteLine(_description);
        Console.Write("Enter the duration in seconds: ");
        _duration = int.Parse(Console.ReadLine());
        Console.WriteLine("Prepare to begin...");
        ShowSpinner(3);
    }

    public void DisplayEndingMessage()
    {
        Console.WriteLine("Good job! You've completed the activity.");
        Console.WriteLine($"You spent {_duration} seconds in the {_name}.");
        ShowSpinner(3);
    }

    protected int GetDuration() => _duration;

    protected void SetName(string name) => _name = name;
    protected void SetDescription(string description) => _description = description;

    protected void ShowSpinner(int seconds)
    {
        List<string> spinner = new List<string> { "|", "/", "-", "\\" };
        DateTime end = DateTime.Now.AddSeconds(seconds);
        int i = 0;
        while (DateTime.Now < end)
        {
            Console.Write(spinner[i % spinner.Count]);
            Thread.Sleep(250);
            Console.Write("\b ");
            i++;
        }
        Console.WriteLine();
    }

    protected void Countdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i);
            Thread.Sleep(1000);
            Console.Write("\b \b");
        }
        Console.WriteLine();
    }
}

public class BreathingActivity : Activity
{
    public BreathingActivity()
    {
        SetName("Breathing Activity");
        SetDescription("This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.");
    }

    protected override void PerformActivity()
    {
        int seconds = GetDuration();
        DateTime end = DateTime.Now.AddSeconds(seconds);
        while (DateTime.Now < end)
        {
            Console.Write("Breathe in...");
            Countdown(3);
            Console.Write("Breathe out...");
            Countdown(3);
        }
    }
}

public class ReflectionActivity : Activity
{
    private List<string> prompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private List<string> questions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    public ReflectionActivity()
    {
        SetName("Reflection Activity");
        SetDescription("This activity will help you reflect on times in your life when you have shown strength and resilience.");
    }

    protected override void PerformActivity()
    {
        Random rand = new Random();
        Console.WriteLine(prompts[rand.Next(prompts.Count)]);
        ShowSpinner(3);
        int seconds = GetDuration();
        DateTime end = DateTime.Now.AddSeconds(seconds);
        while (DateTime.Now < end)
        {
            Console.WriteLine(questions[rand.Next(questions.Count)]);
            ShowSpinner(5);
        }
    }
}

public class ListingActivity : Activity
{
    private List<string> prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity()
    {
        SetName("Listing Activity");
        SetDescription("This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.");
    }

    protected override void PerformActivity()
    {
        Random rand = new Random();
        Console.WriteLine(prompts[rand.Next(prompts.Count)]);
        Countdown(5);
        Console.WriteLine("Start listing items. Press Enter after each one.");
        List<string> items = new List<string>();
        DateTime end = DateTime.Now.AddSeconds(GetDuration());
        while (DateTime.Now < end)
        {
            if (Console.KeyAvailable)
            {
                string entry = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(entry))
                {
                    items.Add(entry);
                }
            }
        }
        Console.WriteLine($"You listed {items.Count} items:");
        foreach (string item in items)
        {
            Console.WriteLine("- " + item);
        }
    }

}

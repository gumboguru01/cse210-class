using System;
using System.Collections.Generic;
using System.IO;

class Journal
{
    private List<string> entries = new List<string>();

    public void AddEntry(string prompt, string response)
    {
        entries.Add($"Prompt: {prompt}\nResponse: {response}\n");
    }

    public void DisplayJournal()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("Your journal is empty.");
        }
        else
        {
            foreach (var entry in entries)
            {
                Console.WriteLine(entry);
            }
        }
    }

    public List<string> GetEntries()
    {
        return entries;
    }
}

class FileHandler
{
    public static void SaveJournal(Journal journal, string filename)
    {
        try
        {
            File.WriteAllLines(filename, journal.GetEntries());
            Console.WriteLine("Journal saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving: {ex.Message}");
        }
    }

    public static Journal LoadJournal(string filename)
    {
        var journal = new Journal();
        try
        {
            if (File.Exists(filename))
            {
                var entries = File.ReadAllLines(filename);
                foreach (var entry in entries)
                {
                    journal.AddEntry(entry.Split(new string[] { "\n" }, StringSplitOptions.None)[0], entry.Split(new string[] { "\n" }, StringSplitOptions.None)[1]);
                }
                Console.WriteLine("Journal loaded successfully.");
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading: {ex.Message}");
        }

        return journal;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Journal journal = new Journal();
        string[] prompts = {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?"
        };

        bool running = true;
        while (running)
        {
            Console.WriteLine("Journal Program Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. View journal");
            Console.WriteLine("3. Save journal to file");
            Console.WriteLine("4. Load journal from file");
            Console.WriteLine("5. Exit");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Random rand = new Random();
                    string prompt = prompts[rand.Next(prompts.Length)];
                    Console.WriteLine($"Prompt: {prompt}");
                    Console.WriteLine("Your response: ");
                    string response = Console.ReadLine();
                    journal.AddEntry(prompt, response);
                    break;
                case "2":
                    journal.DisplayJournal();
                    break;
                case "3":
                    Console.WriteLine("Enter filename to save journal: ");
                    string saveFile = Console.ReadLine();
                    FileHandler.SaveJournal(journal, saveFile);
                    break;
                case "4":
                    Console.WriteLine("Enter filename to load journal: ");
                    string loadFile = Console.ReadLine();
                    journal = FileHandler.LoadJournal(loadFile);
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
    }
}

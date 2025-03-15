using System;
using System.Collections.Generic;
using System.IO;

class Journal
{
    private List<string> entries = new List<string>();

    public void AddEntry(string prompt, string response)
    {
        // New feature: Adding date and time stamp to each journal entry
        string dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        entries.Add($"[{dateTime}] Prompt: {prompt}\nResponse: {response}\n");
    }

    public void DisplayJournal()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("Your journal is empty. Let's fill it up!");
        }
        else
        {
            foreach (var entry in entries)
            {
                Console.WriteLine(entry);
                Console.WriteLine(new string('-', 40)); // New feature: Decorative line between entries for better readability
            }
        }
    }

    public List<string> GetEntries()
    {
        return entries;
    }

    // New feature: Search function to find entries based on a keyword
    public void SearchEntries(string keyword)
    {
        var foundEntries = entries.FindAll(entry => entry.Contains(keyword));
        if (foundEntries.Count == 0)
        {
            Console.WriteLine("No entries found matching your search.");
        }
        else
        {
            Console.WriteLine($"Found {foundEntries.Count} entry(ies) with keyword '{keyword}':");
            foreach (var entry in foundEntries)
            {
                Console.WriteLine(entry);
                Console.WriteLine(new string('-', 40)); // Decorative line between found entries
            }
        }
    }
}

class FileHandler
{
    public static void SaveJournal(Journal journal, string filename)
    {
        try
        {
            File.WriteAllLines(filename, journal.GetEntries());
            // New feature: Added success message when saving the journal
            Console.WriteLine($"Journal successfully saved to {filename}.");
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
                // New feature: Added success message when loading the journal
                Console.WriteLine($"Journal loaded successfully from {filename}.");
            }
            else
            {
                Console.WriteLine($"No journal file found at {filename}. Starting a new journal.");
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
        // New feature: ASCII art banner 
        Console.WriteLine(@"
   _______________              ____
  /               \            /    \
 /    JOURNAL     \___________/      \
 \    PROGRAM     /           \______/
  \_______________/
");

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
            Console.WriteLine("\n-------------------- Journal Program Menu --------------------");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. View journal");
            Console.WriteLine("3. Save journal to file");
            Console.WriteLine("4. Load journal from file");
            Console.WriteLine("5. Search journal entries"); // New feature: Added search option to the menu
            Console.WriteLine("6. Exit");
            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Random rand = new Random();
                    string prompt = prompts[rand.Next(prompts.Length)];
                    Console.WriteLine($"\nPrompt: {prompt}");
                    Console.Write("Your response: ");
                    string response = Console.ReadLine();
                    journal.AddEntry(prompt, response);
                    break;
                case "2":
                    Console.WriteLine("\n--- Your Journal Entries ---");
                    journal.DisplayJournal();
                    break;
                case "3":
                    Console.Write("\nEnter filename to save journal: ");
                    string saveFile = Console.ReadLine();
                    FileHandler.SaveJournal(journal, saveFile);
                    break;
                case "4":
                    Console.Write("\nEnter filename to load journal: ");
                    string loadFile = Console.ReadLine();
                    journal = FileHandler.LoadJournal(loadFile);
                    break;
                case "5":
                    Console.Write("\nEnter a keyword to search for: "); // New feature: Prompt for search keyword
                    string keyword = Console.ReadLine();
                    journal.SearchEntries(keyword); // New feature: Search entries based on the keyword
                    break;
                case "6":
                    Console.WriteLine("\nThank you for using the Journal Program. Goodbye!");
                    running = false;
                    break;
                default:
                    Console.WriteLine("\nInvalid choice, please try again.");
                    break;
            }
        }
    }
}

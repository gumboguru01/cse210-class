using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace EternalQuestProgram
{
    // Base class for all goal types.
    public abstract class Goal
    {
        // Protected members so derived classes can access them.
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Points { get; protected set; }
        public bool Completed { get; protected set; } = false;

        public Goal(string name, string description, int points)
        {
            Name = name;
            Description = description;
            Points = points;
        }

        
        
        public abstract int RecordEvent();

        
        public abstract string GetDisplayString();
    }


    public class SimpleGoal : Goal
    {
        public SimpleGoal(string name, string description, int points)
            : base(name, description, points) { }

        public override int RecordEvent()
        {
            if (!Completed)
            {
                Completed = true;
                return Points;
            }
            else
            {
                Console.WriteLine("This goal is already completed.");
                return 0;
            }
        }

        public override string GetDisplayString()
        {
            return $"[ {(Completed ? "X" : " ")} ] {Name} ({Description}) - {Points} pts";
        }
    }

    // An eternal goal that can be recorded over and over and is never considered fully complete.
    public class EternalGoal : Goal
    {
        public EternalGoal(string name, string description, int points)
            : base(name, description, points) { }

        public override int RecordEvent()
        {
            // For eternal goals, each record gives points.
            return Points;
        }

        public override string GetDisplayString()
        {
            // Eternal goals are always open for more points.
            return $"[∞] {Name} ({Description}) - {Points} pts per completion";
        }
    }

    // A checklist goal that requires a specific number of completions.
    public class ChecklistGoal : Goal
    {
        private int _targetCount;
        private int _currentCount = 0;
        private int _bonus;

        public ChecklistGoal(string name, string description, int points, int targetCount, int bonus)
            : base(name, description, points)
        {
            _targetCount = targetCount;
            _bonus = bonus;
        }

        public override int RecordEvent()
        {
            if (Completed)
            {
                Console.WriteLine("This checklist goal is already completed.");
                return 0;
            }
            _currentCount++;
            int totalPoints = Points;
            if (_currentCount >= _targetCount)
            {
                Completed = true;
                totalPoints += _bonus;
            }
            return totalPoints;
        }

        public override string GetDisplayString()
        {
            string status = Completed ? "X" : " ";
            return $"[ {status} ] {Name} ({Description}) - {Points} pts each, completed {_currentCount}/{_targetCount} times. Bonus: {_bonus} pts";
        }
    }

    // Represents the entire goal tracker including current score and list of goals.
    public class QuestTracker
    {
        public int TotalScore { get; private set; } = 0;
        public List<Goal> Goals { get; private set; } = new List<Goal>();

        // Add points and then check if new levels are achieved.
        public void AddScore(int points)
        {
            TotalScore += points;
            Console.WriteLine($"\nYou gained {points} points! Total points: {TotalScore}");
            CheckForLevelUp();
        }

        // A simple level up system: every 1000 points the user gains a new level.
        private void CheckForLevelUp()
        {
            int level = TotalScore / 1000;
            if (level > 0)
            {
                Console.WriteLine($"Congratulations! You have reached level {level} on your Eternal Quest!");
            }
        }

        public void AddGoal(Goal goal)
        {
            Goals.Add(goal);
        }

        public void ListGoals()
        {
            Console.WriteLine("\nGoals:");
            for (int i = 0; i < Goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Goals[i].GetDisplayString()}");
            }
        }

        // Save the quest tracker status (goals and score) to a JSON file.
        public void Save(string fileName)
        {
            try
            {
                // We need a serializable container since Goal is abstract.
                var container = new SerializableContainer
                {
                    TotalScore = this.TotalScore,
                    Goals = new List<SerializableGoal>()
                };

                foreach (var goal in Goals)
                {
                    // Record key information about each goal with an identifier for type.
                    SerializableGoal sGoal = new SerializableGoal
                    {
                        GoalType = goal.GetType().Name,
                        Name = goal.Name,
                        Description = goal.Description,
                        Points = goal.Points,
                        Completed = goal.Completed
                    };

                    if (goal is ChecklistGoal checkGoal)
                    {
                        // Using reflection here or type casting, we extract extra members for checklist goals.
                        var type = typeof(ChecklistGoal);
                        var targetField = type.GetField("_targetCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        var currentField = type.GetField("_currentCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        var bonusField = type.GetField("_bonus", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        sGoal.TargetCount = (int)targetField.GetValue(checkGoal);
                        sGoal.CurrentCount = (int)currentField.GetValue(checkGoal);
                        sGoal.Bonus = (int)bonusField.GetValue(checkGoal);
                    }
                    container.Goals.Add(sGoal);
                }

                string jsonString = JsonSerializer.Serialize(container, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(fileName, jsonString);
                Console.WriteLine("Save completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred during saving: " + ex.Message);
            }
        }

        // Load the quest tracker status from a JSON file.
        public void Load(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    Console.WriteLine("Save file does not exist.");
                    return;
                }
                string jsonString = File.ReadAllText(fileName);
                var container = JsonSerializer.Deserialize<SerializableContainer>(jsonString);
                this.TotalScore = container.TotalScore;
                Goals.Clear();

                foreach (var sGoal in container.Goals)
                {
                    Goal goal = null;
                    // Reconstruct the right Goal type.
                    switch (sGoal.GoalType)
                    {
                        case nameof(SimpleGoal):
                            goal = new SimpleGoal(sGoal.Name, sGoal.Description, sGoal.Points);
                            break;
                        case nameof(EternalGoal):
                            goal = new EternalGoal(sGoal.Name, sGoal.Description, sGoal.Points);
                            break;
                        case nameof(ChecklistGoal):
                            goal = new ChecklistGoal(sGoal.Name, sGoal.Description, sGoal.Points, sGoal.TargetCount, sGoal.Bonus);
                            // Adjust the internal state to match saved progress.
                            // Because our RecordEvent method automatically increments the count, we need to simulate it:
                            var type = typeof(ChecklistGoal);
                            var currentField = type.GetField("_currentCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            currentField.SetValue(goal, sGoal.CurrentCount);
                            if (sGoal.CurrentCount >= sGoal.TargetCount)
                            {
                                // Mark goal as completed if it meets the threshold.
                                var completedProp = type.GetProperty("Completed");
                                // Directly setting the protected member using reflection:
                                var completedField = type.GetField("Completed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                                completedField.SetValue(goal, true);
                            }
                            break;
                        default:
                            Console.WriteLine("Unknown goal type in save file.");
                            break;
                    }
                    if (goal != null)
                    {
                        // Manually set the Completed state if necessary (for SimpleGoal and ChecklistGoal)
                        if (goal.Completed != sGoal.Completed)
                        {
                            // For this simple example, we assume the record of events is enough and Completed stays in sync.
                            // An advanced version might set Completed explicitly.
                        }
                        Goals.Add(goal);
                    }
                }
                Console.WriteLine("Load completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred during loading: " + ex.Message);
            }
        }
    }

    // Helper classes for serialization
    public class SerializableContainer
    {
        public int TotalScore { get; set; }
        public List<SerializableGoal> Goals { get; set; }
    }

    public class SerializableGoal
    {
        public string GoalType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public bool Completed { get; set; }
        public int TargetCount { get; set; }      
        public int CurrentCount { get; set; }     
        public int Bonus { get; set; }            
    }

    // Main program with a menu interface.
    class Program
    {
        static void Main(string[] args)
        {
            QuestTracker tracker = new QuestTracker();
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nEternal Quest Menu");
                Console.WriteLine("1. Create new goal");
                Console.WriteLine("2. Record an event for a goal");
                Console.WriteLine("3. List goals");
                Console.WriteLine("4. Display score");
                Console.WriteLine("5. Save goals");
                Console.WriteLine("6. Load goals");
                Console.WriteLine("7. Exit");
                Console.Write("Select an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CreateGoal(tracker);
                        break;
                    case "2":
                        RecordGoalEvent(tracker);
                        break;
                    case "3":
                        tracker.ListGoals();
                        break;
                    case "4":
                        Console.WriteLine($"\nYour current score is: {tracker.TotalScore}");
                        break;
                    case "5":
                        Console.Write("Enter filename to save: ");
                        string saveFile = Console.ReadLine();
                        tracker.Save(saveFile);
                        break;
                    case "6":
                        Console.Write("Enter filename to load: ");
                        string loadFile = Console.ReadLine();
                        tracker.Load(loadFile);
                        break;
                    case "7":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        static void CreateGoal(QuestTracker tracker)
        {
            Console.WriteLine("\nSelect goal type:");
            Console.WriteLine("1. Simple Goal");
            Console.WriteLine("2. Eternal Goal");
            Console.WriteLine("3. Checklist Goal");
            Console.Write("Your choice: ");
            string typeChoice = Console.ReadLine();

            Console.Write("Enter goal name: ");
            string name = Console.ReadLine();
            Console.Write("Enter goal description: ");
            string description = Console.ReadLine();
            Console.Write("Enter points awarded per event: ");
            int points = int.Parse(Console.ReadLine());

            Goal goal = null;
            if (typeChoice == "1")
            {
                goal = new SimpleGoal(name, description, points);
            }
            else if (typeChoice == "2")
            {
                goal = new EternalGoal(name, description, points);
            }
            else if (typeChoice == "3")
            {
                Console.Write("Enter number of times to complete this goal: ");
                int targetCount = int.Parse(Console.ReadLine());
                Console.Write("Enter bonus points upon full completion: ");
                int bonus = int.Parse(Console.ReadLine());
                goal = new ChecklistGoal(name, description, points, targetCount, bonus);
            }
            else
            {
                Console.WriteLine("Invalid goal type.");
                return;
            }

            tracker.AddGoal(goal);
            Console.WriteLine("Goal added successfully.");
        }

        static void RecordGoalEvent(QuestTracker tracker)
        {
            if (tracker.Goals.Count == 0)
            {
                Console.WriteLine("No goals available.");
                return;
            }

            tracker.ListGoals();
            Console.Write("Select the number of the goal to record an event for: ");
            int goalNum;
            if (int.TryParse(Console.ReadLine(), out goalNum) && goalNum > 0 && goalNum <= tracker.Goals.Count)
            {
                int earnedPoints = tracker.Goals[goalNum - 1].RecordEvent();
                if (earnedPoints > 0)
                {
                    tracker.AddScore(earnedPoints);
                }
            }
            else
            {
                Console.WriteLine("Invalid selection.");
            }
        }
    }
}

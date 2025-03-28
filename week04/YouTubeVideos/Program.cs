using System;
using System.Collections.Generic;

public class Comment
{
    public string Name { get; set; }
    public string Text { get; set; }

    // Constructor to initialize the comment
    public Comment(string name, string text)
    {
        Name = name;
        Text = text;
    }
}

public class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthInSeconds { get; set; }
    public List<Comment> Comments { get; set; }

    // Constructor to initialize the video
    public Video(string title, string author, int lengthInSeconds)
    {
        Title = title;
        Author = author;
        LengthInSeconds = lengthInSeconds;
        Comments = new List<Comment>(); // Initialize an empty list of comments
    }

    // Method to return the number of comments
    public int GetNumberOfComments()
    {
        return Comments.Count;
    }

    // Method to add a comment to the video
    public void AddComment(Comment comment)
    {
        Comments.Add(comment);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Create some videos
        Video video1 = new Video("Amazing Product Review", "John Doe", 360);
        video1.AddComment(new Comment("Alice", "Great video! Learned a lot."));
        video1.AddComment(new Comment("Bob", "Very informative, thanks!"));
        video1.AddComment(new Comment("Charlie", "I disagree with some points, but still good."));

        Video video2 = new Video("Tech Gadget Unboxing", "Jane Smith", 540);
        video2.AddComment(new Comment("David", "I love the gadget!"));
        video2.AddComment(new Comment("Eve", "Could have been a bit more detailed."));
        video2.AddComment(new Comment("Frank", "The unboxing was exciting, but I need more reviews."));

        Video video3 = new Video("Healthy Lifestyle Tips", "Chris Johnson", 420);
        video3.AddComment(new Comment("Grace", "These tips are very useful, thank you!"));
        video3.AddComment(new Comment("Hannah", "Great advice on nutrition!"));
        video3.AddComment(new Comment("Ivy", "More tips on exercise please!"));

        // Create a list of videos
        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Display video details
        foreach (var video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.LengthInSeconds} seconds");
            Console.WriteLine($"Number of Comments: {video.GetNumberOfComments()}");
            Console.WriteLine("Comments:");

            foreach (var comment in video.Comments)
            {
                Console.WriteLine($"- {comment.Name}: {comment.Text}");
            }
            Console.WriteLine();
        }
    }
}

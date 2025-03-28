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
        Video video1 = new Video("Amazing Product Review", "Mosh Hammadan ", 360);
        video1.AddComment(new Comment("Nyasha", "This video was very informative!"));
        video1.AddComment(new Comment("Tatenda", "Nice review, learned a lot."));
        video1.AddComment(new Comment("Kimberly", "Great job! Keep it up."));
        video1.AddComment(new Comment("Panashe", "I love the details in this video."));

        Video video2 = new Video("Tech Gadget Unboxing", "Tane Smith", 540);
        video2.AddComment(new Comment("Reign", "This gadget looks amazing!"));
        video2.AddComment(new Comment("Panashe", "The unboxing was exciting, I need to get one!"));
        video2.AddComment(new Comment("Mapfumo", "Could you do more reviews like this?"));

        Video video3 = new Video("Healthy Lifestyle Tips", "Johnson", 420);
        video3.AddComment(new Comment("Kimberly", "These tips are so useful!"));
        video3.AddComment(new Comment("Nyasha", "Great advice, very practical."));
        video3.AddComment(new Comment("Manjoni", "Would love to hear more about exercise tips!"));

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

using System;
using System.Collections.Generic;
using System.Linq;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsIssued { get; set; }

    public static List<Book> books = new List<Book>();

    public Book(int bookId, string title, string author)
    {
        BookId = bookId;
        Title = title;
        Author = author;
        IsIssued = false;
    }

    public static void AddDetails(int bookId, string title, string author)
    {
        books.Add(new Book(bookId, title, author));
    }

    public static Book SearchByTitle(string searchTitle)
    {
        foreach (var book in books)
        {
            if (book.Title.ToLower()==searchTitle.ToLower())
            {
                return book;
            }
        }

        return null;
    }

    public static List<Book> SearchByAuthor(string author)
    {
        List<Book> result = new List<Book>();
        foreach(var book in books)
        {
            if (book.Author.ToLower() == author.ToLower())
            {
                result.Add(book);
            } 
        }
        return null;
    }

    public static void IssueBook(int bookId)
    {
        Book? book = books.FirstOrDefault(b => b.BookId == bookId);

        if (book == null)
        {
            Console.WriteLine("Book not found.");
            return;
        }

        if (book.IsIssued)
        {
            Console.WriteLine("This book is already issued.");
            return;
        }

        book.IsIssued = true;
        Console.WriteLine($"Book '{book.Title}' issued successfully.");
    }

    public static void ReturnBook(int bookId)
    {
        Book? book = books.FirstOrDefault(b => b.BookId == bookId);

        if (book == null)
        {
            Console.WriteLine("Book not found.");
            return;
        }

        if (!book.IsIssued)
        {
            Console.WriteLine("This book is not currently issued.");
            return;
        }

        book.IsIssued = false;
        Console.WriteLine($"Book '{book.Title}' returned successfully.");
    }

    public static void Menu()
    {
        while (true)
        {
            Console.WriteLine("\n1. Add Book");
            Console.WriteLine("2. Search by Title");
            Console.WriteLine("3. Search by Author");
            Console.WriteLine("4. Issue Book");
            Console.WriteLine("5. Return Book");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Book ID: ");
                    int id = int.Parse(Console.ReadLine() ?? "0");
                    Console.Write("Enter Title: ");
                    string title = Console.ReadLine() ?? "";
                    Console.Write("Enter Author: ");
                    string author = Console.ReadLine() ?? "";
                    AddDetails(id, title, author);
                    break;

                case "2":
                    Console.Write("Enter title to search: ");
                    string titleToSearch = Console.ReadLine() ?? "";
                    Book? foundByTitle = SearchByTitle(titleToSearch);
                    if (foundByTitle != null)
                    {
                        Console.WriteLine($"Found: {foundByTitle.BookId} - {foundByTitle.Title} - {foundByTitle.Author}");
                    }
                    else
                    {
                        Console.WriteLine("Book not found.");
                    }
                    break;

                case "3":
                    Console.Write("Enter author to search: ");
                    string authorToSearch = Console.ReadLine() ?? "";
                    List<Book> foundByAuthor = SearchByAuthor(authorToSearch);
                    if (foundByAuthor.Count > 0)
                    {
                        foreach (var book in foundByAuthor)
                        {
                            Console.WriteLine($"{book.BookId} - {book.Title} - {book.Author}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No books found for this author.");
                    }
                    break;

                case "4":
                    Console.Write("Enter Book ID to issue: ");
                    int issueId = int.Parse(Console.ReadLine() ?? "0");
                    IssueBook(issueId);
                    break;

                case "5":
                    Console.Write("Enter Book ID to return: ");
                    int returnId = int.Parse(Console.ReadLine() ?? "0");
                    ReturnBook(returnId);
                    break;

                case "6":
                    Console.WriteLine("Exiting program.");
                    return;

                default:
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }

    public static void Main(string[] args)
    {
        AddDetails(1, "Python Programming", "Shravan");
        AddDetails(2, "C# Basics", "Ravi");
        Menu();
    }
}
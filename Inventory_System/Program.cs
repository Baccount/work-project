﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public DateTime PublicationDate { get; set; }
    public int NumberOfCopies { get; set; }
    public bool IsBorrowed { get; set; }
}


public class Program
{
    private static BookInventory inventory = new BookInventory();

    public static void Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("Book Inventory Management");
            Console.WriteLine("1. Add new book");
            Console.WriteLine("2. Remove book");
            Console.WriteLine("3. Update book");
            Console.WriteLine("4. Search book");
            Console.WriteLine("5. Exit");

            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddNewBook();
                    break;
                case 2:
                    RemoveBook();
                    break;
                case 3:
                    UpdateBook();
                    break;
                case 4:
                    //SearchBook();
                    break;
                case 5:
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void AddNewBook()
    {
        try{
            Console.Write("Enter book title: ");
            string title = Console.ReadLine();

            Console.Write("Enter book author: ");
            string author = Console.ReadLine();

            Console.Write("Enter book ISBN: ");
            string isbn = Console.ReadLine();

            Console.Write("Enter book publication date (YYYY-MM-DD): ");
            DateTime publicationDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter number of copies: ");
            int numberOfCopies = int.Parse(Console.ReadLine());

            Book book = new Book
            {
                Title = title,
                Author = author,
                ISBN = isbn,
                PublicationDate = publicationDate,
                NumberOfCopies = numberOfCopies,
                IsBorrowed = false
            };

            inventory.AddBook(book);
            Console.WriteLine("Book added successfully.");
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
        }
    }

    private static void RemoveBook()
    {
        Console.Write("Enter book ISBN or title to remove: ");
        string identifier = Console.ReadLine();

        inventory.RemoveBook(identifier);

        //inventory.RemoveBook(identifier);
        Console.WriteLine("Book removed successfully.");
    }

    private static void UpdateBook()
    {
        // Run the UpdateBookDetails method
        Console.Write("Enter book ISBN or title to update: ");
        string identifier = Console.ReadLine();
        inventory.UpdateBookDetails(identifier);
    }
private static void SearchBook()
    {
        Console.WriteLine("Search by: 1) Title, 2) Author, or 3) ISBN");
        int option = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter search term:");
        string searchTerm = Console.ReadLine();

        Book foundBook = null;

        switch (option)
        {
            case 1:
                //foundBook = inventory.SearchBook(title: searchTerm);
                break;
            case 2:
                //foundBook = inventory.SearchBook(author: searchTerm);
                break;
            case 3:
                //foundBook = inventory.SearchBook(isbn: searchTerm);
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }

        if (foundBook != null)
        {
            Console.WriteLine($"Title: {foundBook.Title}");
            Console.WriteLine($"Author: {foundBook.Author}");
            Console.WriteLine($"ISBN: {foundBook.ISBN}");
            Console.WriteLine($"Publication Date: {foundBook.PublicationDate}");
            Console.WriteLine($"Number of Copies: {foundBook.NumberOfCopies}");
            Console.WriteLine($"Loan Status: {(foundBook.IsBorrowed ? "Borrowed" : "Available")}");
        }
        else
        {
            Console.WriteLine("Book not found.");
        }
    }
}













































public class BookInventory
{

    private List<Book> books;
    private string inventoryFilePath = "inventory.csv";

    public BookInventory()
    {
        CreateCSVFile();
        books = ReadFromCSV();
    }

    // DONE!
    private void CreateCSVFile()
    {
        // create the CSV file if it doesn't exist
        if (!File.Exists(inventoryFilePath))
        {
            File.Create(inventoryFilePath).Close();
        }
    }
    // DONE!
    private List<Book> ReadFromCSV()
    {
        try
        {
            using (var reader = new StreamReader(inventoryFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<BookMap>();
                var records = csv.GetRecords<Book>().ToList();
                return records;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading from CSV file: {ex.Message}");
            return new List<Book>();
        }
    }
    // DONE!
    private void WriteToCSV()
    {
        try
        {
            using (var writer = new StreamWriter(inventoryFilePath))
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
            {

                // write the data to the CSV file
                csv.WriteRecords(books);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error writing to CSV file: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }



    // Implement UpdateBookDetails, and SearchBook methods

    // DONE!
    public void AddBook(Book book)
    {
        // write the book to the CSV file
        books.Add(book);
        WriteToCSV();
    }

    // DONE!
    public void RemoveBook(string identifier)
    {
        // remove any book with the given ISBN or title
        books.RemoveAll(b => b.ISBN == identifier);
        WriteToCSV();

    }
    // TODO in progress
    public void UpdateBookDetails(string identifier)
    {
        // find the book to update
        Book book = books.Find(b => b.ISBN == identifier);
        if (book == null)
        {
            Console.WriteLine("Book not found.");
            return;
        }
        // if book is found, update the book details and write to CSV file
        Console.Write("1. Enter number of copies ");
        Console.WriteLine();
        Console.Write("2. Change loan status ");
        Console.WriteLine();
        int option = int.Parse(Console.ReadLine());
        if (option == 1)
        {
            try{
                Console.Write("Enter number of copies: ");
                int numberOfCopies = int.Parse(Console.ReadLine());
                book.NumberOfCopies = numberOfCopies;
                WriteToCSV();
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                }
        }
        else if (option == 2)
        {
            try{
                Console.Write("Is the book Borrowed? True or False: ");
                bool isBorrowed = bool.Parse(Console.ReadLine());
                book.IsBorrowed = isBorrowed;
                WriteToCSV();
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                }
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }
}




    // DONE!
    public class BookMap : ClassMap<Book>
{
    public BookMap()
    {
        Map(m => m.Title).Name("Title");
        Map(m => m.Author).Name("Author");
        Map(m => m.ISBN).Name("ISBN");
        Map(m => m.PublicationDate).Name("PublicationDate");
        Map(m => m.NumberOfCopies).Name("NumberOfCopies");
        Map(m => m.IsBorrowed).Name("IsBorrowed");
    }
}

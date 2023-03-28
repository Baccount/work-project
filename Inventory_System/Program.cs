﻿using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;


public class Book
{
    // Declare a nullable string property called Title with getter and setter
    public string? Title { get; set; }

    // Declare a nullable string property called Author with getter and setter
    public string? Author { get; set; }

    // Declare a nullable string property called ISBN with getter and setter
    public string? ISBN { get; set; }

    // Declare a DateTime property called PublicationDate with getter and setter
    public DateTime PublicationDate { get; set; }

    // Declare an int property called NumberOfCopies with getter and setter
    public int NumberOfCopies { get; set; }

    // Declare a bool property called IsBorrowed with getter and setter
    public bool IsBorrowed { get; set; }
}


public class Program
{
    // Declare a static BookInventory object called inventory
    private static BookInventory inventory = new BookInventory();

    // Define the Main method, which is the entry point of the console application
    public static void Main(string[] args)
    {
        // Declare a boolean variable called exit and initialize it to false
        bool exit = false;

        // Use a while loop to keep the menu running until the user decides to exit
        while (!exit)
        {
            // Display the menu options to the user
            Console.WriteLine("Book Inventory Management");
            Console.WriteLine("1. Add new book");
            Console.WriteLine("2. Remove book");
            Console.WriteLine("3. Update book");
            Console.WriteLine("4. Search book");
            Console.WriteLine("5. Exit");

            // Prompt the user to enter their choice
            Console.Write("Enter your choice: ");
            int choice = int.Parse(Console.ReadLine());

            // Execute the appropriate action based on the user's choice
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
                    SearchBook();
                    break;
                case 5:
                    // Set the exit variable to true to exit the while loop and end the program
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
        try
        {
            // Prompt the user to enter the book details
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

            // Create a new Book object and set its properties with the entered values
            Book book = new Book
            {
                Title = title,
                Author = author,
                ISBN = isbn,
                PublicationDate = publicationDate,
                NumberOfCopies = numberOfCopies,
                IsBorrowed = false
            };

            // Call the AddBook method from the inventory object with the new book
            inventory.AddBook(book);

            Console.WriteLine("Book added successfully.");
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
        }
    }


    private static void RemoveBook()
    {
        // Prompt the user to enter the book's ISBN or title to remove
        Console.Write("Enter book ISBN or title to remove: ");
        string identifier = Console.ReadLine();

        // Call the RemoveBook method from the inventory object with the given identifier
        inventory.RemoveBook(identifier);

        // Display a message to the user indicating the book has been removed successfully
        Console.WriteLine("Book removed successfully.");
    }

    private static void UpdateBook()
    {
        // Prompt the user to enter the ISBN or Title of the book to update
        Console.Write("Enter the ISBN or Title of your book: ");
        string identifier = Console.ReadLine();

        // Call the UpdateBookDetails method from the inventory object with the given identifier
        inventory.UpdateBookDetails(identifier);
    }
private static void SearchBook()
    {
        // Display search options to the user
        Console.WriteLine("Search by:");
        Console.WriteLine("1. Title");
        Console.WriteLine("2. Author");
        Console.WriteLine("3. ISBN");

        // Read the user's search option as an integer
        int option = int.Parse(Console.ReadLine());

        // Prompt the user to enter the search term
        Console.WriteLine("Enter search term:");
        string searchTerm = Console.ReadLine();
        Console.WriteLine();

        // Declare a nullable Book object called foundBook and initialize it to null
        Book? foundBook = null;

        // Search for the book based on the user's search option
        switch (option)
        {
            case 1:
                foundBook = inventory.SearchBook(title: searchTerm);
                break;
            case 2:
                foundBook = inventory.SearchBook(author: searchTerm);
                break;
            case 3:
                foundBook = inventory.SearchBook(isbn: searchTerm);
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
        // If a book is found, display its details
        if (foundBook != null)
        {
            Console.WriteLine("Book found:");
            Console.WriteLine($"Title: {foundBook.Title}");
            Console.WriteLine($"Author: {foundBook.Author}");
            Console.WriteLine($"ISBN: {foundBook.ISBN}");
            Console.WriteLine($"Publication Date: {foundBook.PublicationDate}");
            Console.WriteLine($"Number of Copies: {foundBook.NumberOfCopies}");
            Console.WriteLine($"Loan Status: {(foundBook.IsBorrowed ? "Borrowed" : "Available")}");
            Console.WriteLine();
        }
        // If no book is found, display a message to the user
        else
        {
            Console.WriteLine("Book not found.");
            Console.WriteLine();
        }
    }
}













































public class BookInventory
{
    // Declare a private list of Book objects called books
    private List<Book> books;

    // Declare a private string called inventoryFilePath, which represents the path to the inventory CSV file
    private string inventoryFilePath = "inventory.csv";

    // Define the constructor for the BookInventory class
    public BookInventory()
    {
        // Call the CreateCSVFile method to create the CSV file if it doesn't already exist
        CreateCSVFile();

        // Call the ReadFromCSV method to read the books list from the CSV file
        books = ReadFromCSV();
    }

    private void CreateCSVFile()
    {
        // Check if the CSV file exists at the specified inventoryFilePath
        if (!File.Exists(inventoryFilePath))
        {
            // If the CSV file doesn't exist, create it and close the file handle
            File.Create(inventoryFilePath).Close();
        }
    }




    private List<Book> ReadFromCSV()
    {
        try
        {
            // Create a StreamReader instance to read from the inventoryFilePath
            using (var reader = new StreamReader(inventoryFilePath))
            // Create a CsvReader instance with CultureInfo.InvariantCulture
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Register the BookMap class to configure the mapping between the Book class and the CSV columns
                csv.Context.RegisterClassMap<BookMap>();

                // Read the records from the CSV file and convert them to a list of Book objects
                var records = csv.GetRecords<Book>().ToList();

                // Return the list of Book objects
                return records;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading from CSV file: {ex.Message}");

            // Return an empty list of Book objects in case of an error
            return new List<Book>();
        }
    }

    private void WriteToCSV()
    {
        try
        {
            // Create a StreamWriter instance to write to the inventoryFilePath
            using (var writer = new StreamWriter(inventoryFilePath))
            // Create a CsvWriter instance with a specified delimiter and CultureInfo
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "," }))
            {
                // Write the data from the books list to the CSV file
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




    public void AddBook(Book book)
    {
        // Read the books from the CSV file and store them in the books list
        books = ReadFromCSV();

        // Use the LINQ FirstOrDefault method to find the first occurrence of a book with the same ISBN
        Book existingBook = books.FirstOrDefault(b => b.ISBN == book.ISBN);

        if (existingBook != null)
        {
            // If the book already exists, update the number of copies by adding the new copies
            existingBook.NumberOfCopies += book.NumberOfCopies;
        }
        else
        {
            // If the book doesn't exist, add it to the list
            books.Add(book);
        }

        // Write the updated books list back to the CSV file
        WriteToCSV();
    }

    public void RemoveBook(string identifier)
    {
        // Read the books from the CSV file and store them in the books list
        books = ReadFromCSV();

        // Remove any book with the given identifier (ISBN) or title from the books list
        books.RemoveAll(b => b.ISBN == identifier || b.Title == identifier);

        // Write the updated books list back to the CSV file
        WriteToCSV();
    }

    public void UpdateBookDetails(string identifier)
    {
        // Read the books from the CSV file and store them in the books list
        books = ReadFromCSV();

        // Find the book to update using the provided identifier (ISBN) or title
        Book book = books.Find(b => b.ISBN == identifier || b.Title == identifier);

        // If no matching book is found, display a message and return
        if (book == null)
        {
            Console.WriteLine("Book not found.");
            Console.WriteLine();
            return;
        }

        // If a matching book is found, display the available update options
        Console.WriteLine("1. Enter number of copies ");
        Console.WriteLine("2. Change loan status ");

        // Read the user's choice from the console
        int option = int.Parse(Console.ReadLine());


        switch (option) {
            case 1:
                try{
                // Prompt the user for the new number of copies and update the book
                Console.Write("Enter number of copies: ");
                int numberOfCopies = int.Parse(Console.ReadLine());
                book.NumberOfCopies = numberOfCopies;

                // Write the updated books list back to the CSV file
                WriteToCSV();
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                }
                break;

            case 2:
                try
                {
                    // Prompt the user for the new loan status
                    Console.Write("Is the book Borrowed? True or False: ");
                    string input = Console.ReadLine();

                    // Parse the user input, allowing for case-insensitive input
                    if (bool.TryParse(input, out bool isBorrowed))
                    {
                        // Update the book's loan status
                        book.IsBorrowed = isBorrowed;

                        // Write the updated books list back to the CSV file
                        WriteToCSV();
                    }
                    else
                    {
                        // Handle invalid input
                        Console.WriteLine("Invalid input. Please enter 'True' or 'False'.");
                    }
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                }
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }

    public Book? SearchBook(string? title = null, string? author = null, string? isbn = null)
    {
        // Read books from the CSV file and store them in the books list
        books = ReadFromCSV();

        // Find the first book in the books list that matches the provided title, author, or ISBN, or return null if no match is found
        Book book = books.FirstOrDefault(b => (title == null || b.Title == title) &&
                                        (author == null || b.Author == author) &&
                                        (isbn == null || b.ISBN == isbn));

        // If no matching book is found, return null
        if (book == null)
        {
            return null;
        }

        // If a matching book is found, return it
        return book;
    }

}




// Define a class called BookMap that inherits from ClassMap<Book> provided by CsvHelper
public class BookMap : ClassMap<Book>
{
    // BookMap constructor for mapping Book properties to CSV columns
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

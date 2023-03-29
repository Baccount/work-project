using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

// Credit: Brandon Moore :)
// Install CsvHelper using this command in the terminal:
// dotnet add package CsvHelper
// https://github.com/JoshClose/CsvHelper


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
            Console.WriteLine("Book Inventory System");
            Console.WriteLine("1. Add new book");
            Console.WriteLine("2. Remove book");
            Console.WriteLine("3. Update book");
            Console.WriteLine("4. Search book");
            Console.WriteLine("5. Exit");

            // Prompt the user to enter their choice
            Console.Write("Enter your choice: ");
            string input = Console.ReadLine() ?? "";
            int choice;

            if (!int.TryParse(input, out choice))
            {
                // Parsing failed
                // Handle the error or prompt the user to enter a valid input
                Console.WriteLine("Invalid choice. Please try again.");
                Main(args);
            }

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
            string title = Console.ReadLine() ?? "Default Title";

            Console.Write("Enter book author: ");
            string author = Console.ReadLine() ?? "Default Author";

            Console.Write("Enter book ISBN: ");
            string isbn = Console.ReadLine() ?? "0000000000000";

            Console.Write("Enter book publication date (YYYY-MM-DD): ");
            DateTime publicationDate = DateTime.Parse(Console.ReadLine() ?? "0001-01-01");

            Console.Write("Enter number of copies: ");
            int numberOfCopies = int.Parse(Console.ReadLine() ?? "0");

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

            Console.WriteLine();
            Console.WriteLine("Book added successfully.");
            Console.WriteLine();
        }
        catch(Exception e){
            Console.WriteLine(e.Message);
            AddNewBook();
        }
    }


    private static void RemoveBook()
    {
        // Prompt the user to enter the book's ISBN or title to remove
        Console.Write("Enter book ISBN or title to remove: ");
        string identifier = Console.ReadLine() ?? "";

        // Call the RemoveBook method from the inventory object with the given identifier
        inventory.RemoveBook(identifier);

        // Display a message to the user indicating the book has been removed successfully
        Console.WriteLine("Book removed successfully.");
    }

    private static void UpdateBook()
    {
        // Prompt the user to enter the ISBN or Title of the book to update
        Console.Write("Enter the ISBN or Title of your book: ");
        string identifier = Console.ReadLine() ?? "";

        // Call the UpdateBookDetails method from the inventory object with the given identifier
        inventory.UpdateBookDetails(identifier);
    }
private static void SearchBook()
    {
        // Display search options to the user
        Console.WriteLine("Search by");
        Console.WriteLine("1. Title");
        Console.WriteLine("2. Author");
        Console.WriteLine("3. ISBN");

        // Read the user's search option as an integer
        int option = int.Parse(Console.ReadLine() ?? "0");

        // Prompt the user to enter the search term
        Console.WriteLine("Enter search term:");
        string searchTerm = Console.ReadLine() ?? "";
        Console.WriteLine();

        // Check if the search term is empty or the option is 0
        if (searchTerm == "" || option == 0)
        {
            // Inform the user that the search term or option is invalid
            Console.WriteLine("Invalid search term or invalid option.");

            // Print an empty line for better output readability
            Console.WriteLine();

            // Call the SearchBook function to prompt the user for a new search term and option
            SearchBook();
        }


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
                Console.WriteLine();
                SearchBook();
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
        int option = int.Parse(Console.ReadLine() ?? "0");

        // If the user enters an invalid option, display a message and call the UpdateBookDetails method again
        if (option == 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
            UpdateBookDetails(identifier);
        }


        switch (option) {
            case 1:
                try{
                    int numberOfCopies;
                    bool validInt = false;

                    while (!validInt)
                    {
                        Console.Write("Enter number of copies: ");
                        string input = Console.ReadLine() ?? "";

                        // Check if the input can be parsed as an integer
                        if (int.TryParse(input, out numberOfCopies))
                        {
                            validInt = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid integer.");
                        }
                    // Update the book's number of copies
                    book.NumberOfCopies = numberOfCopies;
                    }
                }
                catch(Exception e){
                    Console.WriteLine(e.Message);
                }
                // Write the updated books list back to the CSV file
                WriteToCSV();
                break;

            case 2:
                bool validInput = false;

                while (!validInput)
                {
                    try
                    {
                        // Prompt the user for the new loan status
                        Console.Write("Is the book Borrowed? True or False: ");
                        string input = Console.ReadLine() ?? "";

                        // Parse the user input, allowing for case-insensitive input
                        if (bool.TryParse(input, out bool isBorrowed))
                        {
                            // Update the book's loan status
                            book.IsBorrowed = isBorrowed;

                            // Write the updated books list back to the CSV file
                            WriteToCSV();

                            // Set validInput to true to exit the loop
                            validInput = true;
                        }
                        else
                        {
                            // Handle invalid input
                            Console.WriteLine("Invalid input. Please enter 'True' or 'False'.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                break;
            default:
                Console.WriteLine("Invalid option.");
                break;
        }
        Console.WriteLine();
        Console.WriteLine("Book details updated.");
        Console.WriteLine();
    }

    public Book? SearchBook(string? title = null, string? author = null, string? isbn = null)
    {
        // Read books from the CSV file and store them in the books list
        books = ReadFromCSV();

        // Lowercase all the search parameters
        title = title?.ToLower();
        author = author?.ToLower();

        // Use the LINQ FirstOrDefault method to find the first occurrence of a book with the same ISBN, title or author
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

// Represents a book with related properties
public class Book
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? ISBN { get; set; }
    public DateTime PublicationDate { get; set; }
    public int NumberOfCopies { get; set; }
    public bool IsBorrowed { get; set; }
}

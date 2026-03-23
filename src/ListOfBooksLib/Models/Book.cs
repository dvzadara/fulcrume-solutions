namespace ListOfBooksLib.Models
{
    public class Book
    {
        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;
        
        public int NumberOfPages { get; set; }
    }
}

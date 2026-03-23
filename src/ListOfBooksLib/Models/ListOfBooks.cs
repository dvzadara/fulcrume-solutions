using ListOfBooksLib.Repositories;

namespace ListOfBooksLib.Models
{
    public class ListOfBooks
    {
        private readonly List<Book> _books = new List<Book>();

        public List<Book> Books { get => this._books; }

        public ListOfBooks(List<Book> books)
        {
            this._books = books;
        }

        public static ListOfBooks Load(string filePath)
        {
            return XmlBookStorage.Load(filePath);
        }

        public void AddBook(Book book)
        {
            this._books.Add(book);
        }

        public void Sort()
        {
            this._books.Sort((a, b) =>
            {
                var authorCompare = string.Compare(a.Author, b.Author, StringComparison.Ordinal);

                if (authorCompare != 0)
                {
                    return authorCompare;
                }

                return string.Compare(a.Title, b.Title, StringComparison.Ordinal);
            });
        }

        public Book? SearchByTitle(string titlePart)
        {
            return this._books.FirstOrDefault(x => x.Title.Contains(titlePart));
        }

        public void SaveToXml(string filePath)
        {
            XmlBookStorage.Save(this, filePath);
        }
    }
}

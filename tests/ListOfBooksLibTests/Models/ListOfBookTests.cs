using ListOfBooksLib.Models;
using ListOfBooksLib.Repositories;

namespace ListOfBooksTests
{
    public class ListOfBooksTests
    {
        [Fact]
        public void AddBook_ShouldWorkCorrectly()
        {
            // Arrange
            var list = new ListOfBooks(new List<Book>());
            var book = new Book
            {
                Author = "Robert C. Martin",
                Title = "Clean Code",
                NumberOfPages = 464
            };

            // Act
            list.AddBook(book);

            // Assert
            Assert.Single(list.Books);
            Assert.Equal(list.Books.First().Author, book.Author);
            Assert.Equal(list.Books.First().Title, book.Title);
            Assert.Equal(list.Books.First().NumberOfPages, book.NumberOfPages);
        }

        [Fact]
        public void SortByAuthor_ShouldSortAuthorsAlphabetically()
        {
            // Arrange
            var list = new ListOfBooks(new List<Book>
            {
                new Book { Author = "Robert Sedgewick", Title = "Algorithms in C++" },
                new Book { Author = "Bjarne Stroustrup", Title = "The C++ Programming Language" },
                new Book { Author = "Donald Knuth", Title = "The Art of Computer Programming" }
            });

            // Act
            list.Sort();

            // Assert
            Assert.Equal("Bjarne Stroustrup", list.Books[0].Author);
            Assert.Equal("Donald Knuth", list.Books[1].Author);
            Assert.Equal("Robert Sedgewick", list.Books[2].Author);
        }

        [Fact]
        public void SortByAuthor_ShouldSortByTitleWithinSameAuthor()
        {
            // Arrange
            var list = new ListOfBooks(new List<Book>
            {
                new Book { Author = "Donald Knuth", Title = "TAOCP Vol 2" },
                new Book { Author = "Donald Knuth", Title = "TAOCP Vol 1" }
            });

            // Act
            list.Sort();

            // Assert
            Assert.Equal("TAOCP Vol 1", list.Books[0].Title);
            Assert.Equal("TAOCP Vol 2", list.Books[1].Title);
        }

        [Fact]
        public void SearchByTitle_ShouldReturnMatch_WhenContainsPart()
        {
            // Arrange
            var list = new ListOfBooks(new List<Book>
            {
                new Book { Author = "Bjarne Stroustrup", Title = "The C++ Programming Language" }
            });

            // Act
            var result = list.SearchByTitle("Programming");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Bjarne Stroustrup", result!.Author);
        }

        [Fact]
        public void SearchByTitle_ShouldReturnNull_WhenNoMatch()
        {
            // Arrange
            var list = new ListOfBooks(new List<Book>
            {
                new Book { Author = "Robert Martin", Title = "Clean Code" }
            });

            // Act
            var result = list.SearchByTitle("Python");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SearchByTitle_ShouldBeCaseSensitive()
        {
            // Arrange
            var list = new ListOfBooks(new List<Book>
            {
                new Book { Author = "Robert Martin", Title = "Clean Code" }
            });

            // Act
            var result = list.SearchByTitle("clean");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Save_ShouldCreateFile()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();

            var books = new ListOfBooks(new List<Book>
            {
                new Book
                {
                    Author = "Bjarne Stroustrup",
                    Title = "The C++ Programming Language",
                    NumberOfPages = 1376
                }
            });

            // Act
            XmlBookStorage.Save(books, tempFile);

            // Assert
            Assert.True(File.Exists(tempFile));

            // Cleanup
            File.Delete(tempFile);
        }

        [Fact]
        public void Load_ShouldReturnSameBooks_AfterSave()
        {
            // Arrange
            var tempFile = Path.GetTempFileName();

            var original = new ListOfBooks(new List<Book>
            {
                new Book
                {
                    Author = "Robert Martin",
                    Title = "Clean Code",
                    NumberOfPages = 464
                }
            });

            XmlBookStorage.Save(original, tempFile);

            // Act
            var loaded = XmlBookStorage.Load(tempFile);

            // Assert
            Assert.Single(loaded.Books);
            Assert.Equal(original.Books[0].Author, loaded.Books[0].Author);
            Assert.Equal(original.Books[0].Title, loaded.Books[0].Title);
            Assert.Equal(original.Books[0].NumberOfPages, loaded.Books[0].NumberOfPages);

            // Cleanup
            File.Delete(tempFile);
        }
    }
}
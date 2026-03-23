using ListOfBooksLib.Models;
using System.Xml.Serialization;

namespace ListOfBooksLib.Repositories
{
    public static class XmlBookStorage
    {
        public static ListOfBooks Load(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path can`t be null or white space.", nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File {filePath} was not found");
            }

            try
            {
                var serializer = new XmlSerializer(typeof(List<Book>));

                using var stream = new FileStream(filePath, FileMode.Open);
                var books = (List<Book>)serializer.Deserialize(stream)!;

                return new ListOfBooks(books);
            }
            catch (IOException ex) when (IsFileLocked(ex))
            {
                throw new IOException($"File is in use: '{filePath}'", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading file '{filePath}'", ex);
            }
        }

        public static void Save(ListOfBooks books, string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path can`t be null or white space.", nameof(filePath));
            }

            try
            {
                var directory = Path.GetDirectoryName(filePath);

                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    throw new DirectoryNotFoundException($"Directory does not exist: '{directory}'");
                }

                var serializer = new XmlSerializer(typeof(List<Book>));

                using var stream = new FileStream(
                    filePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None);

                serializer.Serialize(stream, books.Books);
            }
            catch (DirectoryNotFoundException) 
            {
                throw;
            }
            catch (IOException ex) when (IsFileLocked(ex))
            {
                throw new IOException($"File is in use: '{filePath}'", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing file '{filePath}'", ex);
            }
        }

        private static bool IsFileLocked(IOException ex)
        {
            int code = ex.HResult & 0xFFFF;
            return code == 32 || code == 33;
        }
    }
}

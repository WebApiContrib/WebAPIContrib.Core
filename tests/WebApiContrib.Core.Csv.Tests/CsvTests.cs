using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace WebApiContrib.Core.Csv.Tests
{
    // note: the JSON tests are here to verify that the two formatters do not conflict with each other
    public class CsvTests
    {
        private TestServer _server;
        public CsvTests()
        {
            _server = new TestServer(new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>());
        }

        [Fact]
        public async Task GetCollection_Text_Csv_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/Csv"));
            var result = await client.SendAsync(request);
            var books = Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.Equal(2, books.Length);
            Assert.Equal(Book.Data[0].Author, books[0].Author);
            Assert.Equal(Book.Data[0].Title, books[0].Title);
            Assert.Equal(Book.Data[1].Author, books[1].Author);
            Assert.Equal(Book.Data[1].Title, books[1].Title);
        }

        [Fact(Skip = "Returns json instead of csv")]
        public async Task GetById_Text_Csv_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/Csv"));
            var result = await client.SendAsync(request);
            var book = Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync())).Single();

            Assert.NotNull(book);
            Assert.Equal(Book.Data[0].Author, book.Author);
            Assert.Equal(Book.Data[0].Title, book.Title);
        }

        [Fact(Skip = "Returns json instead of csv")]
        public async Task Post_Text_Csv_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/Csv"));

            var book = new Book
            {
                Author = "Tim Parks",
                Title = "Italian Ways: On and off the Rails from Milan to Palermo"
            };

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            Serialize(writer,new[] { book });
            writer.Flush();

            HttpContent data = new StreamContent(stream);
            data.Headers.ContentType = new MediaTypeHeaderValue("text/Csv");

            request.Content = data;
            var result = await client.SendAsync(request);

            var echo = Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync())).Single();

            Assert.NotNull(book);
            Assert.Equal(book.Author, echo.Author);
            Assert.Equal(book.Title, echo.Title);
        }

        [Fact]
        public async Task GetCollection_JSON_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.SendAsync(request);
            var books = JsonConvert.DeserializeObject<Book[]>(await result.Content.ReadAsStringAsync());

            Assert.Equal(2, books.Length);
            Assert.Equal(Book.Data[0].Author, books[0].Author);
            Assert.Equal(Book.Data[0].Title, books[0].Title);
            Assert.Equal(Book.Data[1].Author, books[1].Author);
            Assert.Equal(Book.Data[1].Title, books[1].Title);
        }

        private static T[] Deserialize<T>(StreamReader reader)
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Delimiter = ";";
                csv.Configuration.RegisterClassMap<BookMap>();
                return csv.GetRecords<T>().ToArray();
            }
        }

        private static void Serialize<T>(StreamWriter writer, IEnumerable<T> obj)
        {
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture, leaveOpen: true))
            {
                csv.Configuration.RegisterClassMap<BookMap>();

                csv.WriteRecords(obj);
            }
        }

        private class BookMap : ClassMap<Book>
        {
            public BookMap()
            {
                Map(m => m.Title)
                    .Index(0);
                Map(m => m.Author)
                    .Index(1);
            }
        }
    }
}

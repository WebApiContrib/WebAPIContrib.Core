using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace WebApiContrib.Core.Protobuf.Tests
{
    // note: the JSON tests are here to verify that the two formatters do not conflict with each other
    public class ProtobufTests
    {
        private TestServer _server;

        public ProtobufTests()
        {
            _server = new TestServer(new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>());
        }

        [Fact]
        public async Task GetCollection_X_Protobuf_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
            var result = await client.SendAsync(request);
            var books = ProtoBuf.Serializer.Deserialize<Book[]>(await result.Content.ReadAsStreamAsync());

            Assert.Equal(2, books.Length);
            Assert.Equal(Book.Data[0].Author, books[0].Author);
            Assert.Equal(Book.Data[0].Title, books[0].Title);
            Assert.Equal(Book.Data[1].Author, books[1].Author);
            Assert.Equal(Book.Data[1].Title, books[1].Title);
        }

        [Fact]
        public async Task GetCollection_Protobuf_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/protobuf"));
            var result = await client.SendAsync(request);
            var books = ProtoBuf.Serializer.Deserialize<Book[]>(await result.Content.ReadAsStreamAsync());

            Assert.Equal(2, books.Length);
            Assert.Equal(Book.Data[0].Author, books[0].Author);
            Assert.Equal(Book.Data[0].Title, books[0].Title);
            Assert.Equal(Book.Data[1].Author, books[1].Author);
            Assert.Equal(Book.Data[1].Title, books[1].Title);
        }

        [Fact]
        public async Task GetCollection_X_Google_Protobuf_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-google-protobuf"));
            var result = await client.SendAsync(request);
            var books = ProtoBuf.Serializer.Deserialize<Book[]>(await result.Content.ReadAsStreamAsync());

            Assert.Equal(2, books.Length);
            Assert.Equal(Book.Data[0].Author, books[0].Author);
            Assert.Equal(Book.Data[0].Title, books[0].Title);
            Assert.Equal(Book.Data[1].Author, books[1].Author);
            Assert.Equal(Book.Data[1].Title, books[1].Title);
        }

        [Fact]
        public async Task GetById_Protobuf_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/protobuf"));
            var result = await client.SendAsync(request);
            var book = ProtoBuf.Serializer.Deserialize<Book>(await result.Content.ReadAsStreamAsync());

            Assert.NotNull(book);
            Assert.Equal(Book.Data[0].Author, book.Author);
            Assert.Equal(Book.Data[0].Title, book.Title);
        }

        [Fact]
        public async Task GetById_X_Protobuf_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
            var result = await client.SendAsync(request);
            var book = ProtoBuf.Serializer.Deserialize<Book>(await result.Content.ReadAsStreamAsync());

            Assert.NotNull(book);
            Assert.Equal(Book.Data[0].Author, book.Author);
            Assert.Equal(Book.Data[0].Title, book.Title);
        }

        [Fact]
        public async Task GetById_X__Google_Protobuf_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-google-protobuf"));
            var result = await client.SendAsync(request);
            var book = ProtoBuf.Serializer.Deserialize<Book>(await result.Content.ReadAsStreamAsync());

            Assert.NotNull(book);
            Assert.Equal(Book.Data[0].Author, book.Author);
            Assert.Equal(Book.Data[0].Title, book.Title);
        }
        [Fact]
        public async Task Post_X_Protobuf_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));

            var book = new Book
            {
                Author = "Tim Parks",
                Title = "Italian Ways: On and off the Rails from Milan to Palermo"
            };

            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<Book>(stream, book);

            HttpContent data = new StreamContent(stream);
            data.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");

            request.Content = data;
            var result = await client.SendAsync(request);

            var echo = ProtoBuf.Serializer.Deserialize<Book>(await result.Content.ReadAsStreamAsync());

            Assert.NotNull(book);
            Assert.Equal(book.Author, echo.Author);
            Assert.Equal(book.Title, echo.Title);
        }

        [Fact]
        public async Task Post_Protobuf_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/protobuf"));

            var book = new Book
            {
                Author = "Tim Parks",
                Title = "Italian Ways: On and off the Rails from Milan to Palermo"
            };

            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<Book>(stream, book);

            HttpContent data = new StreamContent(stream);
            data.Headers.ContentType = new MediaTypeHeaderValue("application/protobuf");

            request.Content = data;
            var result = await client.SendAsync(request);

            var echo = ProtoBuf.Serializer.Deserialize<Book>(await result.Content.ReadAsStreamAsync());

            Assert.NotNull(book);
            Assert.Equal(book.Author, echo.Author);
            Assert.Equal(book.Title, echo.Title);
        }

        [Fact]
        public async Task Post_X_Google_Protobuf_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-google-protobuf"));

            var book = new Book
            {
                Author = "Tim Parks",
                Title = "Italian Ways: On and off the Rails from Milan to Palermo"
            };

            MemoryStream stream = new MemoryStream();
            ProtoBuf.Serializer.Serialize<Book>(stream, book);

            HttpContent data = new StreamContent(stream);
            data.Headers.ContentType = new MediaTypeHeaderValue("application/x-google-protobuf");

            request.Content = data;
            var result = await client.SendAsync(request);

            var echo = ProtoBuf.Serializer.Deserialize<Book>(await result.Content.ReadAsStreamAsync());

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
    }
}

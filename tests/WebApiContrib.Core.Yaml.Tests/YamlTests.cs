using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using YamlDotNet.Serialization;

namespace WebApiContrib.Core.Yaml.Tests
{
    // note: the JSON tests are here to verify that the two formatters do not conflict with each other
    public class YamlTests
    {
        private TestServer _server;

        public YamlTests()
        {
            _server = new TestServer(new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>());
        }

        [Fact]
        public async Task GetCollection_X_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-yaml"));
            var result = await client.SendAsync(request);
            var books = new Deserializer().Deserialize<Book[]>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.Equal(2, books.Length);
            Assert.Equal(Book.Data[0].Author, books[0].Author);
            Assert.Equal(Book.Data[0].Title, books[0].Title);
            Assert.Equal(Book.Data[1].Author, books[1].Author);
            Assert.Equal(Book.Data[1].Title, books[1].Title);
        }

        [Fact]
        public async Task GetCollection_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/yaml"));
            var result = await client.SendAsync(request);
            var books = new Deserializer().Deserialize<Book[]>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.Equal(2, books.Length);
            Assert.Equal(Book.Data[0].Author, books[0].Author);
            Assert.Equal(Book.Data[0].Title, books[0].Title);
            Assert.Equal(Book.Data[1].Author, books[1].Author);
            Assert.Equal(Book.Data[1].Title, books[1].Title);
        }

        [Fact]
        public async Task GetCollection_Text_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/yaml"));
            var result = await client.SendAsync(request);
            var books = new Deserializer().Deserialize<Book[]>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.Equal(2, books.Length);
            Assert.Equal(Book.Data[0].Author, books[0].Author);
            Assert.Equal(Book.Data[0].Title, books[0].Title);
            Assert.Equal(Book.Data[1].Author, books[1].Author);
            Assert.Equal(Book.Data[1].Title, books[1].Title);
        }

        [Fact]
        public async Task GetCollection_Text_X_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/x-yaml"));
            var result = await client.SendAsync(request);
            var books = new Deserializer().Deserialize<Book[]>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.Equal(2, books.Length);
            Assert.Equal(Book.Data[0].Author, books[0].Author);
            Assert.Equal(Book.Data[0].Title, books[0].Title);
            Assert.Equal(Book.Data[1].Author, books[1].Author);
            Assert.Equal(Book.Data[1].Title, books[1].Title);
        }

        [Fact]
        public async Task GetById_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/yaml"));
            var result = await client.SendAsync(request);
            var book = new Deserializer().Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.NotNull(book);
            Assert.Equal(Book.Data[0].Author, book.Author);
            Assert.Equal(Book.Data[0].Title, book.Title);
        }

        [Fact]
        public async Task GetById_X_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-yaml"));
            var result = await client.SendAsync(request);
            var book = new Deserializer().Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.NotNull(book);
            Assert.Equal(Book.Data[0].Author, book.Author);
            Assert.Equal(Book.Data[0].Title, book.Title);
        }

        [Fact]
        public async Task GetById_Text_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/yaml"));
            var result = await client.SendAsync(request);
            var book = new Deserializer().Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.NotNull(book);
            Assert.Equal(Book.Data[0].Author, book.Author);
            Assert.Equal(Book.Data[0].Title, book.Title);
        }

        [Fact]
        public async Task GetById_Text_X_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/x-yaml"));
            var result = await client.SendAsync(request);
            var book = new Deserializer().Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.NotNull(book);
            Assert.Equal(Book.Data[0].Author, book.Author);
            Assert.Equal(Book.Data[0].Title, book.Title);
        }

        [Fact]
        public async Task Post_X_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-yaml"));

            var book = new Book
            {
                Author = "Tim Parks",
                Title = "Italian Ways: On and off the Rails from Milan to Palermo"
            };

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            new Serializer().Serialize(writer, book);
            writer.Flush();

            HttpContent data = new StreamContent(stream);
            data.Headers.ContentType = new MediaTypeHeaderValue("application/x-yaml");

            request.Content = data;
            var result = await client.SendAsync(request);

            var echo = new Deserializer().Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.NotNull(book);
            Assert.Equal(book.Author, echo.Author);
            Assert.Equal(book.Title, echo.Title);
        }

        [Fact]
        public async Task Post_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/yaml"));

            var book = new Book
            {
                Author = "Tim Parks",
                Title = "Italian Ways: On and off the Rails from Milan to Palermo"
            };

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            new Serializer().Serialize(writer, book);
            writer.Flush();

            HttpContent data = new StreamContent(stream);
            data.Headers.ContentType = new MediaTypeHeaderValue("application/yaml");

            request.Content = data;
            var result = await client.SendAsync(request);

            var echo = new Deserializer().Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.NotNull(book);
            Assert.Equal(book.Author, echo.Author);
            Assert.Equal(book.Title, echo.Title);
        }

        [Fact]
        public async Task Post_Text_Yaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/yaml"));

            var book = new Book
            {
                Author = "Tim Parks",
                Title = "Italian Ways: On and off the Rails from Milan to Palermo"
            };

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            new Serializer().Serialize(writer, book);
            writer.Flush();

            HttpContent data = new StreamContent(stream);
            data.Headers.ContentType = new MediaTypeHeaderValue("text/yaml");

            request.Content = data;
            var result = await client.SendAsync(request);

            var echo = new Deserializer().Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync()));

            Assert.NotNull(book);
            Assert.Equal(book.Author, echo.Author);
            Assert.Equal(book.Title, echo.Title);
        }

        [Fact]
        public async Task Post_Text_X_sYaml_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/x-yaml"));

            var book = new Book
            {
                Author = "Tim Parks",
                Title = "Italian Ways: On and off the Rails from Milan to Palermo"
            };

            MemoryStream stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            new Serializer().Serialize(writer, book);
            writer.Flush();

            HttpContent data = new StreamContent(stream);
            data.Headers.ContentType = new MediaTypeHeaderValue("text/x-yaml");

            request.Content = data;
            var result = await client.SendAsync(request);

            var echo = new Deserializer().Deserialize<Book>(new StreamReader(await result.Content.ReadAsStreamAsync()));

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

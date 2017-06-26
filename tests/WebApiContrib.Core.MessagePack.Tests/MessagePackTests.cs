using MessagePack;
using MessagePack.Resolvers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WebApiContrib.Core.MessagePack.Tests
{
    public class MessagePackTests
    {
        private TestServer _server;

        public MessagePackTests()
        {
            _server = new TestServer(new WebHostBuilder()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>());
        }

        [Fact]
        public async Task GetCollection_MessagePack_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-msgpack"));
            var result = await client.SendAsync(request);
            var books = MessagePackSerializer.Deserialize<Book[]>(await result.Content.ReadAsStreamAsync(), ContractlessStandardResolver.Instance);

            Assert.Equal(2, books.Length);
            Assert.Equal(Book.Data[0].Author, books[0].Author);
            Assert.Equal(Book.Data[0].Title, books[0].Title);
            Assert.Equal(Book.Data[1].Author, books[1].Author);
            Assert.Equal(Book.Data[1].Title, books[1].Title);
        }

        [Fact]
        public async Task GetById_MessagePack_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-msgpack"));
            var result = await client.SendAsync(request);
            var book = MessagePackSerializer.Deserialize<Book>(await result.Content.ReadAsStreamAsync(), ContractlessStandardResolver.Instance);

            Assert.NotNull(book);
            Assert.Equal(Book.Data[0].Author, book.Author);
            Assert.Equal(Book.Data[0].Title, book.Title);
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

        [Fact]
        public async Task GetById_JSON_Header()
        {
            var client = _server.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/books/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var result = await client.SendAsync(request);

            var book = JsonConvert.DeserializeObject<Book>(await result.Content.ReadAsStringAsync());

            Assert.NotNull(book);
            Assert.Equal(Book.Data[0].Author, book.Author);
            Assert.Equal(Book.Data[0].Title, book.Title);
        }
    }
}

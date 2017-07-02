namespace WebApiContrib.Core.Yaml.Tests
{
    public class Book
    {
        public static Book[] Data = new[]
            {
                new Book { Title = "Our Mathematical Universe: My Quest for the Ultimate Nature of Reality", Author = "Max Tegmark"},
                new Book { Title = "Hockey Towns", Author = "Ron MacLean"},
            };

        public string Title { get; set; }

        public string Author { get; set; }
    }
}

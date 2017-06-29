using ProtoBuf;

namespace WebApiContrib.Core.Protobuf.Tests
{
    [ProtoContract]
    public class Book
    {
        public static Book[] Data = new[]
            {
                new Book { Title = "Our Mathematical Universe: My Quest for the Ultimate Nature of Reality", Author = "Max Tegmark"},
                new Book { Title = "Hockey Towns", Author = "Ron MacLean"},
            };

        [ProtoMember(1)]
        public string Title { get; set; }

        [ProtoMember(2)]
        public string Author { get; set; }
    }
}

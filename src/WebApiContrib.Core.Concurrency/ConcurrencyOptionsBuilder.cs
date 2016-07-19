using WebApiContrib.Core.Concurrency.Storage;

namespace WebApiContrib.Core.Concurrency
{
    public class ConcurrencyOptionsBuilder
    {
        public ConcurrencyOptions ConcurrencyOptions { get; private set; }

        internal ConcurrencyOptionsBuilder()
        {
            ConcurrencyOptions = new ConcurrencyOptions();
        }
               
        public void UseInMemoryStorage()
        {
            ConcurrencyOptions.Storage = new InMemoryStorage();
        }
    }
}

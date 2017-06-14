namespace WebApiContrib.Core.Samples.Services
{
    public class WelcomeService : IGreetService
    {
        public string Greet()
        {
            return "Welcome!";
        }
    }
}

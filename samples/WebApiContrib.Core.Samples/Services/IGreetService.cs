using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiContrib.Core.Samples.Services
{
    public interface IGreetService
    {
        string Greet();
    }

    public class WelcomeService : IGreetService
    {
        public string Greet()
        {
            return "Welcome!";
        }
    }

    public class HiService : IGreetService
    {
        public string Greet()
        {
            return "Hi!";
        }
    }
}

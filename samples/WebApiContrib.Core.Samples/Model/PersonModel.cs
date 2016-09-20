namespace WebApiContrib.Core.Samples.Model
{
    public class PersonModel
    {
        public PersonModel(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public int Age { get; }

        public class V2
        {
            public V2(PersonModel person)
            {
                Age = person.Age;
                Name = new Name(person.FirstName, person.LastName);
            }

            public int Age { get; }

            public Name Name { get; }
        }

        public class Name
        {
            public Name(string first, string last)
            {
                First = first;
                Last = last;
            }

            public string First { get; }

            public string Last { get; }
        }
    }
}
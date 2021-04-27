using Newtonsoft.Json;
using Xunit;

namespace AltradyNotifier.Test
{
    public class Converters
    {
        [Fact]
        public void TrimmingConverter_Serialize_Test()
        {
            Person alice;
            string aliceJson;

            alice = new Person
            {
                FirstName = "Alice",
                LastName = "   Wonderland   "
            };
            aliceJson = JsonConvert.SerializeObject(alice);

            Assert.DoesNotContain(aliceJson, x => char.IsWhiteSpace(x));


            alice = new Person
            {
                FirstName = "   Alice   ",
                LastName = "   Wonderland   "
            };
            aliceJson = JsonConvert.SerializeObject(alice);

            Assert.Contains(aliceJson, x => char.IsWhiteSpace(x));
        }

        [Fact]
        public void TrimmingConverter_Deserialize_Test()
        {
            string bobJson;
            Person bob;


            bobJson = "{\"FirstName\":\"Bob\",\"LastName\":\"   Wonderland   \"}";
            bob = JsonConvert.DeserializeObject<Person>(bobJson);

            Assert.DoesNotContain(bob.FirstName, x => char.IsWhiteSpace(x));
            Assert.DoesNotContain(bob.LastName, x => char.IsWhiteSpace(x));


            bobJson = "{\"FirstName\":\"   Bob   \",\"LastName\":\"   Wonderland   \"}";
            bob = JsonConvert.DeserializeObject<Person>(bobJson);

            Assert.Contains(bob.FirstName, x => char.IsWhiteSpace(x));
            Assert.DoesNotContain(bob.LastName, x => char.IsWhiteSpace(x));
        }

        private class Person
        {
            public string FirstName { get; set; }

            [JsonConverter(typeof(Logic.Converters.TrimmingConverter))]
            public string LastName { get; set; }
        }
    }
}

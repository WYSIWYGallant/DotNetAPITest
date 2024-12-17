namespace appAPI.Models
{
    public class Person
    {
        public int PersonID { get; set; }
        public string PersonName { get; set; } = string.Empty;
        public int PersonAge { get; set; }
        public int PersonType { get; set; }

        public override string ToString()
        {
            return $"{PersonName}, {PersonAge}, {PersonType},{PersonID}";
        }
    }
}
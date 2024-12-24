using System;

namespace TelerikWebApp1.Models
{
    public class Person
    {

//Person ID (int) 
        public int PersonID { get; set; }
//Person name(String)
        public string PersonName { get; set; } = string.Empty;
//Person AGE(int)
        public int PersonAge { get; set; }
 //Person Type(int)
        public int PersonType { get; set; }

        public override string ToString()
        {
            return $"{PersonName}, {PersonAge}, {PersonType},{PersonID}";
        }
    }
}
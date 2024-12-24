using System.Collections.Generic;

namespace TelerikWebApp1.Models
{
    public class PersonType
    {
        public List<PersonType> PersonTypes { get; set; } = new List<PersonType>();
        public int PersonTypeID { get; set; }
        public string PersonTypeDescription { get; set; }

        public PersonType(int personTypeID, string personTypeDescription)
        {
            PersonTypeID = personTypeID;
            PersonTypeDescription = personTypeDescription;
        }
    }
}
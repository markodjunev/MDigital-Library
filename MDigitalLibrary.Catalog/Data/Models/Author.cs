namespace MDigitalLibrary.Catalog.Data.Models
{
    using MDigitalLibrary.Models.CommonDb;

    public class Author : BaseDeletableModel<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

        public string PlaceOfBirth { get; set; }
    }
}

namespace MDigitalLibrary.Catalog.Data.Models
{
    using MDigitalLibrary.Models.CommonDb;

    public class Library : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
    }
}

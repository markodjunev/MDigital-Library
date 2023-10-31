namespace MDigitalLibrary.Catalog.Data.Seeders
{
    using MDigitalLibrary.Catalog.Data.Models;
    using MDigitalLibrary.Services;
    using System.Globalization;

    public class AuthorDataSeeder : IDataSeeder
    {
        private static IEnumerable<Author> GetData()
            => new List<Author>
            {
                new Author{ FirstName = "Fyodor", LastName = "Dostoevski", 
                    Birthday = DateTime.ParseExact("November 11, 1821", "MMMM d, yyyy", new CultureInfo("en-US"), DateTimeStyles.AssumeUniversal),
                    PlaceOfBirth = "Moscow" },
            };

        private readonly CatalogDbContext db;

        public AuthorDataSeeder(CatalogDbContext db) => this.db = db;

        public void SeedData()
        {
            if (this.db.Authors.Any())
            {
                return;
            }

            foreach (var author in GetData())
            {
                this.db.Authors.Add(author);
            }

            this.db.SaveChanges();
        }
    }
}

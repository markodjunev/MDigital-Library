namespace MDigitalLibrary.Catalog.Data.Seeders
{
    using MDigitalLibrary.Catalog.Data.Models;
    using MDigitalLibrary.Services;

    public class LibraryDataSeeder : IDataSeeder
    {
        private static IEnumerable<Library> GetData()
            => new List<Library>
            {
                new Library{ Name = "National Library", Address = "square Pencho Slaveykov 4", City = "Sofia" },
                new Library{ Name = "University Library", Address = "str. Sveti Kliment Ohridski 4", City = "Sofia" },
                new Library{ Name = "City Library", Address = "str. Serdika 1", City = "Sofia" },

            };

        private readonly CatalogDbContext db;

        public LibraryDataSeeder(CatalogDbContext db) => this.db = db;

        public void SeedData()
        {
            if (this.db.Libraries.Any())
            {
                return;
            }

            foreach (var Library in GetData())
            {
                this.db.Libraries.Add(Library);
            }

            this.db.SaveChanges();
        }
    }
}

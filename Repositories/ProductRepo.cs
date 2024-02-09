using DECKMASTER.Data;
using DECKMASTER.Models;

namespace DECKMASTER.Repositories
{
    public class ProductRepo
    {
        private readonly ApplicationDbContext _db;

        public ProductRepo(ApplicationDbContext db)
        {
            this._db = db;
        }
        public List<Product> GetAllProducts()
        {
            // Assuming _db is your DbContext instance
            // and Product is the DbSet representing your products in the database
            List<Product> products = _db.Products.ToList();

            return products;
        }
    }
}

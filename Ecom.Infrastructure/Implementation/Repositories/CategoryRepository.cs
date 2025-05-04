using Ecom.Application.Interfaces.Repositories;
using Ecom.Domain.Entities.Product;
using Ecom.Infrastructure.Data;

namespace Ecom.Infrastructure.Implementation.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {

        }
        // Add any additional methods specific to Category repository here
        // For future functionality
    }
}

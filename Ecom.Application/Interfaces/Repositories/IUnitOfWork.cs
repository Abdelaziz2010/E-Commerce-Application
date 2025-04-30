using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        ICartRepository CartRepository { get; }
        Task<int> SaveChangesAsync();
    }
}



namespace Ecom.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        ICartRepository CartRepository { get; }
        IAuthRepository AuthRepository { get; }
        IReviewRepository ReviewRepository { get; }
    }
}

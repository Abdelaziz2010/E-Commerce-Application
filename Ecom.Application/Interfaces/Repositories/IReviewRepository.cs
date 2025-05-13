
using Ecom.Application.DTOs.Reviews;
using Ecom.Domain.Entities.Product;

namespace Ecom.Application.Interfaces.Repositories
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<bool> AddReviewAsync(ReviewDTO reviewDTO, string email);
        Task<IReadOnlyList<ReturnReviewDTO>> GetAllReviewsForProductAsync(int productId);
        Task<bool> DeleteReviewAsync(int reviewId); 
    }
}

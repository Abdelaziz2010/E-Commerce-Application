using AutoMapper;
using Ecom.Application.DTOs.Reviews;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Domain.Entities;
using Ecom.Domain.Entities.Product;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Infrastructure.Implementation.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository 
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public ReviewRepository(AppDbContext context, UserManager<AppUser> userManager, IMapper mapper): base(context)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> AddReviewAsync(ReviewDTO reviewDTO, string email)
        {
            if (reviewDTO is null || email is null)
            {
                return false;
            }
            
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return false;
            }

            // ensure that a user can only has add review
            if (await _context.Reviews.AsNoTracking().AnyAsync(x => x.AppUserId == user.Id && x.ProductId == reviewDTO.ProductId))
            {
                return false;
            }

            var review = new Review
            {
                Stars = reviewDTO.Stars,
                Content = reviewDTO.Content,
                AppUserId = user.Id,
                ProductId = reviewDTO.ProductId,
            };

            await _context.Reviews.AddAsync(review);
            
            await _context.SaveChangesAsync();

            // counting the average rating for a product after adding a new review 
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == reviewDTO.ProductId);

            var reviews = await _context.Reviews.AsNoTracking().Where(x => x.ProductId == product.Id).ToListAsync();
            
            if(reviews.Count > 0)
            {
                double average = reviews.Average(x => x.Stars);
                double roundedRating = Math.Round(average * 2, MidpointRounding.AwayFromZero) / 2;
                product.Rating = roundedRating;
            }
            else
            {
                product.Rating = reviewDTO.Stars;
            }

            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            if(reviewId == 0)
            {
                return false;
            }

            var review = await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(x => x.Id == reviewId);

            if (review is null)
            {
                return false;
            }
            
            _context.Remove(review);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IReadOnlyList<ReturnReviewDTO>> GetAllReviewsForProductAsync(int productId)
        {
            if (productId == 0)
            {
                return null!;
            }

            var reviews = await _context.Reviews.Include(x => x.AppUser)
                .AsNoTracking().Where(x => x.ProductId == productId).ToListAsync();

            if(reviews == null)
            {
                return null!;
            }

            var reviewDTO = _mapper.Map<IReadOnlyList<ReturnReviewDTO>>(reviews); 

            return reviewDTO;
        }
    }
}

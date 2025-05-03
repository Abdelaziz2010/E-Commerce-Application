using AutoMapper;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Application.Services.Implementation;
using Ecom.Application.Services.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;

namespace Ecom.Infrastructure.Implementation.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IConnectionMultiplexer _redis;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IImageManagementService _imageManagementService;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private IPhotoRepository _photoRepository;
        private ICartRepository _cartRepository;
        private IAuthRepository _authRepository;

        //public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);
        //public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
        //public IPhotoRepository PhotoRepository => _photoRepository ??= new PhotoRepository(_context);

        public UnitOfWork(
            AppDbContext context, 
            IImageManagementService imageManagementService, 
            IMapper mapper, 
            IConnectionMultiplexer redis,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _redis = redis;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
            _userManager = userManager;
        }

        //lazy initialization
        public IProductRepository ProductRepository 
        { 
            get
            {
                if(_productRepository == null)
                {
                    _productRepository = new ProductRepository(_context, _mapper, _imageManagementService);
                }
                return _productRepository;
            }
        }
        public ICategoryRepository CategoryRepository 
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }
                return _categoryRepository;
            }
        }
        public IPhotoRepository PhotoRepository
        {
            get
            {
                if (_photoRepository == null)
                {
                    _photoRepository = new PhotoRepository(_context);
                }
                return _photoRepository;
            }
        }
        public ICartRepository CartRepository
        {
            get
            {
                if (_cartRepository == null)
                {
                    _cartRepository = new CartRepository(_redis);
                }
                return _cartRepository;
            }
        }
        public IAuthRepository AuthRepository
        {
            get
            {
                if (_authRepository == null)
                {
                    _authRepository = new AuthRepository(_userManager);
                }
                return _authRepository;
            }
        }
    }
}

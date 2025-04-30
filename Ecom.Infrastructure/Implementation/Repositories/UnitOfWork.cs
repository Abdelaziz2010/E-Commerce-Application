using AutoMapper;
using Ecom.Application.Interfaces.Repositories;
using Ecom.Application.Services.Implementation;
using Ecom.Application.Services.Interfaces;
using Ecom.Infrastructure.Data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Implementation.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private IPhotoRepository _photoRepository;
        private ICartRepository cartRepository;
        private IImageManagementService _imageManagementService;
        private IMapper _mapper;
        private readonly IConnectionMultiplexer _redis;


        //public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);
        //public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_context);
        //public IPhotoRepository PhotoRepository => _photoRepository ??= new PhotoRepository(_context);

        public UnitOfWork(AppDbContext context, IImageManagementService imageManagementService, IMapper mapper, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
            _mapper = mapper;
            _imageManagementService = imageManagementService;
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
                if (cartRepository == null)
                {
                    cartRepository = new CartRepository(_redis);
                }
                return cartRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

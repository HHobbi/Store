using Microsoft.AspNetCore.Hosting;
using Store.Application.Interfaces.Contexts;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.Products.Commands.AddNewCategory;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Application.Services.Products.Commands.DeleteProduct;
using Store.Application.Services.Products.Queries.GetAllCategories;
using Store.Application.Services.Products.Queries.GetCategories;
using Store.Application.Services.Products.Queries.GetProductDetailForAdmin;
using Store.Application.Services.Products.Queries.GetProductDetailForSite;
using Store.Application.Services.Products.Queries.GetProductForAdmin;
using Store.Application.Services.Products.Queries.GetProductForsite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.FacadPattern
{
    public class ProductFacade : IProductFacad
    {

        private readonly IDatabaseContext _db;
        private readonly IHostingEnvironment _environment;
        public ProductFacade(IDatabaseContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _environment = hostingEnvironment;
        }


        private IAddNewCategoryService _addNewCategoryService;
        public IAddNewCategoryService AddNewCategoryService
        {
            get { return _addNewCategoryService = _addNewCategoryService ?? new AddNewCategoryService(_db); }
        }



       

        private IGetCategoriesService _getCategoriesService;
        public IGetCategoriesService GetCategoriesService
        {
            get { return _getCategoriesService = _getCategoriesService ?? new GetCategoriesService(_db); }
        }

        private IDeleteProductService _deleteProductService;
        public IDeleteProductService DeleteProductService
        {
            get { return _deleteProductService = _deleteProductService ?? new DeleteProductService(_db); }

        }

        private IAddNewProductservice _addNewProductService;
        public IAddNewProductservice AddNewProductService
        {
            get { return _addNewProductService = _addNewProductService ?? new AddNewProductService(_db, _environment); }

        }

        private IGetAllCategoriesService _getAllCategoriesService;
        public IGetAllCategoriesService GetAllCategoriesService
        {
            get { return _getAllCategoriesService = _getAllCategoriesService ?? new GetAllCategoriesService(_db); }

        }


        private IGetProductForAdminService _getProductForAdminService;
        public IGetProductForAdminService GetProductForAdminService
        {
            get { return _getProductForAdminService = _getProductForAdminService ?? new GetProductForAdminService(_db); }

        }

        private IGetProductDetailForAdminService _getProductDetailForAdminService;
        public IGetProductDetailForAdminService GetProductDetailForAdminService
        {
            get { return _getProductDetailForAdminService = _getProductDetailForAdminService ?? new GetProductDetailForAdminService(_db); }

        }

        private IGetProductForSiteService _getProductForSiteService;  
            public IGetProductForSiteService GetProductForSiteService
        {
            get { return _getProductForSiteService = _getProductForSiteService ?? new GetProductForSiteService(_db); }

        }

        private IGetProductDetailForSiteService _getProductDetailForSiteService;
        public IGetProductDetailForSiteService GetProductDetailForSiteService
        {
            get { return _getProductDetailForSiteService = _getProductDetailForSiteService ?? new GetProductDetailForSiteService(_db); }

        }

    }
}

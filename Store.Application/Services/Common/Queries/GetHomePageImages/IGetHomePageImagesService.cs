using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.HomePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Common.Queries.GetHomePageImages
{
    public  interface IGetHomePageImagesService
    {
        ResultDto<List<HomePageImagesDto>> Execute();
    }
    public class GetHomePageImagesService : IGetHomePageImagesService
    {
        private readonly IDatabaseContext _db;
        public GetHomePageImagesService(IDatabaseContext db)
        {
            _db = db;
        }
        public ResultDto<List<HomePageImagesDto>> Execute()
        {
            var images = _db.HomePageImages.OrderByDescending(p => p.Id)
                .Select(p => new HomePageImagesDto
                {
                    id=p.Id,
                    Link=p.Link,
                    Src=p.Src,
                    ImageLocation=p.ImageLocation,
                }).ToList();

            return new ResultDto<List<HomePageImagesDto>>
            {
                Data=images,
                IsSuccess=true,
                Message=""
            };
        }
        
    }
    public class HomePageImagesDto
    {
        public long id { get; set; }
        public string Link { get; set; }
        public string Src { get; set; }
        public ImageLocation ImageLocation { get; set; }
    }
}

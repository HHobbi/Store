using Store.Application.Services.Common.Queries.GetHomePageImages;
using Store.Application.Services.Common.Queries.GetSlider;
using Store.Application.Services.Products.Queries.GetProductForsite;
using System.Collections.Generic;

namespace Endpoint.Site.Models.ViewModels.HomePage
{
    public class HomePageViewModel
    {
        public List<SliderDto> Sliders { get; set; }
        public List<HomePageImagesDto> PageImages { get; set; }
        public List<ProductForsiteDto> Camera { get; set; }
        public List<ProductForsiteDto> Mobile { get; set; }
        public List<ProductForsiteDto> Kitchen_Tools { get; set; }
        public List<ProductForsiteDto> Laptop { get; set; }
    }
}

using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Common.Queries.GetSlider
{
    public interface IGetSliderService
    {
        ResultDto<List<SliderDto>> Execute();
    }
    public class GetSliderService : IGetSliderService
    {
        private readonly IDatabaseContext _db;

        public GetSliderService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<List<SliderDto>> Execute()
        {
            var slider = _db.Sliders.OrderByDescending(p => p.Id).ToList().Select(p => new SliderDto
            {
                Src = p.Src,
                Link = p.Link
            }).ToList();

            return new ResultDto<List<SliderDto>>
            {
                IsSuccess = true,
                Data=slider
            };
        }
    }
    public class SliderDto
    {
        public string Src { get; set; }
        public string Link { get; set; }
    }
}

using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finance.Queries.GetRequestPayService
{
    public interface IGetRequestPayService
    {
        ResultDto<RequestPayDto> Execute(Guid guid);
    }
    public class GetRequestPayService:IGetRequestPayService
    {
        private readonly IDatabaseContext _db;
        public GetRequestPayService(IDatabaseContext db)
        {
            _db = db;
        }

        ResultDto<RequestPayDto> IGetRequestPayService.Execute(Guid guid)
        {
            var requestPay = _db.RequestPays.Where(p=>p.Guid==guid).FirstOrDefault();
            if (requestPay != null)
            {
                return new ResultDto<RequestPayDto>
                {
                    Data = new RequestPayDto
                    {
                        Amount = requestPay.Amount,
                        id=requestPay.Id,
                    }
                };
            }
            else 
            {
                throw new Exception("Request Not Found ");
            }
        }
    }
    public class RequestPayDto
    {
        public int Amount { get; set; }
        public long id { get; set; }
    }
}

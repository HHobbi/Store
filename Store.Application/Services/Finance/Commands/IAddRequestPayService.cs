using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Finance.Commands
{
    public interface IAddRequestPayService
    {
        ResultDto<ResultRequestPay> Execute(int Amount,long UserId);
    }

    public class AddRequestPayService:IAddRequestPayService
    {
        private readonly IDatabaseContext _db;
        public AddRequestPayService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto<ResultRequestPay> Execute(int Amount, long UserId)
        {
            var user = _db.Users.Find(UserId);
            RequestPay requestPay = new RequestPay
            {
                Amount=Amount,
                Guid=Guid.NewGuid(),
                UserId=UserId,
                IsPay=false

            };
            _db.RequestPays.Add(requestPay);
            _db.SaveChanges();
            return new ResultDto<ResultRequestPay>
            {
                Data = new ResultRequestPay
                {
                    guid = requestPay.Guid,
                    Amount = requestPay.Amount,
                    Email = user.Email,
                    RequestPayId = requestPay.Id
                },
                IsSuccess=true
                
        
        
            };
        }
    }

    public class ResultRequestPay 
    {
        public Guid guid { get; set; }
        public int Amount { get; set; }
        public string Email { get; set; }
        public long RequestPayId { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Carts;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Carts
{

    public interface ICartService
    {

        ResultDto AddToCart(long ProductId, Guid BrowserId);
        ResultDto RemoveFromCart(long cartItemID, Guid BrowserId);
        ResultDto<CartDto> GetMyCart(Guid BrowserId, long? UserId);
        ResultDto Add(long CartItemId);
        ResultDto LowOff(long CartItemId);
    }

    public class CartService : ICartService
    {
        private readonly IDatabaseContext _db;
        public CartService(IDatabaseContext db)
        {
            _db = db;
        }

        public ResultDto Add(long CartItemId)
        {
            var cartItem = _db.CartItems.Where(p => p.Id == CartItemId).FirstOrDefault();
            cartItem.Count++;
            _db.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true
            };
        }

        public ResultDto AddToCart(long ProductId, Guid BrowserId)
        {
            var cart = _db.Carts.Where(p => p.BrowserId == BrowserId && p.IsFinished == false).FirstOrDefault();
            if (cart == null)
            {
                Cart newCart = new Cart()
                {
                    IsFinished = false,
                    BrowserId = BrowserId,
                };
                _db.Carts.Add(newCart);
                _db.SaveChanges();
                cart = newCart;
            }


            var product = _db.Products.Find(ProductId);

            var cartItem = _db.CartItems.Where(p => p.ProductId == ProductId && p.CartId == cart.Id).FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.Count++;
            }
            else
            {
                CartItem newCartItem = new CartItem()
                {
                    Cart = cart,
                    Count = 1,
                    Price = product.Price,
                    Product = product,


                };
                _db.CartItems.Add(newCartItem);
                _db.SaveChanges();
            }

            return new ResultDto()
            {
                IsSuccess = true,
                Message = $"محصول  {product.Name} با موفقیت به سبد خرید شما اضافه شد ",
            };
        }

        public ResultDto<CartDto> GetMyCart(Guid BrowserId, long? UserId)
        {
            try
            {
                var cart = _db.Carts
                    .Include(p => p.CartItems)
                    .ThenInclude(p => p.Product)
                    .ThenInclude(p => p.ProductImages)
                    .Where(p => p.BrowserId == BrowserId && p.IsFinished == false)
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefault();

                if (cart == null)
                {
                    return new ResultDto<CartDto>()
                    {
                        Data = new CartDto()
                        {
                            CartItems = new List<CartItemDto>(),

                        },
                    };
                }
                if (cart == null)
                {
                    return new ResultDto<CartDto>()
                    {
                        Data=new CartDto() 
                        {
                            CartItems=new List<CartItemDto>(), 
                        }

                    };
                }
                if (UserId != null)
                {
                    var user = _db.Users.Find(UserId);
                    cart.Users = user;
                    _db.SaveChanges();
                }

                return new ResultDto<CartDto>()
                {
                    Data = new CartDto()
                    {
                        ProductCount = cart.CartItems.Count(),
                        SumAmount = cart.CartItems.Sum(p => p.Price * p.Count),
                        CartId = cart.Id,
                        CartItems = cart.CartItems.Select(p => new CartItemDto
                        {
                            Count = p.Count,
                            Price = p.Price,
                            Product = p.Product.Name,
                            Id = p.Id,
                            Images = p.Product?.ProductImages?.FirstOrDefault()?.Src ?? "",
                        }).ToList(),
                    },
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ResultDto LowOff(long CartItemId)
        {
            var cartItem = _db.CartItems.Find(CartItemId);
            if (cartItem.Count <= 1)
            {
                return new ResultDto
                {
                    IsSuccess = false
                };
            }
            cartItem.Count--;
            _db.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true
            };
        }

        public ResultDto RemoveFromCart(long cartItemId, Guid BrowserId)
        {
            var cart = _db.Carts.Where(p => p.BrowserId == BrowserId).FirstOrDefault();
            var cartItem = _db.CartItems.Where(p => p.Id == cartItemId && p.CartId == cart.Id).FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.IsRemoved = true;
                cartItem.Removetime = DateTime.Now;
                _db.SaveChanges();

                return new ResultDto()
                {
                    IsSuccess = true,
                    Message = "محصول با موفقیت حذف شد ."
                };
            }
            else
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "محصول یافت نشد"
                };
            }
        }
    }

    public class CartDto
    {

        public long CartId { get; set; }
        public int ProductCount { get; set; }
        public int SumAmount { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }

    public class CartItemDto
    {
        public long Id { get; set; }
        public string Product { get; set; }
        public string Images { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }

    }
}

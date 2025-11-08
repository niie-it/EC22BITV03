using Microsoft.AspNetCore.Mvc;
using MyEStore.Entities;
using MyEStore.Models;

namespace MyEStore.Controllers
{
	public class CartController : Controller
	{
		private readonly MyeStoreContext _ctx;

		public CartController(MyeStoreContext ctx)
		{
			_ctx = ctx;
		}

		public List<CartItem> CartItems
		{
			get
			{
				var carts = HttpContext.Session.Get<List<CartItem>>("CART");
				if (carts == null)
				{
					carts = new List<CartItem>();
				}
				return carts;
			}
		}


		public IActionResult AddToCart(int id, int qty = 1)
		{
			var gioHang = CartItems;//giỏ hàng hiện tại

			// kiểm tra id (MaHH) truyền qua đã nằm trong giỏ hàng hay chưa
			var item = gioHang.SingleOrDefault(p => p.MaHh == id);
			if (item != null) //đã có
			{
				item.SoLuong += qty;
			}
			else
			{
				var hangHoa = _ctx.HangHoas.SingleOrDefault(p => p.MaHh == id);
				item = new CartItem
				{
					MaHh = id,
					TenHh = hangHoa.TenHh,
					DonGia = hangHoa.DonGia ?? 0,
					Hinh = hangHoa.Hinh,
					SoLuong = qty
				};
				gioHang.Add(item);
			}
			HttpContext.Session.Set("CART", gioHang);

			return RedirectToAction("Index"); //để hiện giỏ hàng
		}

		public IActionResult RemoveCart(int id)
		{
			var gioHang = CartItems;
			var item = gioHang.SingleOrDefault(p => p.MaHh == id);
			if (item != null)
			{
				gioHang.Remove(item);
			}
			HttpContext.Session.Set("CART", gioHang);
			return RedirectToAction("Index");
		}

		public IActionResult UpdateCart(int id, int qty)
		{
			var gioHang = CartItems;
			var item = gioHang.SingleOrDefault(p => p.MaHh == id);
			if (item != null)
			{
				item.SoLuong = qty;
			}
			HttpContext.Session.Set("CART", gioHang);
			return RedirectToAction("Index");
		}

		public IActionResult Index()
		{
			return View(CartItems);
		}
	}
}

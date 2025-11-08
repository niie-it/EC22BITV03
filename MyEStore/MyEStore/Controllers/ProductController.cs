using Microsoft.AspNetCore.Mvc;
using MyEStore.Entities;
using MyEStore.Models;

namespace MyEStore.Controllers
{
	public class ProductController : Controller
	{
		private readonly MyeStoreContext _ctx;

		public ProductController(MyeStoreContext context)
		{
			_ctx = context;
		}

		public IActionResult Index()
		{
			var data = _ctx.HangHoas.AsQueryable();
			var result = data.Select(hh => new HangHoaVM
			{
				MaHh = hh.MaHh,
				TenHh = hh.TenHh,
				DonGia = hh.DonGia ?? 0,
				Hinh = hh.Hinh
			}).ToList();
			return View(result);

		}

		public IActionResult Detail(int id)
		{
			var product = _ctx.HangHoas.SingleOrDefault(p => p.MaHh == id);
			if (product == null)
			{
				return RedirectToAction("Index");
			}
			return View(product);
		}
	}
}

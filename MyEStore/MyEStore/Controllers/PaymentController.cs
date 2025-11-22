using Microsoft.AspNetCore.Mvc;
using MyEStore.Models;
using Newtonsoft.Json.Linq;

namespace MyEStore.Controllers
{
	public class PaymentController : Controller
	{
		private readonly PaypalClient _paypalClient;

		public PaymentController(PaypalClient paypalClient)
		{
			_paypalClient = paypalClient;
		}


		[HttpPost("/payment/create-paypal-order")]
		public async Task<IActionResult> CreateOrder([FromBody]Amount amount)
		{
			//Mã hóa đơn trong DB nhà mình (phải tạo đơn hàng trước)
			var refId = DateTime.Now.Ticks.ToString();

			try
			{
				var response = await _paypalClient.CreateOrder(amount.value, amount.currency_code, refId);
				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}

		[HttpPost("/payment/capture-paypal-order")]
		public IActionResult CaptureOrder(string orderId)
		{
			try
			{
				var response = _paypalClient.CaptureOrder(orderId);
				
				//Lưu đơn hàng (cập nhật trạng thái) vô database

				return Ok(response);
			}
			catch (Exception ex)
			{
				var error = new { ex.GetBaseException().Message };
				return BadRequest(error);
			}
		}
	}
}

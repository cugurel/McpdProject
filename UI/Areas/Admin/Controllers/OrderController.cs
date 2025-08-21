using DataAccess.Concrete;
using DinkToPdf;
using DinkToPdf.Contracts;
using Entity.Concrete;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using UI.Models.Identity;
using PaperKind = DinkToPdf.PaperKind;


namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
		Context context = new Context();

		private readonly ApplicationContext _context;
		private readonly IConverter _converter;

		public OrderController(ApplicationContext context, IConverter converter)
		{
			_context = context;
			_converter = converter;
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult OrderDetail(int id)
		{
			var orderDetails = context.OrderDetails.Where(x => x.OrderId == id).ToList();
			var result = (from od in orderDetails
						  join p in context.Products on od.ProductId equals p.Id
						  select new OrderDetailDto
						  {
							  ProductName = p.Name,
							  Price = od.UnitPrice,
							  Quantity = od.Quantity,
							  TotalPrice = od.TotalPrice,
						  }).ToList();
			

			return View(result);
		}

		public IActionResult OrderReceipt(int id)
		{
			// Veriyi çek
			var orderDetails = context.OrderDetails.Where(x => x.OrderId == id).ToList();

			var result = (from od in orderDetails
						  join p in context.Products on od.ProductId equals p.Id
						  select new OrderDetailDto
						  {
							  ProductName = p.Name,
							  Price = od.UnitPrice,
							  Quantity = od.Quantity,
							  TotalPrice = od.TotalPrice,
						  }).ToList();

			var tr = new CultureInfo("tr-TR");

			var sb = new StringBuilder();
			sb.Append(@"
<!DOCTYPE html>
<html lang='tr'>
<head>
<meta charset='utf-8' />
<style>
  body { font-family: DejaVu Sans, Arial, Helvetica, sans-serif; font-size: 12px; }
  h2 { text-align: center; margin: 0 0 16px 0; }
  table { width: 100%; border-collapse: collapse; }
  th, td { border: 1px solid #ddd; padding: 8px; text-align:center; }
  th { background-color: #f2f2f2; }
  .totals { margin-top:12px; text-align:right; font-weight:bold; }
</style>
</head>
<body>
  <h2>Sipariş Fişi #" + id + @"</h2>
  <table>
    <thead>
      <tr>
        <th>Ürün Adı</th>
        <th>Birim Fiyat</th>
        <th>Adet</th>
        <th>Toplam</th>
      </tr>
    </thead>
    <tbody>");

			decimal grandTotal = 0m;
			foreach (var item in result)
			{
				grandTotal += item.TotalPrice;
				sb.Append($@"
      <tr>
        <td>{System.Net.WebUtility.HtmlEncode(item.ProductName)}</td>
        <td>{item.Price.ToString("C", tr)}</td>
        <td>{item.Quantity}</td>
        <td>{item.TotalPrice.ToString("C", tr)}</td>
      </tr>");
			}

			sb.Append($@"
    </tbody>
  </table>
  <div class='totals'>Genel Toplam: {grandTotal.ToString("C", tr)}</div>
</body>
</html>");

			var html = sb.ToString();

			// DinkToPdf ayarları
			var doc = new HtmlToPdfDocument
			{
				GlobalSettings = new GlobalSettings
				{
					PaperSize = PaperKind.A4,
					Orientation = Orientation.Portrait,
					Margins = new MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 },
					DocumentTitle = $"OrderReceipt_{id}"
				},
				Objects =
			{
				new ObjectSettings
				{
					HtmlContent = html,
					WebSettings = new WebSettings
					{
						DefaultEncoding = "utf-8",
						LoadImages = true,
						PrintMediaType = true,
						EnableIntelligentShrinking = true
					},
                    
                }
			}
			};

			var pdfBytes = _converter.Convert(doc);
			return File(pdfBytes, "application/pdf", $"OrderReceipt_{id}.pdf");
		}
	}
}

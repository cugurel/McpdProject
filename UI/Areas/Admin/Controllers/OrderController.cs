using DataAccess.Concrete;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Entity.Concrete.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using UI.Models.Identity;
namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class OrderController : Controller
	{
		Context context = new Context();

		private readonly ApplicationContext _context;

		public OrderController(ApplicationContext context)
		{
			_context = context;
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

		[HttpGet]
		public async Task<IActionResult> OrderReceipt(int id, [FromServices] IWebHostEnvironment env)
		{
			try
			{
				// 1) Veriyi çek
				var orderDetails = context.OrderDetails.Where(x => x.OrderId == id).ToList();
				if (!orderDetails.Any())
					return NotFound("Sipariş bulunamadı");

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
				var grandTotal = result.Sum(x => x.TotalPrice);

				// 2) Font (Türkçe için TTF göm)
				// wwwroot/fonts/DejaVuSans.ttf dosyasını ekleyin (veya kendi TTF’iniz)
				var fontPath = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "fonts", "DejaVuSans.ttf");
				if (!System.IO.File.Exists(fontPath))
				{
					// fallback: içerik kökünde arayın
					fontPath = Path.Combine(env.ContentRootPath, "wwwroot", "fonts", "DejaVuSans.ttf");
				}
				if (!System.IO.File.Exists(fontPath))
				{
					// En azından hatayı anlamlılaştırın
					return StatusCode(500, "Yazı tipi bulunamadı: wwwroot/fonts/DejaVuSans.ttf");
				}

				var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
				var titleFont = new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD);
				var headerFont = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.BOLD);
				var normalFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL);
				var smallFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL);

				// 3) PDF oluştur
				using (var ms = new MemoryStream())
				{
					var document = new Document(PageSize.A4, 25, 25, 30, 30);

					// *** HATA ALDIĞINIZ SATIR: doğru paket/namespace ile bu çalışır ***
					var writer = PdfWriter.GetInstance(document, ms);

					document.AddAuthor("Sistem");
					document.AddCreationDate();
					document.AddTitle($"Sipariş Fişi #{id}");

					document.Open();

					var title = new Paragraph($"Sipariş Fişi #{id}", titleFont)
					{
						Alignment = Element.ALIGN_CENTER,
						SpacingAfter = 20f
					};
					document.Add(title);

					var date = new Paragraph($"Tarih: {DateTime.Now.ToString("dd.MM.yyyy HH:mm", tr)}", normalFont)
					{
						Alignment = Element.ALIGN_CENTER,
						SpacingAfter = 20f
					};
					document.Add(date);

					// Tablo
					var table = new PdfPTable(4) { WidthPercentage = 100 };
					table.SetWidths(new float[] { 50, 20, 10, 20 });

					PdfPCell H(string t) => new PdfPCell(new Phrase(t, headerFont))
					{
						BackgroundColor = BaseColor.LIGHT_GRAY,
						Padding = 5,
						HorizontalAlignment = Element.ALIGN_LEFT,
						VerticalAlignment = Element.ALIGN_MIDDLE
					};

					table.AddCell(H("Ürün Adı"));
					table.AddCell(H("Birim Fiyat")).HorizontalAlignment = Element.ALIGN_RIGHT;
					table.AddCell(H("Adet")).HorizontalAlignment = Element.ALIGN_RIGHT;
					table.AddCell(H("Toplam")).HorizontalAlignment = Element.ALIGN_RIGHT;

					foreach (var item in result)
					{
						table.AddCell(new PdfPCell(new Phrase(item.ProductName ?? "", normalFont)) { Padding = 5 });
						table.AddCell(new PdfPCell(new Phrase(item.Price.ToString("C", tr), normalFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
						table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), normalFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
						table.AddCell(new PdfPCell(new Phrase(item.TotalPrice.ToString("C", tr), normalFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
					}

					document.Add(table);

					var total = new Paragraph($"Genel Toplam: {grandTotal.ToString("C", tr)}", headerFont)
					{
						Alignment = Element.ALIGN_RIGHT,
						SpacingBefore = 20f
					};
					document.Add(total);

					var footer = new Paragraph($"© {DateTime.Now.Year} - Tüm hakları saklıdır", smallFont)
					{
						Alignment = Element.ALIGN_CENTER,
						SpacingBefore = 30f
					};
					document.Add(footer);

					document.Close();     // writer da Close alır
					writer.Close();

					var pdf = ms.ToArray();
					var fileName = $"Siparis_Fisi_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
					return File(pdf, "application/pdf", fileName);
				}
			}
			catch (Exception ex)
			{
				// Daha fazla bilgi için InnerException’ı da yakalayın
				var msg = ex.InnerException != null
					? $"{ex.Message} | Inner: {ex.InnerException.Message}"
					: ex.Message;

				Console.WriteLine($"PDF oluşturma hatası: {msg}");
				return StatusCode(500, "PDF oluşturulurken bir hata oluştu: " + msg);
			}
		}

	}
}

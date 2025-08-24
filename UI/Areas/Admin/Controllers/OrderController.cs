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
		public async Task<IActionResult> OrderReceipt(int id)
		{
			try
			{
				// Veriyi çek
				var orderDetails = context.OrderDetails.Where(x => x.OrderId == id).ToList();

				if (!orderDetails.Any())
				{
					return NotFound("Sipariş bulunamadı");
				}

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

				// PDF oluştur
				using (MemoryStream ms = new MemoryStream())
				{
					Document document = new Document(PageSize.A4, 25, 25, 30, 30);
					PdfWriter writer = PdfWriter.GetInstance(document, ms);

					// Font ayarları
					BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);
					Font titleFont = new Font(baseFont, 18, Font.BOLD);
					Font headerFont = new Font(baseFont, 12, Font.BOLD);
					Font normalFont = new Font(baseFont, 10, Font.NORMAL);
					Font smallFont = new Font(baseFont, 8, Font.NORMAL);

					document.Open();

					// Başlık
					Paragraph title = new Paragraph($"Sipariş Fişi #{id}", titleFont);
					title.Alignment = Element.ALIGN_CENTER;
					title.SpacingAfter = 20f;
					document.Add(title);

					// Tarih
					Paragraph date = new Paragraph($"Tarih: {DateTime.Now.ToString("dd.MM.yyyy HH:mm", tr)}", normalFont);
					date.Alignment = Element.ALIGN_CENTER;
					date.SpacingAfter = 20f;
					document.Add(date);

					// Tablo oluştur
					PdfPTable table = new PdfPTable(4);
					table.WidthPercentage = 100;

					// Tablo başlıkları
					table.AddCell(new PdfPCell(new Phrase("Ürün Adı", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 5 });
					table.AddCell(new PdfPCell(new Phrase("Birim Fiyat", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
					table.AddCell(new PdfPCell(new Phrase("Adet", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
					table.AddCell(new PdfPCell(new Phrase("Toplam", headerFont)) { BackgroundColor = BaseColor.LIGHT_GRAY, Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });

					// Tablo verileri
					foreach (var item in result)
					{
						table.AddCell(new PdfPCell(new Phrase(item.ProductName, normalFont)) { Padding = 5 });
						table.AddCell(new PdfPCell(new Phrase(item.Price.ToString("C", tr), normalFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
						table.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString(), normalFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
						table.AddCell(new PdfPCell(new Phrase(item.TotalPrice.ToString("C", tr), normalFont)) { Padding = 5, HorizontalAlignment = Element.ALIGN_RIGHT });
					}

					document.Add(table);

					// Genel toplam
					Paragraph total = new Paragraph($"Genel Toplam: {grandTotal.ToString("C", tr)}", headerFont);
					total.Alignment = Element.ALIGN_RIGHT;
					total.SpacingBefore = 20f;
					document.Add(total);

					// Alt bilgi
					Paragraph footer = new Paragraph($"© {DateTime.Now.Year} - Tüm hakları saklıdır", smallFont);
					footer.Alignment = Element.ALIGN_CENTER;
					footer.SpacingBefore = 30f;
					document.Add(footer);

					document.Close();

					byte[] pdf = ms.ToArray();

					// PDF'i response olarak döndür
					return File(pdf, "application/pdf", $"Siparis_Fisi_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
				}
			}
			catch (Exception ex)
			{
				// Log the error
				Console.WriteLine($"PDF oluşturma hatası: {ex.Message}");
				return StatusCode(500, "PDF oluşturulurken bir hata oluştu: " + ex.Message);
			}
		}
	}
}

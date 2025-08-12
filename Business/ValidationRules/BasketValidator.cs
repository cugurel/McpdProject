using Entity.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules
{
	public class BasketValidator:AbstractValidator<Basket>
	{
		public BasketValidator()
		{
			RuleFor(x => x.UserId).NotEmpty().WithMessage("Kullanıcı bilgisi boş olamaz.").NotNull().WithMessage("Kullanıcı bilgisi boş olamaz.");
			RuleFor(x => x.ProductId).NotEmpty().WithMessage("Ürün bilgisi boş olamaz.").NotNull().WithMessage("Ürün bilgisi boş olamaz.");
			RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Adet bilgisi 0'dan büyük olmalıdır.").NotEmpty().WithMessage("Adet bilgisi boş olamaz.").NotNull().WithMessage("Adet bilgisi boş olamaz.");


		}
	}
}

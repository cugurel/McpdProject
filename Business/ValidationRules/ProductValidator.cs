using Entity.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ValidationRules
{
	public class ProductValidator: AbstractValidator<Product>
	{
		public ProductValidator()
		{
			RuleFor(x=>x.Name).NotEmpty().WithMessage("Ürün adı boş olamaz.");
			RuleFor(x=>x.Name).MinimumLength(2).WithMessage("Ürün adı en az 2 karakter olmalıdır.");
			RuleFor(x=>x.CategoryId).NotEmpty().WithMessage("Kategori seçilmelidir.");
			RuleFor(x=>x.Price).NotEmpty().WithMessage("Fiyat boş olamaz.");
			RuleFor(x => x.Price).GreaterThan(0).WithMessage("Fiyat 0'dan büyük olmalıdır.");
			RuleFor(x => x.QuantityInStock).NotEmpty().WithMessage("Stok miktarı boş olamaz.");
			RuleFor(x => x.QuantityInStock).GreaterThanOrEqualTo(0).WithMessage("Stok miktarı 0 veya daha büyük olmalıdır.");
		}
	}
}

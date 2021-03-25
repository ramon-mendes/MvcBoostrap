using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace MvcBoostrap.Models
{
	public class SampleItemModel
	{
		public int Id { get; set; }
		public DateTime Dt { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
	}

	public class SampleItemValidator : AbstractValidator<SampleItemModel>
	{
		public SampleItemValidator()
		{
			RuleFor(e => e.Name).NotEmpty();
			RuleFor(e => e.Dt).NotEmpty();
			RuleFor(e => e.Price).NotEmpty();
		}
	}
}

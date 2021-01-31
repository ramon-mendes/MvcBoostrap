using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcBoostrap.Models
{
	public class TranslationModel
	{
		[Key]
		public int Id { get; set; }
		public string key_pt { get; set; }
		public string trans_en { get; set; }
		public string trans_es { get; set; }
		public string trans_it { get; set; }
		public string trans_al { get; set; }
		public string trans_fr { get; set; }
	}
}
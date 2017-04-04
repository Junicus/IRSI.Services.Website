using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IRSI.Services.Website.Models.AVT
{
    public class AVTItem
    {
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(@"^(\d){4}-(\d){4}$", ErrorMessage = "Invalid expression.  Should be like 2015-2016")]
        [Display(Name = "Fiscal Year")]
        public string FiscalYear { get; set; }

        [Required]
        public int Period { get; set; }

        [Display(Name = "Store")]
        [Required]
        public Guid StoreId { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal FGActual { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal FGTarget { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal AVTActual { get; set; }

        [Required]
        [Range(1, 100)]
        public decimal AVTTheoric { get; set; }

        [Required]
        [MaxLength(150)]
        public string ChampName { get; set; }

        [Required]
        [MaxLength(6)]
        public string ChampId { get; set; }

        [Required]
        public decimal PayTotal { get; set; }
    }
}

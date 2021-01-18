using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ofakim_hw_ma.Data
{
    public class ExConvertEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExCorrencytId { get; set; }

        [Required]
        [MaxLength(10)]
        public string ExCorrencyName { get; set; }
        public decimal Rate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

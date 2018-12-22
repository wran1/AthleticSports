using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dictionary
{
    /// <summary>
    /// 地区
    /// </summary>
    public class City : DbSetBase
    {
        public City()
        {

            SystemId = "000";
            Enable = true;
        }

        [MaxLength(100)]
        [Required]

        public string Name { get; set; }

        [MaxLength(30)]
        [Required]
        public string SystemId { get; set; }

        public bool Enable { get; set; }


        [ScaffoldColumn(false)]
        [NotMapped]
        public bool Selected { get; set; }

    }
}

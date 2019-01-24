using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.TestModels
{
     public class ExportRecord : DbSetBase
    {
        [MaxLength(40)]
        public string SysArea { get; set; }
    }
}

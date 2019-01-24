using Models.SysModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.TestModels
{
    /// <summary>
    /// 队医记录
    /// </summary>
     public class DoctorRecord : DbSetBase
    {

        [ForeignKey("SysUser")]
        [Required]
        public string SysUserId { get; set; }

        public virtual SysUser SysUser { get; set; }

    }
}

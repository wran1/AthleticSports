using Models.SysModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Areas.Api.Models
{
    public class UserInfoModels
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(50), Required]
        public string FullName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Required]
        public string Sex { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [Required]
        public string Birthday { get; set; }
        /// <summary>
        /// 运动等级
        /// </summary>
        [Required]
        public SportGrade SportGrade { get; set; }
        /// <summary>
        /// 专项id
        /// </summary>
        [MaxLength(128), Required]
        public string TrainId { get; set; }
        /// <summary>
        /// 专项全称
        /// </summary>
        public string TrainName { get; set; }
        /// <summary>
        /// 教练id
        /// </summary>
        [MaxLength(128), Required]
        public string DepartmentId { get; set; }
        /// <summary>
        /// 教练名称
        ///</summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 专训开始时间
        /// </summary>
        [Required]
        public int Start4Training { get; set; }
        /// <summary>
        /// 运动年限
        /// </summary>
        public int Train4year { get; set; }
    }
}
using Models.SysModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Areas.Platform.Models
{
    /// <summary>
    /// 公司信息
    /// </summary>
    public class CompanyInfoEditModel
    {
        /// <summary>   
        /// 公司名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        /// <summary>
        /// 是否会员注册
        /// </summary>
        [UIHint("HiddenInput")]
        public bool Registered { get; set; }

        /// <summary>
        /// Log
        /// </summary>
        [MaxLength(100)]
        [DataType("Image")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// 中文简介
        /// </summary>
        [MaxLength(2000)]
        [DataType(DataType.MultilineText)]
        public string ChineseIntroduction { get; set; }

        /// <summary>
        /// 英文简介
        /// </summary>
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public string EnglishIntroduction { get; set; }

        ///// <summary>
        ///// 审核状态
        ///// </summary>
        //public AuditState AuditState { get; set; }

        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

    }
}
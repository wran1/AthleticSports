using Models.SysModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Areas.User.Models
{
    /// <summary>
    /// 账户设置模型
    /// </summary>
    public class AccountEditModel
    {
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 确认新密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "新密码和确认新密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// 资料管理编辑模型
    /// </summary>
    public class DataEditModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(100)]
        public string FullName { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        [MaxLength(100)]
        public string EnglishName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [RegularExpression("1[34578][0-9]{9}", ErrorMessage = "请输入正确的手机号")]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [MaxLength(10)]
        public string Sex { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string WxId { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [MaxLength(100)]
        public string Cposition { get; set; }

        /// <summary>
        /// 英文职位
        /// </summary>
        [MaxLength(100)]
        public string Eposition { get; set; }

        /// <summary>
        /// 固定电话
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string FixedPhone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [MaxLength(20)]
        public string Fox { get; set; }

        /// <summary>
        /// 是否公开联系方式
        /// </summary>
        public bool IsPublicPhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        [Display(Name = "邮箱")]
        [RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", ErrorMessage = "请输入正确的邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [MaxLength(100)]
        [UIHint("NoEditInput")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司英文名称
        /// </summary>
        [MaxLength(100)]
        public string EfCompanyName { get; set; }

        ///// <summary>
        ///// 会员类型
        ///// </summary>
        //public MemberType MemberType { get; set; }

        ///// <summary>
        ///// 公司类型
        ///// </summary>
        //public CompanyType CompanyType { get; set; }

        /// <summary>
        /// 公司网址
        /// </summary>
        [MaxLength(100)]
        public string CompanyUrl { get; set; }

        /// <summary>
        /// 总资产值
        /// </summary>
        [MaxLength(100)]
        [UIHint("NoEditInput")]
        public string TotalAsset { get; set; }

        /// <summary>
        /// 出口经验
        /// </summary>
        public bool IsExportExperience { get; set; }

        /// <summary>
        /// 员工总数
        /// </summary>
        public int TotalStaff { get; set; }

        /// <summary>
        /// 年销售额
        /// </summary>
        [UIHint("NoEditInput")]
        public string AnnuaSales { get; set; }

        /// <summary>
        /// 注册地址
        /// </summary>
        [MaxLength(100)]
        [UIHint("NoEditInput")]
        public string RegisteredAddress { get; set; }

        /// <summary>
        /// 实际地址
        /// </summary>
        [MaxLength(100)]
        [UIHint("NoEditInput")]
        public string ActualAddress { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [MaxLength(100)]
        [DataType(DataType.MultilineText)]
        public string Keywords { get; set; }

        /// <summary>
        /// 英文关键字
        /// </summary>
        [MaxLength(100)]
        [DataType(DataType.MultilineText)]
        public string Keyword4English { get; set; }

        /// <summary>
        /// 主营业务
        /// </summary>
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string MainBusiness { get; set; }

        /// <summary>
        /// 主营业务（英文）
        /// </summary>
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string MainBusiness4English { get; set; }

        /// <summary>
        /// 展会意向
        /// </summary>
        public bool IsExhibitionIntention { get; set; }

        /// <summary>
        /// 其他展会
        /// </summary>
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string OtherExhibition { get; set; }

        /// <summary>
        /// 所属行业
        /// </summary>
        [DataType("IndustryNoEditSelectList")]
        [MaxLength(128)]
        public string IndustryOwnedId { get; set; }

        /// <summary>
        /// 境内展
        /// </summary>
        [MaxLength(128)]
        [DataType("DomesticExhibitionSelectList")]
        public string DomesticExhibitionId { get; set; }

        /// <summary>
        /// 境外展
        /// </summary>
        [MaxLength(128)]
        [DataType("OverseasExhibitionSelectList")]
        public string OverseasExhibitionId { get; set; }

    }

    /// <summary>
    /// 公司介绍编辑模型
    /// </summary>
    public class CompanyIndroduceEditModel
    {
        /// <summary>   
        /// 公司名称
        /// </summary>
        [UIHint("NoEditInput")]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Log
        /// </summary>
        [MaxLength(100)]
        [DataType("Logo")]
        public string LogoUrl { get; set; }

        /// <summary>
        /// 主营产品
        /// </summary>
        [MaxLength(100)]
        public string MainProduct { get; set; }

        /// <summary>
        /// 中文简介
        /// </summary>
        [MaxLength(1000)]
        [DataType(DataType.MultilineText)]
        public string ChineseIntroduction { get; set; }

        /// <summary>
        /// 英文简介
        /// </summary>
        [MaxLength]
        [DataType(DataType.MultilineText)]
        public string EnglishIntroduction { get; set; }
    }
}
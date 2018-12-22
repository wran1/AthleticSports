using Models.SysModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Models
{
    /// <summary>
    /// 网站用户注册模型
    /// </summary>
    public class UserRegisterModel
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        [Required]
        //[RegularExpression("[a-zA-Z][a-zA-Z0-9_]{0,20}", ErrorMessage = "用户名由字母下划线和数字组成，不能以数字或下划线开头。")]
        [RegularExpression("[0-9a-zA-Z]{3,20}", ErrorMessage = "用户名由字母或数字组成，不能包含特殊字符，长度必须在3-20个字符之间。")]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 3)]
        [Remote("CheckUserAccountExists", "Account", ErrorMessage = "用户账号已存在")] // 远程验证（Ajax）
        public string UserName { get; set; }

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
        /// 密码
        /// </summary>
        [Required]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

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

        ///// <summary>
        ///// 身份证号码
        ///// </summary>
        //[MaxLength(18)]
        //[Required]
        //[RegularExpression(@"(^[1-9][0-9]{5}((19[0-9]{2})|(20[0-1]){2})(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])[0-9]{3}[0-9xX]$)|(^[1-9][0-9]{5}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])[0-9]{3}$)", ErrorMessage = "请输入正确的身份证号码")]
        //public string IdNumber { get; set; }

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
        [Required]
        [Remote("CheckCompanyNameExists", "Account", ErrorMessage = "该公司名称已存在")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 公司英文名称
        /// </summary>
        [MaxLength(100)]
        public string EfCompanyName { get; set; }

        ///// <summary>
        ///// 会员类型
        ///// </summary>
        //[Required]
        //public MemberType MemberType { get; set; }

        ///// <summary>
        ///// 公司类型
        ///// </summary>
        //[Required]
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
        public string AnnuaSales { get; set; }

        /// <summary>
        /// 注册地址
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string RegisteredAddress { get; set; }

        /// <summary>
        /// 实际地址
        /// </summary>
        [MaxLength(100)]
        [Required]
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
        [Required]
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
        [DataType("IndustrySelectList")]
        [MaxLength(128)]
        [Required]
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

        /// <summary>
        /// 是否同意用户声明
        /// </summary>
        public bool IsAgreeUserDeclaration { get; set; }
    }
}
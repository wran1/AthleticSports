using Models.SysModels;
using Models.WebsiteManagement;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Web.Areas.Platform.Models
{
    public class SysUserEditModel
    {
        [Key]
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        //[Display(Name = "EnterpriseName")]
        //[Required]
        //public string[] SysEnterprisesId { get; set; }

        //[Required]
        [DataType("SystemId")]
        [Display(Name = "Department")]
        public string DepartmentId { get; set; }

        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Description = "Passwordkeepnull")]
        public string Password { get; set; }

        [MaxLength(100)]
        [Required]
        public string FullName { get; set; }

        [MaxLength(10)]
        public string Sex { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        //public bool EmailConfirmed { get; set; }

        [RegularExpression("1[34578][0-9]{9}")]
        [Required]
        public string PhoneNumber { get; set; }

        //public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "RoleName")]
        [Required]
        public string[] SysRolesId { get; set; }
    }

    public class SysUserViewModel
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        [Required]
        //[RegularExpression("[a-zA-Z][a-zA-Z0-9_]{0,20}", ErrorMessage = "用户名由字母下划线和数字组成，不能以数字或下划线开头。")]
        //[RegularExpression("[0-9a-zA-Z]{0,15}", ErrorMessage = "用户名由字母或数字组成，不能包含特殊字符，长度必须在2-15个字符之间。")]
        //[StringLength(15, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 2)]
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
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Description = "Passwordkeepnull")]
        public string Password { get; set; }

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
        public string FixedPhone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
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
        //[RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", ErrorMessage = "请输入正确的邮箱")]
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
        public string Keywords { get; set; }

        /// <summary>
        /// 英文关键字
        /// </summary>
        [MaxLength(100)]
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

        ///// <summary>
        ///// 审核状态
        ///// </summary>
        //[Required]
        //public AuditState AuditState { get; set; }

        ///// <summary>
        ///// 会员等级
        ///// </summary>
        //[Required]
        //public Level Level { get; set; }
    }
}
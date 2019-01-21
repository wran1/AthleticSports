using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace Web.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        public string Email { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ForgotViewModel
    {
        [Required]
        public string Email { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LoginViewModel
    {

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RegisterViewModel
    {
        [Required]
        //[RegularExpression("[a-zA-Z][a-zA-Z0-9_]{0,20}", ErrorMessage = "用户名由字母下划线和数字组成，不能以数字或下划线开头。")]
        //[StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [RegularExpression("[0-9a-zA-Z]{3,20}", ErrorMessage = "用户名由字母或数字组成，不能包含特殊字符，长度必须在3-20个字符之间。")]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 3)]
        //[Remote("CheckUserAccountExists", "Account", ErrorMessage = "用户账号已存在")] // 远程验证（Ajax）
        public string UserName { get; set; }

        [DataType("SystemId")]
        [Display(Name = "Department")]
        public string DepartmentId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }


        //[Required(ErrorMessage = "请输入验证码"), RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        //[Display(Name = "验证码")]
        //public string VerifyCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChangePasswordViewModel
    {
      
        /// <summary>
        /// 原密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ForgotPasswordViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        //[RegularExpression("[a-zA-Z][a-zA-Z0-9_]{0,20}", ErrorMessage = "用户名由字母下划线和数字组成，不能以数字或下划线开头。")]
        //[StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [RegularExpression("[0-9a-zA-Z]{0,15}", ErrorMessage = "用户名由字母或数字组成，不能包含特殊字符，长度必须在2-15个字符之间。")]
        [StringLength(15, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 2)]

        public string UserName { get; set; }

        //[Required(ErrorMessage = "请输入验证码"), RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        //[Display(Name = "验证码")]
        //public string VerifyCode { get; set; }


        ///// <summary>
        ///// 
        ///// </summary>
        //[Required]
        //[StringLength(100, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        //[Required]
        //[DataType(DataType.Password)]
        //[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        //public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// 身份验证模型
    /// </summary>
    public class PreVerify4ResetPasswordModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [UIHint("HiddenInput"), Required]
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "请输入验证码"), RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }
    }

    /// <summary>
    /// 重置密码模型
    /// </summary>
    public class ResetPasswordModel
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }
}

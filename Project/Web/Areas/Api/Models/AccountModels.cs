using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Areas.Api.Models
{
    /// <summary>
    /// 令牌视图模型
    /// </summary>
    public class AccessTokenViewModel
    {
        public AccessTokenViewModel()
        {
        }

        public AccessTokenViewModel(string token, string tokenType, string userName, int expires_in)
        {
            Access_Token = token;
            Token_Type = tokenType;
            UserName = userName;
            Expires_In = expires_in;
        }
        /// <summary>
        /// 返回的令牌
        /// </summary>
        public string Access_Token { get; set; }
        /// <summary>
        /// 令牌类型
        /// </summary>
        public string Token_Type { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 过期数字
        /// </summary>
        public int Expires_In { get; set; }
    }
    public class Paypassword
    {
        [DataType(DataType.Password)]
        [Display(Name = "当前支付密码")]
        public string OldPayPassword { get; set; }

        [Required(ErrorMessage = "请输入新支付密码")]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新支付密码")]
        public string PayPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认新支付密码")]
        [Compare("PayPassword", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }
    /// <summary>
    /// 所有密保问题
    /// </summary>
    public class SecurityQuestionModel
    {
        
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        [MaxLength(3)]
        [Required]
        public string SystemId { get; set; }
    }

    /// <summary>
    ///用户设置的密保问题
    /// </summary>
    public class SecurityAnswersModel
    {
        /// <summary>
        /// 问题1
        /// </summary>
        [Display(Name = "问题1"), Required(ErrorMessage = "请选择密保问题")]
        public string SecurityQuestionId { get; set; }
        /// <summary>
        /// 答案1
        /// </summary>
        [Display(Name = "答案")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        public string Answer { get; set; }
        /// <summary>
        /// 问题2
        /// </summary>
        [Display(Name = "问题2"), Required(ErrorMessage = "请选择密保问题")]
        public string SecurityQuestionIdII { get; set; }
        /// <summary>
        /// 答案2
        /// </summary>
        [Display(Name = "答案")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        public string AnswerII { get; set; }

        /// <summary>
        /// 问题3
        /// </summary>
        [Display(Name = "问题3"), Required(ErrorMessage = "请选择密保问题")]
        public string SecurityQuestionIdIII { get; set; }
        /// <summary>
        /// 答案3
        /// </summary>
        [Display(Name = "答案")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        public string AnswerIII { get; set; }
    }
   
    /// <summary>
    /// 注册绑定模型
    /// </summary>
    public class RegisterBindModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        [RegularExpression("1[34578][0-9]{9}", ErrorMessage = "请输入正确的手机号。")]
        [Display(Name = "手机")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 验证码(注册时该项为必填项、关联该字段不填)
        /// </summary>
        //[Required]
        //[RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        /// <summary>
        /// 确认密码(注册时该项为必填项、关联该字段不填)
        /// </summary>
        //[Required]
        //[DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// 登录绑定模型
    /// </summary>
    public class LoginBindModel
    {
        /// <summary>
        /// 用户 [用户名 / 手机号 / 邮箱]
        /// </summary>
        [Required]
        [Display(Name = "用户", Description = "用户名 / 手机号 / 邮箱")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

    }

    /// <summary>
    /// 三方登录类型
    /// </summary>
    public static class OpenAuthCatalog
    {
        /// <summary>
        /// 新浪微博
        /// </summary>
        public static string Weibo = "WeiboAppLogin";
        /// <summary>
        /// 微信开放平台
        /// </summary>
        public static string Wechat = "WechatAppLogin";
        /// <summary>
        /// QQ开放平台
        /// </summary>
        public static string Qq = "QQAppLogin";
    }
    /// <summary>
    /// 三方平台登录模型
    /// </summary>
    public class LoginByOpenIdBindModel
    {
        /// <summary>
        /// OpenId所属平台 ["WeiboAppLogin"(新浪微博)|"WechatAppLogin"(微信开放平台)|"QQAppLogin"(QQ开放平台)]
        /// </summary>
        [Required(ErrorMessage ="OpenId所属平台不能为空")]
        public string OpenIdCatalog { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        [Required(ErrorMessage ="OpenId不能为空")]
        public string OpenId { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [Required(ErrorMessage ="签名不能为空")]
        public string Signature { get; set; }
    }
    /// <summary>
    /// 绑定三方平台模型
    /// </summary>
    public class BindingOpenIdBindModel
    {
        /// <summary>
        /// OpenId所属平台 ["WeiboAppLogin"(新浪微博)|"WechatAppLogin"(微信开放平台)|"QQAppLogin"(QQ开放平台)]
        /// </summary>
        [Required(ErrorMessage ="平台类型不能为空")]
        public string OpenIdCatalog { get; set; }
        /// <summary>
        /// OpenId
        /// </summary>
        [Required(ErrorMessage = "OpenId不能为空")]
        public string OpenId { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [Required(ErrorMessage = "签名不能为空")]
        public string Signature { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 头像网址
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 性别 ["M"(男):"F(女)"]
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 是否是刚注册用户
        /// </summary>
        public bool JustRegistered { get; set; }
    }

    /// <summary>
    /// 用户名设置模型
    /// </summary>
    public class UserNameBindModel
    {
        /// <summary>
        /// 用户名 用户名由字母下划线和数字组成，不能以数字或下划线开头。
        /// </summary>
        [Required]
        [Display(Name = "用户名")]
        [RegularExpression("[a-zA-Z][a-zA-Z0-9_]{0,20}", ErrorMessage = "用户名由字母下划线和数字组成，不能以数字或下划线开头。")]
        [StringLength(20, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        public string UserName { get; set; }
    }

    /// <summary>
    /// 登录状态下修改密码绑定模型
    /// </summary>
    public class PasswordBindModel
    {

        /// <summary>
        /// 当前密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "当前密码")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [MaxLength(100,ErrorMessage ="新密码长度不能超过100个字符")]
        [MinLength(6,ErrorMessage ="新密码必须至少包含6个字符")]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string Password { get; set; }
        /// <summary>
        /// 确认新密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "新密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }


    /// <summary>
    /// 设置手机绑定模型
    /// </summary>
    public class SetPhoneBindModel
    {

    }

    /// <summary>
    /// 手机验证修改密码绑定模型
    /// </summary>
    public class ReSetPasswordByPhoneCodeBindModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        [RegularExpression("1[34578][0-9]{9}", ErrorMessage = "请输入正确的手机号。")]
        [Display(Name = "手机")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        [RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        /// <summary>
        /// 确认新密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// 邮件验证修改密码绑定模型
    /// </summary>
    public class ReSetPasswordByEmailCodeBindModel
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱")]
        [Display(Name = "邮箱"), DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的邮箱地址")]
        public string Email { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        [RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "{0}必须在{2}至{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        /// <summary>
        /// 确认新密码
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// 身份验证方式视图模型
    /// </summary>
    public class PreVerifyProviderViewModel
    {
        /// <summary>
        /// 验证方式名称列表
        /// </summary>
        public IList<string> ProviderNames { get; set; }
    }

    #region 验证码

    /// <summary>
    /// 验证码缓存
    /// </summary>
    public class VerifyCodeCachingModel
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }
        /// <summary>
        /// 验证码时间
        /// </summary>
        public DateTime VerifyCodeTime { get; set; }
        /// <summary>
        /// 验证手机或邮箱
        /// </summary>
        public string VerifyPhone { get; set; }
    }
    /// <summary>
    /// 手机验证码登录绑定模型
    /// </summary>
    public class LoginByPhoneBindModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        [RegularExpression("1[34578][0-9]{9}", ErrorMessage = "请输入正确的手机号。")]
        [Display(Name = "手机")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        [RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }
    }
    
    /// <summary>
    /// 发送手机验证码用模型
    /// </summary>
    public class SendCode2PhoneBindModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = "请输入手机号码")]
        [RegularExpression("1[34578][0-9]{9}", ErrorMessage = "请输入正确的手机号。")]
        [Display(Name = "手机")]
        public string PhoneNumber { get; set; }
    }
    /// <summary>
    /// 手机验证码验证用绑定模型，设置绑定手机
    /// </summary>
    public class VerifyCode4PhoneBindModel
    {
        public VerifyCode4PhoneBindModel()
        {
            DelayMinutes = 0;
        }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        [RegularExpression("1[34578][0-9]{9}", ErrorMessage = "请输入正确的手机号。")]
        [Display(Name = "手机")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        [RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }

        /// <summary>
        /// 有效性延时分钟数，在分步验证操作中使用，比如先验证手机号点击下一步设置密码完成注册的操作，可能在最后一步验证码失效，需要设置有效性延时
        /// </summary>
        [Required,Display(Name = "有效性延时分钟数")]
        public int DelayMinutes { get; set; }

    }

    
    /// <summary>
    /// 发送邮箱验证码用模型
    /// </summary>
    public class SendCode2EmailBindModel
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱")]
        [Display(Name = "邮箱"),DataType(DataType.EmailAddress,ErrorMessage = "请输入正确的邮箱地址")]
        public string Email { get; set; }
    }/// <summary>
     /// 邮箱验证码验证用绑定模型
     /// </summary>
    public class VerifyCode4EmailBindModel
    {
        public VerifyCode4EmailBindModel()
        {
            DelayMinutes = 0;
        }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "请输入邮箱")]
        [Display(Name = "邮箱"), DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的邮箱地址")]
        public string Email { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required]
        [RegularExpression("[0-9]{6}", ErrorMessage = "验证码错误。")]
        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }

        /// <summary>
        /// 有效性延时分钟数，在分步验证操作中使用，比如先验证手机号点击下一步设置密码完成注册的操作，可能在最后一步验证码失效，需要设置有效性延时
        /// </summary>
        [Required, Display(Name = "有效性延时分钟数")]
        public int DelayMinutes { get; set; }
    }
    #endregion
}
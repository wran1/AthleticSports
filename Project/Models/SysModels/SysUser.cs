using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.WebsiteManagement;
using Models.Dictionary;

namespace Models.SysModels
{
    /// <summary>
    /// 账户类型
    /// </summary>
    public enum AccountType
    {
        后台管理账户 = 0,
        企业账户 = 1,
    }
    //public enum Sex
    //{
    //    男=1,
    //    女=2
    //}
    public enum SportGrade
    {
        国际级运动健将,
        运动健将,
        一级运动员,
        二级运动员,
        三级运动员,
    }

    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class SysUser : IdentityUser, IDbSetBase
    {
        public SysUser()
        {
            CreatedDate = DateTimeLocal.Now;
            Deleted = false;
            AccountType = 0;
           
            SportGrade = SportGrade.三级运动员;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SysUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SysUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
        //姓名
        [MaxLength(50)]
        public string FullName { get; set; }
        //用户标记
        public string Sign { get; set; }
        /// <summary>
        /// 头像URL
        /// </summary>
        [MaxLength(300)]
        public string Picture { get; set; }

        [MaxLength(10)]
        public string Sex { get; set; }

        [MaxLength(10)]
        public string Birthday { get; set; }

        //运动等级
        public SportGrade SportGrade { get; set; }

        [MaxLength(128)]
        [ForeignKey("Train")]
        //[Display(Name = "Train"), DataType("CitySelectList")]
        public string TrainId { get; set; }

        [ScaffoldColumn(false)]
        public virtual Train Train { get; set; }

        /// <summary>
        /// 专训开始时间
        /// </summary>
        public int Start4Training { get; set; }
        
        [Editable(false)]
        [DataType(DataType.DateTime)]
        [MaxLength(50)]
        public string CreatedDate { get; set; }

        [Editable(false)]
        [DataType(DataType.DateTime)]
        public string UpdatedDate { get; set; }

        [Editable(false)]
        [MaxLength(128)]
        public string CreatedBy { get; set; }

        [Editable(false)]
        [MaxLength(128)]
        public string UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        [ForeignKey("UpdatedBy")]
        public virtual SysUser UserUpdatedBy { get; set; }


        [MaxLength(200)]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }

        [ScaffoldColumn(false)]
        [Index(IsClustered = false)]
        public bool Deleted { get; set; }

        public bool Locked { get; set; }

        public AccountType AccountType { get; set; }

        //[MaxLength(128), ForeignKey("PersonalInfo")]
        //public string PersonalInfoId { get; set; }
        //public virtual PersonalInfo PersonalInfo { get; set; }


        //[ScaffoldColumn(false)]
        //public virtual ICollection<SysEnterpriseSysUser> SysEnterpriseSysUsers { get; set; } = new List<SysEnterpriseSysUser>();

        [ScaffoldColumn(false)]
        public virtual ICollection<SysDepartmentSysUser> SysDepartmentSysUsers { get; set; } = new List<SysDepartmentSysUser>();

    }
    
    ///// <summary>
    ///// 会员扩展信息
    ///// </summary>
    //public class PersonalInfo : DbSetBase
    //{
    //    public PersonalInfo()
    //    {
    //        IsPublicPhone = false;
    //    }
    //    /// <summary>
    //    /// 英文名
    //    /// </summary>
    //    [MaxLength(100)]
    //    public string  EnglishName { get; set; }
    //    /// <summary>
    //    /// 微信号
    //    /// </summary>
    //    [MaxLength(50)]
    //    public string  WxId { get; set; }
    //    /// <summary>
    //    /// 职位
    //    /// </summary>
    //    [MaxLength(100)]
    //    public string  Cposition { get; set; }
    //    /// <summary>
    //    /// 英文职位
    //    /// </summary>
    //    [MaxLength(100)]
    //    public string Eposition { get; set; }
    //    /// <summary>
    //    /// 固定电话
    //    /// </summary>
    //    [MaxLength(20)]
    //    public string FixedPhone { get; set; }
    //    /// <summary>
    //    /// 传真
    //    /// </summary>
    //    [MaxLength(20)]
    //    public string  Fox { get; set; }
    //    /// <summary>
    //    /// 是否公开联系方式
    //    /// </summary>
    //    public bool IsPublicPhone { get; set; }

    //    /// <summary>
    //    /// 是否同意用户声明
    //    /// </summary>
    //    public bool IsAgreeUserDeclaration { get; set; }

    //}
  
}

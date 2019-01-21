using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
using Fissoft.EntityFramework.Fts;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.SysModels;
using Models.Dictionary;
using Models.WebsiteManagement;
using Models.CmsModels;
using Models.TestModels;

namespace Services
{
    public sealed class ApplicationDbContext : IdentityDbContext<SysUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
            DbInterceptors.Init();
        }

        #region Cms

        /// <summary>
        /// Cms栏目
        /// </summary>
        public DbSet<CmsCategory> CmsCategories { get; set; }

        /// <summary>
        /// Cms文章
        /// </summary>
        public DbSet<CmsArtical> CmsArticals { get; set; }

        #endregion

        #region 数据字典

        /// <summary>
        /// 地区
        /// </summary>
        public DbSet<City> Cities { get; set; }
   

        #endregion

        #region 系统表

        //public DbSet<SysEnterprise> SysEnterprises { get; set; }

        //public DbSet<SysEnterpriseSysUser> SysEnterpriseSysUsers { get; set; }

        public DbSet<SysRole> SysRoles { get; set; }
        public DbSet<IdentityUserRole> UserRoles { get; set; }
        public DbSet<SysArea> SysAreas { get; set; }
        public DbSet<SysController> SysControllers { get; set; }
        public DbSet<SysControllerSysAction> SysControllerSysActions { get; set; }
        public DbSet<SysAction> SysActions { get; set; }
        public DbSet<SysRoleSysControllerSysAction> SysRoleSysControllerSysActions { get; set; }
        public DbSet<SysHelp> SysHelps { get; set; }
        public DbSet<SysHelpClass> SysHelpClasses { get; set; }

        public DbSet<SysUserLog> SysUserLogs { get; set; }

        public DbSet<SysKeyword> SysKeywords { get; set; }//用户所有搜索记录

        //系统消息
        public DbSet<SysBroadcast> SysMessages { get; set; }

        //系统消息读取记录
        public DbSet<SysBroadcastReceived> SysBroadcastReceiveds { get; set; }

        public DbSet<SysSignalR> SysSignalRs { get; set; }

        public DbSet<SysSignalROnline> SysSignalROnlines { get; set; }
   
        /// <summary>
        /// 验证码存储
        /// </summary>
        public DbSet<VerifyCode> VerifyCodes { get; set; }
        
        /// <summary>
        /// 组织架构
        /// </summary>
         public DbSet<SysDepartment> SysDepartments { get; set; }

        /// <summary>
        /// 用户关联部门
        /// </summary>
        public DbSet<SysDepartmentSysUser> SysDepartmentSysUser { get; set; }


        #endregion

        #region Test
        public DbSet<SubjectiveTest> SubjectiveTests { get; set; }
        public DbSet<TrainingType> TrainingTypes { get; set; }
        public DbSet<TrainingRelation> TrainingRelations { get; set; }
        public DbSet<TrainingPeople> TrainingPeoples { get; set; }
        public DbSet<PeriodicTestResult> PeriodicTestResults { get; set; }

        #endregion


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(38, 2));

            //为表生成 基本的存储过程 Insert Update Delete
            modelBuilder.Types().Configure(a => a.MapToStoredProcedures());

            base.OnModelCreating(modelBuilder);
        }

        public int Commit()
        {
            return SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return SaveChangesAsync();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        //用户表
    }
}
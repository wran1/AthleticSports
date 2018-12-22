using System;
using System.Data.Entity;
using System.Linq;
using IServices.ISysServices;
using Models.SysModels;
using Services.Infrastructure;
using System.Threading.Tasks;


namespace Services.SysServices
{

    public class SysUserService : RepositoryBase<SysUser>, ISysUserService
    {

        public SysUserService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }
    //public class PersonalInfoService : RepositoryBase<PersonalInfo>, IPersonalInfoService
    //{

    //    public PersonalInfoService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
    //        : base(databaseFactory, userInfo)
    //    {
    //    }

    //}
    //public class CompanyInfoService : RepositoryBase<CompanyInfo>, ICompanyInfoService
    //{

    //    public CompanyInfoService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
    //        : base(databaseFactory, userInfo)
    //    {
    //    }

    //}
}
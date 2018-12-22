using IServices.Infrastructure;
using Models.SysModels;
using System.Threading.Tasks;

namespace IServices.ISysServices
{
    public interface ISysUserService : IRepository<SysUser>
    {
    }
    //public interface IPersonalInfoService : IRepository<PersonalInfo>
    //{
    //}
    //public interface ICompanyInfoService : IRepository<CompanyInfo>
    //{
    //}
}
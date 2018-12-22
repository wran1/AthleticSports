using IServices.IWebsiteManagementServices;
using IServices.ISysServices;
using Models.WebsiteManagement;
using Services.Infrastructure;

namespace Services.WebsiteManagementServices
{
    
    public class AboutUsService : RepositoryBase<AboutUs>, IAboutUsService
    {
        public AboutUsService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
    
}
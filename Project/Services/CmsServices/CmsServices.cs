using IServices.ICmsServices;
using IServices.ISysServices;
using Models.CmsModels;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CmsServices
{
    public class CmsCategoryService : RepositoryBase<CmsCategory>, ICmsCategoryService
    {
        public CmsCategoryService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
    public class CmsArticalService : RepositoryBase<CmsArtical>, ICmsArticalService
    {
        public CmsArticalService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
    public class CmsArticalHitService : RepositoryBase<CmsArticalHit>, ICmsArticalHitService
    {
        public CmsArticalHitService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
}

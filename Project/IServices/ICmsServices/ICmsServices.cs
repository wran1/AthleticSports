using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IServices.Infrastructure;
using Models.CmsModels;

namespace IServices.ICmsServices
{
    public interface ICmsCategoryService : IRepository<CmsCategory>
    {
    }
    public interface ICmsArticalService : IRepository<CmsArtical>
    {
    }
    public interface ICmsArticalHitService : IRepository<CmsArticalHit>
    {
    }
}

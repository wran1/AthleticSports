using IServices.IDictionaryServices;
using IServices.ISysServices;
using Models.Dictionary;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DictionaryService
{
    public class CityService : RepositoryBase<City>, ICityService
    {
        public CityService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
    public class TrainService : RepositoryBase<Train>, ITrainService
    {
        public TrainService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }

}

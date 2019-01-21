using IServices.ISysServices;
using IServices.ITestServices;
using Models.TestModels;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.TestServices
{
    public class SubjectiveTestService : RepositoryBase<SubjectiveTest>, ISubjectiveTestServices
    {
        public SubjectiveTestService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
  
}

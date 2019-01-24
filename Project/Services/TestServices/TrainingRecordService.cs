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
    public class TrainingTypeService : RepositoryBase<TrainingType>, ITrainingTypeService
    {
        public TrainingTypeService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
    public class TrainingRelationService : RepositoryBase<TrainingRelation>, ITrainingRelationService
    {
        public TrainingRelationService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
    public class TrainingPeopleService : RepositoryBase<TrainingPeople>, ITrainingPeopleService
    {
        public TrainingPeopleService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
}

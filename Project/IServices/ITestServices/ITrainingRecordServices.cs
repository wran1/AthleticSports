using IServices.Infrastructure;
using Models.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.ITestServices
{
    public interface ITrainingTypeService : IRepository<TrainingType>
    {
    }
    public interface ITrainingRelationService : IRepository<TrainingRelation>
    {
    }
    public interface ITrainingPeopleService : IRepository<TrainingPeople>
    {
    }
}

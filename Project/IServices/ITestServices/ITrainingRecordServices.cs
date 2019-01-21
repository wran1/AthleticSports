using IServices.Infrastructure;
using Models.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IServices.ITestServices
{
    public interface ITrainingTypeServices : IRepository<TrainingType>
    {
    }
    public interface ITrainingRelationServices : IRepository<TrainingRelation>
    {
    }
    public interface ITrainingPeopleServices : IRepository<TrainingPeople>
    {
    }
}

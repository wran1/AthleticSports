using IServices.Infrastructure;
using Models.Dictionary;

namespace IServices.IDictionaryServices
{
    public interface ICityService : IRepository<City>
    {
    }
    public interface ITrainService : IRepository<Train>
    {
    }

}

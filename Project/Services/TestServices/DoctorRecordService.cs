using IServices.ISysServices;
using IServices.ITestServices;
using Models.TestModels;
using Services.Infrastructure;


namespace Services.TestServices
{
    public class DoctorRecordService : RepositoryBase<DoctorRecord>, IDoctorRecordService
    {
        public DoctorRecordService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
    public class ExportRecordService : RepositoryBase<ExportRecord>, IExportRecordService
    {
        public ExportRecordService(IDatabaseFactory databaseFactory, IUserInfo userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
}

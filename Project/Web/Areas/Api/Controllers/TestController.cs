using IServices.Infrastructure;
using IServices.ISysServices;
using IServices.ITestServices;
using Models.TestModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Areas.Api.Models;

namespace Web.Areas.Api.Controllers
{
    [Authorize]
    [RoutePrefix("API/Test")]
    public class TestController : ApiController
    {
        private readonly IUserInfo _iUserInfo;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly ISubjectiveTestService _isubjectiveTestService;
        private readonly IPainPointService _ipainPointService;
        private readonly ISysDepartmentSysUserService _iSysDepartmentSysUserService;
        private readonly ITrainingRelationService _iTrainingRelationService;
        public TestController(ITrainingRelationService iTrainingRelationService, ISysDepartmentSysUserService iSysDepartmentSysUserService, IPainPointService ipainPointService,IUserInfo iUserInfo, IUnitOfWork iUnitOfWork, ISubjectiveTestService isubjectiveTestService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iUserInfo = iUserInfo;
            _isubjectiveTestService = isubjectiveTestService;
            _ipainPointService = ipainPointService;
            _iSysDepartmentSysUserService = iSysDepartmentSysUserService;
            _iTrainingRelationService = iTrainingRelationService;
        }
        /// <summary>
        /// 获取当天运动员主观评测数据
        /// </summary>
        /// <returns></returns>
        [Route("GetUserSubjective")]
        public APIResult<SubjectiveTest> GetUserSubjective()
        {
            var day = DateTime.Now.Date.ToString();
           var model=  _isubjectiveTestService.GetAll(a => a.SysUserId == _iUserInfo.UserId).Where(a=> a.CreatedDate.IndexOf(day) >= 0).FirstOrDefault();
            if(model!=null)
            {
                return new APIResult<SubjectiveTest>(model);
            }
            return new APIResult<SubjectiveTest>(null);
        }
        /// <summary>
        /// 保存当天运动员主观评测数据
        /// PointNames 如果选择了部位用英文逗号隔开, 最后逗号结尾
        /// </summary>
        /// <returns></returns>
        [Route("SaveUserSubjective")]
        public async Task<APIResult<bool>> SaveUserSubjective(SubjectiveTest collect,string PointNames)
        {
            if (ModelState.IsValid)
            {
                var pointid = "";
                if (!string.IsNullOrEmpty(collect.Id))
                {
                    _isubjectiveTestService.Delete(collect.Id);
                    pointid = collect.Id;
                    _isubjectiveTestService.Save(collect.Id, collect);
                }
                else
                {
                    collect.SysUserId = _iUserInfo.UserId;
                    pointid = Guid.NewGuid().ToString();
                    collect.Id = pointid;
                    _isubjectiveTestService.Save(null, collect);
                }
                await _iUnitOfWork.CommitAsync();
                if(!string.IsNullOrEmpty(PointNames) && !string.IsNullOrEmpty(collect.Id))
                {
                    PointNames = PointNames.Substring(0, PointNames.Length - 1);
                    string[] sArray = PointNames.Split(',');
                    if(!string.IsNullOrEmpty(collect.Id))
                    {
                        var painpoints = _ipainPointService.GetAll(a => a.SubjectiveTestId == collect.Id);
                        foreach (var item in painpoints)
                        {
                            _ipainPointService.Delete(item.Id);
                        }
                    }
                    foreach (var item in sArray)
                    {
                        var painPoint = new PainPoint();
                        _ipainPointService.Save(null,new PainPoint{ PointName=(PointName)Enum.Parse(typeof(PointName), item), SubjectiveTestId= pointid });
                    }
                    await _iUnitOfWork.CommitAsync();
                }
                return new APIResult<bool>(true);
            }
            return new APIResult<bool>(false, 100, "操作失败");
        }
        /// <summary>
        /// 获取当前用户的体能训练项目名称
        /// </summary>
        /// <returns></returns>
        [Route("GetTrainName")]
        public APIResult<List<TrainNameModel>> GetTrainName()
        {
            var DepartId = _iSysDepartmentSysUserService.GetAll(a => a.SysUserId == _iUserInfo.UserId).FirstOrDefault().Id;
            var result = _iTrainingRelationService.GetAll(a => a.SysDepartmentId == DepartId).Select(a =>
             new TrainNameModel
             {
                 TrainId = a.TrainingTypeId,
                 Name=a.TrainingType.Name, 
             }).ToList();
            return new APIResult<List<TrainNameModel>>(result);
        }


    }
}

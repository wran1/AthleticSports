using AutoMapper;
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
        private readonly ITrainingPeopleService _iTrainingPeopleService;
        private readonly IPeriodicTestResultService _iPeriodicTestResultService;
        private readonly ITrainingTypeService _trainingTypeService;
        
        
        public TestController(ITrainingTypeService trainingTypeService,IPeriodicTestResultService iPeriodicTestResultService,ITrainingPeopleService iTrainingPeopleService,ITrainingRelationService iTrainingRelationService, ISysDepartmentSysUserService iSysDepartmentSysUserService, IPainPointService ipainPointService,IUserInfo iUserInfo, IUnitOfWork iUnitOfWork, ISubjectiveTestService isubjectiveTestService)
        {
            _iUnitOfWork = iUnitOfWork;
            _iUserInfo = iUserInfo;
            _isubjectiveTestService = isubjectiveTestService;
            _ipainPointService = ipainPointService;
            _iSysDepartmentSysUserService = iSysDepartmentSysUserService;
            _iTrainingRelationService = iTrainingRelationService;
            _iTrainingPeopleService = iTrainingPeopleService;
            _iPeriodicTestResultService = iPeriodicTestResultService;
            _trainingTypeService = trainingTypeService;
        }
        /// <summary>
        /// 获取当天运动员主观评测数据  添加day传参
        /// </summary>
        /// <param name="day">传选择的日期，不传默认当天时间 时间格式（yyyy-MM-dd）</param>
        /// <returns></returns>
        [Route("GetUserSubjective")]
        public APIResult<SubjectTestModel> GetUserSubjective(string day)
        {
            if(string.IsNullOrEmpty(day))
            {
                day = DateTime.Now.ToString("yyyy-MM-dd");
            }
            //var day = DateTime.Now.ToString("yyyy-MM-dd");
           var model=  _isubjectiveTestService.GetAll(a => a.SysUserId == _iUserInfo.UserId).Where(a=> a.DateSign==day);
            if(model!=null)
            {
                var result = model.Select(a=> new SubjectTestModel {
                    Id = a.Id,
                    DateSign = a.DateSign,
                    Desire = a.Desire,
                    Evaluate = a.Evaluate,
                    Fatigue = a.Fatigue,
                    FatigueLevel = a.FatigueLevel,
                    FitnessMinute = a.FitnessMinute,
                    MatchMinute = a.MatchMinute,
                    MorPulse = a.MorPulse,
                    SleepDuration = a.SleepDuration,
                    SleepQuality = a.SleepQuality,
                    SorenessLevel = a.SorenessLevel,
                    SpecialMinute = a.SpecialMinute,
                    TrainIntensity = a.TrainIntensity,
                    TrainStatus = a.TrainStatus,
                    Weight = a.Weight,
                     DoctorRecord=a.DoctorRecord,
                }).FirstOrDefault();
                return new APIResult<SubjectTestModel>(result);
            }
            return new APIResult<SubjectTestModel>(null);
        }
        /// <summary>
        ///APP运动员权限 获取运动员的疼痛部位
        /// </summary>
        /// <returns></returns>
        [Route("GetPaintNames")]
        public APIResult<List<PointNames>> GetPaintNames(string id)
        {
           var point=  _ipainPointService.GetAll(a=>a.SubjectiveTestId==id).Select(a=> new PointNames { PointName= a.PointName.ToString() }).ToList();
            return new APIResult<List<PointNames>>(point);
        }
        /// <summary>
        /// APP教练权限 获取运动员的疼痛部位列表
        /// </summary>
        /// <param name="userid">运动员id</param>
        /// <param name="pagesize">每页数量</param>
        /// <param name="pageindex">页数</param>
        /// <returns></returns>
        [Route("GetPaintList")]
        public APIResult<List<PointList>> GetPaintList(string userid,int pagesize = 1, int pageindex = 1)
        {
            var model = _isubjectiveTestService.GetAll(a => a.SysUserId == userid).Where(a => a.PainPoints.Count() > 0).Select(a=>new PointList
            {
                  DateSign=a.DateSign,
                  SubjectiveTestId=a.Id,
                  PointNames=a.PainPoints.Select(p=>new PointNames { PointName= p.PointName.ToString()}).ToList()
            }).OrderByDescending(a => a.DateSign).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
           
            return new APIResult<List<PointList>>(model);
        }
        /// <summary>
        /// APP 保存运动员主观评测数据（如果是新增数据 Id传null或不传，如果是修改数据Id传需要修改的Id）
        /// PointNames 如果选择了部位用英文逗号隔开, 最后逗号结尾
        /// </summary>
        /// <returns></returns>
        [Route("SaveUserSubjective")]
        public async Task<APIResult<string>> SaveUserSubjective(SubjectTestModel collect,string PointNames)
        {
            if (ModelState.IsValid)
            {
                var pointid = "";
                if (string.IsNullOrEmpty(collect.DateSign))
                {
                    collect.DateSign = DateTime.Now.ToString("yyyy-MM-dd");
                }
                var config = new MapperConfiguration(a => a.CreateMap<SubjectTestModel, SubjectiveTest>());
                var sb = config.CreateMapper().Map<SubjectiveTest>(collect);
                sb.SysUserId = _iUserInfo.UserId;
                if (!string.IsNullOrEmpty(collect.Id))
                {
                    //_isubjectiveTestService.Delete(collect.Id);
                    pointid = collect.Id;
                    _isubjectiveTestService.Save(collect.Id, sb);
                }
                else
                {
                    sb.SysUserId = _iUserInfo.UserId;
                    pointid = Guid.NewGuid().ToString();
                    sb.Id = pointid;
                    _isubjectiveTestService.Save(null, sb);
                }
                await _iUnitOfWork.CommitAsync();
                if(!string.IsNullOrEmpty(PointNames))
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
                return new APIResult<string>(pointid);
            }
            return new APIResult<string>("", 100, "操作失败",ModelState);
        }
        /// <summary>
        /// APP 保存教练评价
        /// </summary>
        /// <param name="id"></param>
        /// <param name="evaluate"></param>
        /// <returns></returns>
        [Route("SaveEvaluate")]
        public async Task<APIResult<bool>> SaveEvaluate(string id,int evaluate)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var subtest = _isubjectiveTestService.GetById(id);
                subtest.Evaluate = evaluate;
                _isubjectiveTestService.Save(id, subtest);
                await _iUnitOfWork.CommitAsync();
                return new APIResult<bool>(true);
            }
            return new APIResult<bool>(false,100,"数据有误");

        }
        /// <summary>
        /// APP 保存队医的记录
        /// </summary>
        /// <param name="id">主观评测id</param>
        /// <param name="record">队医记录</param>
        /// <returns></returns>
        [Route("SaveDoctorRecord")]
        public async Task<APIResult<bool>> SaveDoctorRecord(string id, string record)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var subtest = _isubjectiveTestService.GetById(id);
                subtest.DoctorRecord = record;
                _isubjectiveTestService.Save(id, subtest);
                await _iUnitOfWork.CommitAsync();
                return new APIResult<bool>(true);
            }
            return new APIResult<bool>(false, 100, "数据有误");

        }
        /// <summary>
        /// PC 获取某个运动员所有伤痛记录
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        [Route("GetAllDoctorRecord")]
        public APIResult<List<DoctorRecordModel>> GetAllDoctorRecord(string userid, int pagesize = 1, int pageindex = 1)
        {
            if (!string.IsNullOrEmpty(userid))
            {
              var result=  _isubjectiveTestService.GetAll(a=>a.SysUserId== userid).Select(a=> new DoctorRecordModel
              {
                  Id=a.Id,
                   Record=a.DoctorRecord,
                   Date=a.DateSign
              }).OrderByDescending(a=>a.Date).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                return new APIResult<List<DoctorRecordModel>>(result);
            }
            return new APIResult<List<DoctorRecordModel>>(null);
        }
        /// <summary>
        /// 获取时间段的某个运动员主评记录
        /// </summary>
        /// <param name="sportuserid">运动员id</param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="pagesize">app不需要可以传999</param>
        /// <param name="pageindex">>app不需要传1就行</param>
        /// <returns></returns>
        [Route("GetAllUserSubjective")]
        public APIResult<List<SubjectTestModel>> GetAllUserSubjective(string sportuserid,string starttime,string endtime,int pagesize = 1, int pageindex = 1)
        {
            var list = _isubjectiveTestService.GetAll(a=>a.SysUserId== sportuserid);
            if(!string.IsNullOrEmpty(starttime)&& !string.IsNullOrEmpty(endtime))
            {
                var result = list.Where(a=>a.DateSign.CompareTo(starttime)>=0 && a.DateSign.CompareTo(endtime) <= 0).OrderBy(a=>a.DateSign).Select(a=>new SubjectTestModel
                {
                    Id=a.Id,
                    DateSign=a.DateSign,
                    Desire=a.Desire,
                    Evaluate=a.Evaluate,
                    Fatigue=a.Fatigue,
                    FatigueLevel=a.FatigueLevel,
                    FitnessMinute=a.FitnessMinute,
                    MatchMinute=a.MatchMinute,
                    MorPulse=a.MorPulse,
                    SleepDuration=a.SleepDuration,
                    SleepQuality=a.SleepQuality,
                    SorenessLevel=a.SorenessLevel,
                    SpecialMinute=a.SpecialMinute,
                    TrainIntensity=a.TrainIntensity,
                    TrainStatus=a.TrainStatus,
                    Weight=a.Weight,
                }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();

                return new APIResult<List<SubjectTestModel>>(result);
            }
            return new APIResult<List<SubjectTestModel>>(null,100,"时间选择有误");
        }
        /// <summary>
        ///APP教练 获取所有的体能训练项目名称
        /// </summary>
        /// <returns></returns>
        [Route("GetAllTrainNames")]
        public APIResult<List<TrainNameModel>> GetAllTrainNames()
        {
            var model=_trainingTypeService.GetAll().OrderBy(a=>a.Position).Select(a=>
             new TrainNameModel
             {
                 TrainId = a.Id,
                 Name = a.Name,
             }).ToList();
            return new APIResult<List<TrainNameModel>>(model);
        }

        /// <summary>
        ///PC/APP 获取当前用户的体能训练项目名称
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
        /// <summary>
        ///APP教练权限 保存配置体能训练项目
        /// </summary>
        /// <returns></returns>
        [Route("SaveTrainNames")]
        public async Task<APIResult<bool>> SaveTrainNames(List<string> TrainingTypeids)
        {
            var DepartId = _iSysDepartmentSysUserService.GetAll(a => a.SysUserId == _iUserInfo.UserId).FirstOrDefault().SysDepartmentId;
            if (!string.IsNullOrEmpty(DepartId))
            {
                //清除原有数据
                _iTrainingRelationService.Delete(a => a.SysDepartmentId.Equals(DepartId));
            }
            //_iSysRoleService.Save(id, collection);
            if (TrainingTypeids != null)
            {
                foreach (var item in TrainingTypeids)
                {
                    _iTrainingRelationService.Save(null, new TrainingRelation
                    {
                        SysDepartmentId = DepartId,
                        TrainingTypeId = item
                    });
                }
            }
            await _iUnitOfWork.CommitAsync();
            return new APIResult<bool>(true);
        }
        /// <summary>
        ///PC/APP 保存体能训练
        /// </summary>
        /// <returns></returns>
        [Route("SaveTrainResult")]
        public async Task<APIResult<bool>> SaveTrainResult(List<TrainResultModel> listTrain)
        {
            
            foreach (var item in listTrain)
            {
               var model= _iTrainingPeopleService.GetAll(a => a.SysUserId == _iUserInfo.UserId && a.TestDate== item.TestDate).FirstOrDefault();
                if(model != null)
                {
                    _iTrainingPeopleService.Delete(model.Id);
                }
                var trainingPeople = new TrainingPeople();
                trainingPeople.TrainingTypeId = item.TrainId;
                trainingPeople.SysUserId= _iUserInfo.UserId;
                trainingPeople.Value = item.Value;
                trainingPeople.TestDate = item.TestDate;
         
                _iTrainingPeopleService.Save(null, trainingPeople);
            }
            await _iUnitOfWork.CommitAsync();
            return new APIResult<bool>(true);
        }
        /// <summary>
        /// PC/APP 获取最新体能训练数据(date不传为最新的数据)
        /// </summary>
        /// <param name="date">获取某一天的数据（格式为yyyy-MM-dd）</param>
        /// <returns></returns>
        [Route("GetNewTrainResult")]
        public APIResult<List<TrainResultModel>> GetNewTrainResult(string date)
        {
            var DepartId = _iSysDepartmentSysUserService.GetAll(a => a.SysUserId == _iUserInfo.UserId).FirstOrDefault().Id;
            var trainresults = _iTrainingRelationService.GetAll(a => a.SysDepartmentId == DepartId).Select(a => a.TrainingTypeId).ToList();
            var result = new List<TrainResultModel>();
            if(trainresults.Count()>0)
            {
                if (string.IsNullOrEmpty(date))
                {
                    foreach (var item in trainresults)
                    {
                        var model = _iTrainingPeopleService.GetAll(a => a.TrainingTypeId == item).OrderByDescending(a => a.TestDate).Select(a => new TrainResultModel
                        {
                            TrainId = a.TrainingTypeId,
                            Value = a.Value,
                            TestDate = a.TestDate

                        }).FirstOrDefault();
                        result.Add(model);
                    }
                    return new APIResult<List<TrainResultModel>>(result);
                }
                else
                {
                    foreach (var item in trainresults)
                    {
                        var model = _iTrainingPeopleService.GetAll(a => a.TrainingTypeId == item).Where(a => a.TestDate == date).OrderByDescending(a => a.CreatedDate).Select(a => new TrainResultModel
                        {
                            TrainId = a.TrainingTypeId,
                            Value = a.Value,
                            TestDate = a.TestDate

                        }).FirstOrDefault();
                        result.Add(model);
                    }
                    return new APIResult<List<TrainResultModel>>(result);
                }
            }
            return new APIResult<List<TrainResultModel>>(null,100,"该运动员还未配置体能项目");



        }
        /// <summary>
        /// APP/PC  获取时间段的某个运动员体能训练数据
        /// </summary>
        /// <param name="sportuserid">运动员id</param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="pagesize">app不需要可以传999</param>
        /// <param name="pageindex">>app不需要传1就行</param>
        /// <returns></returns>
        [Route("GetAllTrainResult")]
        public APIResult<List<AllTrainModel>> GetAllTrainResult(string sportuserid, string starttime, string endtime, int pagesize = 1, int pageindex = 1)
        {
            if(!string.IsNullOrEmpty(sportuserid))
            {
                var trainpeople = _iTrainingPeopleService.GetAll(a => a.SysUserId == sportuserid);
                if(!string.IsNullOrEmpty(starttime))
                {
                    trainpeople = trainpeople.Where(a => a.TestDate.CompareTo(starttime) >= 0);
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    trainpeople = trainpeople.Where(a => a.TestDate.CompareTo(endtime) <= 0);
                }
                var model= trainpeople.Select(a => new AllTrainModel
                {
                    TypeName = a.TrainingType.Name,
                    Value = a.Value,
                    TestDate = a.TestDate
                }).OrderByDescending(a => a.TestDate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                return new APIResult<List<AllTrainModel>>(model);
            }
            return new APIResult<List<AllTrainModel>>(null);
         
        }
        /// <summary>
        /// APP/PC  获取时间段的某个运动员生理生化数据
        /// </summary>
        /// <param name="sportuserid">运动员id</param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="pagesize">app不需要可以传999</param>
        /// <param name="pageindex">>app不需要传1就行</param>
        /// <returns></returns>
        [Route("GetAllPeriodicTest")]
        public APIResult<List<PeriodicTestResult>> GetAllPeriodicTest(string sportuserid, string starttime, string endtime, int pagesize = 1, int pageindex = 1)
        {
            if (!string.IsNullOrEmpty(sportuserid))
            {
                var periodic=_iPeriodicTestResultService.GetAll(a => a.SysUserId == sportuserid);
                if (!string.IsNullOrEmpty(starttime))
                {
                    periodic = periodic.Where(a => a.Testdate.CompareTo(starttime) >= 0);
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    periodic = periodic.Where(a => a.Testdate.CompareTo(endtime) <= 0);
                }
                var model = periodic.OrderByDescending(a => a.Testdate).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                return new APIResult<List<PeriodicTestResult>>(model);
            }
            return new APIResult<List<PeriodicTestResult>>(null);
        }


    }
}

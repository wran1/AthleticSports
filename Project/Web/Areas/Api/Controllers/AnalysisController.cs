using IServices.ISysServices;
using IServices.ITestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Areas.Api.Models;

namespace Web.Areas.Api.Controllers
{
    [Authorize]
    [RoutePrefix("API/Analysis")]
    public class AnalysisController : ApiController
    {
        private readonly IUserInfo _iUserInfo;
        private readonly IBodyCompositionService _iBodyCompositionService;
        private readonly IPeriodicTestResultService _iPeriodicTestResultService;
        private readonly ISubjectiveTestService _iSubjectiveTestService;
        
        public AnalysisController(ISubjectiveTestService iSubjectiveTestService,IUserInfo iUserInfo, IBodyCompositionService iBodyCompositionService, IPeriodicTestResultService iPeriodicTestResultService)
        {
            _iUserInfo = iUserInfo;
            _iBodyCompositionService = iBodyCompositionService;
            _iPeriodicTestResultService = iPeriodicTestResultService;
            _iSubjectiveTestService = iSubjectiveTestService;
        }
        /// <summary>
        /// PC/APP 体脂率（和体成分）走势图
        /// </summary>
        /// <param name="sportuserid"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [Route("GetBodyFatTrend")]
        public APIResult<List<BodyFatModel>> GetBodyFatTrend(string sportuserid,string starttime,string endtime)
        {
            if(!string.IsNullOrEmpty(sportuserid))
            {
                var bodycomposition= _iBodyCompositionService.GetAll(a => a.SysUserId == sportuserid);
                if(!string.IsNullOrEmpty(starttime))
                {
                    bodycomposition = bodycomposition.Where(a=>a.Testdate.CompareTo(starttime)>=0);
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    bodycomposition = bodycomposition.Where(a => a.Testdate.CompareTo(endtime)<= 0);
                }
                var model = bodycomposition.Select(a => new BodyFatModel
                {
                     BF=a.BF,
                     Testdate=a.Testdate
                }).OrderBy(a=>a.Testdate).ToList();
                return new APIResult<List<BodyFatModel>>(model);
            }
            return new APIResult<List<BodyFatModel>>(null,100,"数据错误");
        }

        /// <summary>
        /// PC/APP 睾酮/皮质醇（和睾酮）走势图
        /// </summary>
        /// <param name="sportuserid"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [Route("GetPeriodicTrend")]
        public APIResult<List<PeriodicModel>> GetPeriodicTrend(string sportuserid, string starttime, string endtime)
        {
            if (!string.IsNullOrEmpty(sportuserid))
            {
                var bodycomposition = _iPeriodicTestResultService.GetAll(a => a.SysUserId == sportuserid);
                if (!string.IsNullOrEmpty(starttime))
                {
                    bodycomposition = bodycomposition.Where(a => a.Testdate.CompareTo(starttime) >= 0);
                }
                if (!string.IsNullOrEmpty(endtime))
                {
                    bodycomposition = bodycomposition.Where(a => a.Testdate.CompareTo(endtime) <= 0);
                }
                var model = bodycomposition.Select(a => new PeriodicModel
                {
                    Ratio = Math.Round(a.Testosterone/a.Cortisol),
                    Testosterone=a.Testosterone,
                    Testdate = a.Testdate
                }).OrderBy(a => a.Testdate).ToList();
                return new APIResult<List<PeriodicModel>>(model);
            }
            return new APIResult<List<PeriodicModel>>(null, 100, "数据错误");
        }
        /// <summary>
        /// 运动员强度走势图
        /// </summary>
        /// <param name="sportuserid"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        [Route("GetExerciseIntensityTrend")]
        public APIResult<List<ExerciseIntensityTrendModel>> GetExerciseIntensityTrend(string sportuserid, string starttime, string endtime)
        {
            var Exerciselist = new List<ExerciseIntensityTrendModel>();
            var item =new ExerciseIntensityTrendModel();
            var begin = Convert.ToDateTime(starttime);
            var end = Convert.ToDateTime(endtime);
            var weeks = TotalWeeks(begin, end);
            var sw = (int)begin.DayOfWeek;
            var date1 = begin.AddDays(1 - sw);
            if(weeks>0)
            {
                for (int i = 0; i < weeks; i++)
                {
                    var AStart = date1.AddDays(i * 7);
                    var AEnd = date1.AddDays(i * 7 + 7);
                    item = AvgIntensity(AStart, AEnd);
                    item.Testdate = (i + 1).ToString();
                    Exerciselist.Add(item);
                }
                return new APIResult<List<ExerciseIntensityTrendModel>>(Exerciselist);
            }
            return new APIResult<List<ExerciseIntensityTrendModel>>(null);
        }
        #region 统计一段时间占多少个星期
        ///<summary> 
        ///统计一段时间占多少个星期
        ///</summary> 
        ///<param name="AStart">开始日期</param> 
        ///<param name="AEnd">结束日期</param> 
        ///<returns>返回个数</returns> 
        public static int TotalWeeks(DateTime AStart, DateTime AEnd)
        {
            TimeSpan vTimeSpan = new TimeSpan(AEnd.Ticks - AStart.Ticks);
            int Result = (int)vTimeSpan.TotalDays / 7;
            for (int i = 0; i <= vTimeSpan.TotalDays % 7; i++)
                if (AStart.AddDays(i).DayOfWeek == AStart.DayOfWeek)
                    return Result + 1;
            return Result;
        }

        public ExerciseIntensityTrendModel AvgIntensity(DateTime AStart, DateTime AEnd)
        {
            var model = new ExerciseIntensityTrendModel();
            DateTime AStart4weeks = AStart.AddDays(-21);
            var Ratio= _iSubjectiveTestService.GetAll().Where(a => a.DateSign.CompareTo(AStart4weeks) >= 0 && a.DateSign.CompareTo(AEnd) <= 0).Average(a => a.TrainIntensity);
            var avgResult = _iSubjectiveTestService.GetAll().Where(a => a.DateSign.CompareTo(AStart) >= 0 && a.DateSign.CompareTo(AEnd) <= 0).Average(a => a.TrainIntensity);
            var standard = _iSubjectiveTestService.GetAll().Where(a => a.DateSign.CompareTo(AStart) >= 0 && a.DateSign.CompareTo(AEnd) <= 0).Sum(a => Math.Pow(a.TrainIntensity - avgResult, 2));
            var standardResult = Math.Sqrt(standard /7);
            double RatioResult = avgResult / Ratio;
            model.AvgIntensity = avgResult;
            model.StandardIntensity = standardResult;
            model.RatioIntensity = RatioResult;
            return model;
        }
        #endregion
    }
}

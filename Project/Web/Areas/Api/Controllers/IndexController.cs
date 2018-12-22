using IServices.ICmsServices;
using IServices.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Http;
using Web.Areas.Api.Models;

namespace Web.Areas.Api.Controllers
{
   
    [RoutePrefix("API/Index")]
    public class IndexController : ApiController
    {
        private readonly ICmsCategoryService _iCmsCategoryService;
        private readonly ICmsArticalService _iCmsArticalService;
        private readonly IUnitOfWork _iUnitOfWork;
        public IndexController(ICmsCategoryService iCmsCategoryService, ICmsArticalService iCmsArticalService, IUnitOfWork iUnitOfWork)
        {
            _iCmsCategoryService = iCmsCategoryService;
            _iCmsArticalService = iCmsArticalService;
            _iUnitOfWork = iUnitOfWork;
        }

        /// <summary>
        /// 获取首页的数据或列表页
        /// </summary>
        /// <param name="Type">type为空是所有类型的置顶数据</param>
        /// <param name="First">为true为首页数据</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">每页个数</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetFrontispiece")]
        public APIResult<List<FrontispieceModel>> GetFrontispiece(string Type, bool First, int pageindex = 1, int pagesize = 1)
        {
            var model = _iCmsArticalService.GetAll(a=>!a.Deleted && a.CmsCategory.Enable);
            if(!string.IsNullOrEmpty(Type))
            {
                model = model.Where(a => a.CmsCategory.Name == Type);
            }
            if (First)
            {
                model = model.Where(a => a.IsTop && a.Enable);
            }
            else
            {
                model = model.Where(a => a.Enable);
            }
            var result = model.Select(a => new FrontispieceModel
            {
                Id = a.Id,
                Type = a.CmsCategory.Name,
                Title = a.Title ?? "",
                Subtitle = a.Subtitle ?? "",
                Abstract = a.Abstract ?? "",
                VideoAttachFile = string.IsNullOrEmpty(a.VideoUrl) ? string.IsNullOrEmpty(a.VideoAttachFile) ? "" : "http ://newbusinesscircles.chinacloudsites.cn" + a.VideoAttachFile.Substring(a.VideoAttachFile.IndexOf("\"url\":\"") + 7, a.VideoAttachFile.Length - a.VideoAttachFile.IndexOf("\"url\":\"") - 10) : a.VideoUrl,
                VoiceAttachFile = string.IsNullOrEmpty(a.AudioUrl) ? string.IsNullOrEmpty(a.VoiceAttachFile) ? "" : "http ://newbusinesscircles.chinacloudsites.cn" + a.VoiceAttachFile.Substring(a.VoiceAttachFile.IndexOf("\"url\":\"") + 7, a.VoiceAttachFile.Length - a.VoiceAttachFile.IndexOf("\"url\":\"") - 10) : a.AudioUrl,
                Author = a.Author ?? "",
                Content = a.Content ?? "",
                CoverImage = a.CoverImage ?? "",
                ByImage = a.ByImage ?? "",
                ImgAlbum = a.ImgAlbum ?? "",
                Keywords = a.Keywords ?? "",
                Post = a.Post ?? "",
                Sourse = a.Sourse ?? "",
                PublishTime=a.PublishTime,
                Externallinks=a.Externallinks,
                IsTop=a.IsTop

            }).OrderBy(a => a.Type).ThenByDescending(a => a.IsTop).ThenByDescending(a=>a.PublishTime);
            if (pageindex!=0 && pagesize!=0)
            {
                var resultmodel = result.Skip((pageindex - 1) * pagesize).Take(pagesize);
                return new APIResult<List<FrontispieceModel>>(resultmodel.ToList());
            }
            return new APIResult<List<FrontispieceModel>>(result.ToList());

        }
        /// <summary>
        /// 往期回顾列表
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("ToreviewList")]
        public APIResult<List<ToReviewModel>> GetreviewList()
        {
            var model = _iCmsArticalService.GetAll(a => a.CmsCategory.Name == "卷首").Select(a => new ToReviewModel
            {
                Id=a.Id,
                ByImage = a.ByImage ?? "",
                Byname = a.Byname ?? "",
                Year = a.PublishTime 
            }).OrderByDescending(a=> a.Year).ToList();

            return new APIResult<List<ToReviewModel>>(model);
        }
        /// <summary>
        /// 详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetDetail")]
        public APIResult<FrontispieceModel> GetDetail(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var model = _iCmsArticalService.GetAll(a => a.Id == id && a.Enable).Select(a => new FrontispieceModel
                {
                    Id=a.Id,
                    Type = a.CmsCategory.Name,
                    Title = a.Title ?? "",
                    Subtitle = a.Subtitle ?? "",
                    Abstract = a.Abstract ?? "",
                    VideoAttachFile = string.IsNullOrEmpty(a.VideoUrl) ? string.IsNullOrEmpty(a.VideoAttachFile) ? "" : "http ://newbusinesscircles.chinacloudsites.cn" + a.VideoAttachFile.Substring(a.VideoAttachFile.IndexOf("\"url\":\"")+7, a.VideoAttachFile.Length- a.VideoAttachFile.IndexOf("\"url\":\"")-10) : a.VideoUrl,
                    VoiceAttachFile = string.IsNullOrEmpty(a.AudioUrl) ? string.IsNullOrEmpty(a.VoiceAttachFile) ? "" : "http ://newbusinesscircles.chinacloudsites.cn" + a.VoiceAttachFile.Substring(a.VoiceAttachFile.IndexOf("\"url\":\"")+7, a.VoiceAttachFile.Length - a.VoiceAttachFile.IndexOf("\"url\":\"") - 10) : a.AudioUrl,
                    Author = a.Author ?? "",
                    Content = a.Content ?? "",
                    CoverImage = a.CoverImage ?? "",
                    ByImage = a.ByImage ?? "",
                    ImgAlbum = a.ImgAlbum ?? "",
                    Keywords = a.Keywords ?? "",
                    Post = a.Post ?? "",
                    Sourse = a.Sourse ?? "",
                     
                    PublishTime = a.PublishTime
                }).FirstOrDefault();
                return new APIResult<FrontispieceModel>(model);
            };
            return new APIResult<FrontispieceModel>(null);
        }
        /// <summary>
        /// 找服务和找合作下拉
        /// </summary>
        /// <param name="Type">找服务/找合作</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetExclusive")]
        public APIResult<List<TypeClassModel>> GetExclusive(string Type)
        {
            var model = _iCmsCategoryService.GetAll(a => a.Enable &&  a.SystemId.Length > 3);
            if (Type=="找服务")
            {
                model = model.Where(a => a.SystemId.Substring(0, 3) == "900");
            }
            else if(Type=="找合作")
            {
                model = model.Where(a => a.SystemId.Substring(0, 3) == "800");
            }
            var result = model.OrderBy(a=>a.SystemId).Select(a => new TypeClassModel { Id = a.Id, Name = a.Name }).ToList();
            return new APIResult<List<TypeClassModel>>(result);
        }
        /// <summary>
        /// 专属福利:找服务/找合作
        /// </summary>
        /// <param name="Type">找服务/找合作</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetExclusiveList")]
        public APIResult<List<ExclusiveModel>> GetExclusiveList(string Type)
        {
            var model = _iCmsArticalService.GetAll(a => a.Enable && a.IsTop && a.CmsCategory.SystemId.Length > 3);
            if (Type == "找服务")
            {
                model = model.Where(a => a.CmsCategory.SystemId.Substring(0, 3) == "900");
            }
            else if (Type == "找合作")
            {
                model = model.Where(a => a.CmsCategory.SystemId.Substring(0, 3) == "800");
            }
            model = model.OrderBy(a => a.CmsCategory.SystemId);
            var result = model.Select(a => new ExclusiveModel { typeid=a.CmsCategoryId, Id = a.Id, Type=a.CmsCategory.Name, Name = a.Title, Image=a.CoverImage, Externallinks=a.Externallinks, UpdateTime=a.UpdatedDate });
            var resultmodel = result.ToList();
            return new APIResult<List<ExclusiveModel>>(resultmodel);
        }
        /// <summary>
        /// 专属福利:找服务/找合作二级列表页
        /// </summary>
        /// <param name="Type">找服务/找合作</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页数</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetserviceCooperation")]
        public APIResult<List<serviceCooperationModel>> GetserviceCooperation(string Type, int pageindex = 1, int pagesize = 1)
        {
            var model = _iCmsArticalService.GetAll(a => a.Enable && a.CmsCategory.SystemId.Length > 3);
            if (Type == "找服务")
            {
                model = model.Where(a => a.CmsCategory.SystemId.Substring(0, 3) == "900");
            }
            else if (Type == "找合作")
            {
                model = model.Where(a => a.CmsCategory.SystemId.Substring(0, 3) == "800");
            }
            var result = model.Select(a => new serviceCooperationModel
            {
                Id = a.Id,
                Type = a.CmsCategory.Name,
                Title = a.Title,
                Subtitle=a.Subtitle,
                Image = a.CoverImage,
                Abstract = a.Abstract,
                Externallinks = a.Externallinks,
                Creatdatetime = a.CreatedDate
            }).OrderByDescending(a => a.Creatdatetime).Skip((pageindex - 1) * pagesize).Take(pagesize);
            var resultmodel = result.ToList();
            return new APIResult<List<serviceCooperationModel>>(resultmodel);
        }

        /// <summary>
        /// 风尚：演出/书架/时尚 数字声音：数字/声音
        /// </summary>
        /// <param name="Type">风尚/数字声音（一级栏目名称）</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetSubColumnTop")]
        public APIResult<List<SubColumnTop>> GetSubColumnTop(string Type)
        {
            var categoryId = _iCmsCategoryService.GetAll(a => a.Name == Type).Select(a => a.SystemId).FirstOrDefault();
            if (!string.IsNullOrEmpty(categoryId))
            {
                var model = _iCmsArticalService.GetAll(a => a.Enable && a.IsTop && a.CmsCategory.SystemId.Length > 3
                && a.CmsCategory.SystemId.Contains(categoryId)).OrderByDescending(a => a.PublishTime);
                var resultmodel = model.Select(a => new SubColumnTop
                {
                    Id = a.Id,
                    Type = a.CmsCategory.Name,
                    Name = a.Title,
                    Image = a.CoverImage,
                    Abstract = a.Abstract,
                    PublishTime = a.PublishTime,
                }).ToList();
                return new APIResult<List<SubColumnTop>>(resultmodel);
            }
            var result = new List<SubColumnTop>();
            return new APIResult<List<SubColumnTop>>(result);
        }

        /// <summary>
        /// 风尚：演出/书架/时尚 数字声音：数字/声音二级列表页
        /// </summary>
        /// <param name="Type">风尚/数字声音（一级栏目名称）</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页数</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("GetSubColumnList")]
        public APIResult<List<SubColumnTop>> GetSubColumnList(string Type, int pageindex = 1, int pagesize = 1)
        {
            var categoryId = _iCmsCategoryService.GetAll(a => a.Name == Type).Select(a => a.SystemId).FirstOrDefault();
            if (!string.IsNullOrEmpty(categoryId))
            {
                var model = _iCmsArticalService.GetAll(a => a.Enable && a.CmsCategory.SystemId.Length > 3
                && a.CmsCategory.SystemId.Contains(categoryId)).OrderByDescending(a => a.PublishTime);
                var resultmodel = model.Select(a => new SubColumnTop
                {
                    Id = a.Id,
                    Type = a.CmsCategory.Name,
                    Name = a.Title,
                    Image = a.CoverImage,
                    Abstract = a.Abstract,
                    PublishTime = a.PublishTime,
                }).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
                return new APIResult<List<SubColumnTop>>(resultmodel);
            }
            var result = new List<SubColumnTop>();
            return new APIResult<List<SubColumnTop>>(result);
        }
    }
}

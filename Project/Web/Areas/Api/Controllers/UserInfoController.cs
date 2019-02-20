using IServices.Infrastructure;
using IServices.ISysServices;
using Models.SysModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Web.Areas.Api.Models;

namespace Web.Areas.Api.Controllers
{
    [Authorize]
    [RoutePrefix("API/User")]
    public class UserInfoController : ApiController
    {
        private readonly ISysUserService _iSysUserService;
        private readonly IUnitOfWork _iUnitOfWork;
        private readonly IUserInfo _iUserInfo;
        private readonly ISysDepartmentSysUserService _iSysDepartmentSysUserService;
        public UserInfoController(ISysDepartmentSysUserService iSysDepartmentSysUserService,ISysUserService iSysUserService, IUserInfo iUserInfo, IUnitOfWork iUnitOfWork)
        {
            _iSysUserService = iSysUserService;
            _iUnitOfWork = iUnitOfWork;
            _iUserInfo = iUserInfo;
            _iSysDepartmentSysUserService = iSysDepartmentSysUserService;
        }
        /// <summary>
        /// 获取用户信息 
        /// </summary>
        /// <returns></returns>
        [Route("GetUserInfo")]
        public APIResult<UserInfoModels> GetUserInfo()
        {
            var user = _iSysUserService.GetById(_iUserInfo.UserId);
            if (user != null)
            {
                var data = new UserInfoModels()
                {
                    FullName = user.FullName,
                    Birthday = user.Birthday,
                    Sex = user.Sex,
                    SportGrade = user.SportGrade,
                    DepartmentId = user.SysDepartmentSysUsers.FirstOrDefault().SysDepartmentId,
                    DepartmentName = user.SysDepartmentSysUsers.FirstOrDefault().SysDepartment.Name,
                    TrainId = user.TrainId,
                    TrainName = user.Train.Name,
                    Start4Training = user.Start4Training,
                    Train4year = DateTime.Now.Year - user.Start4Training,
                };
                return new APIResult<UserInfoModels>(data);
            }
           return new APIResult<UserInfoModels>(null);
        }
        /// <summary>
        /// 保存个人信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("SaveUserInfo")]
        public async Task<APIResult<bool>> SaveUserInfo(UserInfoModels model)
        {
            if (ModelState.IsValid)
            {
                var user = _iSysUserService.GetById(_iUserInfo.UserId);
                user.FullName = model.FullName;
                user.Birthday = model.Birthday;
                user.Sex = model.Sex;
                user.SportGrade = model.SportGrade;
                user.TrainId = model.TrainId;
                user.Start4Training = model.Start4Training;

                //todo 保存教练
                if (!string.IsNullOrEmpty(model.DepartmentId))
                {

                    _iSysDepartmentSysUserService.Delete(a => a.SysUserId == model.DepartmentId);

                    _iSysDepartmentSysUserService.Save(null,
                        new SysDepartmentSysUser { SysDepartmentId = model.DepartmentId, SysUserId = user.Id });

                }

                _iSysUserService.Save(user.Id, user);
                await _iUnitOfWork.CommitAsync();
                return new APIResult<bool>(true);
            }
            return new APIResult<bool>(false, 100, "操作失败");
        }
        /// <summary>
        /// 保存用户头像
        /// </summary>
        /// <returns></returns>
        [Route("SetUserHead")]
        public async Task<APIResult<bool>> SaveUserInfo()
        {
            var file = HttpContext.Current.Request.Files[0];
            if (file == null)
            {
                return new APIResult<bool>(false, 100, "上传的文件不能为空");
    
            }
            var contentType = file.ContentType.ToLower();
            if (!(contentType == "image/png" || contentType == "image/jpg" || contentType == "image/jpeg"))
            {
                return new APIResult<bool>(false, 100, "不支持的图片格式");
            }
            var extName = Path.GetExtension(file.FileName);
            var filename = Guid.NewGuid() + extName;

            var outStream = new MemoryStream();

            file.InputStream.CopyTo(outStream);

            var uploadFileRoot = "userfiles";

            if (ConfigurationManager.AppSettings["UploadFileRoot"] != null)
            {
                uploadFileRoot = ConfigurationManager.AppSettings["UploadFileRoot"];
            }

            //上传到网站目录
            var url = "/" + uploadFileRoot + "/" + _iUserInfo.UserId + "/";

            var path = HttpContext.Current.Server.MapPath(url);

            if (!Directory.Exists(path))//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(path);
            }

            var filePhysicalPath = path + filename;//我把它保存在网站根目录的 upload 文件夹，需要在项目中添加对应的文件夹

            outStream.Position = 0;

            outStream.CopyTo(new FileStream(filePhysicalPath, FileMode.CreateNew, FileAccess.ReadWrite));  //上传文件到指定文件夹
            if (url != null)
            {
                var user = _iSysUserService.GetById(_iUserInfo.UserId);
                if (user != null)
                {
                    user.Picture = url + filename;
                    _iSysUserService.Save(user.Id, user);
                    await _iUnitOfWork.CommitAsync();
                    return new APIResult<bool>(true);
                }
            }
            return new APIResult<bool>(false, 100, "上传失败");

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace Web.Areas.Api.Models
{
    /// <summary>
    /// API返回数据统一结构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResult<T>
    {

        public APIResult(T data, int stateCode = 0, string message = "操作成功", ModelStateDictionary modelState = null)
        {
            Data = data;
            StateCode = stateCode;
            Message = message;
            ErrorMessges = SetErrorMessges(modelState);
        }


        /// <summary>
        /// 正常返回的数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 状态码
        /// 0为成功，Message为操作成功的提示信息，
        /// 1-99需要下一步操作，在API返回值中定义并说明
        /// 100需提取ModelState中的错误信息，
        /// 其他提取Message（此时为错误信息）
        /// </summary>
        public int StateCode { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// ErrorMessges //Todo:错误信息Json格式过于复杂，需转换一下
        /// </summary>
        public Dictionary<string, string> ErrorMessges { get; set; }

        /// <summary>
        /// 根据ModelState设置ErrorMessges
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public Dictionary<string, string> SetErrorMessges(ModelStateDictionary modelState)
        {
            if (modelState != null && !modelState.IsValid)
            {
                Dictionary<string, string> errs = new Dictionary<string, string>();
                foreach (var key in modelState.Keys)
                {
                    var errMessage = string.Join(" ", modelState[key].Errors.Select(e => e.ErrorMessage).ToList());
                    if (!string.IsNullOrEmpty(errMessage))
                    {
                        var errkey = key.Contains(".") ? key.Substring(key.LastIndexOf(".") + 1) : key;
                        errs.Add(errkey, errMessage);
                    }
                }
                return errs;
            }
            return null;
        }
    }
}
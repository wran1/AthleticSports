using Models.WebsiteManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Web.Areas.Platform.Models
{
    /// <summary>
    /// 回复编辑
    /// </summary>
    public class ConsultReplayModel
    {
        //[Key]
        //[ScaffoldColumn(false)]
        //public string id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(20)]
        public string UserName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [RegularExpression("1[34578][0-9]{9}")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [DataType(DataType.Html)]
        public string Content { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        [DataType(DataType.Html)]
        public string ReplyContent { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? ReplyDateTime { get; set; }
    }
}
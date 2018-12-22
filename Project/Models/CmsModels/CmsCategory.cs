using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CmsModels
{
    /// <summary>
    /// 栏目
    /// </summary>
    public class CmsCategory : DbSetBase
    {
        public CmsCategory()
        {
            SystemId = "000";
            Enable = true;
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 系统编号
        /// </summary>
        [MaxLength(30)]
        [Required]
        public string SystemId { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        [MaxLength(100)]
        public string ViewName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [MaxLength(100)]
        public string Ico { get; set; }

        public bool Enable { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public bool Picture { get; set; }

        /// <summary>
        /// 视频
        /// </summary>
        public bool Video { get; set; }

        /// <summary>
        /// 音频
        /// </summary>
        public bool Audio { get; set; }

        /// <summary>
        /// 图集
        /// </summary>
        public bool Album { get; set; }

        /// <summary>
        /// 视频附件
        /// </summary>
        public bool VideoAttachFile { get; set; }

        /// <summary>
        /// 声音附件
        /// </summary>
        public bool VoiceAttachFile { get; set; }

        /// <summary>
        /// 子栏目
        /// </summary>
        public bool SubColumn { get; set; }


        /// <summary>
        /// 卷首别名
        /// </summary>
        public bool Byname { get; set; }
        /// <summary>
        /// 卷首小图标
        /// </summary>
        public bool ByImage { get; set; }
        ///// <summary>
        ///// 外链
        ///// </summary>
        public bool Externallinks { get; set; }
    }
}

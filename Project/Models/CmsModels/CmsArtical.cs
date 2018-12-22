using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CmsModels
{
    /// <summary>
    /// Cms文章
    public class CmsArtical : DbSetBase
    {
        public CmsArtical()
        {
            PublishTime = DateTime.UtcNow.AddHours(8);
            CmsArticalHits = new List<CmsArticalHit>();
            Enable = true;
        }

        /// <summary>
        /// 栏目Id
        /// </summary>
        [ForeignKey("CmsCategory")]
        [MaxLength(128)]
        public string CmsCategoryId { get; set; }
        [ScaffoldColumn(false)]
        public virtual CmsCategory CmsCategory { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        [MaxLength(200)]
        public string Subtitle { get; set; }

        /// <summary>
        /// 卷首别名
        /// </summary>
        [MaxLength(200)]
        public string Byname { get; set; }
        /// <summary>
        /// 卷首小图标
        /// </summary>
        [MaxLength(100)]
        [DataType("Image")]
        public string ByImage { get; set; }

        /// <summary>
        /// 封面图
        /// </summary>
        [MaxLength(100)]
        [DataType("Image")]
        public string CoverImage { get; set; }

        /// <summary>
        /// 图集
        /// </summary>
        [DataType("Files")]
        [MaxLength]
        public string ImgAlbum { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [MaxLength(500)]
        [DataType(DataType.MultilineText)]
        public string Abstract { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [MaxLength(100)]
        public string Keywords { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? PublishTime { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [MaxLength(100)]
        public string Author { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        [MaxLength(100)]
        public string Post { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [MaxLength(100)]
        public string Sourse { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [DataType(DataType.Html)]
        public string Content { get; set; }

        /// <summary>
        /// 视频url
        /// </summary>
        [MaxLength(200)]
        public string VideoUrl { get; set; }

        /// <summary>
        /// 音频url
        /// </summary>
        [MaxLength(200)]
        public string AudioUrl { get; set; }

        /// <summary>
        /// 视频附件
        /// </summary>
        [MaxLength]
        [DataType("Files")]
        public string VideoAttachFile { get; set; }
        /// <summary>
        /// 声音附件
        /// </summary>
        [MaxLength]
        [DataType("Files")]
        public string VoiceAttachFile { get; set; }
        /// <summary>
        /// 外链
        /// </summary>
        public string Externallinks { get; set; }

        public bool Enable { get; set; }

        /// <summary>
        /// 置顶
        /// </summary>
        public bool IsTop { get; set; }
        /// <summary>
        /// 热门
        /// </summary>
        public bool IsHot { get; set; }
        /// <summary>
        /// 最新标签
        /// </summary>
        public bool IsNew { get; set; }


        [ScaffoldColumn(false)]
        public virtual ICollection<CmsArticalHit> CmsArticalHits { get; set; }

    }

    public class CmsArticalHit : DbSetBase
    {

    }
}

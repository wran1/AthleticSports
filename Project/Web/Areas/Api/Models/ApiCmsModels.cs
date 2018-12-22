using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Areas.Api.Models
{
    /// <summary>
    /// 卷首
    /// </summary>
    public class FrontispieceModel
    {

        public string Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
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
        /// 封面图
        /// </summary>
        [MaxLength(100)]
        public string CoverImage { get; set; }
        /// <summary>
        /// 小图标
        /// </summary>
        [MaxLength(100)]
        public string ByImage { get; set; }
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

        ///// <summary>
        ///// 视频url
        ///// </summary>
        //[MaxLength(200)]
        //public string VideoUrl { get; set; }

        /// <summary>
        /// 外链
        /// </summary>
        [MaxLength(200)]
        public string Externallinks { get; set; }

        /// <summary>
        /// 视频
        /// </summary>
        [MaxLength]
        public string VideoAttachFile { get; set; }

        /// <summary>
        /// 声音
        /// </summary>
        [MaxLength]
        public string VoiceAttachFile { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? PublishTime { get; set; }
        /// <summary>
        /// 置顶
        /// </summary>
        public bool IsTop { get; set; }
        ///// <summary>
        ///// 热门
        ///// </summary>
        //public bool IsHot { get; set; }
        ///// <summary>
        ///// 最新标签
        ///// </summary>
        //public bool IsNew { get; set; }

    }
    /// <summary>
    /// 往期回顾
    /// </summary>
    public class ToReviewModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 卷首别名
        /// </summary>
        [MaxLength(200)]
        public string Byname { get; set; }
        /// <summary>
        /// 卷首小图标
        /// </summary>
        [MaxLength(100)]
        public string ByImage { get; set; }
        /// <summary>
        /// 年
        /// </summary>
        public DateTime? Year { get; set; }

    }
    /// <summary>
    /// 栏目
    /// </summary>
    public class TypeClassModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }
    /// <summary>
    /// 专属福利首页
    /// </summary>
    public class ExclusiveModel
    {
        /// <summary>
        /// 类型id
        /// </summary>
        public string typeid { get; set; }
        /// <summary>
        /// 信息id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图片，第一条显示图片
        /// </summary>
        [MaxLength(100)]
        public string Image { get; set; }
        /// <summary>
        /// 外链
        /// </summary>
        public string Externallinks { get; set; }
        public string UpdateTime { get; set; }
    }
    /// <summary>
    /// 专属福利二级页
    /// </summary>
    public class serviceCooperationModel
    {
        public string Id { get; set; }
        /// <summary>
        /// 大标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 小标题
        /// </summary>
        public string Subtitle { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Abstract { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 图片，第一条显示图片
        /// </summary>
        [MaxLength(100)]
        public string Image { get; set; }
        /// <summary>
        /// 外链
        /// </summary>
        public string Externallinks { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string Creatdatetime { get; set; }
    }
    /// <summary>
    /// 数字声音/风尚列表
    /// </summary>
    public class SubColumnTop
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 封面图
        /// </summary>
        [MaxLength(100)]
        public string Image { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? PublishTime { get; set; }
    }
}
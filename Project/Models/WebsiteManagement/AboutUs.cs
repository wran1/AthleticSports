using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.WebsiteManagement
{
    /// <summary>
    /// 关于我们类型
    /// </summary>
    public enum AboutUsType
    {
        泰达简介 = 0,
        泰达金融 = 1,
        中心简介 = 2,
        部门职能 = 3,
    }

    /// <summary>
    /// 关于我们
    /// </summary>
    public class AboutUs : DbSetBase
    {
        public AboutUs()
        {
            Enable = true;
            AboutUsHits = new List<AboutUsHit>();
        }

        /// <summary>
        /// 关于我们类型
        /// </summary>
        public AboutUsType AboutUsType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

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

        public bool Enable { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<AboutUsHit> AboutUsHits { get; set; }
    }

    /// <summary>
    /// 点击量
    /// </summary>
    public class AboutUsHit : DbSetBase
    {

    }
}

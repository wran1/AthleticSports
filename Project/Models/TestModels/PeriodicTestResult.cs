using Models.SysModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.TestModels
{
    /// <summary>
    /// 生理生化指标
    /// </summary>
    public class PeriodicTestResult : DbSetBase
    {
       
        /// <summary>
        /// 白细胞
        /// </summary>
        public double Leukocyte { get; set; }
        /// <summary>
        /// 红细胞
        /// </summary>
        public double Erythrocyte { get; set; }
        /// <summary>
        /// 中性粒细胞
        /// </summary>
        public double Neutrophils { get; set; }
        /// <summary>
        /// 淋巴细胞
        /// </summary>
        public double Lymphocyte { get; set; }
        /// <summary>
        /// 血红蛋白
        /// </summary>
        public double Hemoglobin { get; set; }

        /// <summary>
        /// 血球压积
        /// </summary>
        public double Hematocrit { get; set; }
        /// <summary>
        /// 血尿素
        /// </summary>
        public double BloodUrea  { get; set; }
        /// <summary>
        /// 肌酸激酶
        /// </summary>
        public double CreatineKinase  { get; set; }
        /// <summary>
        /// 皮质醇
        /// </summary>
        public double Cortisol { get; set; }
        /// <summary>
        /// 睾酮
        /// </summary>
        public double Testosterone { get; set; }

        ////用户标记
        //public string Sign { get; set; }
        /// <summary>
        /// 测试时间
        /// </summary>
        public string Testdate { get; set; }

        [ForeignKey("SysUser")]
        [Required]
        public string SysUserId { get; set; }

        public virtual SysUser SysUser { get; set; }

    }
    public class BodyComposition : DbSetBase
    {
        //体脂率
        public double BF { get; set; }
        /// <summary>
        /// 总质量=肌肉+脂肪+骨矿物盐
        /// </summary>
        public double TotalMass { get; set; }
        /// <summary>
        /// 肌肉
        /// </summary>
        public double Muscle { get; set; }
        /// <summary>
        /// 脂肪
        /// </summary>
        public double Fat { get; set; }
        /// <summary>
        /// 骨矿物盐
        /// </summary>
        public double BoneMSalt { get; set; }
        [DataType(DataType.DateTime)]
        public string Testdate { get; set; }

        [ForeignKey("SysUser")]
        [Required]
        public string SysUserId { get; set; }

        public virtual SysUser SysUser { get; set; }

    }
}

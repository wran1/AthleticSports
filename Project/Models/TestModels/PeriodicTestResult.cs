using System;
using System.Collections.Generic;
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
        //体脂率
        public double BF { get; set; }
        /// <summary>
        /// 瘦体重
        /// </summary>
        public double LeanWeight { get; set; }
        /// <summary>
        /// 骨密度
        /// </summary>
        public double BoneDensity { get; set; }
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
        
    }
}

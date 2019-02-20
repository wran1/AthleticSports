using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Api.Models
{
    /// <summary>
    /// 体脂率Model
    /// </summary>
    public class BodyFatModel
    {
        /// <summary>
        /// 体脂率
        /// </summary>
        public double BF { get; set; }
        /// <summary>
        /// 总质量
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
        /// <summary>
        /// 时间
        /// </summary>
        public string Testdate { get; set; }
    }
    public class PeriodicModel
    {
        /// <summary>
        /// 睾酮/皮质醇
        /// </summary>
        public double Ratio { get; set; }
        /// <summary>
        /// 睾酮
        /// </summary>
        public double Testosterone { get; set; }
        /// <summary>
        /// 测试时间
        /// </summary>
        public string Testdate { get; set; }
    }
    public class ExerciseIntensityTrendModel
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string Testdate { get; set; }
        /// <summary>
        /// 强度平均值（柱状）
        /// </summary>
        public double AvgIntensity { get; set; }
        /// <summary>
        /// 强度标准差（工行）
        /// </summary>
        public double StandardIntensity { get; set; }
        /// <summary>
        /// 强度比
        /// </summary>
        public double RatioIntensity { get; set; }
        
    }
}
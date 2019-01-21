using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.TestModels
{
    /// <summary>
    /// 主观评测表
    /// </summary>
    public class SubjectiveTest : DbSetBase
    {
        /// <summary>
        /// 晨脉
        /// </summary>
        public int MorPulse { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public double Weight { get; set; }
        /// <summary>
        /// 睡眠时间
        /// </summary>
        public double SleepDuration { get; set; }
        /// <summary>
        /// 睡眠质量
        /// </summary>
        public int SleepQuality { get; set; }
        /// <summary>
        /// 饮食欲望
        /// </summary>
        public int Desire { get; set; }
        //酸痛指数
        public int SorenessLevel { get; set; }
        //伤病疼痛指数
        public int FatigueLevel { get; set; }
        //训练状态评价
        public int TrainStatus { get; set; }
        //自我感觉疲劳度
        public int Fatigue { get; set; }
        //训练时间
        public int Minute { get; set; }
        //训练强度
        public int TrainIntensity { get; set; }
        //教练评价
        public int Evaluate { get; set; }
    }
}

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
        //体能训练时长
        public int FitnessMinute { get; set; }
        //专项训练时长
        public int SpecialMinute { get; set; }
        //比赛时长
        public int MatchMinute { get; set; }
        //训练强度
        public int TrainIntensity { get; set; }
        //教练评价
        public int Evaluate { get; set; }
        /// <summary>
        /// 队医记录
        /// </summary>
        public string DoctorRecord { get; set; }
        
        //时间()
        public string DateSign { get; set; }

        [ForeignKey("SysUser")]
        [Required]
        public string SysUserId { get; set; }

        public virtual SysUser SysUser { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<PainPoint> PainPoints { get; set; }
    }
    public enum PointName
    {
        头,
        肩,
        胸,
        腹部,
        髋,
        内收肌,
        股四头肌,
        膝,
        小腿前部,
        足,
        颈,
        上背部,
        下背部,
        臀部,
        腘绳肌,
        小腿后群,
    }
    public class PainPoint : DbSetBase
    {
        /// <summary>
        /// 疼痛位置
        /// </summary>
        public PointName PointName { get; set; }

        [ForeignKey("SubjectiveTest")]
        [Required]
        public string SubjectiveTestId { get; set; }

        public virtual SubjectiveTest SubjectiveTest { get; set; }

    }
}

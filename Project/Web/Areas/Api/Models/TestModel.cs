﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.Areas.Api.Models
{
    public class SubjectTestModel
    {
        public string Id { get; set; }
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
        /// <summary>
        /// 酸痛指数
        /// </summary>
        public int SorenessLevel { get; set; }
        //伤病疼痛指数
        /// <summary>
        /// 伤病疼痛指数
        /// </summary>
        public int FatigueLevel { get; set; }
        //训练状态评价
        /// <summary>
        /// 训练状态评价
        /// </summary>
        public int TrainStatus { get; set; }
        //自我感觉疲劳度
        /// <summary>
        /// 自我感觉疲劳度
        /// </summary>
        public int Fatigue { get; set; }
        /// <summary>
        /// 体能训练时长
        /// </summary>
        public int FitnessMinute { get; set; }
        /// <summary>
        /// 专项训练时长
        /// </summary>
        public int SpecialMinute { get; set; }
        //比赛时长
        /// <summary>
        /// 比赛时长
        /// </summary>
        public int MatchMinute { get; set; }
        //训练强度
        public int TrainIntensity { get; set; }
        /// <summary>
        /// 教练评价
        /// </summary>
        public int Evaluate { get; set; }
        /// <summary>
        /// 队医记录
        /// </summary>
        public string DoctorRecord { get; set; }
        public string DateSign { get; set; }
    }
    /// <summary>
    /// 体能训练指标
    /// </summary>
    public class TrainNameModel
    {
        public string TrainId { get; set; }
        public string Name { get; set; }
    }
    public class TrainResultModel
    {
        /// <summary>
        /// 项目id
        /// </summary>
        public string TrainId { get; set; }
        /// <summary>
        /// 项目值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 评测时间
        /// </summary>
        [DataType(DataType.DateTime)]
        public string TestDate { get; set; }

    }
    public class PointNames
    {
        public string PointName { get; set; }
    }
    public class PointList
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string DateSign { get; set; }
        /// <summary>
        /// 主观评测id
        /// </summary>
        public string SubjectiveTestId { get; set; }
        /// <summary>
        /// 疼痛部位
        /// </summary>

        public virtual ICollection<PointNames> PointNames { get; set; }
    }
    public class DoctorRecordModel
    {
        public string Id { get; set; }
        public string Record { get; set; }
        public string Date { get; set; }
    }
    /// <summary>
    /// 体能测试Model
    /// </summary>
    public class AllTrainModel
    {
        /// <summary>
        /// 测试项目
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 测试值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 测试日期
        /// </summary>
        [DataType(DataType.DateTime)]
        public string TestDate { get; set; }

    }
}
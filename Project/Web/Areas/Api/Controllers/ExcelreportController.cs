using DoddleReport;
using IServices.ISysServices;
using IServices.ITestServices;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

using NPOI.HSSF.UserModel;
using Web.Areas.Api.Models;
using System.Linq.Expressions;
using Models.TestModels;
using System.Data;
using System.Reflection;
using IServices.Infrastructure;

namespace Web.Areas.Api.Controllers
{
    [Authorize]
    [RoutePrefix("API/Excelreport")]
    public class ExcelreportController : ApiController
    {
        private readonly IUserInfo _iUserInfo;
        private readonly ISubjectiveTestService _isubjectiveTestService;
        private readonly IPeriodicTestResultService _iPeriodicTestResultService;
        private readonly IBodyCompositionService _iBodyCompositionService;
        private readonly IExportRecordService _iExportRecordService;
        private readonly ITrainingPeopleService _iTrainingPeopleService;
        private readonly IUnitOfWork _iUnitOfWork;
        public ExcelreportController(ITrainingPeopleService iTrainingPeopleService,IUnitOfWork iUnitOfWork,IExportRecordService iExportRecordService,IBodyCompositionService iBodyCompositionService,IUserInfo iUserInfo, IPeriodicTestResultService iPeriodicTestResultService, ISubjectiveTestService isubjectiveTestService)
        {
            _iUserInfo = iUserInfo;
            _isubjectiveTestService = isubjectiveTestService;
            _iPeriodicTestResultService = iPeriodicTestResultService;
            _iBodyCompositionService = iBodyCompositionService;
            _iExportRecordService = iExportRecordService;
            _iTrainingPeopleService = iTrainingPeopleService;
            _iUnitOfWork = iUnitOfWork;
        }
        /// <summary>
        /// 主观评测导出
        /// </summary>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="userid">选择运动员id （英文逗号隔开）</param>
        /// <param name="projectName">选择的中文项目（英文逗号隔开）</param>
        /// <param name="projectValue">选择的英文文项目（英文逗号隔开，顺序要完全和中文保持一致）</param>
        /// <returns></returns>
        [HttpGet]
        [Route("SubjectiveExport")]
        public HttpResponseMessage  SubjectiveExport(string starttime, string endtime, string userid, string projectName, string projectValue)
        {
            //CompanyInfoName as 公司名称
            var model = _isubjectiveTestService.GetAll();
            if (!string.IsNullOrEmpty(starttime))
            {
                model = model.Where(a => a.DateSign.CompareTo(starttime) >= 0);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                model = model.Where(a => a.DateSign.CompareTo(endtime) <= 0);
            }
            if(!string.IsNullOrEmpty(userid))
            {
                userid = userid.Substring(0, userid.Length - 1);
                string[] sArray = userid.Split(',');
                string strName = "";
                var count = sArray.Count();
                if(count==1)
                {
                    strName = sArray[0];
                }
                else
                {
                    for(int i=1;i<count;i++)
                    {
                        strName = strName + "|| a.SysUserId == " + sArray[i];
                    }
                }
              
                model = model.Where(a => a.SysUserId == strName);
            }
            model = model.OrderBy(a => a.CreatedDate);
            var resultmodel = model.Select(a => new {
                a.SysUser.FullName,
                a.MorPulse,
                a.Weight,
                a.SleepDuration,
                a.SleepQuality,
                a.Desire,
                a.SorenessLevel,
                a.FatigueLevel,
                a.TrainStatus,
                a.Fatigue,
                a.TrainIntensity,
                a.Evaluate,
                a.DateSign
            });
            DataTable dtsubject = ToDataTable(resultmodel);

            _iExportRecordService.Save(null, new ExportRecord { SysArea = "主观评测" });
            _iUnitOfWork.CommitAsync();

            //创建Excel文件的对象  
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet  
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //sheet1.Equals(reportresult);/*.CreateAndFillSheet(workbook, lstRes);*/

            ICellStyle style = book.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            //style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;



            ICellStyle style4Header = book.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            style4Header.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style4Header.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            //新建一个字体样式对象
            IFont font = book.CreateFont();
            //设置字体加粗样式
            //font.IsBold = true;
            font.Boldweight = short.MaxValue;
            //使用SetFont方法将字体样式添加到单元格样式中 
            style4Header.SetFont(font);

            string[] pName = projectName.Split(',');
            IRow row1 = sheet1.CreateRow(0);
            ICell cell4Header = row1.CreateCell(0);
            for (int y = 0; y < pName.Count(); y++)
            {
                if (y == 0)
                {
                    cell4Header.CellStyle = style4Header;
                    cell4Header.SetCellValue(pName[0]);
                }
                else
                {
                    cell4Header = row1.CreateCell(y);
                    cell4Header.CellStyle = style4Header;
                    cell4Header.SetCellValue(pName[y]);
                }
            }
            for (int i = 0; i < model.Count(); i++)
            {
                IRow rowtemp = sheet1.CreateRow(i + 1);
                string[] pValue = projectValue.Split(',');
                for (int y = 0; y < pValue.Count(); y++)
                {

                    ICell cell = rowtemp.CreateCell(y);
                    rowtemp.CreateCell(y).SetCellValue(dtsubject.Rows[i][pValue[y]].ToString());
                    cell.CellStyle = style;
                    cell.SetCellValue(dtsubject.Rows[i][pValue[y]].ToString());
                }

            }
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
                         book.Write(stream);
                         stream.Seek(0, SeekOrigin.Begin);
                         book = null;
                         HttpResponseMessage mResult = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                         mResult.Content = new StreamContent(stream);
                         mResult.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                         mResult.Content.Headers.ContentDisposition.FileName =  "主观评价" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                       mResult.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");
            
             return mResult;

        }
        /// <summary>
        /// 生理生化导出
        /// </summary>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="userid">选择运动员id （英文逗号隔开）</param>
        /// <param name="projectName">选择的中文项目（英文逗号隔开）</param>
        /// <param name="projectValue">选择的英文文项目（英文逗号隔开，顺序要完全和中文保持一致）</param>
        /// <returns></returns>
        [HttpGet]
        [Route("PeriodicTestExport")]
        public HttpResponseMessage PeriodicTestExport(string starttime,string endtime,string userid,string projectName,string projectValue)
        {
            //CompanyInfoName as 公司名称
            var model = _iPeriodicTestResultService.GetAll();
            if (!string.IsNullOrEmpty(starttime))
            {
                model = model.Where(a => a.Testdate.CompareTo(starttime) >= 0);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                model = model.Where(a => a.Testdate.CompareTo(endtime) <= 0);
            }
            if (!string.IsNullOrEmpty(userid))
            {
                userid = userid.Substring(0, userid.Length - 1);
                string[] sArray = userid.Split(',');
                string strName = "";
                var count = sArray.Count();
                if (count == 1)
                {
                    strName = sArray[0];
                }
                else
                {
                    for (int i = 1; i < count; i++)
                    {
                        strName = strName + "|| a.SysUserId == " + sArray[i];
                    }
                }

                model = model.Where(a => a.SysUserId == strName);
            }
            model = model.OrderBy(a => a.CreatedDate);
            var resultmodel = model.Select(a => new {
                 a.SysUser.FullName,
                 a.BloodUrea,
                 a.Cortisol,
                 a.CreatineKinase,
                 a.Erythrocyte,
                 a.Hematocrit,
                 a.Hemoglobin,
                 a.Leukocyte,
                 a.Lymphocyte,
                 a.Neutrophils,
                 a.Testosterone,
                 a.Testdate
            });
            DataTable dtperiod= ToDataTable(resultmodel);
            _iExportRecordService.Save(null, new ExportRecord { SysArea = "生理生化" });
            _iUnitOfWork.CommitAsync();
            //创建Excel文件的对象  
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet  
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //sheet1.Equals(reportresult);/*.CreateAndFillSheet(workbook, lstRes);*/

            ICellStyle style = book.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            //style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            ICellStyle style4Header = book.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            style4Header.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style4Header.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            //新建一个字体样式对象
            IFont font = book.CreateFont();
            //设置字体加粗样式
            //font.IsBold = true;
            font.Boldweight = short.MaxValue;
            //使用SetFont方法将字体样式添加到单元格样式中 
            style4Header.SetFont(font);

            string[] pName = projectName.Split(',');
            IRow row1 = sheet1.CreateRow(0);
            ICell cell4Header = row1.CreateCell(0);
            for (int y = 0; y < pName.Count(); y++)
            {
                if (y == 0)
                {
                    cell4Header.CellStyle = style4Header;
                    cell4Header.SetCellValue(pName[0]);
                }
                else
                {
                    cell4Header = row1.CreateCell(y);
                    cell4Header.CellStyle = style4Header;
                    cell4Header.SetCellValue(pName[y]);
                }
            }
            for (int i = 0; i < model.Count(); i++)
            {
                IRow rowtemp = sheet1.CreateRow(i + 1);
                string[] pValue = projectValue.Split(',');
                for (int y = 0; y < pValue.Count(); y++)
                {
                   
                    ICell cell = rowtemp.CreateCell(y);
                    rowtemp.CreateCell(y).SetCellValue(dtperiod.Rows[i][pValue[y]].ToString());
                    cell.CellStyle = style;
                    cell.SetCellValue(dtperiod.Rows[i][pValue[y]].ToString());
                }

            }


            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            book.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            book = null;
            HttpResponseMessage mResult = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            mResult.Content = new StreamContent(stream);
            mResult.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            mResult.Content.Headers.ContentDisposition.FileName = "生理生化" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            mResult.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");

            return mResult;

        }

        /// <summary>
        /// 体成分导出
        /// </summary>
        /// <param name="starttime">开始时间</param>
        /// <param name="endtime">结束时间</param>
        /// <param name="userid">选择运动员id （英文逗号隔开）</param>
        /// <param name="projectName">选择的中文项目（英文逗号隔开）</param>
        /// <param name="projectValue">选择的英文文项目（英文逗号隔开，顺序要完全和中文保持一致）</param>
        /// <returns></returns>
        [HttpGet]
        [Route("BodyCompositionExport")]
        public HttpResponseMessage BodyCompositionExport(string starttime, string endtime, string userid, string projectName, string projectValue)
        {
            //CompanyInfoName as 公司名称
            var model = _iBodyCompositionService.GetAll();
            if (!string.IsNullOrEmpty(starttime))
            {
                model = model.Where(a => a.Testdate.CompareTo(starttime) >= 0);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                model = model.Where(a => a.Testdate.CompareTo(endtime) <= 0);
            }
            if (!string.IsNullOrEmpty(userid))
            {
                userid = userid.Substring(0, userid.Length - 1);
                string[] sArray = userid.Split(',');
                string strName = "";
                var count = sArray.Count();
                if (count == 1)
                {
                    strName = sArray[0];
                }
                else
                {
                    for (int i = 1; i < count; i++)
                    {
                        strName = strName + "|| a.SysUserId == " + sArray[i];
                    }
                }

                model = model.Where(a => a.SysUserId == strName);
            }
            model = model.OrderBy(a => a.CreatedDate);
            var resultmodel = model.Select(a => new {
                a.SysUser.FullName,
                a.BF,
                a.Muscle,
                a.Fat,
                a.BoneMSalt,
                a.TotalMass,
                a.Testdate,
               
            });
            DataTable dtsubject = ToDataTable(resultmodel);

            _iExportRecordService.Save(null, new ExportRecord { SysArea = "体成分" });
            _iUnitOfWork.CommitAsync();

            //创建Excel文件的对象  
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet  
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //sheet1.Equals(reportresult);/*.CreateAndFillSheet(workbook, lstRes);*/

            ICellStyle style = book.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            //style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;



            ICellStyle style4Header = book.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            style4Header.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style4Header.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            //新建一个字体样式对象
            IFont font = book.CreateFont();
            //设置字体加粗样式
            //font.IsBold = true;
            font.Boldweight = short.MaxValue;
            //使用SetFont方法将字体样式添加到单元格样式中 
            style4Header.SetFont(font);

            string[] pName = projectName.Split(',');
            IRow row1 = sheet1.CreateRow(0);
            ICell cell4Header = row1.CreateCell(0);
            for (int y = 0; y < pName.Count(); y++)
            {
                if (y == 0)
                {
                    cell4Header.CellStyle = style4Header;
                    cell4Header.SetCellValue(pName[0]);
                }
                else
                {
                    cell4Header = row1.CreateCell(y);
                    cell4Header.CellStyle = style4Header;
                    cell4Header.SetCellValue(pName[y]);
                }
            }
            for (int i = 0; i < model.Count(); i++)
            {
                IRow rowtemp = sheet1.CreateRow(i + 1);
                string[] pValue = projectValue.Split(',');
                for (int y = 0; y < pValue.Count(); y++)
                {

                    ICell cell = rowtemp.CreateCell(y);
                    rowtemp.CreateCell(y).SetCellValue(dtsubject.Rows[i][pValue[y]].ToString());
                    cell.CellStyle = style;
                    cell.SetCellValue(dtsubject.Rows[i][pValue[y]].ToString());
                }

            }
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            book.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            book = null;
            HttpResponseMessage mResult = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            mResult.Content = new StreamContent(stream);
            mResult.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            mResult.Content.Headers.ContentDisposition.FileName = "体成分" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            mResult.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");

            return mResult;

        }
        /// <summary>
        /// 体能数据导出
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="userid"></param>
        /// <param name="projectValue">传体能项的id（英文逗号隔开，顺序要完全和中文保持一致）</param>
        /// <returns></returns>
        [HttpGet]
        [Route("TrainingPeopleExport")]
        public HttpResponseMessage TrainingPeopleExport(string starttime, string endtime, string userid, string projectValue)
        {
            var model = _iTrainingPeopleService.GetAll();
            if (!string.IsNullOrEmpty(starttime))
            {
                model = model.Where(a => a.TestDate.CompareTo(starttime) >= 0);
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                model = model.Where(a => a.TestDate.CompareTo(endtime) <= 0);
            }
            if (!string.IsNullOrEmpty(userid))
            {
                userid = userid.Substring(0, userid.Length - 1);
                string[] sArray = userid.Split(',');
                foreach (var item in sArray)
                {
                    model = model.Where(a => a.SysUserId == item);
                }
            }
            if (!string.IsNullOrEmpty(projectValue))
            {
                projectValue = projectValue.Substring(0, userid.Length - 1);
                string[] sArray = projectValue.Split(',');
                string strName = "";
                var count = sArray.Count();
                if (count == 1)
                {
                    strName = sArray[0];
                }
                else
                {
                    for (int i = 1; i < count; i++)
                    {
                        strName = strName + "|| a.TrainingTypeId == " + sArray[i];
                    }
                }

                model = model.Where(a => a.SysUserId == strName);
            }
            model = model.OrderBy(a => a.TestDate).ThenBy(a => a.SysUserId);
            var resultmodel = model.Select(a => new
            {
                a.SysUser.FullName,
                a.TrainingType.Name,
                a.Value,
                a.TestDate,
            }).OrderBy(a=>a.FullName).ThenBy(a=>a.TestDate).ToList();

            _iExportRecordService.Save(null, new ExportRecord { SysArea = "体能训练" });
            _iUnitOfWork.CommitAsync();

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            ICellStyle style = book.CreateCellStyle();
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            ICellStyle style4Header = book.CreateCellStyle();
            style4Header.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style4Header.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

            //新建一个字体样式对象
            IFont font = book.CreateFont();
            font.Boldweight = short.MaxValue;
            style4Header.SetFont(font);

            IRow row1 = sheet1.CreateRow(0);
            ICell cell4Header = row1.CreateCell(0);
            cell4Header.CellStyle = style4Header;
            cell4Header.SetCellValue("姓名");
            cell4Header = row1.CreateCell(1);
            cell4Header.CellStyle = style4Header;
            cell4Header.SetCellValue("测试时间");
            cell4Header = row1.CreateCell(2);
            cell4Header.CellStyle = style4Header;
            cell4Header.SetCellValue("测试项目");
            cell4Header = row1.CreateCell(3);
            cell4Header.CellStyle = style4Header;
            cell4Header.SetCellValue("测试值");
            //string[] pName = projectName.Split(',');
            //IRow row1 = sheet1.CreateRow(0);
            //ICell cell4Header = row1.CreateCell(0);
            //for (int y = 0; y < pName.Count(); y++)
            //{
            //    if (y == 0)
            //    {
            //        cell4Header.CellStyle = style4Header;
            //        cell4Header.SetCellValue(pName[0]);
            //    }
            //    else
            //    {
            //        cell4Header = row1.CreateCell(y);
            //        cell4Header.CellStyle = style4Header;
            //        cell4Header.SetCellValue(pName[y]);
            //    }
            //}

            for (int i = 0; i < resultmodel.Count(); i++)
            {
                IRow rowtemp = sheet1.CreateRow(i + 1);
                //string[] pValue = projectValue.Split(',');
                //for (int y = 0; y < pValue.Count(); y++)
                //{

                //    ICell cell = rowtemp.CreateCell(y);
                //    rowtemp.CreateCell(y).SetCellValue(dtsubject.Rows[i][pValue[y]].ToString());
                //    cell.CellStyle = style;
                //    cell.SetCellValue(dtsubject.Rows[i][pValue[y]].ToString());
                //}
                ICell cell = rowtemp.CreateCell(0);
                cell.CellStyle = style;
                cell.SetCellValue(resultmodel[i].FullName);
                ICell cell1 = rowtemp.CreateCell(1);
                cell1.CellStyle = style;
                cell1.SetCellValue(resultmodel[i].TestDate);
                ICell cell2 = rowtemp.CreateCell(2);
                cell2.CellStyle = style;
                cell2.SetCellValue(resultmodel[i].Name);
                ICell cell3 = rowtemp.CreateCell(3);
                cell3.CellStyle = style;
                cell3.SetCellValue(resultmodel[i].Value);
            }
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            book.Write(stream);
            stream.Seek(0, SeekOrigin.Begin);
            book = null;
            HttpResponseMessage mResult = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            mResult.Content = new StreamContent(stream);
            mResult.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            mResult.Content.Headers.ContentDisposition.FileName = "体能训练" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            mResult.Content.Headers.ContentType = new MediaTypeHeaderValue("application/ms-excel");

            return mResult;


        }
        /// <summary>
        /// LINQ返回DataTable类型
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="varlist"> </param>
        /// <returns> </returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names
            PropertyInfo[] oProps = null;
            if (varlist == null)
                return dtReturn;
            foreach (T rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;
                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

    }


}

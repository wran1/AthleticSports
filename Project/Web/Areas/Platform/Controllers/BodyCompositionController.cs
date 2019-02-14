using Common;
using IServices.Infrastructure;
using IServices.ISysServices;
using IServices.ITestServices;
using Models.TestModels;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Platform.Helpers;
using Web.Helpers;

namespace Web.Areas.Platform.Controllers
{
    public class BodyCompositionController : Controller
    {
        private readonly IBodyCompositionService _bodyCompositionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISysUserService _sysUserService;
        public BodyCompositionController(ISysUserService sysUserService,IUnitOfWork unitOfWork, IBodyCompositionService bodyCompositionService)
        {
            _unitOfWork = unitOfWork;
            _bodyCompositionService = bodyCompositionService;
            _sysUserService = sysUserService;
        }
        // GET: Platform/BodyComposition
        public ActionResult Index(string keyword, string ordering, int pageIndex = 1)
        {
            var model =
                 _bodyCompositionService.GetAll()
                                   .Select(
                                       a =>
                                       new
                                       {
                                           a.SysUser.FullName,
                                           a.BF,
                                           a.Weight,
                                           a.Muscle,
                                           a.Fat,
                                           a.BoneMSalt,
                                           a.Testdate,
                                           a.Id,
                                       }).Search(keyword);

            if (!string.IsNullOrEmpty(ordering))
            {
                model = model.OrderBy(ordering, null);
            }

            return View(model.ToPagedList(pageIndex));
        }
        //导入，未完成
        public async Task<ActionResult> Import()
        {
            try
            {
                HttpPostedFileBase file = Request.Files["file"];//接收客户端传递过来的数据.
                if (file == null)
                {
                    return Content("请选择上传的Excel文件");
                }
                else
                {
                    //对文件的格式判断，此处省略
                    //PeriodicTestResult db = new PeriodicTestResult();//EF上下文对象
                    Stream inputStream = file.InputStream;
                    HSSFWorkbook hssfworkbook = new HSSFWorkbook(inputStream);
                    NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
                    // IRow headerRow = sheet.GetRow(0);//第一行为标题行
                    // int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
                    int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1

                    for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        BodyComposition model = new BodyComposition();

                        if (row != null)
                        {
                            if (row.GetCell(0) != null)
                            {
                                var username =GetCellValue(row.GetCell(0));
                                var user= _sysUserService.GetAll(a => a.UserName == username).FirstOrDefault();
                                if(user!=null)
                                {
                                    model.SysUserId = user.Id;
                                    if (row.GetCell(1) != null)
                                    {
                                        model.Testdate =DateTime.Parse(GetCellValue(row.GetCell(1))).ToString("yyyy-MM-dd");
                                    }
                                    if (row.GetCell(2) != null)
                                    {
                                        model.BF = double.Parse(GetCellValue(row.GetCell(2)));
                                    }
                                    if (row.GetCell(3) != null)
                                    {
                                        model.Muscle = double.Parse(GetCellValue(row.GetCell(3)));
                                    }
                                    if (row.GetCell(4) != null)
                                    {
                                        model.Fat = double.Parse(GetCellValue(row.GetCell(4)));
                                    }
                                    if (row.GetCell(5) != null)
                                    {
                                        model.BoneMSalt = double.Parse(GetCellValue(row.GetCell(5)));
                                    }
                                    model.Weight = model.Muscle + model.Fat + model.BoneMSalt;
                                    _bodyCompositionService.Save(null, model);

                                }
                            }
                           
                        }
                       

                    }
                    await _unitOfWork.CommitAsync();
                    return RedirectToAction("Index");
        
                }

            }
            catch (Exception e)
            {

                return Content(e.ToString());
            }
        }
        /// <summary>
        /// 根据Excel列类型获取列的值
        /// </summary>
        /// <param name="cell">Excel列</param>
        /// <returns></returns>
        private static string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(object id)
        {
            _bodyCompositionService.Delete(id);
            await _unitOfWork.CommitAsync();
            return new DeleteSuccessResult();
        }
    }
}
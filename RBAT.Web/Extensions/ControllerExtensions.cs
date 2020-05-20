using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RBAT.Core.Clasess;
using RBAT.Core.Models;
using RBAT.Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using RBAT.Logic;
using System.IO;
using OfficeOpenXml;

namespace RBAT.Web.Extensions
{
    public static class ControllerExtensions
    {       
        public async static Task<JsonResult> ReadFromExcel(this Controller timeComponentController, IFormFile fileSelect, int projectID, int elementID, DateTime startDate, TimeComponent timeComponent, ITimeComponentService service, ILogger logger)
        {
            var filePath = Path.GetTempFileName();
            string fileExtension = Path.GetExtension(fileSelect.FileName).ToLower();

            var fileLocation = new FileInfo(filePath);

            try
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    await fileSelect.CopyToAsync(stream);
                }
                using (ExcelPackage package = new ExcelPackage(fileLocation))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                    int totalRows = workSheet.Dimension.Rows;

                    var dataList = new List<PastedTimeComponent>();

                    try
                    {
                        for (int i = 1; i <= totalRows; i++)
                        {
                            if (timeComponent == TimeComponent.Custom)
                            {
                                if (workSheet.Cells[i, 3].Value != null || workSheet.Cells[i, 1].Value == null || workSheet.Cells[i, 2].Value == null)
                                {
                                    throw new Exception("Please check your data. You must provide two columns of data.");
                                }
                                dataList.Add(new PastedTimeComponent
                                {
                                    TimeStep = int.Parse(workSheet.Cells[i, 1].Value.ToString()),
                                    Value = double.Parse(workSheet.Cells[i, 2].Value.ToString())
                                });
                            }
                            else
                            {
                                if (workSheet.Cells[i, 2].Value != null)
                                {
                                    throw new Exception("Please check your data. You must provide only one column of data.");
                                }
                                dataList.Add(new PastedTimeComponent
                                {
                                    Value = double.Parse(workSheet.Cells[i, 1].Value.ToString())
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is FormatException)
                        {
                            throw new Exception("Please check your data. Only numbers can be imported.");
                        }
                        else if (ex is ArgumentNullException)
                        {
                            throw new Exception($"Please check your data. You must provide {(timeComponent == TimeComponent.Custom ? "two columns" : "only one column")} of data.");
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    await service.SaveAllFromFile(elementID, startDate, timeComponent, dataList, projectID);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return new JsonResult(new { message = ex.Message });
            }

            finally
            {
                if (System.IO.File.Exists(fileLocation.FullName))
                {
                    System.IO.File.Delete(fileLocation.FullName);
                }
            }
            return new JsonResult(new { ok = true });
        }
    }
}

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace CameoMvc.Models
{
    public class M_OfficeExcel
    {
        //判斷Office Version
        private static bool isXlsx(string fileName)
        {
            if (Path.GetExtension(fileName).ToLower().Trim() == ".xlsx") return true;
            else return false;
        }

        #region DataTable to Excel
        ///  <summary> 
        /// 由DataTable導出Excel 
        ///  < /summary> 
        ///  <param name="sourceTable"> 要導出數據的DataTable </param> 
        ///  <param name="fileName"> 指定Excel工作表名稱</param> 
        public static void ExportDataTableToExcel(DataTable sourceTable, string fileName, string sheetName)
        {
            ExportDataTableToExcel(sourceTable, fileName, sheetName, false);
        }
        ///  <summary> 
        /// 由DataTable導出Excel 
        ///  < /summary> 
        ///  <param name="sourceTable"> 要導出數據的DataTable </param> 
        ///  <param name="fileName"> 指定Excel工作表名稱</param> 
        /// <param name="isAutoSizeColumn">是否自動展開欄位寬度</param>
        public static void ExportDataTableToExcel(DataTable sourceTable, string fileName, string sheetName, bool isAutoSizeColumn)
        {
            MemoryStream ms = null;
            try
            {
                if (isXlsx(fileName))
                    ms = ExportDataTableToExcel(sourceTable, sheetName, true, isAutoSizeColumn) as MemoryStream;
                else ms = ExportDataTableToExcel(sourceTable, sheetName, false, isAutoSizeColumn) as MemoryStream;
                HttpContext.Current.Response.AddHeader("Accept-Language", "zh-tw");
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + (char)34 + System.Web.HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + (char)34);
                HttpContext.Current.Response.AddHeader("Content-Length", ms.ToArray().Length.ToString());
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //the belowing line will must be exception. so change to CompleteRequest.
                //HttpContext.Current.Response.End();
                ms.Close();
                ms = null;
            }
            catch (Exception exp) { }
        }
        ///  <summary> 
        /// 由DataTable導出Excel 
        ///  </summary> 
        ///  <param name="sourceTable"> 要導出數據的DataTable </param> 
        ///  <param name="sheetName"> Excel工作表名稱</param>
        ///  <param name="isXlsx"> true:office2007(含)以上版本</param> 
        ///  <returns> Excel Stream< /returns> 
        public static Stream ExportDataTableToExcel(DataTable SourceTable, string sheetName, bool isXlsx)
        {
            return ExportDataTableToExcel(SourceTable, sheetName, isXlsx, false);
        }
        ///  <summary> 
        /// 由DataTable導出Excel 
        ///  </summary> 
        ///  <param name="sourceTable"> 要導出數據的DataTable </param> 
        ///  <param name="sheetName"> Excel工作表名稱</param>
        ///  <param name="isXlsx"> true:office2007(含)以上版本</param> 
        /// <param name="isAutoColumn">是否自動展開欄位寬度</param>
        ///  <returns> Excel Stream< /returns> 
        public static Stream ExportDataTableToExcel(DataTable SourceTable, string sheetName, bool isXlsx, bool isAutoSizeColumn)
        {
            IWorkbook workbook = null;
            MemoryStream ms = null;
            ISheet sheet = null;
            IRow headerRow = null, dataRow = null;
            ICellStyle cs = null;
            IFont font = null;
            string[] cnt = null;
            try
            {
                if (isXlsx)
                {
                    workbook = new XSSFWorkbook();
                    sheet = (XSSFSheet)workbook.CreateSheet(sheetName);
                    headerRow = (XSSFRow)sheet.CreateRow(0);
                }
                else
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet(sheetName);
                    headerRow = (HSSFRow)sheet.CreateRow(0);
                }

                ms = new MemoryStream();
                foreach (DataColumn column in SourceTable.Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

                    if (isXlsx) cs = (XSSFCellStyle)workbook.CreateCellStyle();
                    else cs = (HSSFCellStyle)workbook.CreateCellStyle();
                    //字體
                    if (isXlsx) { font = (XSSFFont)workbook.CreateFont(); font.Color = IndexedColors.RoyalBlue.Index; }
                    else { font = (HSSFFont)workbook.CreateFont(); font.Color = NPOI.HSSF.Util.HSSFColor.RoyalBlue.Index; }
                    font.FontName = "Yahei Consolas Hybrid";
                    cs.SetFont(font);
                    //背景顏色
                    if (isXlsx) cs.FillBackgroundColor = IndexedColors.Yellow.Index;
                    else cs.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                    //水平對齊
                    cs.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    //垂直對齊
                    cs.VerticalAlignment = VerticalAlignment.Center;

                    //將目前欄位的CellStyle設定為自動換行
                    if (column.ColumnName.Contains("\n"))
                    {
                        //自動換行
                        cs.WrapText = true;
                        //找出換行分隔
                        cnt = column.ColumnName.Split(Environment.NewLine.ToCharArray());
                        //變更行高
                        headerRow.HeightInPoints = (cnt.Length) * sheet.DefaultRowHeight / 20;
                    }
                    headerRow.GetCell(column.Ordinal).CellStyle = cs;
                }

                int rowIndex = 1;
                foreach (DataRow row in SourceTable.Rows)
                {
                    if (isXlsx) dataRow = (XSSFRow)sheet.CreateRow(rowIndex);
                    else dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in SourceTable.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());

                        //將目前欄位的CellStyle設定為自動換行
                        if (row[column].ToString().Contains("\n"))
                        {
                            cnt = row[column].ToString().Split(Environment.NewLine.ToCharArray());
                            if (isXlsx) cs = (XSSFCellStyle)workbook.CreateCellStyle();
                            else cs = (HSSFCellStyle)workbook.CreateCellStyle();

                            cs.WrapText = true;

                            dataRow.GetCell(column.Ordinal).CellStyle = cs;
                            //因為換行所以愈設幫他Row的高度變成兩倍
                            dataRow.HeightInPoints = (cnt.Length) * sheet.DefaultRowHeight / 20;
                        }
                    }
                    ++rowIndex;
                }
                //autosie the columns and rows
                if (isAutoSizeColumn)
                {
                    for (int i = 0; i <= SourceTable.Columns.Count; ++i)
                        sheet.AutoSizeColumn(i);
                }
                workbook.Write(ms);
                ms.Flush();
            }
            catch (Exception exp)
            {
                return null;
            }
            finally
            {
                ms.Close();
                sheet = null;
                headerRow = null;
                workbook = null;
            }
            return ms;
        }
        #endregion
        #region Excel to DataTable
        ///  <summary> 
        /// 由Excel導入DataTable 
        ///  </summary> 
        ///  <param name="excelFilePath"> Excel文件路徑，為物理路徑。</param> 
        ///  <param name="sheetName"> Excel工作表名稱</param> 
        ///  <param name="headerRowIndex"> Excel表頭行索引</param> 
        ///  <returns> DataTable </returns> 
        public static DataTable ImportDataTableFromExcel(string excelFilePath, string sheetName, int headerRowIndex)
        {
            IWorkbook workbook = null;
            try
            {
                using (FileStream excelFileStream = System.IO.File.OpenRead(excelFilePath))
                {
                    int sheetIndex = 0;
                    if (isXlsx(excelFilePath))
                        workbook = new XSSFWorkbook(excelFileStream);
                    else
                        workbook = new HSSFWorkbook(excelFileStream);
                    sheetIndex = workbook.GetSheetIndex(sheetName);
                    return ImportDataTableFromExcel(excelFileStream, sheetIndex, headerRowIndex, isXlsx(excelFilePath));
                }
            }
            catch (Exception exp) { return null; }
        }
        ///  <summary> 
        /// 由Excel導入DataTable 
        ///  </summary> 
        ///  <param name="excelFilePath"> Excel文件路徑，為物理路徑。</param> 
        ///  <param name="sheetName"> Excel工作表索引</param> 
        ///  <param name="headerRowIndex"> Excel表頭行索引</param>
        ///  <returns> DataTable </returns> 
        public static DataTable ImportDataTableFromExcel(string excelFilePath, int sheetIndex, int headerRowIndex)
        {
            try
            {
                using (FileStream excelFileStream = System.IO.File.OpenRead(excelFilePath))
                {
                    return ImportDataTableFromExcel(excelFileStream, sheetIndex, headerRowIndex, isXlsx(excelFilePath));
                }
            }
            catch (Exception exp) { return null; }
        }
        ///  <summary> 
        /// 由Excel導入DataTable 
        ///  </summary> 
        ///  <param name="excelFileStream"> Excel文件流</param> 
        ///  <param name="sheetName"> Excel工作表索引</param> 
        ///  <param name="headerRowIndex"> Excel表頭行索引</param>
        ///  <param name="isXlsx"> true:office2007以上版本</param>
        ///  <returns> DataTable </returns> 
        public static DataTable ImportDataTableFromExcel(Stream excelFileStream, string sheetName, int headerRowIndex, bool isXlsx)
        {
            IWorkbook workbook = null;
            try
            {
                int sheetIndex = 0;
                if (isXlsx)
                    workbook = new XSSFWorkbook(excelFileStream);
                else
                    workbook = new HSSFWorkbook(excelFileStream);
                sheetIndex = workbook.GetSheetIndex(sheetName);
                return ImportDataTableFromExcel(excelFileStream, sheetIndex, headerRowIndex, isXlsx);
            }
            catch (Exception exp) { return null; }
        }
        ///  <summary> 
        /// 由Excel導入DataTable 
        ///  </summary> 
        ///  <param name="excelFileStream"> Excel文件流</param> 
        ///  <param name="sheetName"> Excel工作表索引</param> 
        ///  <param name="headerRowIndex"> Excel表頭行索引</param>
        ///  <returns> DataTable </returns> 
        public static DataTable ImportDataTableFromExcel(Stream excelFileStream, int sheetIndex, int headerRowIndex, bool isXlsx)
        {
            DataTable table = new DataTable();
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow headerRow = null;
            try
            {
                if (isXlsx)
                {
                    workbook = new XSSFWorkbook(excelFileStream);
                    sheet = (XSSFSheet)workbook.GetSheetAt(sheetIndex);
                    headerRow = (XSSFRow)sheet.GetRow(headerRowIndex);
                }
                else
                {
                    workbook = new HSSFWorkbook(excelFileStream);
                    sheet = (HSSFSheet)workbook.GetSheetAt(sheetIndex);
                    headerRow = (HSSFRow)sheet.GetRow(headerRowIndex);
                }
                int cellCount = headerRow.LastCellNum;

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    // 如果遇到第一個空列，則不再繼續向後讀取，並且紀錄最後的欄數
                    if (headerRow.GetCell(i) == null || headerRow.GetCell(i).StringCellValue.Trim() == "") { cellCount = i; break; }

                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                IRow row = null;
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    if (isXlsx) row = (XSSFRow)sheet.GetRow(i);
                    else row = (HSSFRow)sheet.GetRow(i);
                    // 如果行為NULL 則判定檔案結尾，不再繼續向後讀取
                    if (row == null) break;

                    int emptyIndex = 1;
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        dataRow[j] = row.GetCell(j);
                        if (dataRow[j] == null || dataRow[j].ToString().Trim() == "") emptyIndex++;

                    }
                    //整列儲存格皆為空白, 判定檔案結尾
                    if (emptyIndex == table.Columns.Count) break;
                    table.Rows.Add(dataRow);
                }
                excelFileStream.Close();
                workbook = null;
                sheet = null;
                return table;
            }
            catch (Exception exp) { return null; }
        }
        #endregion
    }
}
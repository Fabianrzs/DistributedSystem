using System.Collections;
using System.Globalization;
using System.Reflection;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Reports.Application.Abstractions.FileProcessor;
using Reports.Domain.Attributes.Excel;

namespace Reports.Infrastructure.Implementations.FileProcessor;

public class FileProcessingService : IFileProcessingService
{
    public List<TModel> ProcessExcelFile<TModel>(IFormFile file, CancellationToken cancellationToken) where TModel : class, new()
    {
        var entities = new List<TModel>();

        using Stream stream = file.OpenReadStream();
        using var workbook = new XLWorkbook(stream);
        IXLWorksheet worksheet = workbook.Worksheet(1);
        IEnumerable<IXLRangeRow> rows = worksheet.RangeUsed()!.RowsUsed().Skip(1);

        PropertyInfo[] properties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (IXLRangeRow row in rows)
        {
            try
            {
                var entity = new TModel();

                foreach (PropertyInfo property in properties)
                {
                    ExcelColumnNumberAttribute? columnAttr = property.GetCustomAttribute<ExcelColumnNumberAttribute>();
                    if (columnAttr == null)
                    { continue; }
                    IXLCell cell = row.Cell(columnAttr.ColumnIndex);
                    object? value = ConvertCellValue(cell, property.PropertyType);
                    property.SetValue(entity, value);
                }

                entities.Add(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en fila {row.RowNumber()}: {ex.Message}");
            }
        }

        return entities;
    }

    public IFormFile GenerateExcelFile<TModel>(List<TModel> data, CancellationToken cancellationToken) where TModel : class, new()
    {
        using var workbook = new XLWorkbook();
        IXLWorksheet worksheet = workbook.Worksheets.Add("Data");

        var columnProperties = typeof(TModel)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => new
            {
                Property = p,
                ColumnAttr = p.GetCustomAttribute<ExcelColumnNumberAttribute>(),
                HeaderAttr = p.GetCustomAttribute<ExcelColumnNameAttribute>()
            })
            .Where(x => x.ColumnAttr != null)
            .OrderBy(x => x.ColumnAttr!.ColumnIndex)
            .ToList();

        PropertyInfo? dynamicListProperty = typeof(TModel)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(p => p.GetCustomAttribute<ExcelDynamicListAttribute>() != null);

        ExcelDynamicListAttribute? dynamicAttr = dynamicListProperty?.GetCustomAttribute<ExcelDynamicListAttribute>();
        List<string> dynamicHeaderList = GetDynamicHeaders(data, dynamicListProperty, dynamicAttr);

        WriteHeaders(worksheet, [.. columnProperties.Select(x => (x.Property, x.HeaderAttr))], dynamicHeaderList);

        WriteDataRows(worksheet, data, [.. columnProperties.Select(x => x.Property)], dynamicListProperty, dynamicAttr, dynamicHeaderList);

        worksheet.Columns().AdjustToContents();

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        return new FormFile(stream, 0, stream.Length, "excel", $"reports_{DateTimeOffset.UtcNow:yyyyMMddHHmmss}.xlsx")
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };
    }

    #region Private Methods

    private void WriteHeaders(IXLWorksheet worksheet, IEnumerable<(PropertyInfo Property, ExcelColumnNameAttribute? HeaderAttr)> columnProperties, List<string> dynamicHeaderList)
    {
        int colIndex = 1;

        foreach ((PropertyInfo property, ExcelColumnNameAttribute headerAttr) in columnProperties)
        {
            IXLCell cell = worksheet.Cell(1, colIndex++);
            cell.Value = headerAttr?.Name ?? property.Name;
            cell.Style.Font.Bold = true;
            ExcelHeaderStyleAttribute? styleAttr = property.GetCustomAttribute<ExcelHeaderStyleAttribute>();
            ApplyStyleToCell(cell, styleAttr);
        }

        foreach (string header in dynamicHeaderList)
        {
            IXLCell cell = worksheet.Cell(1, colIndex++);
            cell.Value = header;
            cell.Style.Font.Bold = true;
        }
    }

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
    private void WriteDataRows<TModel>(IXLWorksheet worksheet, List<TModel> data, List<PropertyInfo> columnProperties, PropertyInfo? dynamicListProperty, ExcelDynamicListAttribute? dynamicAttr, List<string> dynamicHeaderList)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
    {
        var groupedProps = columnProperties
            .Where(p => p.GetCustomAttribute<ExcelGroupDisplayAttribute>() != null)
            .Select(p => new { Property = p, GroupAttr = p.GetCustomAttribute<ExcelGroupDisplayAttribute>()! })
            .ToList();

        var regularProps = columnProperties.Except(groupedProps.Select(g => g.Property)).ToList();

        int row = 2;

        while (row - 2 < data.Count)
        {
            TModel? currentItem = data[row - 2];
            string? keyProp = groupedProps.FirstOrDefault()?.GroupAttr.GroupByProperty;
            PropertyInfo? groupKeyProp = !string.IsNullOrWhiteSpace(keyProp) ? typeof(TModel).GetProperty(keyProp!) : null;
            object? groupKey = groupKeyProp?.GetValue(currentItem);
            int groupSize = groupKeyProp != null ? CountGroupRows(data, row - 2, groupKeyProp, groupKey) : 1;

            int col = 1;
            foreach (PropertyInfo? prop in regularProps)
            {
                for (int i = 0; i < groupSize; i++)
                {
                    ExcelColumnType columnType = prop.GetCustomAttribute<ExcelColumnTypeAttribute>()?.Type ?? ExcelColumnType.Default;
                    object? value = prop.GetValue(data[row - 2 + i]);
                    SetCellValueSafely(worksheet.Cell(row + i, col), value, columnType);
                }
                col++;
            }

            foreach (var g in groupedProps)
            {
                object? value = g.Property.GetValue(currentItem);
                if (groupSize > 1)
                {
                    IXLRange range = worksheet.Range(row, col, row + groupSize - 1, col);
                    range.Merge();
                    range.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                }
                ExcelColumnType columnType = g.Property.GetCustomAttribute<ExcelColumnTypeAttribute>()?.Type ?? ExcelColumnType.Default;

                SetCellValueSafely(worksheet.Cell(row, col), value, columnType);
                col++;
            }

            for (int i = 0; i < groupSize; i++)
            {
                TModel? item = data[row - 2 + i];
                Dictionary<string, decimal> dynamicValues = GetDynamicValues(item, dynamicListProperty, dynamicAttr);
                int dynCol = col;
                foreach (string header in dynamicHeaderList)
                {
                    worksheet.Cell(row + i, dynCol++).Value = dynamicValues.TryGetValue(header, out decimal val) ? val : 0;
                }
            }

            row += groupSize;
        }
    }

    private Dictionary<string, decimal> GetDynamicValues<TModel>(TModel item, PropertyInfo? dynamicListProperty, ExcelDynamicListAttribute? dynamicAttr)
    {
        var result = new Dictionary<string, decimal>();
        if (dynamicListProperty == null || dynamicAttr == null || dynamicListProperty.GetValue(item) is not IEnumerable list)
        {
            return result;
        }

        foreach (object? element in list)
        {
            string? name = element.GetType().GetProperty(dynamicAttr.TargetProperty)?.GetValue(element)?.ToString();
            object? valueObj = element.GetType().GetProperty(dynamicAttr.ValueProperty)?.GetValue(element);
            if (!string.IsNullOrWhiteSpace(name) && valueObj is not null)
            {
                decimal value = Convert.ToDecimal(valueObj, CultureInfo.InvariantCulture);
                result[name] = result.TryGetValue(name, out decimal existing) ? existing + value : value;
            }
        }

        return result;
    }

    private List<string> GetDynamicHeaders<TModel>(List<TModel> data, PropertyInfo? listProperty, ExcelDynamicListAttribute? attribute)
    {
        var headers = new HashSet<string>();
        if (listProperty == null || attribute == null)
        {
            return [.. headers];
        }

        foreach (TModel? item in data)
        {
            if (listProperty.GetValue(item) is not IEnumerable list)
            {
                continue;
            }

            foreach (object? element in list)
            {
                string? value = element.GetType().GetProperty(attribute.TargetProperty)?.GetValue(element)?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    headers.Add(value);
                }
            }
        }

        return [.. headers.OrderBy(x => x)];
    }

    private int CountGroupRows<TModel>(List<TModel> data, int startIndex, PropertyInfo keyProp, object? keyValue)
    {
        int count = 0;
        for (int i = startIndex; i < data.Count; i++)
        {
            if (!Equals(keyProp.GetValue(data[i]), keyValue))
            {
                break;
            }

            count++;
        }
        return count;
    }

    private void ApplyStyleToCell(IXLCell cell, ExcelHeaderStyleAttribute? styleAttr)
    {
        if (styleAttr == null)
        {
            return;
        }
        if (!string.IsNullOrWhiteSpace(styleAttr.FontColorHex))
        {
            cell.Style.Font.FontColor = XLColor.FromHtml(styleAttr.FontColorHex);
        }

        if (!string.IsNullOrWhiteSpace(styleAttr.BackgroundColorHex))
        {
            cell.Style.Fill.BackgroundColor = XLColor.FromHtml(styleAttr.BackgroundColorHex);
        }
    }

    private void SetCellValueSafely(IXLCell cell, object? value, ExcelColumnType columnType)
    {
        cell.Value = value switch
        {
            null => string.Empty,
            int i => i,
            decimal d => d,
            double db => db,
            float f => f,
            DateTime dt => dt,
            bool b => b,
            _ => value.ToString() ?? string.Empty
        };

        if (columnType == ExcelColumnType.Default && value is DateTime)
        {
            cell.Style.DateFormat.Format = "dd/MM/yyyy";
            return;
        }

        switch (columnType)
        {
            case ExcelColumnType.Date:
                cell.Style.DateFormat.Format = "dd/MM/yyyy";
                break;
            case ExcelColumnType.Currency:
                cell.Style.NumberFormat.Format = "$ #,##0.00";
                break;
            case ExcelColumnType.Percentage:
                cell.Style.NumberFormat.Format = "0.00%";
                break;
            case ExcelColumnType.Number:
                cell.Style.NumberFormat.Format = "#,##0.00";
                break;
        }
    }

    private object? ConvertCellValue(IXLCell cell, Type targetType)
    {
        if (cell.IsEmpty())
        {
            return null;
        }

        return targetType switch
        {
            var t when t == typeof(int) => cell.GetValue<int>(),
            var t when t == typeof(decimal) => cell.GetValue<decimal>(),
            var t when t == typeof(string) => cell.GetValue<string>(),
            var t when t == typeof(DateTime) => ConvertToDateTime(cell),
            _ => null
        };
    }
    private static readonly string[] formats = ["dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "dd-MM-yyyy"];

    private DateTime? ConvertToDateTime(IXLCell cell)
    {
        try
        {
            return cell.DataType switch
            {
                XLDataType.DateTime => cell.GetDateTime(),
                XLDataType.Number => DateTime.FromOADate(cell.GetDouble()),
                XLDataType.Text => DateTime.TryParseExact(cell.GetString().Trim(),
                    formats,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt : null,
                _ => null
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al convertir la fecha en la celda {cell.Address}: {ex.Message}");
        }
    }

    #endregion
}

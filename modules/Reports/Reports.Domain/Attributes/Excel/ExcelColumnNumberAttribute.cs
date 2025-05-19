namespace Reports.Domain.Attributes.Excel;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ExcelColumnNumberAttribute : Attribute
{
    public int ColumnIndex { get; }

    public ExcelColumnNumberAttribute(int columnIndex)
    {
        ColumnIndex = columnIndex;
    }
}

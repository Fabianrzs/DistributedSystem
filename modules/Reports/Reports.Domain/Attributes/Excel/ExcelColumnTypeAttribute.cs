namespace Reports.Domain.Attributes.Excel;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelColumnTypeAttribute : Attribute
{
    public ExcelColumnType Type { get; }

    public ExcelColumnTypeAttribute(ExcelColumnType type)
    {
        Type = type;
    }
}

namespace Reports.Domain.Attributes.Excel;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelGroupDisplayAttribute : Attribute
{
    public string GroupByProperty { get; }

    public ExcelGroupDisplayAttribute(string groupByProperty)
    {
        GroupByProperty = groupByProperty;
    }
}

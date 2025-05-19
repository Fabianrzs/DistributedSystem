namespace Reports.Domain.Attributes.Excel;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ExcelColumnNameAttribute : Attribute
{
    public string Name { get; }

    public ExcelColumnNameAttribute(string name)
    {
        Name = name;
    }
}

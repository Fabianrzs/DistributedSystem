namespace Reports.Domain.Attributes.Excel;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelDynamicListAttribute : Attribute
{
    public string TargetProperty { get; }
    public string ValueProperty { get; }

    public ExcelDynamicListAttribute(string targetProperty, string valueProperty)
    {
        TargetProperty = targetProperty;
        ValueProperty = valueProperty;
    }
}

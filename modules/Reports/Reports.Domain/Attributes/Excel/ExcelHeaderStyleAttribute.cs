namespace Reports.Domain.Attributes.Excel;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelHeaderStyleAttribute : Attribute
{
    public string FontColorHex { get; }
    public string BackgroundColorHex { get; }

    public ExcelHeaderStyleAttribute(string fontColorHex, string backgroundColorHex)
    {
        FontColorHex = fontColorHex;
        BackgroundColorHex = backgroundColorHex;
    }
}

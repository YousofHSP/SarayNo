using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Models;

public enum FieldType
{
    Select,
    Text,
    MultiSelect,
    Number,
    Checkbox,
    Time,
    DateTime,
    Radio,
    Hidden
}
public class Field
{
    public string Name { get; set; }
    public string Label { get; set; }
    public FieldType Type { get; set; }
    public string Value { get; set; }
    public List<string> Values { get; set; } = [];
    public List<SelectListItem> Items { get; set; } = [ ];
}
public class CreateViewModel
{
    public string Title { get; set; }
    public bool Error { get; set; }
    public PropertyInfo[] Properties { get; set; }
    public Dictionary<string, List<SelectListItem>> Options { get; set; } = new();
    public List<Field> Fields { get; set; } = [];

}
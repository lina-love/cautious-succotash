using System.Reflection;

namespace CourseLibrary.API.Services;

public class PropertyCheckerService : IPropertyCheckerService
{
    public bool TypeHasProperties<T>(string fields)
    {
        var propertyInfos = typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (string.IsNullOrWhiteSpace(fields))
        {
            return true;
        }

        var fieldsAfterSplit = fields.Split(',');

        foreach (var field in fieldsAfterSplit)
        {
            var propertyName = field.Trim();

            var propertyInfo = propertyInfos
                .FirstOrDefault(pi => pi.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));

            if (propertyInfo == null)
            {
                return false;
            }
        }

        return true;
    }
}

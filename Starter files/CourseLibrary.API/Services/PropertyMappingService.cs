using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;

namespace CourseLibrary.API.Services;

public class PropertyMappingService : IPropertyMappingService
{
    private readonly Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new(["Id"]) },
            { "MainCategory", new(["MainCategory"]) },
            { "Age", new(["DateOfBirth"], revert: true) },
            { "Name", new(["FirstName", "LastName"]) }
        };

    private readonly Dictionary<string, PropertyMappingValue> _coursePropertyMapping =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new(["Id"]) },
            { "Title", new(["Title"]) }
        };

    private readonly IList<IPropertyMapping> _propertyMappings = [];

    public PropertyMappingService()
    {
        _propertyMappings.Add(
            new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));

        _propertyMappings.Add(
            new PropertyMapping<CourseDto, Course>(_coursePropertyMapping));
    }

    public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
    {
        var matchingMapping = _propertyMappings
            .OfType<PropertyMapping<TSource, TDestination>>();

        if (matchingMapping.Count() == 1)
        {
            return matchingMapping.First().MappingDictionary;
        }

        throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)}, {typeof(TDestination)}>");
    }

    public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
    {
        var propertyMapping = GetPropertyMapping<TSource, TDestination>();

        if (string.IsNullOrWhiteSpace(fields))
        {
            return true;
        }

        var fieldsAfterSplit = fields.Split(',');

        foreach (var field in fieldsAfterSplit)
        {
            var trimmedField = field.Trim();

            var indexOfFirstSpace = trimmedField.IndexOf(" ", StringComparison.Ordinal);

            var propertyName = indexOfFirstSpace == -1
                ? trimmedField
                : trimmedField.Remove(indexOfFirstSpace);

            if (!propertyMapping.ContainsKey(propertyName))
            {
                return false;
            }
        }

        return true;
    }
}

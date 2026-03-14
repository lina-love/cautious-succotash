using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace CourseLibrary.API.ActionsConstraints;

[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
public class RequestHeaderMatchesMediaTypeAttribute : Attribute, IActionConstraint
{
    private readonly string _requestHeaderToMatch;
    private readonly MediaTypeCollection _mediaTypes = [];

    public RequestHeaderMatchesMediaTypeAttribute(
        string requestHeaderToMatch,
        string mediaType,
        params string[] otherMediaTypes)
    {
        _requestHeaderToMatch = requestHeaderToMatch ?? throw new ArgumentNullException(nameof(requestHeaderToMatch));

        if (MediaTypeHeaderValue.TryParse(mediaType, out var parsedMediaType))
        {
            _mediaTypes.Add(parsedMediaType);
        }
        else
        {
            throw new ArgumentException($"The media type {mediaType} is not a valid media type.");
        }

        foreach (var otherMediaType in otherMediaTypes)
        {
            if (MediaTypeHeaderValue.TryParse(otherMediaType, out var parsedOtherMediaType))
            {
                _mediaTypes.Add(parsedOtherMediaType);
            }
            else
            {
                throw new ArgumentException($"The media type {mediaType} is not a valid media type.");
            }
        }
    }

    public int Order { get; }

    public bool Accept(ActionConstraintContext context)
    {
        var requestHeaders = context.RouteContext.HttpContext.Request.Headers;
        if (!requestHeaders.TryGetValue(_requestHeaderToMatch, out StringValues value))
        {
            return false;
        }

        var parsedRequestMediaType = new MediaType(value!);

        foreach (var mediaType in _mediaTypes)
        {
            var parsedMediaType = new MediaType(mediaType);
            if (parsedRequestMediaType.Equals(parsedMediaType))
            {
                return true;
            }
        }

        return false;
    }
}

using AutoMapper;
using CourseLibrary.API.ActionsConstraints;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace CourseLibrary.API.Controllers;

[ApiController]
[Route("api/authors")]
public class AuthorsController : ControllerBase
{
    private readonly ICourseLibraryRepository _courseLibraryRepository;
    private readonly IMapper _mapper;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly IPropertyCheckerService _propertyCheckerService;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public AuthorsController(
        ICourseLibraryRepository courseLibraryRepository,
        IMapper mapper,
        IPropertyMappingService propertyMappingService,
        IPropertyCheckerService propertyCheckerService,
        ProblemDetailsFactory problemDetailsFactory)
    {
        _courseLibraryRepository = courseLibraryRepository ??
            throw new ArgumentNullException(nameof(courseLibraryRepository));
        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
        _propertyMappingService = propertyMappingService ??
            throw new ArgumentNullException(nameof(propertyMappingService));
        _propertyCheckerService = propertyCheckerService ??
            throw new ArgumentNullException(nameof(propertyCheckerService));
        _problemDetailsFactory = problemDetailsFactory ??
            throw new ArgumentNullException(nameof(problemDetailsFactory));
    }

    [HttpGet(Name = "GetAuthors")]
    [HttpHead]
    public async Task<IActionResult> GetAuthors(
        [FromQuery] AuthorsResourceParameters authorsResourceParameters,
        [FromHeader(Name = "Accept")] string? mediaType)
    {
        if (!MediaTypeHeaderValue.TryParse(mediaType, out var parsedMediaType))
        {
            return BadRequest(
                _problemDetailsFactory.CreateProblemDetails(HttpContext,
                    statusCode: 400,
                    detail: $"Accept header media type value is not a valid media type. {parsedMediaType?.MediaType}"));
        }

        if (!_propertyCheckerService.TypeHasProperties<AuthorDto>(authorsResourceParameters.Fields!))
        {
            return BadRequest(
                _problemDetailsFactory.CreateProblemDetails(HttpContext,
                    statusCode: 400,
                    title: $"Not all requested data shapping fields exist on " +
                    $"the resource: {authorsResourceParameters.Fields}"));
        }

        if (!_propertyMappingService.ValidMappingExistsFor<AuthorDto, Entities.Author>(authorsResourceParameters.OrderBy!))
        {
            return BadRequest();
        }

        // get authors from repo
        var authorsFromRepo = await _courseLibraryRepository.GetAuthorsAsync(authorsResourceParameters);

        var paginationMetadata = new PaginationMetadata
        {
            TotalCount = authorsFromRepo.TotalCount,
            PageSize = authorsFromRepo.PageSize,
            CurrentPage = authorsFromRepo.CurrentPage,
            TotalPages = authorsFromRepo.TotalPages
        };

        if (parsedMediaType.MediaType == "application/vnd.marvin.hateoas+json")
        {
            var previousPageLink = authorsFromRepo.HasPrevious
                ? CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = authorsFromRepo.HasNext
                ? CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage)
                : null;

            var links = CreateLinksForAuthors(authorsResourceParameters, authorsFromRepo.HasNext, authorsFromRepo.HasPrevious);

            var shapedAuthors = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo).ShapData(authorsResourceParameters.Fields)
                as IEnumerable<IDictionary<string, object>>;

            paginationMetadata.NextPageLink = nextPageLink;
            paginationMetadata.PreviousPageLink = previousPageLink;

            var shapedAuthorsWithLinks = shapedAuthors!.Select(author =>
            {
                var authorAsDictionary = author as IDictionary<string, object>;
                var authorLinks = CreateLinksForAuthor((Guid)author["Id"]!, null);
                authorAsDictionary.Add("links", authorLinks);
                return authorAsDictionary;
            });

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            }));

            // return them
            return Ok(new
            {
                value = shapedAuthorsWithLinks,
                links = links
            });
        }

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(paginationMetadata, new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        }));

        return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo).ShapData(authorsResourceParameters.Fields));
    }

    private string? CreateAuthorsResourceUri(
        AuthorsResourceParameters authorsResourceParameters,
        ResourceUriType type)
    {
        return type switch
        {
            ResourceUriType.PreviousPage => Url.Link("GetAuthors",
                new
                {
                    pageNumber = authorsResourceParameters.PageNumber - 1,
                    pageSize = authorsResourceParameters.PageSize,
                    mainCategory = authorsResourceParameters.MainCategory,
                    searchQuery = authorsResourceParameters.SearchQuery,
                    orderBy = authorsResourceParameters.OrderBy,
                    fields = authorsResourceParameters.Fields
                }),
            ResourceUriType.NextPage => Url.Link("GetAuthors",
                new
                {
                    pageNumber = authorsResourceParameters.PageNumber + 1,
                    pageSize = authorsResourceParameters.PageSize,
                    mainCategory = authorsResourceParameters.MainCategory,
                    searchQuery = authorsResourceParameters.SearchQuery,
                    orderBy = authorsResourceParameters.OrderBy,
                    fields = authorsResourceParameters.Fields
                }),
            ResourceUriType.Current or _ => Url.Link("GetAuthors",
                new
                {
                    pageNumber = authorsResourceParameters.PageNumber,
                    pageSize = authorsResourceParameters.PageSize,
                    mainCategory = authorsResourceParameters.MainCategory,
                    searchQuery = authorsResourceParameters.SearchQuery,
                    orderBy = authorsResourceParameters.OrderBy,
                    fields = authorsResourceParameters.Fields
                }),
        };
    }

    [Produces("application/json", "application/vnd.marvin.author.full+json")]
    [HttpGet("{authorId}", Name = "GetAuthor")]
    public async Task<IActionResult> GetAuthor(
        Guid authorId,
        string? fields,
        [FromHeader(Name = "Accept")] string? mediaType)
    {
        if (!MediaTypeHeaderValue.TryParse(mediaType, out var parsedMediaType))
        {
            return BadRequest(
                _problemDetailsFactory.CreateProblemDetails(HttpContext,
                    statusCode: 400,
                    detail: $"Accept header media type value is not a valid media type. {parsedMediaType?.MediaType}"));
        }

        if (!_propertyCheckerService.TypeHasProperties<AuthorDto>(fields))
        {
            return BadRequest(
                _problemDetailsFactory.CreateProblemDetails(HttpContext,
                    statusCode: 400,
                    title: $"Not all requested data shapping fields exist on " +
                    $"the resource: {fields}"));
        }

        // get author from repo
        var authorFromRepo = await _courseLibraryRepository.GetAuthorAsync(authorId);

        if (authorFromRepo == null)
        {
            return NotFound();
        }

        var includeLinks = parsedMediaType.SubTypeWithoutSuffix.
            EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

        var links = new List<LinkDto>();

        if (includeLinks)
        {
            links = CreateLinksForAuthor(authorId, fields).ToList();
        }

        var primaryMediaType = includeLinks ?
            parsedMediaType.SubTypeWithoutSuffix.Substring(0, parsedMediaType.SubTypeWithoutSuffix.Length - 8)
            : parsedMediaType.SubTypeWithoutSuffix;

        if (primaryMediaType == "vnd.marvin.author.full")
        {
            var fullResourceToReturn = _mapper.Map<AuthorFullDto>(authorFromRepo).ShapData(fields) as IDictionary<string, object>;

            if (includeLinks)
            {
                fullResourceToReturn.Add("links", links);
            }

            return Ok(fullResourceToReturn);
        }

        //if (parsedMediaType.MediaType == "application/vnd.marvin.hateoas+json")
        //{
        //    var links = CreateLinksForAuthor(authorId, fields);

        //    var linkedResourceToReturn = _mapper.Map<AuthorDto>(authorFromRepo).ShapData(fields) as IDictionary<string, object>;

        //    linkedResourceToReturn.Add("links", links);

        //    // return author
        //    return Ok(linkedResourceToReturn);
        //}

        return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
    }

    private IEnumerable<LinkDto> CreateLinksForAuthor(Guid authorId, string? fields)
    {
        var links = new List<LinkDto>();

        if (string.IsNullOrWhiteSpace(fields))
        {
            links.Add(new LinkDto(Url.Link("GetAuthor", new { authorId }), "self", "GET"));
        }
        else
        {
            links.Add(new LinkDto(Url.Link("GetAuthor", new { authorId, fields }), "self", "GET"));
        }

        links.Add(new LinkDto(Url.Link("CreateCourseForAuthor", new { authorId }), "create_course_for_author", "POST"));
        links.Add(new LinkDto(Url.Link("GetCoursesForAuthor", new { authorId }), "get_courses_for_author", "GET"));

        return links;
    }

    private IEnumerable<LinkDto> CreateLinksForAuthors(AuthorsResourceParameters authorsResourceParameters, bool hasNext, bool hasPrevious)
    {
        var links = new List<LinkDto>();

        links.Add(new LinkDto(CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.Current), "self", "GET"));

        if (hasNext)
        {
            links.Add(new LinkDto(CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage), "next_page", "GET"));
        }

        if (hasPrevious)
        {
            links.Add(new LinkDto(CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage), "previous_page", "GET"));
        }

        return links;
    }

    [HttpPost(Name = "CreateAuthorWithDateOfDeath")]
    [RequestHeaderMatchesMediaType("Content-Type", "application/vnd.marvin.authorforcreationwithdateofdeath+json")]
    [Consumes("application/vnd.marvin.authorforcreationwithdateofdeath+json")]
    public async Task<ActionResult<AuthorDto>> CreateAuthorWithDateOfDeath(AuthorForCreationWithDateOfDeathDto author)
    {
        var authorEntity = _mapper.Map<Entities.Author>(author);

        _courseLibraryRepository.AddAuthor(authorEntity);
        await _courseLibraryRepository.SaveAsync();

        var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

        var links = CreateLinksForAuthor(authorToReturn.Id, null);

        var linkedResourceToReturn = authorToReturn.ShapData(null) as IDictionary<string, object>;

        linkedResourceToReturn.Add("links", links);

        return CreatedAtRoute("GetAuthor",
            new { authorId = linkedResourceToReturn["Id"] },
            linkedResourceToReturn);
    }

    [HttpPost(Name = "CreateAuthor")]
    [RequestHeaderMatchesMediaType("Content-Type",
        "application/json",
        "application/vnd.marvin.authorforcreation+json")]
    [Consumes("application/json", "application/vnd.marvin.authorforcreationwithdateofbirth+json")]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorForCreationDto author)
    {
        var authorEntity = _mapper.Map<Entities.Author>(author);

        _courseLibraryRepository.AddAuthor(authorEntity);
        await _courseLibraryRepository.SaveAsync();

        var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

        var links = CreateLinksForAuthor(authorToReturn.Id, null);

        var linkedResourceToReturn = authorToReturn.ShapData(null) as IDictionary<string, object>;

        linkedResourceToReturn.Add("links", links);

        return CreatedAtRoute("GetAuthor",
            new { authorId = linkedResourceToReturn["Id"] },
            linkedResourceToReturn);
    }

    [HttpOptions]
    public IActionResult GetAuthorsOptions()
    {
        Response.Headers.Append("Allow", "GET, HEAD, POST");
        return NoContent();
    }

    public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
    {
        var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

        return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
    }
}

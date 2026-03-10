using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Text.Json;

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
        [FromQuery] AuthorsResourceParameters authorsResourceParameters)
    {
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
        var authorsFromRepo = await _courseLibraryRepository
            .GetAuthorsAsync(authorsResourceParameters);

        //var previousPageLink = authorsFromRepo.HasPrevious
        //    ? CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage)
        //    : null;

        //var nextPageLink = authorsFromRepo.HasNext
        //    ? CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage)
        //    : null;

        var paginationMetadata = new
        {
            totalCount = authorsFromRepo.TotalCount,
            pageSize = authorsFromRepo.PageSize,
            currentPage = authorsFromRepo.CurrentPage,
            totalPages = authorsFromRepo.TotalPages,
            //previousPageLink,
            //nextPageLink
        };

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        var links = CreateLinksForAuthors(authorsResourceParameters, authorsFromRepo.HasNext, authorsFromRepo.HasPrevious);

        var shapedAuthors = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo).ShapData(authorsResourceParameters.Fields)
            as IEnumerable<IDictionary<string, object>>;

        var shapedAuthorsWithLinks = shapedAuthors!.Select(author =>
        {
            var authorAsDictionary = author as IDictionary<string, object>;
            var authorLinks = CreateLinksForAuthor((Guid)author["Id"]!, null);
            authorAsDictionary.Add("links", authorLinks);
            return authorAsDictionary;
        });

        // return them
        return Ok(new
        {
            value = shapedAuthorsWithLinks,
            links = links
        });
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

    [HttpGet("{authorId}", Name = "GetAuthor")]
    public async Task<IActionResult> GetAuthor(Guid authorId, string? fields)
    {
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

        var links = CreateLinksForAuthor(authorId, fields);

        var linkedResourceToReturn = _mapper.Map<AuthorDto>(authorFromRepo).ShapData(fields) as IDictionary<string, object>;

        linkedResourceToReturn.Add("links", links);

        // return author
        return Ok(linkedResourceToReturn);
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

    [HttpPost]
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

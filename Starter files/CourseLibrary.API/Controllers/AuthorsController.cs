using AutoMapper;
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

        var previousPageLink = authorsFromRepo.HasPrevious
            ? CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.PreviousPage)
            : null;

        var nextPageLink = authorsFromRepo.HasNext
            ? CreateAuthorsResourceUri(authorsResourceParameters, ResourceUriType.NextPage)
            : null;

        var paginationMetadata = new
        {
            totalCount = authorsFromRepo.TotalCount,
            pageSize = authorsFromRepo.PageSize,
            currentPage = authorsFromRepo.CurrentPage,
            totalPages = authorsFromRepo.TotalPages,
            previousPageLink,
            nextPageLink
        };

        Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

        // return them
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
            _ => Url.Link("GetAuthors",
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

        // return author
        return Ok(_mapper.Map<AuthorDto>(authorFromRepo).ShapData(fields));
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> CreateAuthor(AuthorForCreationDto author)
    {
        var authorEntity = _mapper.Map<Entities.Author>(author);

        _courseLibraryRepository.AddAuthor(authorEntity);
        await _courseLibraryRepository.SaveAsync();

        var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

        return CreatedAtRoute("GetAuthor",
            new { authorId = authorToReturn.Id },
            authorToReturn);
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

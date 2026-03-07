namespace CourseLibrary.API.ResourceParameters;

public abstract class AuthorsResourceParameters : PaingationResourceParameters
{
    public string? MainCategory { get; set; }
    public string? SearchQuery { get; set; }
    public string? OrderBy { get; set; } = "Name";
}

namespace CourseLibrary.API.ResourceParameters;

public class CoursesResourceParameters : PaingationResourceParameters
{
    public string? SearchQuery { get; set; }
    public string? OrderBy { get; set; } = "Title";
}

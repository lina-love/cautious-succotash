namespace CourseLibrary.API.ResourceParameters;

public class AuthorsResourceParameters
{
    const int maxPageSize = 20;
    const int minPageSize = 1;

    public string? MainCategory { get; set; }
    public string? SearchQuery { get; set; }

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }

    private int _pageNumber = 1;
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = (value < _pageNumber) ? minPageSize : value;
    }
}

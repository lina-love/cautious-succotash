namespace CourseLibrary.API.ResourceParameters;

public abstract class PaingationResourceParameters
{
    const int _maxPageSize = 20;
    const int _minPageSize = 1;

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
    }

    private int _pageNumber = 1;
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = (value < _pageNumber) ? _minPageSize : value;
    }
}

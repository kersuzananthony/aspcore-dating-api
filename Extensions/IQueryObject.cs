namespace DatingAPI.Extensions
{
    public interface IQueryObject
    {
        int Page { get; set; }

        int PageSize { get; set; }
    }
}
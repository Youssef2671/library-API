namespace LibraryAPI.Models
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
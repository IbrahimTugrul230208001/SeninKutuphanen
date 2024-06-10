namespace LibraryManagementWeb.WebAPI.Models
{
    public class Book
    {
        public string? UserName { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
        public int CompletedPages { get; set; }
        public int TotalPages { get; set; }
        public string? Status { get; set; }
    }
}

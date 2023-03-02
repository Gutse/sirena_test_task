namespace RouteSearch.Core.Errors
{
    public class Error
    {
        //for ability to translate error on different languages on frontend
        public int Code { get; set; }
        public string? Description { get; set; }
    }
}
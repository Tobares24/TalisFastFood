namespace Talis.Infrastructure.Commons.Bases.Request
{
    public class BaseFiltersRequest : BasePaginationRequest
    {
        public string TextFilter { get; set; } = "";
        public int? StateFilter { get; set; } = null;
        public int? Category { get; set; } = null;  
    }
}

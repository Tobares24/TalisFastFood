using Talis.Infrastructure.Commons.Bases.Request;

namespace Talis.Web.Models
{
    public class Pagination<T> : BaseFiltersRequest
    {
        public List<T> Elements { get; set; }
    }
}

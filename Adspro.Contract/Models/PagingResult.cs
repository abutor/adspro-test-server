namespace Adspro.Contract.Models
{
    public class PagingResult<T>
    {
        public T[] Values { get; set; } = [];
        public int Total { get; set; }
        public int Page { get; set; }

        public int Limit { get; set; }
    }
}

namespace FamilyBudgetAPI
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string OrderMode { get; set; }

        public Pagination()
        {
            PageNumber = 1;
            PageSize = 5;
            OrderMode = "Id";
        }

        public Pagination(int pageNumber, int pageSize, string orderMode)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize < 5 ? 5 : pageSize;
            OrderMode = orderMode;
        }
    }
}

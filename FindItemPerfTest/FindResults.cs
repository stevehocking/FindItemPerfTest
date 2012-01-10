
namespace FindItemPerfTest
{
    public class FindResults
    {
        protected int Found { get; set; }
        protected int NotFound { get; set; }
        protected int TotalCount { get; set; }

        public FindResults()
        {
            Found = 0;
            NotFound = 0;
            TotalCount = 0;
        }

        public void FoundItem()
        {
            Found++;
            TotalCount++;
        }

        public void NotFoundItem()
        {
            NotFound++;
            TotalCount++;
        }

        public decimal FoundPercent
        {
            get
            {
                return (decimal)Found / (Found + NotFound);
            }
        }

        public decimal NotFoundPercent
        {
            get
            {
                return (decimal)NotFound / (Found + NotFound);
            }
        }
    }
}

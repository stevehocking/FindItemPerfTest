using System.Collections.Generic;
using System.Linq;

namespace FindItemPerfTest
{
    public class TestClass
    {
        private string[] searchItems = null;
        private string[] searchTerms = null;
        private int searchCount = 0;

        public TestClass(string[] SearchItems, string[] SearchTerms, int SearchCount)
        {
            searchItems = SearchItems;
            searchTerms = SearchTerms;
            searchCount = SearchCount;
        }

        public FindResults DoFind()
        {
            FindResults results = new FindResults();
            List<string> findList = new List<string>(searchItems);

            for (int i = 0; i < searchCount; i++)
            {
                var found = findList.Find(test => test == searchTerms[i % searchTerms.Length]);

                if (string.IsNullOrEmpty(found))
                {
                    results.NotFoundItem();
                }
                else
                {
                    results.FoundItem();
                }
            }

            return results;
        }

        public FindResults DoContains()
        {
            FindResults results = new FindResults();
            List<string> findList = new List<string>(searchItems);

            for (int i = 0; i < searchCount; i++)
            {
                if (findList.Contains(searchTerms[i % searchTerms.Length]))
                {
                    results.FoundItem();
                }
                else
                {
                    results.NotFoundItem();
                }
            }

            return results;
        }

        public FindResults DoWhere()
        {
            FindResults results = new FindResults();
            List<string> whereList = new List<string>(searchItems);

            for (int i = 0; i < searchCount; i++)
            {
                var found = whereList.Where(test => test == searchTerms[i % searchTerms.Length]);

                if (found.Count() > 0)
                {
                    results.FoundItem();
                }
                else
                {
                    results.NotFoundItem();
                }
            }

            return results;
        }

        public FindResults DoAny()
        {
            FindResults results = new FindResults();
            List<string> anyList = new List<string>(searchItems);

            for (int i = 0; i < searchCount; i++)
            {
                if (anyList.Any(test => test == searchTerms[i % searchTerms.Length]))
                {
                    results.FoundItem();
                }
                else
                {
                    results.NotFoundItem();
                }
            }

            return results;
        }
    }
}

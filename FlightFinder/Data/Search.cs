using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightFinder.Pages{
    public class Search
    {
        public string userSearch;
        public void SetSearch(string Input)
        {
            userSearch = Input;
        }

        public string GetSearch()
        {
            return userSearch;
        }
    }
}

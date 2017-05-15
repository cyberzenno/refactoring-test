using Refactoring.CodeTest.Interfaces;
using System.Collections.Generic;

namespace Refactoring.CodeTest
{
    public class FailoverRepository : IFailoverRepository
    {
        public List<FailoverEntry> GetFailOverEntries()
        {
            // Return all from fail entries from database
            return new List<FailoverEntry>();
        } 
    }
}
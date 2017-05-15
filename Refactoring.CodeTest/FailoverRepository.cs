using Refactoring.CodeTest.Interfaces;
using System;
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

        public int CountFailoverEntriesForTheLastMinutes(int minutes)
        {
            var failoverEntries = GetFailOverEntries();

            var failedRequests = 0;

            foreach (var failoverEntry in failoverEntries)
            {
                if (failoverEntry.DateTime > DateTime.Now.AddMinutes(minutes))
                {
                    failedRequests++;
                }
            }

            return failedRequests;
        }
    }
}
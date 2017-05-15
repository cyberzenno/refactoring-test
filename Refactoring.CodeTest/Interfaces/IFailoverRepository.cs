using System;
using System.Collections.Generic;
namespace Refactoring.CodeTest.Interfaces
{
    public interface IFailoverRepository
    {
        List<FailoverEntry> GetFailOverEntries();
        int CountFailoverEntriesForTheLastMinutes(int minutes);
    }
}

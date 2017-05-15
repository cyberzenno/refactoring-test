using System;
namespace Refactoring.CodeTest.Interfaces
{
    public interface IFailoverRepository
    {
        System.Collections.Generic.List<FailoverEntry> GetFailOverEntries();
    }
}

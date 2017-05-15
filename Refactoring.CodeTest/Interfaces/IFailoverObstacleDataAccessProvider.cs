using System;
namespace Refactoring.CodeTest.Interfaces
{
    public interface IFailoverObstacleDataAccessProvider
    {
        ObstacleResponse GetObstacleById(int id);
    }
}

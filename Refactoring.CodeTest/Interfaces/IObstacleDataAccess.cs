using System;
namespace Refactoring.CodeTest.Interfaces
{
    public interface IObstacleDataAccess
    {
        ObstacleResponse LoadObstacle(int obstacleId);
    }
}

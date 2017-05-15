using System;
namespace Refactoring.CodeTest.Interfaces
{
    public interface IObstacleService
    {
        Obstacle GetObstacle(int obstacleId, bool isObstacleArchived);
    }
}

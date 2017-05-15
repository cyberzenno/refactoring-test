using System;
namespace Refactoring.CodeTest.Interfaces
{
    public interface IArchivedDataService
    {
        Obstacle GetArchivedObstacle(int obstacleId);
    }
}

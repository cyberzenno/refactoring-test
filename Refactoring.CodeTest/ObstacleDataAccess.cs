using Refactoring.CodeTest.Interfaces;
namespace Refactoring.CodeTest
{
    public class ObstacleDataAccess :IObstacleDataAccess
    {
        public ObstacleResponse LoadObstacle(int obstacleId)
        {
            // Retrieve obstacle from 3rd party webservice
            return new ObstacleResponse();
        }
    }
}
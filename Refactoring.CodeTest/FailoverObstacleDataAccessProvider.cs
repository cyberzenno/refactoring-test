using Refactoring.CodeTest.Interfaces;
namespace Refactoring.CodeTest
{
    public class FailoverObstacleDataAccessProvider : IFailoverObstacleDataAccessProvider
    {
        public ObstacleResponse GetObstacleById(int id)
        {
            // Retrieve obstacle from database
            return FailoverObstacleDataAccess.GetObstacleById(id);
        }
    }
}
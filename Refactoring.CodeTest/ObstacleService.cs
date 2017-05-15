using System;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Refactoring.CodeTest
{
    public class ObstacleService
    {
        public Obstacle GetObstacle(int ObstacleId, bool isObstacleArchived)
        {
            Obstacle archivedObstacle = null;

            if (isObstacleArchived)
            {
                var archivedDataService = new ArchivedDataService();
                archivedObstacle = archivedDataService.GetArchivedObstacle(ObstacleId);

                return archivedObstacle;
            }
            else
            {
                var failoverRespository = new FailoverRepository();
                var failoverEntries = failoverRespository.GetFailOverEntries();

                var failedRequests = 0;

                foreach (var failoverEntry in failoverEntries)
                {
                    if (failoverEntry.DateTime > DateTime.Now.AddMinutes(-10))
                    {
                        failedRequests++;
                    }
                }

                ObstacleResponse ObstacleResponse = null;
                Obstacle Obstacle = null;

                if (failedRequests > 100 && (ConfigurationManager.AppSettings["IsFailoverModeEnabled"] == "true" || ConfigurationManager.AppSettings["IsFailoverModeEnabled"] == "True"))
                {
                    ObstacleResponse = FailoverObstacleDataAccess.GetObstacleById(ObstacleId);
                }
                else
                {
                    var dataAccess = new ObstacleDataAccess();
                    ObstacleResponse = dataAccess.LoadObstacle(ObstacleId);
                }

                if (ObstacleResponse.IsArchived)
                {
                    var archivedDataService = new ArchivedDataService();
                    Obstacle = archivedDataService.GetArchivedObstacle(ObstacleId);
                }
                else
                {
                    Obstacle = ObstacleResponse.Obstacle;
                }

                return Obstacle;
            }
        }
    }
}

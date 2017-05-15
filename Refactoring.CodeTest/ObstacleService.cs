using Refactoring.CodeTest.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Refactoring.CodeTest
{
    public class ObstacleService : IObstacleService
    {
        private readonly IArchivedDataService archivedDataService;
        private readonly IFailoverRepository failoverRepository;
        private readonly IFailoverObstacleDataAccessProvider failoverObstacleDataAccessProvider;
        private readonly IObstacleDataAccess obstacleDataAccess;
        private readonly IConfigurationProvider configurationProvider;

        public ObstacleService(
            IArchivedDataService archivedDataService,
            IFailoverRepository failoverRepository,
            IFailoverObstacleDataAccessProvider failoverObstacleDataAccessProvider,
            IObstacleDataAccess obstacleDataAccess,
            IConfigurationProvider configurationProvider
            )
        {
            this.archivedDataService = archivedDataService;
            this.failoverRepository = failoverRepository;
            this.failoverObstacleDataAccessProvider = failoverObstacleDataAccessProvider;
            this.obstacleDataAccess = obstacleDataAccess;
            this.configurationProvider = configurationProvider;
        }

        public Obstacle GetObstacle(int obstacleId, bool isObstacleArchived)
        {
            Obstacle archivedObstacle = null;

            if (isObstacleArchived)
            {
                //var archivedDataService = new ArchivedDataService();
                archivedObstacle = archivedDataService.GetArchivedObstacle(obstacleId);

                return archivedObstacle;
            }
            else
            {
                //var failoverRespository = new FailoverRepository();
                var failoverEntries = failoverRepository.GetFailOverEntries();

                var failedRequests = 0;

                foreach (var failoverEntry in failoverEntries)
                {
                    if (failoverEntry.DateTime > DateTime.Now.AddMinutes(-10))
                    {
                        failedRequests++;
                    }
                }

                ObstacleResponse obstacleResponse = null;
                Obstacle obstacle = null;

                if (failedRequests > 100 && configurationProvider.IsFailoverModeEnabled())
                {
                    obstacleResponse = failoverObstacleDataAccessProvider.GetObstacleById(obstacleId);
                }
                else
                {
                    //var dataAccess = new ObstacleDataAccess();
                    obstacleResponse = obstacleDataAccess.LoadObstacle(obstacleId);
                }

                if (obstacleResponse.IsArchived)
                {
                    //var archivedDataService = new ArchivedDataService();
                    obstacle = archivedDataService.GetArchivedObstacle(obstacleId);
                }
                else
                {
                    obstacle = obstacleResponse.Obstacle;
                }

                return obstacle;
            }
        }
    }
}

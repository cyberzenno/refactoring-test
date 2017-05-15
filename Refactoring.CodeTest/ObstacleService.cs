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
            if (isObstacleArchived)
            {
                var archivedObstacle = archivedDataService.GetArchivedObstacle(obstacleId);
                return archivedObstacle;
            }
            else
            {
                ObstacleResponse obstacleResponse = null;
                Obstacle obstacle = null;

                if (IsFailoverRequired())
                {
                    obstacleResponse = failoverObstacleDataAccessProvider.GetObstacleById(obstacleId);
                }
                else
                {
                    obstacleResponse = obstacleDataAccess.LoadObstacle(obstacleId);
                }

                if (obstacleResponse.IsArchived)
                {
                    obstacle = archivedDataService.GetArchivedObstacle(obstacleId);
                }
                else
                {
                    obstacle = obstacleResponse.Obstacle;
                }

                return obstacle;
            }
        }

        public bool IsFailoverRequired()
        {
            if (configurationProvider.IsFailoverModeEnabled())
            {
                return failoverRepository.CountFailoverEntriesForTheLastMinutes(10) > 100;
            }

            return false;
        }

    }
}

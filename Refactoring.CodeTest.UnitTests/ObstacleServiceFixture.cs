using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Refactoring.CodeTest.Interfaces;
using Refactoring.CodeTest;

namespace Refactoring.CodeTest.UnitTests
{
    [TestClass]
    public class ObstacleServiceFixture
    {
        private Mock<IArchivedDataService> archivedDataService;
        private Mock<IFailoverRepository> failoverRepository;
        private Mock<IFailoverObstacleDataAccessProvider> failoverObstacleDataAccessProvider;
        private Mock<IObstacleDataAccess> obstacleDataAccess;
        private Mock<IConfigurationProvider> configurationProvider;

        private ObstacleService obstacleService;

        [TestInitialize]
        public void Setup()
        {
            archivedDataService = new Mock<IArchivedDataService>();
            failoverRepository = new Mock<IFailoverRepository>();
            failoverObstacleDataAccessProvider = new Mock<IFailoverObstacleDataAccessProvider>();
            obstacleDataAccess = new Mock<IObstacleDataAccess>();
            configurationProvider = new Mock<IConfigurationProvider>();

            obstacleService = new ObstacleService(archivedDataService.Object,
                failoverRepository.Object,
                failoverObstacleDataAccessProvider.Object,
                obstacleDataAccess.Object,
                configurationProvider.Object);
        }

        [TestMethod]
        public void GetArchivedObstacle_NotFound()
        {
            //Arrange
            archivedDataService.Setup(x => x.GetArchivedObstacle(1)).Returns((Obstacle)null);

            //Act
            var result = obstacleService.GetObstacle(1, true);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetArchivedObstacle()
        {
            //Arrange
            archivedDataService.Setup(x => x.GetArchivedObstacle(1)).Returns(new Obstacle()).Verifiable();

            //Act
            var result = obstacleService.GetObstacle(1, true);

            //Assert
            Assert.IsNotNull(result);
            //verify if GetArchivedObstacle has been called
            archivedDataService.Verify();
        }

        [TestMethod]
        public void GetObstacle_FromFailover_NotArchived()
        {
            //Arrange   
            configurationProvider.Setup(x => x.IsFailoverModeEnabled()).Returns(true);

            failoverRepository.Setup(x => x.CountFailoverEntriesForTheLastMinutes(10)).Returns(101);

            failoverObstacleDataAccessProvider.Setup(x => x.GetObstacleById(1))
                .Returns(new ObstacleResponse() { IsArchived = false, Obstacle = new Obstacle() }).Verifiable();

            archivedDataService.Setup(x => x.GetArchivedObstacle(1)).Throws(new Exception("Archive called for not archived Obstacle"));

            //Act
            var result = obstacleService.GetObstacle(1, false);

            //Assert 
            //verify if failoverObstacleDataAccessProvider.GetObstacleById has been called
            failoverObstacleDataAccessProvider.Verify();

        }

        [TestMethod]
        public void GetObstacle_FromFailover_Archived()
        {
            //Arrange       
            configurationProvider.Setup(x => x.IsFailoverModeEnabled()).Returns(true);

            failoverRepository.Setup(x => x.CountFailoverEntriesForTheLastMinutes(10)).Returns(101);

            failoverObstacleDataAccessProvider.Setup(x => x.GetObstacleById(1))
                .Returns(new ObstacleResponse() { IsArchived = true, Obstacle = new Obstacle() }).Verifiable();

            archivedDataService.Setup(x => x.GetArchivedObstacle(1)).Returns(new Obstacle()).Verifiable();

            //Act
            var result = obstacleService.GetObstacle(1, false);

            //Assert
            //verify if failoverObstacleDataAccessProvider.GetObstacleById has been called
            failoverObstacleDataAccessProvider.Verify();
            //verify if archivedDataService.GetArchivedObstacle has been called
            archivedDataService.Verify();
        }

        [TestMethod]
        public void GetObstacle()
        {
            //Arrange
            obstacleDataAccess.Setup(x => x.LoadObstacle(1)).Returns(new ObstacleResponse() { Obstacle = new Obstacle() });

            archivedDataService.Setup(x => x.GetArchivedObstacle(1)).Throws(new Exception("Archive called for not archived Obstacle"));

            failoverObstacleDataAccessProvider.Setup(x => x.GetObstacleById(1)).Throws(new Exception("Failover called for not failover Obstacle"));

            //Act
            var result = obstacleService.GetObstacle(1, false);

            //Assert
            Assert.IsNotNull(result);
        }


    }
}

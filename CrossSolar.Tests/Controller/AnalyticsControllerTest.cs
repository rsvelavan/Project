using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using CrossSolar.Controllers;
using CrossSolar.Models;
using CrossSolar.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;
using CrossSolar.Domain;
using System;
using Microsoft.EntityFrameworkCore;

namespace CrossSolar.Tests.Controller
{
    public class AnalyticsControllerTest
    {
        public AnalyticsControllerTest()
        {
            _analyticsController = new AnalyticsController(_analyticsRepositoryMock.Object, _panelRepositoryMock.Object);

        }

        private  AnalyticsController _analyticsController;
        private  Mock<IAnalyticsRepository> _analyticsRepositoryMock = new Mock<IAnalyticsRepository>();

        private readonly Mock<IPanelRepository> _panelRepositoryMock = new Mock<IPanelRepository>();

        
        [Fact]
        public async Task Analytics_Add()
        {
            // Arrange
            var panel = new OneHourElectricityInsert
            {
                KiloWatt = 195,
                PanelId = "Panel10009"
            };
            var controller = new AnalyticsController(_analyticsRepositoryMock.Object, _panelRepositoryMock.Object);
            var result = await controller.Post(panel);
            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public void Values_Get_Specific()
        {
            // Arrange
            var controller = new AnalyticsController(_analyticsRepositoryMock.Object, _panelRepositoryMock.Object);
            string PanelId = "Rsvelavan";
            // Act
           var result =  controller.Get(PanelId);
            // Assert
            Assert.NotNull(result);
        }

        
        [Fact]
        public async Task DayResult_ShouldGetDetails()
        {
            // Arrange

            // Act

            var result = await _analyticsController.DayResults("08-07-2018");

            // Assert
            Assert.NotNull(result);

        }
    }
    
}

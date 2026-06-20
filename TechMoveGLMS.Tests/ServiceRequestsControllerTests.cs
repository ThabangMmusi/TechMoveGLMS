using Xunit;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.API.Controllers;
using TechMoveGLMS.Data;
using TechMoveGLMS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TechMoveGLMS.Tests
{
    public class ServiceRequestsControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task PostServiceRequest_ValidRequest_ReturnsCreated()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            
            // Add a client and an active contract
            var client = new Client { Id = 1, Name = "Test Client" };
            var contract = new Contract { Id = 1, ClientId = 1, Status = ContractStatus.Active, Title = "Active Contract" };
            context.Clients.Add(client);
            context.Contracts.Add(contract);
            await context.SaveChangesAsync();

            var controller = new ServiceRequestsController(context);
            var request = new ServiceRequest 
            { 
                ContractId = 1, 
                Title = "Test Request", 
                AmountUSD = 100,
                ExchangeRate = 18.0m,
                AmountZAR = 1800.0m
            };

            // Act
            var result = await controller.PostServiceRequest(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedRequest = Assert.IsType<ServiceRequest>(createdAtActionResult.Value);
            Assert.Equal("Test Request", returnedRequest.Title);
        }

        [Fact]
        public async Task PostServiceRequest_ExpiredContract_ReturnsBadRequest()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            
            // Add an expired contract
            var contract = new Contract { Id = 1, Status = ContractStatus.Expired, Title = "Expired Contract" };
            context.Contracts.Add(contract);
            await context.SaveChangesAsync();

            var controller = new ServiceRequestsController(context);
            var request = new ServiceRequest { ContractId = 1, Title = "Should Fail" };

            // Act
            var result = await controller.PostServiceRequest(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Contract is Expired", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task PostServiceRequest_OnHoldContract_ReturnsBadRequest()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            
            // Add an on-hold contract
            var contract = new Contract { Id = 1, Status = ContractStatus.OnHold, Title = "On Hold Contract" };
            context.Contracts.Add(contract);
            await context.SaveChangesAsync();

            var controller = new ServiceRequestsController(context);
            var request = new ServiceRequest { ContractId = 1, Title = "Should Fail" };

            // Act
            var result = await controller.PostServiceRequest(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Contract is OnHold", badRequestResult.Value.ToString());
        }
    }
}

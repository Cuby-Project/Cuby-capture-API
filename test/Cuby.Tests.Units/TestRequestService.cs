using Cuby.Data;
using Cuby.Data.dto;
using Cuby.Data.Models;
using Cuby.Services.impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Cuby.Tests.Units
{
    [TestClass]
    public sealed class TestRequestService
    {
        public required RequestService _requestService;
        public required RequestDbContext _context;

        [TestInitialize]
        public void TestInit()
        {
            DbContextOptions<RequestDbContext> options = new DbContextOptionsBuilder<RequestDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new RequestDbContext(options);
            _requestService = new RequestService(_context, new LoggerFactory().CreateLogger<RequestService>());
        }

        [TestMethod]
        public async Task InitiateRequestShouldReturnRequestId()
        {
            // Act
            string result = await _requestService.InitiateRequest();

            // Assert
            Assert.IsTrue(result.Length > 0);
        }

        [TestMethod]
        public async Task InitiateRequestShouldCreateRequestInDb()
        {
            // Act
            string result = await _requestService.InitiateRequest();

            // Assert
            Assert.AreEqual(result, _context.Requests.FindAsync(result).Result.Id);
        }

        [TestMethod]
        public async Task AddStepDoneShouldAddStepToRequest()
        {
            // Arrange
            string requestId = await _requestService.InitiateRequest();
            RequestSteps step = RequestSteps.WaitingForUserCapture;

            // Act
            await _requestService.AddStepDone(requestId, step);

            // Assert
            Request request = await _context.Requests.FindAsync(requestId);
            Assert.IsTrue(request.StepsDone.Contains(step));
        }

        [TestMethod]
        public async Task AddStepDoneShouldThrowArgumentException_WhenRequestIdIsEmpty()
        {
            // Act
            async Task action() => await _requestService.AddStepDone("", RequestSteps.WaitingForUserCapture);

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(action);
        }

        [TestMethod]
        public async Task AddStepDoneShouldThrowArgumentNullException_WhenRequestIdIsNull()
        {
            // Act
            async Task action() => await _requestService.AddStepDone(null, RequestSteps.WaitingForUserCapture);

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(action);
        }

        [TestMethod]
        public async Task AddStepDoneShouldThrowArgumentExceptionWhenRequestNotFound()
        {
            // Act
            async Task action() => await _requestService.AddStepDone("non-existent-id", RequestSteps.WaitingForUserCapture);

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(action);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Ensure the database is deleted
            _context.Database.EnsureDeleted();
        }
    }
}

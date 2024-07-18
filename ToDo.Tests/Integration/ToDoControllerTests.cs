using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Moq;
using ToDo.Server.Services.Interfaces;
using ToDo.Server.Models.Entities;
using ToDo.Tests.Helpers;
using Newtonsoft.Json;
using ToDo.Server.Models.Dtos;

namespace Tests.Integration
{
    public class ToDoControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;

        public ToDoControllerTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("/ToDo")]
        public async Task GetAllToDos_ReturnsSuccessStatusCode(string url)
        {
            // Arrange
            _factory.MockService.Setup(service => service.GetAll())
                .ReturnsAsync(new List<Item> { new Item { Id = Guid.NewGuid(), ToDo = "Test", IsChecked = false, CreatedAt = DateTime.Now } });

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.NotNull(content);
        }

        [Theory]
        [InlineData("/ToDo")]
        public async Task GetAllToDos_ReturnsNotFoundStatusCode(string url)
        {
            // Arrange
            _factory.MockService.Setup(service => service.GetAll())
                .ReturnsAsync((List<Item>)null);

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/ToDo")]
        public async Task GetAllToDos_ReturnsInternalServerStatusCode(string url)
        {
            // Arrange
            _factory.MockService.Setup(service => service.GetAll())
                .Throws(new Exception("Error"));

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Theory]
        [InlineData("/ToDo/GetToDoListById")]
        public async Task GetToDosById_ReturnsSuccessStatusCode(string url)
        {

            // Arrange

            var testItem = new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };

            _factory.MockService.Setup(service => service.GetItem(testItem.Id))
                .ReturnsAsync(testItem);

            // Act
            url = url + $"/{testItem.Id}";
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
            Assert.NotNull(content);
        }

        [Theory]
        [InlineData("/ToDo/GetToDoListById")]
        public async Task GetToDosById_ReturnsNotFoundStatusCode(string url)
        {
            // Arrange

            var testItem = new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };

            _factory.MockService.Setup(service => service.GetItem(testItem.Id))
                .ReturnsAsync((Item) null);


            // Act
            url = url + $"/{testItem.Id}";
            var response = await _client.GetAsync(url);

            // Assert
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/ToDo/GetToDoListById")]
        public async Task GetToDosById_ReturnsInternalServerStatusCode(string url)
        {
            // Arrange
            var testItem = new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };

            _factory.MockService.Setup(service => service.GetItem(testItem.Id))
                .Throws(new Exception("Error"));

            // Act
            url = url + $"/{testItem.Id}";
            var response = await _client.GetAsync(url);


            // Assert
            var content = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Theory]
        [InlineData("/ToDo/ToggleItem")]
        public async Task ToggleToDoItem_ReturnsSuccessStatusCode(string url)
        {
            // Arrange
            var testItemId = Guid.NewGuid();
            var testItem = new Item { Id = testItemId, CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };
            var toggleCompleteDto = new ToggleCompleteDto { IsChecked = true };

            _factory.MockService.Setup(service => service.GetItem(testItemId))
                .ReturnsAsync(testItem);

            _factory.MockService.Setup(service => service.ToggleItem(testItemId, toggleCompleteDto.IsChecked))
                .Returns(Task.CompletedTask);

            var content = new StringContent(JsonConvert.SerializeObject(toggleCompleteDto), Encoding.UTF8, "application/json");


            // Act
            url = url + $"/{testItem.Id}";
            var response = await _client.PatchAsync(url, content);

            // Assert
            var returnedContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("/ToDo/ToggleItem")]
        public async Task ToggleToDoItem_ReturnsNotFoundStatusCode(string url)
        {
            // Arrange
            var testItemId = Guid.NewGuid();
            var testItem = new Item { Id = testItemId, CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };
            var toggleCompleteDto = new ToggleCompleteDto { IsChecked = true };

            _factory.MockService.Setup(service => service.GetItem(testItemId))
                .ReturnsAsync((Item)null);

            _factory.MockService.Setup(service => service.ToggleItem(testItemId, toggleCompleteDto.IsChecked))
                .Returns(Task.CompletedTask);

            var content = new StringContent(JsonConvert.SerializeObject(toggleCompleteDto), Encoding.UTF8, "application/json");


            // Act
            url = url + $"/{testItem.Id}";
            var response = await _client.PatchAsync(url, content);

            // Assert
            var returnedContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/ToDo/ToggleItem")]
        public async Task ToggleToDoItem_ReturnsInternalServerStatusCode(string url)
        {
            // Arrange
            var testItemId = Guid.NewGuid();
            var testItem = new Item { Id = testItemId, CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };
            var toggleCompleteDto = new ToggleCompleteDto { IsChecked = true };

            _factory.MockService.Setup(service => service.GetItem(testItemId))
                .ReturnsAsync(testItem);

            _factory.MockService.Setup(service => service.ToggleItem(testItemId, toggleCompleteDto.IsChecked))
                .Throws(new Exception("Error"));

            var content = new StringContent(JsonConvert.SerializeObject(toggleCompleteDto), Encoding.UTF8, "application/json");


            // Act
            url = url + $"/{testItem.Id}";
            var response = await _client.PatchAsync(url, content);

            // Assert
            var returnedContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Theory]
        [InlineData("/ToDo")]
        public async Task DeleteToDoItem_ReturnsSuccessStatusCode(string url)
        {
            // Arrange
            var testItemId = Guid.NewGuid();
            var testItem = new Item { Id = testItemId, CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };

            _factory.MockService.Setup(service => service.GetItem(testItemId))
                .ReturnsAsync(testItem);

            _factory.MockService.Setup(service => service.Delete(testItemId))
                .Returns(Task.CompletedTask);

            // Act
            url = url + $"/{testItem.Id}";
            var response = await _client.DeleteAsync(url);

            // Assert
            var returnedContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Theory]
        [InlineData("/ToDo")]
        public async Task DeleteToDoItem_ReturnsNotFoundStatusCode(string url)
        {
            // Arrange
            var testItemId = Guid.NewGuid();
            var testItem = new Item { Id = testItemId, CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };

            _factory.MockService.Setup(service => service.GetItem(testItemId))
                .ReturnsAsync((Item)null);

            _factory.MockService.Setup(service => service.Delete(testItemId))
                .Returns(Task.CompletedTask);


            // Act
            url = url + $"/{testItem.Id}";
            var response = await _client.DeleteAsync(url);

            // Assert
            var returnedContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/ToDo")]
        public async Task DeleteToDoItem_ReturnsInternalServerStatusCode(string url)
        {
            // Arrange
            var testItemId = Guid.NewGuid();
            var testItem = new Item { Id = testItemId, CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };

            _factory.MockService.Setup(service => service.GetItem(testItemId))
                .ReturnsAsync(testItem);

            _factory.MockService.Setup(service => service.Delete(testItemId))
                .Throws(new Exception("Error"));

            // Act
            url = url + $"/{testItem.Id}";
            var response = await _client.DeleteAsync(url);

            // Assert
            var returnedContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}

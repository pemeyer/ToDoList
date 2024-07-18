using ToDo.Server.Models;
using Moq;
using ToDo.Server.Services.Interfaces;
using ToDo.Server.Services;
using ToDo.Server.Models.Entities;
using Microsoft.EntityFrameworkCore;
using ToDo.Tests.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;

namespace ToDo.Tests.Unit
{
    public class ToDoServiceTests
    {
        [Fact]
        public async void Should_ReturnsItemList_FromGetAll_()
        {
            var items = new List<Item>
            {
                new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" },
                new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = true, ToDo = "Clean the house" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Item>>();
            mockSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(items.Provider);
            mockSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(items.Expression);
            mockSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(items.ElementType);
            mockSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(() => items.GetEnumerator());

            mockSet.As<IAsyncEnumerable<Item>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Item>(items.GetEnumerator()));

            var mock = new Mock<ToDoContext>(new DbContextOptions<ToDoContext>());
            mock.Setup(c => c.Items).Returns(mockSet.Object);

            IToDoService service = new ToDoService(mock.Object);
            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Feed the cat", result[0].ToDo.ToString());
            Assert.Equal("Clean the house", result[1].ToDo.ToString());
        }

        [Fact]
        public async void ShouldDelete_Item()
        {
            var testItem = new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };

            var items = new List<Item>
            {
                testItem,
                new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = true, ToDo = "Clean the house" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Item>>();
            mockSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(items.Provider);
            mockSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(items.Expression);
            mockSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(items.ElementType);
            mockSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(() => items.GetEnumerator());

            mockSet.As<IAsyncEnumerable<Item>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Item>(items.GetEnumerator()));

            var mock = new Mock<ToDoContext>(new DbContextOptions<ToDoContext>());
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) =>
                {
                    var id = (Guid)ids[0];
                    return items.FirstOrDefault(i => i.Id == id);
                });

            mock.Setup(c => c.Items).Returns(mockSet.Object);

            IToDoService service = new ToDoService(mock.Object);
            
            await service.Delete(testItem.Id);

            mockSet.Verify(m => m.Remove(It.Is<Item>(i => i == testItem)), Times.Once);
            mock.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void ShouldGet_ItemById()
        {
            var testItem = new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };

            var items = new List<Item>
            {
                testItem,
                new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = true, ToDo = "Clean the house" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Item>>();
            mockSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(items.Provider);
            mockSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(items.Expression);
            mockSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(items.ElementType);
            mockSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(() => items.GetEnumerator());

            mockSet.As<IAsyncEnumerable<Item>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Item>(items.GetEnumerator()));

            var mock = new Mock<ToDoContext>(new DbContextOptions<ToDoContext>());
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) =>
                {
                    var id = (Guid)ids[0];
                    return items.FirstOrDefault(i => i.Id == id);
                });

            mock.Setup(c => c.Items).Returns(mockSet.Object);

            IToDoService service = new ToDoService(mock.Object);

            var result = await service.GetItem(testItem.Id);

            Assert.Equal("Feed the cat", result.ToDo);
        }

        [Fact]
        public async void ShouldToggleChecked_Item()
        {
            var testItem = new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = false, ToDo = "Feed the cat" };

            var items = new List<Item>
            {
                testItem,
                new Item { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, IsChecked = true, ToDo = "Clean the house" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Item>>();
            mockSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(items.Provider);
            mockSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(items.Expression);
            mockSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(items.ElementType);
            mockSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(() => items.GetEnumerator());

            mockSet.As<IAsyncEnumerable<Item>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Item>(items.GetEnumerator()));

            var mock = new Mock<ToDoContext>(new DbContextOptions<ToDoContext>());
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) =>
                {
                    var id = (Guid)ids[0];
                    return items.FirstOrDefault(i => i.Id == id);
                });

            mock.Setup(c => c.Items).Returns(mockSet.Object);
            
            IToDoService service = new ToDoService(mock.Object);
            await service.ToggleItem(testItem.Id, true);

            var result = await service.GetItem(testItem.Id);

            Assert.True(result.IsChecked);
        }
    }
}
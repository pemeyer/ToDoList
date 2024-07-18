using ToDo.Server.Models.Dtos;
using ToDo.Server.Models.Entities;

namespace ToDo.Server.Services.Interfaces
{
    public interface IToDoService
    {
        public Task Delete(Guid id);
        public Task Update(Guid id, AddItemDto itemDto);
        public Task ToggleItem(Guid id, bool IsChecked);
        public Task<Item> Add(AddItemDto itemDto);
        public Task<Item> GetItem(Guid id);
        public Task<List<Item>> GetAll();
    }
}

using ToDo.Server.Models.Dtos;
using ToDo.Server.Models.Entities;

namespace ToDo.Server.Services.Interfaces
{
    public interface IToDoService
    {
        public Task Delete(Guid id);
        public Task<Item> Add(ItemDto itemDto);
        public Task<Item> GetItem(Guid id);
        public Task<List<Item> > GetAll();
    }
}

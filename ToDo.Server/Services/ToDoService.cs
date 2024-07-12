using Microsoft.EntityFrameworkCore;
using ToDo.Server.Models;
using ToDo.Server.Models.Dtos;
using ToDo.Server.Models.Entities;
using ToDo.Server.Services.Interfaces;

namespace ToDo.Server.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ToDoContext _context;

        public ToDoService(ToDoContext context) 
        {
            _context = context; 
        }

        public async Task<Item> Add(AddItemDto itemDto)
        {
            Guid id = Guid.NewGuid();
            Item item = new Item()
            {
                Id = id,
                ToDo = itemDto.Todo,
                IsChecked = false,
                CreatedAt = DateTime.Now,
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return item;
        }

        public async Task Delete(Guid id)
        {
            var item = await GetItem(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Item>> GetAll()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetItem(Guid id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task Update(Guid id, AddItemDto itemDto)
        {
            var item = await GetItem(id);
            item.ToDo = itemDto.Todo;
            item.IsChecked = itemDto.IsChecked;
            _context.Items.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}

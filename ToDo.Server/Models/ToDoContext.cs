using Microsoft.EntityFrameworkCore;
using ToDo.Server.Models.Entities;
namespace ToDo.Server.Models
{
    public class ToDoContext : DbContext 
    {
        public ToDoContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public virtual DbSet<Item> Items { get; set; }
    }
}

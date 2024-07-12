namespace ToDo.Server.Models.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string ToDo { get; set; }
        public bool IsChecked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

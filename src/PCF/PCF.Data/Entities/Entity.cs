namespace PCF.Data.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;

    }
}

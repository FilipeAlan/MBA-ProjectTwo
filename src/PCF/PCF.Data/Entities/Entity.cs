namespace PCF.Data.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
        public DateTime? ModificadoEm { get; set; } = DateTime.UtcNow;

    }
}

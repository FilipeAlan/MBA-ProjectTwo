namespace PCF.Core.Entities.Base
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
        public DateTime? ModificadoEm { get; set; } = DateTime.UtcNow;
    }
}
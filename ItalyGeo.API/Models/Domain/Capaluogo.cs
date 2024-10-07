namespace ItalyGeo.API.Models.Domain
{
    public class Capaluogo
    {
        public Guid ComuneId { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? RegionId { get; set; }
    }
}

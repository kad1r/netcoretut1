namespace Data.Dtos
{
    public class EfConfig
    {
        public bool LazyLoading { get; set; } = false;
        public bool ProxyCreationEnabled { get; set; } = false;
        public bool AutoDetectChanges { get; set; } = false;
    }
}

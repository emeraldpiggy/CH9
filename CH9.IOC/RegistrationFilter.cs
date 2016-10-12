namespace CH9.IOC
{
    public struct RegistrationFilter
    {
        public bool RegisterProcessScoped { get; set; }
        public bool RegisterNonProcessScoped { get; set; }
        public bool RegisterTransient { get; set; }
        public bool RegisterShared { get; set; }
    }
}
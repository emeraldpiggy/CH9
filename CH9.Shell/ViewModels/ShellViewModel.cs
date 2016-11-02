namespace CH9.Shell.ViewModels{
    public class ShellViewModel :IShell
    {
        public ShellViewModel(CleaningHouseViewModel chVm)
        {
            ChVm = chVm;
        }

        public CleaningHouseViewModel ChVm { get; }
    }
}
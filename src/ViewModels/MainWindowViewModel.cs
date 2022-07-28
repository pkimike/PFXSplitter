namespace PFXSplitter.ViewModels {
    class MainWindowViewModel : AsyncViewModel {
        public MainWindowViewModel() {
            FromPfx = new FromPfxViewModel();
        }

        public FromPfxViewModel FromPfx { get; }
    }
}

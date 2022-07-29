namespace Pkcs12Converter.ViewModels {
    class MainWindowViewModel : DefaultViewModel {
        public MainWindowViewModel() {
            FromPfx = new FromPfxViewModel();
            ToPfx = new ToPfxViewModel();
        }

        public FromPfxViewModel FromPfx { get; }
        public ToPfxViewModel ToPfx { get; }
    }
}

using MauiAppShellTest.ViewModels;

namespace MauiAppShellTest
{
    public partial class App : Application
    {
        public App(AppShellViewmodel viewmodel)
        {
            InitializeComponent();

            MainPage = new AppShell(viewmodel);
        }
    }
}
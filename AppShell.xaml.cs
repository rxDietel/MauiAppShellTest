using MauiAppShellTest.ViewModels;

namespace MauiAppShellTest
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewmodel viewmodel)
        {
            BindingContext = viewmodel;
            InitializeComponent();
        }
    }
}
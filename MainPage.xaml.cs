namespace MauiAppShellTest
{
    public partial class MainPage : ContentPage
    {
        private int count;

        public MainPage() : this(null)
        {
        }

        public MainPage(string title)
        {
            InitializeComponent();
            if (title != null)
                ChangingLabel.Text = title;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
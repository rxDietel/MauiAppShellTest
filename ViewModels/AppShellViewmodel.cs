using CommunityToolkit.Mvvm.Input;

namespace MauiAppShellTest.ViewModels
{
    public partial class AppShellViewmodel
    {
        private static bool _updateTriggerEnabled = true;
        private IList<ShellItem> ShellItems => Shell?.Items;

        private ShellItem Root => ShellItems.First(item => item.Route == "Items");

        private static Shell Shell => Shell.Current;

        [RelayCommand]
        private void AddShellItem()
        {
            var name = "Test";
            var index = Random.Shared.Next(100);
            var insertAt = 0;
            while (Root.Items.Any(x => x.Title == $"{name} ({index})")) index = Random.Shared.Next(100);
            foreach (var item in Root.Items)
            {
                if (int.TryParse(item.CurrentItem.AutomationId, out var currentIndex) && currentIndex > index) break;

                insertAt++;
            }

            InsertShellItem(new ShellContent
            {
                Title = $"{name} ({index})",
                ContentTemplate = new DataTemplate(() => new MainPage($"{name} ({index})")),
                AutomationId = index.ToString()
            }, insertAt);
        }

        [RelayCommand]
        private void DeleteShellItem()
        {
            if (Root.Items.Count > 0)
            {
                var item = Root.Items.ElementAt(Random.Shared.Next(Root.Items.Count - 1));
                if (Root.Items.Count == 1)
                    Root.IsVisible = false;
                item.IsVisible = false;
                Root.Items.Remove(item);
                Root.IsVisible = true;
            }
        }

        [RelayCommand]
        private async Task RemoveHome()
        {
            var item = ShellItems.FirstOrDefault(item => item.Title == "Home");
            if (item != null)
            {
                item.IsVisible = false;

                await Task.Delay(5000);


                item.IsVisible = true;

                TriggerFlyoutUpdate();
            }
        }

        [RelayCommand]
        private async Task Sort()
        {
            using var trigger = new UpdateFlyoutTrigger(500);
            var store = Shell.CurrentItem;
            foreach (var item in ShellItems)
            {
                item.IsVisible = false;
                await Task.Delay(5);
                item.IsVisible = true;
            }

            await Shell.GoToAsync(new ShellNavigationState($"///{store.Route}"));
        }

        [RelayCommand]
        private void DisableTrigger()
        {
            _updateTriggerEnabled = !_updateTriggerEnabled;
        }

        private void InsertShellItem(ShellContent item, int index = -1)
        {
            Shell.Dispatcher.Dispatch(() =>
            {
                Root.IsVisible = false;
                if (index == -1) Root.Items.Add(item);
                else Root.Items.Insert(index, item);
                Root.IsVisible = true;
                Shell.CurrentItem = item;
                _ = Sort();
            });
        }

        private void TriggerFlyoutUpdate()
        {
            using var trigger = new UpdateFlyoutTrigger();
        }

        private class UpdateFlyoutTrigger : IDisposable
        {
            private readonly int _delay;
            private readonly bool _hasTriggered;

            public UpdateFlyoutTrigger(int delay = 100)
            {
                _delay = delay;
                if (Shell.FlyoutBehavior == FlyoutBehavior.Locked && _updateTriggerEnabled)
                {
                    _hasTriggered = true;
                    Shell.FlyoutBehavior = FlyoutBehavior.Flyout;
                }
            }

            public void Dispose()
            {
                if (_hasTriggered)
                    Task.Run(async () =>
                    {
                        var delay = _delay;
                        await Task.Delay(delay);
                        Shell.Dispatcher.Dispatch(() => Shell.FlyoutBehavior = FlyoutBehavior.Locked);
                    });
            }
        }
    }
}
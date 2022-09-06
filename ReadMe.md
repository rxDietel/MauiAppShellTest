# Maui Shell Test Aplication
Application created for working around various bugs and inconveniences of AppShell

## 1. Titlebar not changing
### Description:
The titlebar won't change upon changing between sub items of a flyout item.

### Reproduction:
- Start App
- click on `add Shell Item` until you got 2-3 Test pages
- switch between Test pages and ocassionaly to home page

### Expected:
- App shows title of current page

### Happening:
- Title will change to current page if switching from Home to any Test page and back.
- Tilte won't change if switching between Test pages.

### Workaround
- none

## 2. Flyout Items won't update correctly
### Description
Flyout won't update with correct content if `Shell.FlyoutBehavior="Locked"` until Flyout is hidden or page Resize. This also happens if you change the language in a localized app.

### Reproduction:
- Start App
- click on `toggle Update Trigger`
- add or remove Tabs

### Expected:
- Flyout will update upon changes

### Happening:
- Flyout won't update until the window is resized

### Workaround:
After each update to the Flyout Menu:

- Change from `Shell.FlyoutBehavior="Locked"` to `Shell.FlyoutBehavior="Flyout"`
- `await Task.Delay(>= 100ms)`
- Change back to `Shell.FlyoutBehavior="Locked"` from `Shell.FlyoutBehavior="Flyout"`

## 3. Flyout Items sorting
### Description:
FlyoutItems aren't sorted on Windows as expected.

### Reproduction:
- Start App
- click on `remove home for some time`

### Expected
- Home Flyout disapears upon click `item.IsVisible = false;`
- Home Flyout reapears in the same place after 5 seconds `item.IsVisible = true;`

### Happening
- Home Flyout disapears upon click `item.IsVisible = false;`
- Home Flyout reapears at the end of the list after 5 seconds `item.IsVisible = true;`

### Workaround:
Hide all content and let it reapear in order.
```
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
```

## 4. AppShell crashes upon adding/removing items
### Description:
Upon adding or removing FlyoutItems during runtime, the AppShell occasionally crashes with nullReferenceExceptions.

### Reprodution:
#### 1. Add Items:
- Start App
- click on `disable add workaround`
- click on `add Shell Item`
- click on `add Shell Item`
=> App will crash

#### 2. Remove Items:
- Start App
- click on `disable remove workaround`
- click on `add Shell Item`
- click on `add Shell Item`
- click on `add Shell Item`
- click on `remove Shell Item`
- click on `remove Shell Item`
- click on `remove Shell Item`
=> App will crash

### Expected:
- Items will be removed

### Happening:
- App throws NullReferenceException

### Workaround
- Workaround is instable
- Make item invisible before removing
- In some cases Make containing item invisible before removing
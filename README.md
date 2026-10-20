# GearCalc – WinForms vs. WPF vs. WPF with MVVM

The same tiny app built three ways. Enter module, number of teeth and pressure angle; it calculates the pitch, tip and base diameter of a gear.

## How to run
Open `GearCalc.sln` in Visual Studio 2022 (Windows), right-click a project → **Set as Startup Project** → F5.
Requires the .NET 8 SDK with the "Desktop development" workload.

## Read the projects in this order

### 1_WinForms – what you already know
- `MainForm.cs`: controls created in C# with **fixed pixel positions** (`Location = new Point(140, 130)`)
- `CalculateButton_Click`: the logic **reads the TextBoxes and writes the Label directly**

### 2_WpfCodeBehind – WPF, but used like WinForms
- `MainWindow.xaml`: the UI is **described in XAML**; a **Grid** with rows and columns replaces pixel positions
- `MainWindow.xaml.cs`: still a Click handler that accesses controls by `x:Name`
- Lesson: WPF *can* be used like WinForms, but you miss its main strength

### 3_WpfMvvm – the real WPF way
| File | Role |
|---|---|
| `GearCalculator.cs` | **Model**: the calculation |
| `MainViewModel.cs` | **ViewModel**: screen state (properties) and actions (command), no UI types |
| `MainWindow.xaml` | **View**: layout and `{Binding ...}` only |
| `RelayCommand.cs` | Helper that turns a method into a command |

What happens when you type and click:
1. Typing in a TextBox updates `Module`, `Teeth` or `PressureAngle` in the ViewModel (**two-way binding**)
2. WPF asks `CanCalculate()`, so the button is **disabled automatically** for invalid input
3. Clicking runs `Calculate()` through the **command**
4. `Calculate()` sets `PitchDiameter` etc.; `SetField` raises **PropertyChanged**
5. The result TextBlocks **update themselves**: the ViewModel never touches a control

## The differences in one table

| | WinForms | WPF code-behind | WPF MVVM |
|---|---|---|---|
| UI defined in | C# | XAML | XAML |
| Layout | Fixed pixels | Grid (adapts to size) | Grid (adapts to size) |
| Button logic | `Click` handler | `Click` handler | `Command` |
| Getting input | `_moduleBox.Text` | `ModuleBox.Text` | Binding → `Module` property |
| Showing results | `_resultLabel.Text = ...` | `ResultText.Text = ...` | Binding ← `PitchDiameter` property |
| Disabling the button | Manual code | Manual code | Automatic via `CanExecute` |
| Testable without a window | No | No | **Yes**, the ViewModel is a plain class |

## Why MVVM is testable
```csharp
var vm = new MainViewModel { Module = "2", Teeth = "30", PressureAngle = "20" };
vm.CalculateCommand.Execute(null);
Assert.Equal(60.0, vm.PitchDiameter);   // no window, no button, no TextBox
```

## Next step: Prism
Prism provides ready-made versions of the helpers written by hand here:
- `INotifyPropertyChanged` + `SetField` → Prism's `BindableBase` + `SetProperty`
- `RelayCommand` → Prism's `DelegateCommand`

On top, Prism adds modules, regions, the EventAggregator and dependency injection for large applications.

## Exercises
1. In `3_WpfMvvm`, add a **root diameter** result: df = m · (z − 2.5)
2. Add a **tooth height** result: h = 2.25 · m
3. Make the window wider in the designer and watch the Grid adapt; compare with the WinForms window

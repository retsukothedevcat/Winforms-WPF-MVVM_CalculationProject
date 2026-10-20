using System.Windows.Input;

namespace GearCalc.WpfMvvm;

// A command = an action the View can trigger, plus a rule for when it is allowed.
// Replaces the Click event handler. (Prism provides the same thing as DelegateCommand.)
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    // WPF calls this to decide whether the button is enabled.
    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    // WPF calls this when the button is clicked.
    public void Execute(object? parameter) => _execute();

    // Lets WPF re-check CanExecute automatically, e.g. after the user types.
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}

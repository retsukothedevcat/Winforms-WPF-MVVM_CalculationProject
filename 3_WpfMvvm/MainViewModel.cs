using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GearCalc.WpfMvvm;

// VIEWMODEL: holds the state of the screen and the actions.
// It knows NOTHING about TextBoxes or Buttons -> it can be unit-tested without a window.
public class MainViewModel : INotifyPropertyChanged
{
    // ---- Inputs (bound to the TextBoxes) ----
    private string _module = "2";
    private string _teeth = "30";
    private string _pressureAngle = "20";

    public string Module        { get => _module;        set => SetField(ref _module, value); }
    public string Teeth         { get => _teeth;         set => SetField(ref _teeth, value); }
    public string PressureAngle { get => _pressureAngle; set => SetField(ref _pressureAngle, value); }

    // ---- Results (bound to the result TextBlocks) ----
    private double _pitchDiameter;
    private double _tipDiameter;
    private double _baseDiameter;

    public double PitchDiameter { get => _pitchDiameter; private set => SetField(ref _pitchDiameter, value); }
    public double TipDiameter   { get => _tipDiameter;   private set => SetField(ref _tipDiameter, value); }
    public double BaseDiameter  { get => _baseDiameter;  private set => SetField(ref _baseDiameter, value); }

    // ---- Action (bound to the Button) ----
    public ICommand CalculateCommand { get; }

    public MainViewModel()
    {
        CalculateCommand = new RelayCommand(Calculate, CanCalculate);
    }

    private bool CanCalculate() => TryReadInputs(out _, out _, out _);

    private void Calculate()
    {
        if (!TryReadInputs(out double m, out int z, out double alpha)) return;

        // Only set properties. The View updates itself through data binding.
        PitchDiameter = GearCalculator.PitchDiameter(m, z);
        TipDiameter   = GearCalculator.TipDiameter(m, z);
        BaseDiameter  = GearCalculator.BaseDiameter(m, z, alpha);
    }

    private bool TryReadInputs(out double m, out int z, out double alpha)
    {
        z = 0;
        alpha = 0;
        return double.TryParse(Module, out m) && m > 0
            && int.TryParse(Teeth, out z) && z >= 3
            && double.TryParse(PressureAngle, out alpha) && alpha > 0 && alpha < 45;
    }

    // ---- INotifyPropertyChanged: tells the View "this property changed, refresh it" ----
    public event PropertyChangedEventHandler? PropertyChanged;
     // Helper method to set a field and raise the PropertyChanged event if the value changed. 
    //notice the T generic type parameter, which allows this method to work with any type of field.
    ///notice the [CallerMemberName] attribute, which automatically provides the name of the calling property, so you don't have to specify it manually.
    /// notice the use of EqualityComparer<T>.Default.Equals to compare the old and new values, which works for both value types and reference types.
    /// notice the ref keyword, which allows the method to modify the original field passed in as an argument.
    /// notice propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); which raises the PropertyChanged event if there are any subscribers.
    /// notice ?. This is the null-conditional operator, which checks if PropertyChanged is not null before invoking it, preventing a NullReferenceException.
    /// notice the string? propertyName = null parameter, which allows
     private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) 

    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}











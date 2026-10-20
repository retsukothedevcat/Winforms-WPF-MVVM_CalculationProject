using System.Windows;

namespace GearCalc.WpfCodeBehind;

// Code-behind: works like WinForms. Controls are accessed by their x:Name.
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();   // loads the XAML
    }

    private void CalculateButton_Click(object sender, RoutedEventArgs e)
    {
        if (!double.TryParse(ModuleBox.Text, out double m) || m <= 0 ||
            !int.TryParse(TeethBox.Text, out int z) || z < 3 ||
            !double.TryParse(AngleBox.Text, out double alpha) || alpha <= 0 || alpha >= 45)
        {
            ResultText.Text = "Please enter valid numbers.";
            return;
        }

        ResultText.Text =
            $"Pitch diameter d  = {GearCalculator.PitchDiameter(m, z):0.000} mm\n" +
            $"Tip diameter da   = {GearCalculator.TipDiameter(m, z):0.000} mm\n" +
            $"Base diameter db  = {GearCalculator.BaseDiameter(m, z, alpha):0.000} mm";
    }
}

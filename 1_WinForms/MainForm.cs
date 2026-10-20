namespace GearCalc.WinForms;

// WinForms: the UI is built in C# code. Normally the Visual Studio designer writes
// this part into MainForm.Designer.cs; it is written by hand here so everything is in one file.
public class MainForm : Form
{
    private readonly TextBox _moduleBox = new() { Text = "2" };
    private readonly TextBox _teethBox = new() { Text = "30" };
    private readonly TextBox _angleBox = new() { Text = "20" };
    private readonly Button _calculateButton = new() { Text = "Calculate" };
    private readonly Label _resultLabel = new() { AutoSize = true };

    public MainForm()
    {
        Text = "Gear calculator (WinForms)";
        ClientSize = new Size(360, 260);

        // Every control gets a fixed pixel position.
        AddRow("Module m [mm]", _moduleBox, 20);
        AddRow("Number of teeth z", _teethBox, 55);
        AddRow("Pressure angle [°]", _angleBox, 90);

        _calculateButton.Location = new Point(140, 130);
        _calculateButton.Click += CalculateButton_Click;   // event handler
        Controls.Add(_calculateButton);

        _resultLabel.Location = new Point(20, 175);
        Controls.Add(_resultLabel);
    }

    private void AddRow(string labelText, TextBox box, int y)
    {
        Controls.Add(new Label { Text = labelText, Location = new Point(20, y + 3), AutoSize = true });
        box.Location = new Point(140, y);
        box.Width = 120;
        Controls.Add(box);
    }

    // The logic lives in the event handler: read controls, calculate, write controls.
    private void CalculateButton_Click(object? sender, EventArgs e)
    {
        if (!double.TryParse(_moduleBox.Text, out double m) || m <= 0 ||
            !int.TryParse(_teethBox.Text, out int z) || z < 3 ||
            !double.TryParse(_angleBox.Text, out double alpha) || alpha <= 0 || alpha >= 45)
        {
            _resultLabel.Text = "Please enter valid numbers.";
            return;
        }

        _resultLabel.Text =
            $"Pitch diameter d  = {GearCalculator.PitchDiameter(m, z):0.000} mm\n" +
            $"Tip diameter da   = {GearCalculator.TipDiameter(m, z):0.000} mm\n" +
            $"Base diameter db  = {GearCalculator.BaseDiameter(m, z, alpha):0.000} mm";
    }
}

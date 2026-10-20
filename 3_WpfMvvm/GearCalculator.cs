namespace GearCalc.WpfMvvm;

// MODEL: pure calculation, no UI. Identical in all three versions.
public static class GearCalculator
{
    // Pitch (reference) diameter: d = m * z
    public static double PitchDiameter(double module, int teeth) => module * teeth;

    // Tip diameter: da = m * (z + 2)
    public static double TipDiameter(double module, int teeth) => module * (teeth + 2);

    // Base diameter (where the involute starts): db = d * cos(alpha)
    public static double BaseDiameter(double module, int teeth, double pressureAngleDeg) =>
        PitchDiameter(module, teeth) * Math.Cos(pressureAngleDeg * Math.PI / 180.0);
}

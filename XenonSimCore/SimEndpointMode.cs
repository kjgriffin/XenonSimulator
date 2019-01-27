using System;
using System.Runtime;

public enum SimEndpointMode
{
    DEFAULTUNKOWN,
    StrongDriver,
    WeakDriver,
    Floating,
    LowImpedanceInput,
    HighImpedanceInput,
    INPUT,
    OUTPUT,
}

public static class SimEndpointModeStringExtensions
{
    public static SimEndpointMode ToSimEndpointMode(this string str)
    {
        SimEndpointMode mode = SimEndpointMode.DEFAULTUNKOWN;
        if (Enum.TryParse(str, true, out mode))
        {
            return mode;
        }
        Console.WriteLine(string.Format("Unknown SimEndpointMode {0}", str));
        return mode;
    }
}
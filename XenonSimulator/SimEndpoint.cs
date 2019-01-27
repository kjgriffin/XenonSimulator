using System;
using System.Collections.Generic;
using System.Text;
using Xenon.Sim.Core;

namespace Xenon.Simulator
{
    public class SimEndpoint : ISimEndpoint
    {
        string guid;
        string ISimEndpoint.Guid { get => guid; set => guid = value; }
        string dtype;
        string ISimEndpoint.DataType { get => dtype; set => dtype = value; }
        SimNodeStatus ISimEndpoint.Status { get; set; }
        ISimData Value = new SimData();

        SimEndpointMode mode = SimEndpointMode.DEFAULTUNKOWN;

        SimEndpointMode ISimEndpoint.GetMode()
        {
            return mode;
        }

        ISimData ISimEndpoint.GetValue()
        {
            if (Value == null)
            {
                Value = new SimData();
                Value.SimpleData = "ERROR PIPELINE BUBBLE";
            }
            return Value;
        }

        void ISimEndpoint.SetMode(SimEndpointMode mode)
        {
            this.mode = mode;
        }

        void ISimEndpoint.SetValue(ISimData value)
        {
            Value = value;   
        }

        void ISimEndpoint.DebugLog()
        {
            Console.WriteLine(string.Format("SimEndpoint: {0}, datatype:{1}", guid, dtype));
        }
    }
}

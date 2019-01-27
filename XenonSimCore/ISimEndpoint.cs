using System;
using System.Collections.Generic;
using System.Text;

namespace Xenon.Sim.Core
{
    public interface ISimEndpoint
    {
        string Guid { get; set; }

        string DataType { get; set; }
        SimNodeStatus Status { get; set; }

        ISimData GetValue();
        void SetValue(ISimData value);

        SimEndpointMode GetMode();
        void SetMode(SimEndpointMode mode);

        void DebugLog();

    }
}

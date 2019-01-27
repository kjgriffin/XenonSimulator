using System;
using System.Collections.Generic;
using System.Text;

namespace Xenon.Sim.Core
{
    public interface ISimNode
    {
        string Guid { get; set; }
        SimNodeStatus Status { get; set; }
        ISimData Value { get; set; }

        void AddConnection(string id);
        void Update(int g_tick, Func<string, ISimModule> getModule, Func<string, ISimEndpoint> getPort, Func<string, ISimNode> getNode);

        void DebugLog();

    }
}

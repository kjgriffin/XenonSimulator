using System;
using System.Collections.Generic;
using System.Text;
using Xenon.Sim.Core;

namespace Xenon.Simulator
{
    public class SimNode : ISimNode
    {
        string guid;
        string ISimNode.Guid { get => guid; set => guid = value; }
        SimNodeStatus status;
        SimNodeStatus ISimNode.Status { get => status; set => status = value; }
        ISimData value;
        ISimData ISimNode.Value { get => value; set=> this.value = value; }

        List<string> Connections = new List<string>();



        void ISimNode.AddConnection(string id)
        {
            Connections.Add(id);
        }

        void ISimNode.Update(int g_tick, Func<string, ISimModule> getModule, Func<string, ISimEndpoint> getPort, Func<string, ISimNode> getNode)
        {

            Console.WriteLine("SimNode:{0} g_tick{1}", guid, g_tick);
            // for now just let everything try to drive

            foreach(string id in Connections)
            {
                if (getPort(id).GetMode() == SimEndpointMode.OUTPUT)
                {
                    value = getPort(id).GetValue();
                }
            }

            foreach (string id in Connections)
            {
                if (getPort(id).GetMode() == SimEndpointMode.INPUT)
                {
                    getPort(id).SetValue(value);
                }
            }

        }

        void ISimNode.DebugLog()
        {
            Console.WriteLine(string.Format("SimNode: {0}", guid));
            foreach (string con in Connections)
            {
                Console.WriteLine(string.Format("\t{0}", con));
            }
        }
    }
}

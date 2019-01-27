using System;
using System.Collections.Generic;
using System.Text;
using Xenon.Sim.Core;

namespace Xenon.Simulator
{
    public class SimUniverse
    {


        const string DEFAULTPORTGUID = "$$DEFAULTNOPORT";

        Dictionary<string, ISimNode> AllNodes;
        Dictionary<string, ISimEndpoint> AllPorts;
        Dictionary<string, ISimModule> AllModules;

        List<string> MySubModuleRefs;
        List<string> MyNodesRefs;


        public void AddNode(string id, ISimNode node)
        {
            AllNodes.Add(id, node);
        }

        public void AddPort(string id, ISimEndpoint port)
        {
            AllPorts.Add(id, port);
        }

        public void AddModule(string id, ISimModule module)
        {
            module.Create();
            AllModules.Add(id, module);
        }

        public void AddSubModule(string id)
        {
            MySubModuleRefs.Add(id);
        }

        public void AddInternalNode(string id)
        {
            MyNodesRefs.Add(id);
        }

        public ISimNode GetNode(string id)
        {
            return AllNodes[id];
        }

        public ISimModule GetModule(string id)
        {
            return AllModules[id];
        }

        public ISimEndpoint GetPort(string id)
        {
            if (AllPorts.ContainsKey(id))
            {
                return AllPorts[id];
            }
            else
            {
                return AllPorts[DEFAULTPORTGUID];
            }
        }


        public void Update(int g_tick)
        {
            // update every node I reference
            // update every componnet
            // between every component re-update every node of mine
            foreach (string node in MyNodesRefs)
            {
                AllNodes[node].Update(g_tick, GetModule, GetPort, GetNode);
            }
            foreach (string module in MySubModuleRefs)
            {
                AllModules[module].Update(g_tick, GetModule, GetPort, GetNode);
                foreach (string node in MyNodesRefs)
                {
                    AllNodes[node].Update(g_tick, GetModule, GetPort, GetNode);
                }
            }

        }


        public void DebugLog()
        {
            Console.WriteLine(string.Format("Universe Structure"));
            Console.WriteLine("GlobalNodes");
            foreach (string id in MyNodesRefs)
            {
                Console.WriteLine(id);
            }
            Console.WriteLine("TopLevelModules");
            foreach (string id in MySubModuleRefs)
            {
                Console.WriteLine(id);
            }
            Console.WriteLine();

            Console.WriteLine("Node Definitions");
            foreach (var node in AllNodes)
            {
                node.Value.DebugLog();
            }

            Console.WriteLine("Port Definitions");
            foreach (var port in AllPorts)
            {
                port.Value.DebugLog();
            }

            Console.WriteLine("Module Definitions");
            foreach (var module in AllModules)
            {
                module.Value.DebugLog();
            }

        }


        public void Create()
        {
            AllNodes = new Dictionary<string, ISimNode>();
            AllPorts = new Dictionary<string, ISimEndpoint>();
            AllModules = new Dictionary<string, ISimModule>();
            MyNodesRefs = new List<string>();
            MySubModuleRefs = new List<string>();

            // Add defaults
            ISimEndpoint defaultport = new SimEndpoint();
            defaultport.Guid = DEFAULTPORTGUID;
            SimData payload = new SimData();
            payload.SimpleData = "";
            defaultport.SetValue(payload);
            defaultport.DataType = "std.types.empty";
            AllPorts.Add(DEFAULTPORTGUID, defaultport);



        }


        public void Init()
        {
            // init all nodes with thier default values
            // init all submodules
        }

    }
}

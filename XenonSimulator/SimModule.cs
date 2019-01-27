using System;
using System.Collections.Generic;
using System.Text;
using Xenon.Sim.Core;

namespace Xenon.Simulator
{
    public class SimModule : ISimModule
    {
        string guid;
        string ISimModule.Guid { get => guid; set => guid = value; }

        Dictionary<string, string> myargs = new Dictionary<string, string>();
        void ISimModule.AddArg(string arg, string value)
        {
            myargs.Add(arg, value);
        }

        HashSet<string> mynodes = new HashSet<string>();
        void ISimModule.AddNode(string id)
        {
            mynodes.Add(id);
        }

        HashSet<string> myports = new HashSet<string>();
        void ISimModule.AddPort(string id)
        {
            myports.Add(id);
        }

        Dictionary<string, string> mysimargs = new Dictionary<string, string>();
        void ISimModule.AddSimArg(string simarg, string value)
        {
                mysimargs.Add(simarg, value);
        }

        HashSet<string> mysubmodules = new HashSet<string>();
        void ISimModule.AddSubModule(string id)
        {
            mysubmodules.Add(id);
        }

        void ISimModule.Init(Func<string, ISimModule> getModule)
        {
            foreach (string id in mysubmodules)
            {
                getModule(id).Init(getModule);
            }
        }

        void ISimModule.Create()
        {

        }

        void ISimModule.Render()
        {
            throw new NotImplementedException();
        }

        void ISimModule.Update(int g_tick, Func<string, ISimModule> getModule, Func<string, ISimEndpoint> getPort, Func<string, ISimNode> getNode)
        {
            // update every node I reference
            // update every componnet
            // between every component re-update every node of mine
            foreach (string node in mynodes)
            {
                getNode(node).Update(g_tick, getModule, getPort, getNode);
            }
            foreach (string module in mysubmodules)
            {
                getModule(module).Update(g_tick, getModule, getPort, getNode);
                foreach (string node in mynodes)
                {
                    getNode(node).Update(g_tick, getModule, getPort, getNode);
                }
            }
        }

        void ISimModule.DebugLog()
        {
            Console.WriteLine(string.Format("SimModule: {0}", guid));
            Console.WriteLine("Args");
            foreach (KeyValuePair<string, string> arg in myargs)
            {
                Console.WriteLine(string.Format("{0} {1}", arg.Key, arg.Value));
            }
            Console.WriteLine("SimArgs");
            foreach (KeyValuePair<string, string> arg in mysimargs)
            {
                Console.WriteLine(string.Format("{0} {1}", arg.Key, arg.Value));
            }
            Console.WriteLine("PortRefs");
            foreach (string id in myports)
            {
                Console.WriteLine(id);
            }
            Console.WriteLine("NodeRefs");
            foreach (string id in mynodes)
            {
                Console.WriteLine(id);
            }
            Console.WriteLine("SubModules");
            foreach (string id in mysubmodules)
            {
                Console.WriteLine(id);
            }


            Console.WriteLine();
        }
    }
}

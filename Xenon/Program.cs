using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using Xenon.Plugin.Framework;
using Xenon.Simulator;
using Xenon.Sim.Core;
using Xenon.Build.Engine;

namespace Xenon
{
    class Program
    {
        private static Dictionary<string, IXenonPlugin> _Plugins = new Dictionary<string, IXenonPlugin>();

        static void Main(string[] args)
        {


            XBuildEngine buildEngine = new XBuildEngine();
            buildEngine.StartEngine();
            SimUniverse u = buildEngine.GetSimUniverse();




            // try to build a simple universe
            //SimUniverse universe = new SimUniverse();
            //universe.Create();

            //// add a testbox
            //ISimModule tb = (ISimModule)Activator.CreateInstance(PluginLoader.Plugins["std.test.TestBox"]);
            //universe.AddModule("myuniverse.box1", tb);
            //universe.GetModule("myuniverse.box1").Create();
            //universe.GetModule("myuniverse.box1").AddArg("channels", "1");
            //universe.AddSubModule("myuniverse.box1");





            string s = "";
            int i = 0;
            while (true)
            {
                s = Console.ReadLine();
                u.Update(i++);
            }




        }
    }
}

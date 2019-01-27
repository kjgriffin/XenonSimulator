using System;
using System.Collections.Generic;
using System.Text;
using Xenon.Sim.Core;
using XenonPluginFramework;

namespace XenonCorePlugins
{
    class TestGen : AModule
    {
        public override string Type { get => "std.test.TestGen"; }

        public override void Update(int g_tick, Func<string, ISimModule> getModule, Func<string, ISimEndpoint> getPort, Func<string, ISimNode> getNode)
        {
            // generate some value on each output port
            Console.WriteLine(string.Format("TestGen::{0} update_tick:{1}", Guid, g_tick));

            foreach (string id in myportrefs)
            {
                // check if port is currently in output mode, if so generate a value for it
                ISimEndpoint p = getPort(id);

                if (p.GetMode() == SimEndpointMode.OUTPUT)
                {
                    ISimData payload = new SimpleSimData();
                    payload.SimpleData = System.Guid.NewGuid().ToString();
                    p.SetValue(payload);
                }

            }
            

        }


    }
}

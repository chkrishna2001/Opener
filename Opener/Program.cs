using Opener.Helpers;
using System;
using System.ComponentModel;
using Unity;

namespace Opener
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                App app = new App();
                app.Run(); 
            }
            else
            {
                var container = new UnityContainer();
                container.RegisterType<XMLSerializerNDeSerializer>();
                container.RegisterType<OpenerDataHelper>();
                container.RegisterType<OpenerKeyProcessor>();
                var processor = container.Resolve<OpenerKeyProcessor>();
                processor.ProcessArgs(args);
            }
        }
    }
}

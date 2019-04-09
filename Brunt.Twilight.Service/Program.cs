using System.ServiceProcess;


namespace Brunt.Twilight.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new BruntTwilight()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}

using Servicio_DescargarArchivo_Galileo.Capa_Negocio;
using Servicio_DescargarArchivo_Galileo.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Servicio_DescargarArchivo_Galileo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                if (Settings.Default.ModoDesarrollo)
                {
                    Servicio servicio = new Servicio();
                }
                else
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                new Service1()
                    };
                    ServiceBase.Run(ServicesToRun);
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}

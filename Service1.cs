using Servicio_DescargarArchivo_Galileo.Capa_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;


namespace Servicio_DescargarArchivo_Galileo
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            new Servicio();
        }

        protected override void OnStop()
        {
        }
    }
}

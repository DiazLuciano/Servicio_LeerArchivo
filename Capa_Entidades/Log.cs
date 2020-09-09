using Servicio_DescargarArchivo_Galileo.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio_DescargarArchivo_Galileo.Capa_Entidades
{
    public sealed class Log
    {
        static readonly object padlock = new object();
        static Log _instance = null;

        public static Log Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Log();
                    }
                    return _instance;
                }
            }
        }

        public void EscribirEnLog(string message)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(Settings.Default.RutaLog, true);

                streamWriter.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy") + "]" + message);

                streamWriter.Close();
            }
            catch (Exception ex)
            {
                StreamWriter streamWriter = new StreamWriter(Settings.Default.RutaLog, true);

                streamWriter.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy") + "]" + "ERROR: Ocurrio un error en Log.EscribirEnLog().Excepcion: " + ex.Message);

                streamWriter.Close();
            }
        }

        public void EscribirEnLogNoImportados(string message)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(Settings.Default.RutaLogNoImportados, true);

                streamWriter.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy") + "]" + message);

                streamWriter.Close();
            }
            catch (Exception ex)
            {
                StreamWriter streamWriter = new StreamWriter(Settings.Default.RutaLog, true);

                streamWriter.WriteLine("[" + DateTime.Now.ToString("dd/MM/yyyy") + "]" + "ERROR: Ocurrio un error en Log.EscribirEnLog().Excepcion: " + ex.Message);

                streamWriter.Close();
            }
        }
    }
}

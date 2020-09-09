using Servicio_DescargarArchivo_Galileo.Capa_Conexion;
using Servicio_DescargarArchivo_Galileo.Capa_Controladores;
using Servicio_DescargarArchivo_Galileo.Capa_Entidades;
using Servicio_DescargarArchivo_Galileo.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servicio_DescargarArchivo_Galileo.Capa_Negocio
{
    public class Servicio
    {
        public Thread Worker = null;

        public Servicio()
        {
            if (!Settings.Default.ModoDesarrollo)
            {
                Log.Instance.EscribirEnLog("INFO: Iniciando Servicio");

                Worker = new Thread(new ThreadStart(InicializarServicio));
                Worker.IsBackground = true; // esta linea hace que el thread termine cuando se estopea el servicio, sino sigue corriendo
                Worker.Start();
            }
            else
            {
                InicializarServicio();
            }
        }

        public void InicializarServicio()
        {
            try
            {
                Working();
            }
            catch (Exception ex)
            {
                Log.Instance.EscribirEnLog("Error en InicializarServicio.Excepcion: " + ex.Message);
            }
            finally
            {
                Worker = new Thread(new ThreadStart(InicializarServicio));
                Worker.IsBackground = true;
                Worker.Start();
            }
        }

        public void Working()
        {
            try
            {

                //Log.Instance.EscribirEnLog("INFO: entro a la funcion Working. Va a leer la ruta del directorio " + Settings.Default.RutaArchivoALeer);

                AccesoDatos accesoDatos = new AccesoDatos();

                string[] rutaArchivos = Directory.GetFiles(Settings.Default.RutaArchivoALeer);

                //Log.Instance.EscribirEnLog("INFO: Leyo el directorio, va a recorrer los archivos que estan dentro");

                foreach (var ruta in rutaArchivos)
                {
                    Log.Instance.EscribirEnLog("INFO: Voy a leer archivo " + ruta.ToString());
                    string nombreArchivo = ruta.Split(new string[] { "\\" }, StringSplitOptions.None).Last();
                    String extensionArchivo = System.IO.Path.GetExtension(ruta).ToLower();

                    Log.Instance.EscribirEnLog("INFO: Voy a verificar el tipo de extension que sea .csv");
                    String[] extensionPermitida = { ".csv" };

                    for (int i = 0; i < extensionPermitida.Length; i++)
                    {
                        if (extensionArchivo == extensionPermitida[i])
                        {
                            Log.Instance.EscribirEnLog("INFO: La extension es correcta, voy a leer las lineas del archivo");
                            using (StreamReader reader = new StreamReader(ruta))
                            {
                                string line = "";

                                while ((line = reader.ReadLine()) != null)
                                {
                                    var splitResult = line.Split(',').ToList();

                                    AgregarEnBaseDeDatos(splitResult);
                                    Log.Instance.EscribirEnLog("INFO: Voy a leer otra linea");
                                }
                            }
                        }
                    }

                    //Luego que salga del while, muevo el archivo a procesados.
                    Log.Instance.EscribirEnLog("INFO: Voy a mover el archivo procesado");
                    File.Move(Settings.Default.RutaArchivoALeer + "\\" + nombreArchivo, Settings.Default.RutaArchivoProcesado + "\\" + nombreArchivo);
                    Log.Instance.EscribirEnLog("INFO: El archivo " + nombreArchivo + " fue movido a la carpeta de procesados");

                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.EscribirEnLog("ERROR(CATCH): Ocurrio un error en Servicio.Working. Excepcion: " + ex.Message);
            }
        }

        public void AgregarEnBaseDeDatos(List<string> splitResult)
        {
            try
            {
                ControladorBaseDatos controladorBaseDatos = new ControladorBaseDatos();

                if (splitResult.Count > 4) //Inserto en IMP_ORDINI_RIGHE
                {
                    Log.Instance.EscribirEnLog("INFO: Voy a insertar registro en la base IMP_ORDINI_RIGHE");
                    string rig_ordine = splitResult[0].ToString();
                    string rig_articolo = splitResult[1].ToString();
                    decimal rig_qtar = Convert.ToDecimal(splitResult[2].ToString());

                    int resultado = controladorBaseDatos.agregar_IMP_ORDINI_RIGHE(rig_ordine, rig_articolo, rig_qtar);

                    if (resultado > 0)
                        Log.Instance.EscribirEnLog("INFO: Se inserto en la base IMP_ORDINI_RIGHE la orden " + rig_ordine);
                    else
                    {
                        Log.Instance.EscribirEnLog("ELSE: No se inserto en la base IMP_ORDINI_RIGHE la orden " + rig_ordine);
                        Log.Instance.EscribirEnLogNoImportados("No se inserto en la base IMP_ORDINI la orden " + rig_ordine);
                    }
                    
                }
                else //Inserto en IMP_ORDINI
                {
                    Log.Instance.EscribirEnLog("INFO: Voy a insertar registro en la base IMP_ORDINI");
                    string ord_ordine = splitResult[1].ToString();
                    string ord_des = splitResult[2].ToString();
                    string ord_tipoop = splitResult[3].ToString();

                    int resultado = controladorBaseDatos.agregar_IMP_ORDINI(ord_ordine, ord_des, ord_tipoop);
                    if (resultado > 0)
                        Log.Instance.EscribirEnLog("Se inserto en la base IMP_ORDINI la orden " + ord_ordine);
                    else
                    {
                        Log.Instance.EscribirEnLogNoImportados("No se inserto en la base IMP_ORDINI la orden " + ord_ordine);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Instance.EscribirEnLog("ERROR(CATCH): Ocurrio un error en Servicio.AgregarEnBaseDeDatos. Excepcion: " + ex.Message);
            }
        }
    }
}

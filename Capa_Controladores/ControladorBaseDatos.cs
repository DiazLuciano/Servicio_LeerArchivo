using Servicio_DescargarArchivo_Galileo.Capa_Conexion;
using Servicio_DescargarArchivo_Galileo.Capa_Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio_DescargarArchivo_Galileo.Capa_Controladores
{
    public class ControladorBaseDatos
    {
        AccesoDatos accesoDatos = new AccesoDatos();

        public int agregar_IMP_ORDINI(string ord_ordine,string ord_des, string ord_tipoop)
        {
            try
            {
                AccesoDatos accesoDatos = new AccesoDatos();
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_INSERTAR_IMP_ORDINI";

                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@ord_ordine";
                param1.Direction = ParameterDirection.Input;
                param1.SqlDbType = SqlDbType.VarChar;
                param1.Value = ord_ordine;
                command.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@ord_des";
                param2.Direction = ParameterDirection.Input;
                param2.SqlDbType = SqlDbType.VarChar;
                param2.Value = ord_des;
                command.Parameters.Add(param2);

                SqlParameter param3 = new SqlParameter();
                param3.ParameterName = "@ord_tipoop";
                param3.Direction = ParameterDirection.Input;
                param3.SqlDbType = SqlDbType.VarChar;
                param3.Value = ord_tipoop;
                command.Parameters.Add(param3);

                int i = accesoDatos.ejecQueryDevuelveInt(command);

                return 1;
            }
            catch (Exception ex)
            {
                Log.Instance.EscribirEnLogNoImportados("ERROR: No se inserto en la BD la orden: " + ord_ordine + ". AccesoDatos.agregar_IMP_ORDINI(). Excepcion: " + ex.Message);
                return 0;
            }
        }

        public int agregar_IMP_ORDINI_RIGHE(string rig_ordine, string rig_articolo, decimal rig_qtar)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_INSERTAR_IMP_ORDINI_RIGHE";

                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@rig_ordine";
                param1.Direction = ParameterDirection.Input;
                param1.SqlDbType = SqlDbType.VarChar;
                param1.Value = rig_ordine;
                command.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@rig_articolo";
                param2.Direction = ParameterDirection.Input;
                param2.SqlDbType = SqlDbType.VarChar;
                param2.Value = rig_articolo;
                command.Parameters.Add(param2);

                SqlParameter param3 = new SqlParameter();
                param3.ParameterName = "@rig_qtar";
                param3.Direction = ParameterDirection.Input;
                param3.SqlDbType = SqlDbType.Decimal;
                param3.Value = rig_qtar;
                command.Parameters.Add(param3);

                accesoDatos.ejecCommand(command);

                return 1;
            }
            catch (Exception ex)
            {
                Log.Instance.EscribirEnLogNoImportados("ERROR: No se inserto en la BD la orden: "+ rig_ordine + ". AccesoDatos.agregar_IMP_ORDINI_RIGHE(). Excepcion: " + ex.Message);
                return 0;
            }
        }
    }
}

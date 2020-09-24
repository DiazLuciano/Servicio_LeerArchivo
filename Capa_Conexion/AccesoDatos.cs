using Servicio_DescargarArchivo_Galileo.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio_DescargarArchivo_Galileo.Capa_Conexion
{
    public class AccesoDatos
    {
        public SqlConnection ConnectionBD = new SqlConnection();
        public string strcondatos;

        //datos de conexion
        private string serverBD;
        private string usuarioBD;
        private string PasswordBD;
        private string basedatos;

        public AccesoDatos()
        {
            //PC LOCAL
            strcondatos = @"Data Source=localhost\SQLExpress;Initial Catalog=HOST_IMPEXP;Integrated Security=SSPI;";

        }

        public void Conectar()
        {
            try
            {
                if (this.ConnectionBD.State == 0)
                {
                    ConnectionBD.ConnectionString = strcondatos;
                    ConnectionBD.Open();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DesConectar()
        {
            try
            {
                ConnectionBD.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
		/// Ejecuta un string Command de selección y devuelve un OdbcDataReader.
		/// Aclaración: Luego de recorrer el DataReader, recordar cerrar la conexión (AccesoDatosOdbc.DesConectar())
		/// </summary>
		/// <param name="Command"></param>
		/// <returns></returns>
        public SqlDataReader ExecuteReader(SqlCommand Command)
        {
            try
            {
                Command.Connection = ConnectionBD;
                //Da.CommandTimeout = 0; //Para que no se vaya por timeout la conexión
                Conectar();
                SqlDataReader Dr = Command.ExecuteReader();
                return Dr;
            }
            catch (Exception e)
            {
                DesConectar();
                throw e;
            }
            finally
            {

            }
        }

        public DataTable execDT(SqlCommand Command)
        {
            try
            {
                Command.Connection = this.ConnectionBD;
                Conectar();
                SqlDataAdapter da = new SqlDataAdapter(Command);

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            catch (Exception e)
            {
                DesConectar();
                throw e;
            }
            finally
            {
                //todo Rodrigo
                DesConectar();
            }
        }

        /// <summary>
        /// Ejecuta un sqlCommand de store y lo devuelve
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public SqlCommand ejecCommand(SqlCommand cmd)
        {
            try
            {
                cmd.Connection = this.ConnectionBD;

                Conectar();
                int resp = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                DesConectar();
            }
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int ejecQueryDevuelveInt(SqlCommand cmd)
        {
            int resp = 0;
            try
            {
                cmd.Connection = this.ConnectionBD;
                cmd.CommandTimeout = 60;
                Conectar();
                resp = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                DesConectar();
            }
            return resp;
        }
    }
}

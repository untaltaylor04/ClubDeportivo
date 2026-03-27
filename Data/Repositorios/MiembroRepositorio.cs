using ClubDeportivo.Data.Contratos;
using ClubDeportivo.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClubDeportivo.Data.Repositorios
{
    public class MiembroRepositorio : IMiembro
    {
        private readonly string cadenaConexion;
        public MiembroRepositorio(IConfiguration config) 
        {
            cadenaConexion = config["ConnectionStrings:DB"] ?? "";
        }

        public int Actualizar(Miembro miembro)
        {
            int resultado;
            using (var conexion = new OracleConnection(cadenaConexion))
            {
                using (var cmd = new OracleCommand("sp_ActualizarMiembro", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_ID_Miembro", OracleDbType.Int32).Value = miembro.idMiembro;
                    cmd.Parameters.Add("p_Nombre", OracleDbType.Varchar2).Value = miembro.nombre;
                    cmd.Parameters.Add("p_Apellido", OracleDbType.Varchar2).Value = miembro.apellido;
                    cmd.Parameters.Add("p_Fecha_Nacimiento", OracleDbType.Date).Value = miembro.fechaNacimiento;
                    cmd.Parameters.Add("p_Telefono", OracleDbType.Varchar2).Value = (object)miembro.telefono ?? DBNull.Value;
                    cmd.Parameters.Add("p_Email", OracleDbType.Varchar2).Value = (object)miembro.email ?? DBNull.Value;
                    cmd.Parameters.Add("p_Direccion", OracleDbType.Varchar2).Value = (object)miembro.direccion ?? DBNull.Value;

                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery();
                }
            }
            return resultado;
        }

        public int Eliminar(int id)
        {
            int resultado;
            using (var conexion = new OracleConnection(cadenaConexion))
            {
                using (var cmd = new OracleCommand("sp_EliminarMiembro", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery();
                }
            }
            return resultado;
        }

        public List<Miembro> listar()
        {
            var lista = new List<Miembro>();
            using (var conexion = new OracleConnection(cadenaConexion))
            {
                string sql = "SELECT * FROM vw_MiembrosActivos";
                using (var cmd = new OracleCommand(sql, conexion))
                {
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var miembro = new Miembro()
                            {
                                idMiembro = reader.GetInt32(0),
                                nombre = reader.GetString(1),
                                apellido = reader.GetString(2),
                                fechaNacimiento = reader.GetDateTime(3),
                                telefono = reader.IsDBNull(4) ? null : reader.GetString(4),
                                email = reader.IsDBNull(5) ? null : reader.GetString(5),
                                direccion = reader.IsDBNull(6) ? null : reader.GetString(6),
                                estado = reader.GetString(7),
                            };
                            lista.Add(miembro);
                        }
                    }
                }
            }
            return lista;
        }

        public Miembro ObtenerPorId(int id)
        {
            Miembro miembro = null;

            using (var conexion = new OracleConnection(cadenaConexion))
            {
                using (var cmd = new OracleCommand("sp_ObtenerMiembroPorId", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
                    cmd.Parameters.Add("p_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    conexion.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            miembro = new Miembro
                            {
                                idMiembro = reader.GetInt32(0),
                                nombre = reader.GetString(1),
                                apellido = reader.GetString(2),
                                fechaNacimiento = reader.GetDateTime(3),
                                telefono = reader.IsDBNull(4) ? null : reader.GetString(4),
                                email = reader.IsDBNull(5) ? null : reader.GetString(5),
                                direccion = reader.IsDBNull(6) ? null : reader.GetString(6),
                                estado = reader.GetString(7)
                            };
                        }
                    }
                }
            }
            return miembro;
        }

        public int Registrar(Miembro miembro)
        {
            int resultado;
            using (var conexion = new OracleConnection(cadenaConexion))
            {
                using (var cmd = new OracleCommand("sp_InsertarMiembro", conexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_Nombre", OracleDbType.Varchar2).Value = miembro.nombre;
                    cmd.Parameters.Add("p_Apellido", OracleDbType.Varchar2).Value = miembro.apellido;
                    cmd.Parameters.Add("p_Fecha_Nacimiento", OracleDbType.Date).Value = miembro.fechaNacimiento;
                    cmd.Parameters.Add("p_Telefono", OracleDbType.Varchar2).Value =(object)miembro.telefono ?? DBNull.Value;
                    cmd.Parameters.Add("p_Email", OracleDbType.Varchar2).Value =(object)miembro.email ?? DBNull.Value;
                    cmd.Parameters.Add("p_Direccion", OracleDbType.Varchar2).Value =(object)miembro.direccion ?? DBNull.Value;
                    cmd.Parameters.Add("p_Estado", OracleDbType.Varchar2).Value =miembro.estado ?? "activo";

                    conexion.Open();
                    resultado = cmd.ExecuteNonQuery();
                }
            }
            return resultado;
        }
    }
}

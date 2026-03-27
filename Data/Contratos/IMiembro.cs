using ClubDeportivo.Models;
namespace ClubDeportivo.Data.Contratos
{
    public interface IMiembro
    {
        List<Miembro> listar();
        Miembro ObtenerPorId(int id);
        int Registrar(Miembro miembro);
        int Actualizar(Miembro miembro);
        int Eliminar(int id);
    }
}

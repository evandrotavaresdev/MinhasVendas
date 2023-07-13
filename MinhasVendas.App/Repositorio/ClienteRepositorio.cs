using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;


namespace MinhasVendas.App.Repositorio
{
    public class ClienteRepositorio : Repositorio<Cliente>, IClienteRespositorio
    {
        public ClienteRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {}

    }
}

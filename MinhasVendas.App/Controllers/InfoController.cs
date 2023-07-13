using Microsoft.AspNetCore.Mvc;

namespace MinhasVendas.App.Controllers
{
    public class InfoController : Controller
    {
        private readonly IConfiguration _configuration;
        public InfoController(IConfiguration configuration)
        {
                _configuration = configuration;
        }
        public string Index()
        {
            

            var nome = _configuration["Info:Nome"];
            var conexao = _configuration.GetConnectionString("MinhaConexao");


            return $"Info: Nmome: {nome} ### Conexão: {conexao}";
        }
    }
}

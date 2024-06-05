using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BuildersBoleto.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthcheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="HealthcheckController"/>.
        /// </summary>
        /// <param name="healthCheckService">Serviço de verificação de saúde.</param>
        public HealthcheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// Verifica a saúde da aplicação.
        /// </summary>
        /// <returns>
        /// Retorna o status de saúde da aplicação, incluindo o status individual de cada verificação de saúde configurada.
        /// </returns>
        /// <response code="200">Se a aplicação está saudável ou contém informações detalhadas de saúde.</response>
        /// <response code="500">Se houve algum problema ao verificar a saúde da aplicação.</response>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            var healthStatus = report.Status == HealthStatus.Healthy ? "Healthy" : "Unhealthy";

            var healthReport = new
            {
                status = healthStatus,
                results = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    data = e.Value.Data
                })
            };

            return Ok(healthReport);
        }
    }
}

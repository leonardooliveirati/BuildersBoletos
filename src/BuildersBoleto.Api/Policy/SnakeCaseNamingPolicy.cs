using System.Text.Json;
using BuildersBoleto.Api.Configuration;

namespace BuildersBoleto.Api.Policy
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToSnakeCase();
    }
}

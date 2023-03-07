using ApiApplication.Helpers;
using System.Text.Json;

namespace ApiApplication.Controllers
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();
        public override string ConvertName(string name)
        {
            return name.ToSnakeCase();
        }
    }
}

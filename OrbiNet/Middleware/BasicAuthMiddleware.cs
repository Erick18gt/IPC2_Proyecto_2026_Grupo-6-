using System.Text;

namespace OrbiNet.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string? authorizationHeader =
                context.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(
                    "Authorization header requerido");
                return;
            }

            if (!authorizationHeader.StartsWith("Basic "))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(
                    "Tipo de autenticacion invalido");
                return;
            }

            string encodedCredentials =
                authorizationHeader.Substring("Basic ".Length);

            string decodedCredentials =
                Encoding.UTF8.GetString(
                    Convert.FromBase64String(encodedCredentials));

            string[] parts = decodedCredentials.Split(':');

            if (parts.Length != 2)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(
                    "Credenciales invalidas");
                return;
            }

            string username = parts[0];
            string password = parts[1];

            if (username != "admin" ||
                password != "orbitnet123")
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(
                    "Usuario o contraseña incorrectos");
                return;
            }

            await next(context);
        }
    }
}
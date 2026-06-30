using OrbiNet.Services.Ingestion;
using OrbiNet.Middleware;
using OrbiNet.Services.Ingestion;
using OrbiNet.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<XmlIngestionService>();
builder.Services.AddSingleton<SimulationService>();
builder.Services.AddSingleton<XmlIngestionService>();
builder.Services.AddSingleton<RegexValidtorService>();
builder.Services.AddSingleton<GraphvizRenderService>();

builder.Services.AddSingleton<DistributedRoutingService>(
    serviceProvider =>
    {
        var routing = new DistributedRoutingService();

        routing.RegistrarNodo("SAT-001");
        routing.RegistrarNodo("SAT-002");
        routing.RegistrarNodo("SAT-010");

        return routing;
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<BasicAuthMiddleware>();

app.UseAuthorization();

app.MapStaticAssets();
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
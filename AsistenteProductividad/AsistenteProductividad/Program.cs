using AsistenteProductividad.Services;

var builder = WebApplication.CreateBuilder(args);

// Habilitar controladores y vistas
builder.Services.AddControllersWithViews();

// Registrar el servicio de OpenAI
builder.Services.AddSingleton<OpenAIService>(sp =>
{
    var apiKey = builder.Configuration["OpenAI:ApiKey"];

    if (string.IsNullOrWhiteSpace(apiKey))
        throw new Exception("No se encontró la API Key, revisar appsettings.Development.json");

    return new OpenAIService(apiKey);
});

var app = builder.Build();

// Configuración específica para producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 🔥 Esto SIEMPRE debe ejecutarse
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Ruta por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chat}/{action=Index}/{id?}");

app.Run();

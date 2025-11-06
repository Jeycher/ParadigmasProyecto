using AsistenteProductividad.Services; 

var builder = WebApplication.CreateBuilder(args); 


builder.Services.AddControllersWithViews();
// esto es para que podamos usar controladores y vistas en la app




builder.Services.AddSingleton<OpenAIService>(sp =>
{
    // aca leemos la apiKey desde el appsettings.json que es donde se encuentra la apiKey
    var apiKey = builder.Configuration["OpenAI:ApiKey"];

    // si no encuentra niguna Api mostramos el error 
    if (string.IsNullOrWhiteSpace(apiKey))
        throw new Exception("No se encontró la API Key, revisar en el appsettings.Development.json");

    
    return new OpenAIService(apiKey);
});


var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();


    app.UseHttpsRedirection(); // redirige a https automaticamente
    app.UseStaticFiles(); // permite usar css, js, imagenes, etc


    app.UseRouting(); 
    app.UseAuthorization(); 

    // aca definimos la ruta para que apunta al ChatController y su Index
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Chat}/{action=Index}/{id?}");
   

    app.Run();
}
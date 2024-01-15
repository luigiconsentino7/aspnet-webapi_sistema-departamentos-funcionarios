using System;
using System.IO;
using aspnet_evosystem.Data;
using aspnet_evosystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConectionDb"));
});

// Adicione serviços do Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EvoSystemsProject.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Luigi Consentino",
            Url = new Uri("https://www.linkedin.com/in/luigi-consentino/")
        }
    });

    var xmlFile = "aspnet_evosystem.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);


    // Adicione as linhas abaixo para incluir exemplos nos modelos JSON
    c.SchemaFilter<SwaggerExampleFilter>();
});


var app = builder.Build();



// Configure o pipeline de solicitações HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "aspnet_evosystem v1");
    });
}

// Configuração do CORS
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
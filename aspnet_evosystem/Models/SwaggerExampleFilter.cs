using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace aspnet_evosystem.Models;
public class SwaggerExampleFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
 
        if (context.Type == typeof(Funcionario))
        {
            model.Example = new OpenApiObject
            {
                
                ["nome"] = new OpenApiString("string"),
                ["sobrenome"] = new OpenApiString("string"),
                ["rg"] = new OpenApiString("string"),
                ["departamentoId"] = new OpenApiInteger(0)
 
            };
        }
        else if (context.Type == typeof(Departamento))
        {
            model.Example = new OpenApiObject
            {

                ["nome"] = new OpenApiString("string"),
                ["sigla"] = new OpenApiString("string")

            };
        }
    }
}

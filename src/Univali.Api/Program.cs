using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Univali.Api;
using Univali.Api.Configuration;
using Univali.Api.DbContexts;
using Univali.Api.Extensions;
using Univali.Api.Repositories;
using Univali.Api.Features.Queries.GetCustomers;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000);
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<Data>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddLogging();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]!)
        )

    };
});

builder.Services.AddDbContext<CustomerContext>(options =>
{
    options
    .UseNpgsql("Host=localhost;Database=Univali;Username=postgres;Password=123456");
}
);

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
})


.ConfigureApiBehaviorOptions(setupAction =>
{
    setupAction.InvalidModelStateResponseFactory = context =>
    {
        // Cria a fábrica de um objeto de detalhes de problema de validação
        var problemDetailsFactory = context.HttpContext.RequestServices
            .GetRequiredService<ProblemDetailsFactory>();


        // Cria um objeto de detalhes de problema de validação
        var validationProblemDetails = problemDetailsFactory
            .CreateValidationProblemDetails(
                context.HttpContext,
                context.ModelState);


        // Adiciona informações adicionais não adicionadas por padrão
        validationProblemDetails.Detail =
            "See the errors field for details.";
        validationProblemDetails.Instance =
            context.HttpContext.Request.Path;


        // Relata respostas do estado de modelo inválido como problemas de validação
        validationProblemDetails.Type =
            "https://courseunivali.com/modelvalidationproblem";
        validationProblemDetails.Status =
            StatusCodes.Status422UnprocessableEntity;
        validationProblemDetails.Title =
            "One or more validation errors occurred.";


        return new UnprocessableEntityObjectResult(
            validationProblemDetails)
        {
            ContentTypes = { "application/problem+json" }
        };
    };
});




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
await app.ResetDatabaseAsync(logger);

app.Run();

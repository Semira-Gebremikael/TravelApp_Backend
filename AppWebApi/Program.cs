using Configuration;
using DbContext;
using DbRepos;
using Services;

var builder = WebApplication.CreateBuilder(args);

#region Insert standard WebApi services
// NOTE: global cors policy needed for JS and React frontends
builder.Services.AddCors();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "AppWebbApi",
        Description = "My first API, exploring application interactions.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Semira",
            Email = "semiragere1@gmail.com",
            Url = new Uri("https://github.com/semu150")
        }
    });

});


#endregion


#region Dependency Inject Custom logger
builder.Services.AddSingleton<ILoggerProvider, csInMemoryLoggerProvider>();
#endregion

#region Dependency Inject PersonsService
//Services are typically added as Scoped as one scope is a Web client request
//- Transient objects are always different in the IndexModel and in the middleware.
//- Scoped objects are the same for a given request but differ across each new request.
//- Singleton objects are the same for every request.

//DI injects the DbRepos into csPersonService
//Services are typically added as Scoped as one scope is a Web client request
builder.Services.AddScoped<csPersonsDbRepos>(); 

//WebController have a matching constructor, so service must be created
//Services are typically added as Scoped as one scope is a Web client request
//builder.Services.AddSingleton<IPersonsService, csPersonsServiceModel>();
builder.Services.AddScoped<IPersonsService, csPersonsServiceDb>();

#endregion


var app = builder.Build();

#region Configure the HTTP request pipeline
//In this example always use Swagger 
//if (app.Environment.IsDevelopment())
//{

app.UseSwagger();
app.UseSwaggerUI();

//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion

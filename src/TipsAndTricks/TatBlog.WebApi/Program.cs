using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;

var builder = WebApplication.CreateBuilder(args);
{
    //Add services to the container
    builder.ConfigureCors()
        .ConfigureNLog()
        .ConfigureServices()
        .ConfigureSwaggerOpenApi()
        .ConfigureMapster();
}

var app = builder.Build();
{ 
    //Configure the HTTP request pipeline.
    app.SetupRequestPipeline();

    app.Run();
}


using CourseLibrary.API.DataStore;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//xmlDataContractSerializer - formatter is used for CONTENT NEGOTIATION FEATURE
builder.Services.AddControllers(setupAction =>
{
    //THE BELOW IS THE DEFAULT BEHAVIOUR
    //setupAction.ReturnHttpNotAcceptable = false; 
    setupAction.ReturnHttpNotAcceptable = true;
    //setupAction.OutputFormatters.Add(
    //    new XmlDataContractSerializerOutputFormatter());
}).AddXmlDataContractSerializerFormatters();


//RANJIT - Dependency Injection relations are registered
builder.Services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();
builder.Services.AddScoped<IAuthorData, AuthorData>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//RANJIT - UseRouting, UseEndpoints, MapControllers are related to have the request ROUTE to the controller
app.UseRouting();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

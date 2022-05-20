using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
//using Neo4j.Driver;
using Neo4jClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////Neo4j connect to DB server
var client = new BoltGraphClient(new Uri("neo4j+s://f7345ac5.databases.neo4j.io"),"neo4j", "EUg50eak7AVsWt-n_UGHeRXrXb2AKIKfq2HfdVPEFOQ");
client.ConnectAsync();
builder.Services.AddSingleton<IGraphClient>(client);

//builder.Services.AddSingleton(GraphDatabase.Driver(
//                Environment.GetEnvironmentVariable("neo4j+s://f7345ac5.databases.neo4j.io"),
//                AuthTokens.Basic(
//                    Environment.GetEnvironmentVariable("neo4j"),
//                    Environment.GetEnvironmentVariable("EUg50eak7AVsWt-n_UGHeRXrXb2AKIKfq2HfdVPEFOQ")
//                )
//                ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

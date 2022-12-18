using ApiAeropuertos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<sistem21_avionesafContext>(

   x => x.UseMySql("server=sistemas19.com;user=sistem21_AF;password=avionesaf;database=sistem21_avionesaf", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.17-mariadb"))
          );
builder.Services.AddControllers();


var app = builder.Build();
app.UseRouting();
app.UseFileServer();
app.UseEndpoints(endpoints => endpoints.MapControllers());


app.Run();

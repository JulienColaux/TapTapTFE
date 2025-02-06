
using BLL.Services;
using DAL.Repositories;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<JoueurDAL>(); // Ajoute la DAL
            builder.Services.AddScoped<JoueurBLL>(); // Ajoute le BLL
            builder.Services.AddScoped<SaisonDAL>(); // Ajoute la DAL
            builder.Services.AddScoped<SaisonBLL>(); // Ajoute le BLL
            builder.Services.AddScoped<TropheeDAL>(); // Ajoute la DAL
            builder.Services.AddScoped<TropheeBLL>(); // Ajoute le BLL
            builder.Services.AddScoped<GradeDAL>(); // Ajoute la DAL
            builder.Services.AddScoped<GradeBLL>(); // Ajoute le BLL
            builder.Services.AddScoped<PartieDAL>(); // Ajoute la DAL
            builder.Services.AddScoped<PartieBLL>(); // Ajoute le BLL
            builder.Services.AddScoped<UserDAL>(); // Ajoute la DAL
            builder.Services.AddScoped<UserBLL>(); // Ajoute le BLL


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Ajout de la configuration CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularClient",
                    policy => policy.WithOrigins("http://localhost:4200") // Autoriser Angular
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials()); // Facultatif si vous envoyez des cookies ou tokens
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAngularClient");


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

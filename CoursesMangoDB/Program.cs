
using CoursesMangoDB.Data;
using CoursesMangoDB.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

namespace CoursesMangoDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<MongoCrud>();
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            //create
            app.MapPost("/api/course", async(Courses course, MongoCrud mongoDb) =>
            {
                var record = await mongoDb.AddCourse("Manager" ,course);
                return Results.Ok(record);
            });


            //Get 
            app.MapGet("/api/courses", async (MongoCrud db) =>
            {
                var c = await db.GetAllCourses("Courses");
                return Results.Ok(c);
            });


            //GetById
            app.MapGet("/api/course{id}", async (string id, MongoCrud db) =>
            {
                var course = await db.GetCourse(id, "Courses");

                return course == null ? Results.NotFound("Invaliv Id") : Results.Ok(course);
            });


            //Update
            app.MapPut("/api/course/{id}", async (string id, MongoCrud db,  Courses updateCourse) =>
            {
                var existing = await db.GetCourse(id, "Courses");
                
                if(existing == null)
                {
                    return Results.NotFound("Invlid ID");

                }
                updateCourse.Id = existing.Id;

                var result = await db.UpdateCoures("Courses", id, updateCourse);
                return Results.Ok(result);
            });

            //delete
            app.MapDelete("/api/course/{id}", async (string id, MongoCrud db) =>
            {
                var course = await db.DeleteCourse("Courses", id);
                return Results.Ok(course);
            });


            app.Run();
        }
    }
}

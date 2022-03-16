using ManTyres.DAL.Infrastructure.MongoDB.Interfaces;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using TeqLinkPortal.API.Extensions;
using ManTyres.WEB.Extensions;
using AutoMapper;
using ManTyres.BLL.Mapping;
using Tyre.WSL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(c => c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("Authentication:Mongo"));
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddControllersWithViews();

// Automapper
var mappingConfig = new MapperConfiguration(_ => _.AddProfile(new MappingProfile()));
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.LoadServices();
builder.Services.LoadRepositories();


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
    {
       googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
       googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

builder.Services.AddAuthorization();


builder.Services.AddSwaggerGen(c =>
{
   c.SwaggerDoc("v1", new OpenApiInfo { Title = "ManTyres", Version = "v1" });
   c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
   {
      Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                        Enter 'Bearer' [space] and then your token in the text input below.
                        \r\n\r\nExample: 'Bearer 12345abcdef'",
      Name = "Authorization",
      In = ParameterLocation.Header,
      Type = SecuritySchemeType.ApiKey,
      Scheme = "Bearer"
   });
   c.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" }
                        },
                        new List<string>()
                    }
             });
});

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
   app.UseDeveloperExceptionPage();
else
   app.UseHsts();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TeqLinkPortal v1"));

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();

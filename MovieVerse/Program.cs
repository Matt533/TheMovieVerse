using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieVerse.Application_Layer.Interfaces.IRepository;
using MovieVerse.Application_Layer.Interfaces.IService;
using MovieVerse.Application_Layer.Services;
using MovieVerse.Data_Layer;
using MovieVerse.Data_Layer.Repositories;
using MovieVerse.Domain_Layer.Interfaces.IService;
using MovieVerse.Infrastructional_Layer;
using MovieVerse.Domain_Layer.Models;
using MovieVerse.Domain_Layer.Interfaces.IRepository;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string apiKey = builder.Configuration["TMDBApi"];

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Registering DB Context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}
);

// Added Identity Core for Authentication
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
options.Password.RequireDigit = true;
options.Password.RequireLowercase = true;
options.Password.RequireUppercase = true;
options.Password.RequireNonAlphanumeric = false;
options.Password.RequiredLength = 6;
options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>();

//Registering Controllers
builder.Services.AddControllers();

//Registering HttpClient
builder.Services.AddHttpClient<ITMDbService, TMDbService>();

//Registering Services
builder.Services.AddHostedService<MovieSyncHostedService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ITMDbService, TMDbService>();
builder.Services.AddScoped<IActorService, ActorService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserMovieWatchService, UserMovieWatchService>();
builder.Services.AddScoped<IUserMovieFavoriteService, UserFavoriteMovieService>();

//Registering Repositories
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IUserMovieFavoriteRepository, UserMovieFavoriteRepository>();
builder.Services.AddScoped<IUserMovieWatchRepository, UserMovieWatchRepository>();

//Adding Json Web Tokens for Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],

        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["SigningKey"])
        )
    };
});

//Registering TokenProvider
builder.Services.AddSingleton<TokenService>();

//Enabling connection with the Frontend of the App
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

//Configuration for Swagger with Token Authorization
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


 var app = builder.Build();

//Enabling Frontend to make Api Calls
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Enabling Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PriceBondAPI.Models;
using PriceBondAPI.Repositories.BondRepository;
using PriceBondAPI.Repositories.DenominationRepository;
using PriceBondAPI.Repositories.DrawRepository;
using PriceBondAPI.Repositories.UserRepository;
using System.Text;
using Microsoft.AspNetCore.Identity;
using PriceBondAPI.Repositories.TokkenRepository;
using PriceBondAPI.Repositories.TokenRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Price Bond API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name="Authorization",
        In=ParameterLocation.Header,
        Type=SecuritySchemeType.ApiKey,
        Scheme=JwtBearerDefaults.AuthenticationScheme
    });
    // Adds a security requirement for JWT Bearer Authentication
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
           // Specifies the reference to the security scheme defined above (JWT Bearer)
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
            // Indicates the scheme type (Oauth2) for the security requirement
                Scheme = "Oauth2",
            // The name of the security requirement, which matches the one defined above (JWT Bearer)
                Name = JwtBearerDefaults.AuthenticationScheme,
            // The location of the authorization token, in this case, it is in the header
                In = ParameterLocation.Header
            },
            // An empty list of scopes, indicating no specific scope requirements for the endpoint
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<PbdatabaseContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("PbConnectionString")));

builder.Services.AddDbContext<PbAuthDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("PbAuthConnectionString")));

builder.Services.AddScoped<IUserRepository, SqlUserRepository> ();
builder.Services.AddScoped<IDenominationRepository, SqlDenominationRepository> ();
builder.Services.AddScoped<IBondRepository, SqlBondRepository>();
builder.Services.AddScoped<IDrawRepository, SqlDrawRepository>();
builder.Services.AddScoped<ITokenRepository,TokenRepository> ();


builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("PbAuth")
    .AddEntityFrameworkStores<PbAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase=false;
    options.Password.RequireUppercase=false;
    options.Password.RequireNonAlphanumeric=false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
}
);

//Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        });


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

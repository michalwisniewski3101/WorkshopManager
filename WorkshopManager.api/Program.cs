using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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



builder.Services.AddDbContext<WorkshopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WorkshopManagerDatabase")));

// Dodaj inne us³ugi
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNuxtApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

//builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<WorkshopContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<WorkshopContext>()
    .AddDefaultTokenProviders();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("RequireSeniorMechanicRole", policy => policy.RequireRole("Starszy Mechanik"));
    options.AddPolicy("RequireJuniorMechanicRole", policy => policy.RequireRole("M³odszy Mechanik"));
    options.AddPolicy("RequireClientRole", policy => policy.RequireRole("Klient"));
    options.AddPolicy("RequireMechanicRole", policy => policy.RequireRole("Starszy Mechanik", "M³odszy Mechanik"));
});



var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roleNames = { "Administrator", "Starszy Mechanik", "M³odszy Mechanik", "Klient" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Dodanie testowych u¿ytkowników
    var testUsers = new[]
    {
        new { UserName = "admin", Email = "admin@example.com", Role = "Administrator" },
        new { UserName = "mechanic", Email = "mechanic@example.com", Role = "Starszy Mechanik" },
        new { UserName = "mechanicjr", Email = "mechanicjr@example.com", Role = "M³odszy Mechanik" },
        new { UserName = "client", Email = "client@example.com", Role = "Klient" }
    };

    foreach (var testUser in testUsers)
    {
        var user = await userManager.FindByNameAsync(testUser.UserName);
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = testUser.UserName,
                Email = testUser.Email,
                EmailConfirmed = true // Potwierdzenie e-mail, jeœli to konieczne
            };
            var result = await userManager.CreateAsync(user, "Qaz123!@");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, testUser.Role);
            }
        }
    }
}





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowNuxtApp");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

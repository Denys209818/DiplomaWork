using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using Volonterio.Data;
using Volonterio.Data.Entities;
using Volonterio.Data.Services;
using Volonterio.Mappers;
using Volonterio.Services;

var builder = WebApplication.CreateBuilder(args);

    builder.WebHost
    .UseKestrel(options =>
    {
        options.Limits.MaxRequestBodySize = long.MaxValue;
    });
    

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddDbContext<EFContext>((DbContextOptionsBuilder b) => {
    b.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, AppRole>((IdentityOptions opts) => { 
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireDigit = false;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = false;
    opts.Password.RequiredLength = 5;
})
    .AddEntityFrameworkStores<EFContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication((AuthenticationOptions opts) => {
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer((JwtBearerOptions jwt) => {
        jwt.RequireHttpsMetadata = false;
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = false,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetValue<string>("private_key")))
        };
    });

builder.Services.AddScoped<IJwtBearerService, JwtBearerService>();
builder.Services.AddAutoMapper(typeof(UserMapper));
builder.Services.AddAutoMapper(typeof(PostMapper));

builder.Services.AddControllers().AddNewtonsoftJson(opts => {

    opts.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
    opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});



builder.Services.AddCors();

var app = builder.Build();
app.UseCors((CorsPolicyBuilder builder) => {
    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});


if (app.Environment.IsDevelopment())
{
    
}

app.UseSwagger();
app.UseSwaggerUI();
app.SeedAll();

ForwardedHeadersOptions opts = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor| ForwardedHeaders.XForwardedProto
};

opts.KnownNetworks.Clear();
opts.KnownProxies.Clear();

app.UseForwardedHeaders(opts);

app.UseAuthentication();
app.UseAuthorization();

string dir = Path.Combine(Directory.GetCurrentDirectory(), "Images");
if(!Directory.Exists(dir))
{
    Directory.CreateDirectory(dir);
}

app.UseStaticFiles(new StaticFileOptions { 

    FileProvider = new PhysicalFileProvider(dir),
    RequestPath = "/Images"
});


app.MapControllers();

app.Run();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
await app.RunAsync();

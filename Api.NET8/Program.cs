using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = false;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SADASDASFdasdasdasdASDASDASDA1212312123423423423423423423423423423423423423432")),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
    };
});
builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    opts.AddPolicy("User", policy => policy.RequireRole("User"));
    opts.AddPolicy("Anonimo", policy => policy.RequireRole("Anonimo"));
    opts.AddPolicy("Autor", policy => policy.RequireRole("Autor"));
});

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", [Authorize]() => "Hello World!");
app.MapGet("/testeRoleUser", [Authorize] () => "Funcionou").RequireAuthorization("User");
app.MapGet("/testeRoleAdmin", [Authorize] () => "Funcionou").RequireAuthorization("Admin");
app.MapGet("/testeRoleAnonimo", [Authorize] () => "Funcionou").RequireAuthorization("Anonimo");
app.MapGet("/testeRoleAutor", [Authorize] () => "Funcionou").RequireAuthorization("Autor");

app.Run();

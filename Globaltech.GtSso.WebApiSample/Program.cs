using Globaltech.GtSso.WebApiSample;
using Globaltech.GtSso.WebApiSample.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    // Configure the authentication scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.OpenIdConnect,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
});

var ssoConfg = builder.Configuration.GetSection("SsoConfig");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = ssoConfg["Authority"];
        options.Audience = ssoConfg["Audience"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ClockSkew = TimeSpan.Zero,
            ValidAudience = ssoConfg["Audience"],
            ValidIssuer = ssoConfg["Authority"]
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Policies.PolicyBasedOnScope, policy =>
       policy.RequireAssertion(context =>
       {
           var scopeClaim = context.User.FindFirst(claim => claim.Type == "scope");

           if (scopeClaim != null)
           {
               var scopes = scopeClaim.Value.Split(' ');
               return scopes.Any(s => s.Equals("profile", StringComparison.OrdinalIgnoreCase));
           }

           return false;
       }));
});

builder.Services.AddTransient(typeof(UserInfo));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ClaimsMiddleware>();

app.MapControllers();

app.Run();

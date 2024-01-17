using LMS.WebAPI.DatabaseContext;
using LMS.WebAPI.Identity;
using LMS.WebAPI.ServiceContracts;
using LMS.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Adding the services for ServiceContracts into IOC Container
builder.Services.AddTransient<IJWTService, JWTService>();

//Adding services for the DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection1"));
});

//Adding Identity Services
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    //Password Complexity
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser,ApplicationRole,ApplicationDbContext,Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole,ApplicationDbContext,Guid>>();

//Adding Swagger Services
builder.Services.AddEndpointsApiExplorer(); //Here EndPoints means the action methods of API Controller.
builder.Services.AddSwaggerGen(); //generates Open Api Specification. (or) it configures swagger to generate the documention for API's endpoints. 

//Adding CORS Service
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>());
        policyBuilder.WithHeaders("Authorization", "origin", "accept", "content-type");
        policyBuilder.WithMethods("GET", "POST", "PUT", "DELETE");
    });
});

//Creating Custom CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("CustomPolicy1", policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration.GetSection("AllowedOrigins2").Get<string[]>());
        policyBuilder.WithHeaders("Authorization", "origin");
        policyBuilder.WithMethods("GET");
    });
});

//Adding Authentication Service
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//Adding Authorization Service
builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHsts();
app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    //Enabling the Swagger inside Developement Environment
    app.UseSwagger();   //Enables the Endpoints for Swagger Json
    app.UseSwaggerUI(); //Enables the tool UI to test the API Endpoints
}

app.UseRouting();
app.UseCors();  //Adding Cors to the Request Pipeline in b/w UseRouting And UseAuthorization

app.UseAuthentication();
app.UseAuthorization();  //Middleware which is responsible to check the JWT Token that is submitted is valid or not.
 
app.MapControllers();

app.Run();

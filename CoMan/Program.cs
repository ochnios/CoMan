using CoMan.Data;
using CoMan.Models;
using CoMan.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CoManDbContext>(options =>
    options.UseSqlServer(connectionString).UseLazyLoadingProxies());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CoManDbContext>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireStudent", policy => policy.RequireRole("Student"));
    options.AddPolicy("RequireTeacher", policy => policy.RequireRole("Teacher"));
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ModifyTopics", policy => policy.RequireRole("Admin", "Teacher"));
    options.AddPolicy("ViewCooperationRequests", policy => policy.RequireRole("Teacher", "Student"));
    options.AddPolicy("ViewCooperations", policy => policy.RequireRole("Teacher", "Student"));
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<ITopicService, TopicService>();
builder.Services.AddTransient<ICooperationRequestService, CooperationRequestService>();
builder.Services.AddTransient<ICooperationService, CooperationService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ILogger, Logger<Program>>();

// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoMan", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.FirstOrDefault());

    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    //c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "docs";
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoMan Docs v1");
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

    var seeder = new DataSeeder(logger, roleManager, unitOfWork);
    await seeder.SeedRoles(Role.GetNames(typeof(Role)));
    await seeder.SeedTopicsFromJSON("Data/TopicsForSeeder.json");
}

app.Run();

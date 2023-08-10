using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzaStar.Data;
using PizzaStar.Data.Helpers;
using PizzaStar.Interfaces;
using PizzaStar.Models;
using PizzaStar.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMemoryCache();
builder.Services.AddSession();

IConfigurationRoot _confString = new ConfigurationBuilder().
    SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

builder.Services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer(_confString.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 5;   // ����������� �����
    opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
    opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
    opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
    opts.Password.RequireDigit = false; // ��������� �� �����
})
    .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

//������� ������������ ����� � ���������� � �������
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig!);

//���������� ������ ��� �������� �����
builder.Services.AddScoped<EmailSender>();

//����� ������������� ������, ��� �������������� ������ - 1 ���.
builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(1));

//���������� ������ ��� ������ � �����������
builder.Services.AddTransient<ICategory, CategoryRepository>();

//���������� ������ ��� ������ � �������
builder.Services.AddTransient<IProduct, ProductRepository>();

//���������� ������ ��� ������ � ��������
builder.Services.AddScoped(e => CartRepository.GetCart(e));

//���������� ������ ��� ������ � ��������
builder.Services.AddTransient<IOrder, OrderRepository>();

var app = builder.Build();

app.UseSession();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<User>>();
        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DbInit.InitializeAsync(userManager, rolesManager);
        var applicationContext = services.GetRequiredService<ApplicationContext>();
        await DbInit.InitializeContentAsync(applicationContext);
        //���������� ��������� ���������� �������
        //await DbInit.CreateSeedDataAsync(applicationContext, rowCount:15, categories: new int[] { 1, 2, 3 });
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();  //����������� ��������������
app.UseAuthorization();  //����������� �����������


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection;
using PlacowkaOswiatowaQuiz.Helpers;
using PlacowkaOswiatowaQuiz.Helpers.Filters;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Services;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

//Dodaje AntiForgeryToken do formularzy:
//<form method="post">...
builder.Services.AddControllersWithViews(o =>
{
    o.Filters.Add(typeof(UserLoggedInFilter));
});

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddTransient<HttpClientMiddleware>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(builder.Configuration
        .GetSection("QuizApiSettings")
        .Get<QuizApiSettings>());

builder.Services.AddSingleton(builder.Configuration
        .GetSection("QuizApiUrl")
        .Get<QuizApiUrl>());

builder.Services.AddHttpClient(
    builder.Configuration.GetValue<string>("QuizApiUrl:ClientName"),
    (provider, client) =>
    {
        var apiSettings = provider.GetRequiredService<QuizApiSettings>();
        var apiUrl = provider.GetRequiredService<QuizApiUrl>();
        client.BaseAddress =
            new Uri(apiUrl.Host + '/' + nameof(apiSettings.Data) + '/');

        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.Clear();
    })
    .AddHttpMessageHandler<HttpClientMiddleware>();

builder.Services.AddHttpClient<IUserService, UserService>(
    (provider, client) =>
    {
        var apiSettings = provider.GetRequiredService<QuizApiSettings>();
        var apiUrl = provider.GetRequiredService<QuizApiUrl>();
        client.BaseAddress =
            new Uri(apiUrl.Host + '/' + nameof(apiSettings.User) + '/');

        client.Timeout = TimeSpan.FromSeconds(90);
    })
    .AddHttpMessageHandler<HttpClientMiddleware>();

// dodanie obsługi sesji
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.Cookie.Name = ".PlacowkaOswiatowa.Session";
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.IsEssential = true;
    //o.Cookie.HttpOnly = true;
    //o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    //o.Cookie.SameSite = SameSiteMode.Strict;

});

//builder.Services.AddDataProtection()
//    .PersistKeysToFileSystem(new DirectoryInfo(@"/keys/"))
//    .ProtectKeysWithCertificate(
//        new X509Certificate2("/https/PlacowkaOswiatowaQuiz.pfx", "Str0ngP@ssw0rd2"))
//    .UnprotectKeysWithAnyCertificate(
//        new X509Certificate2("/https/PlacowkaOswiatowaQuiz.pfx", "Str0ngP@ssw0rd2"))
//    .UseCryptographicAlgorithms(
//        new AuthenticatedEncryptorConfiguration
//        {
//            EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
//            ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
//        })
//    ;

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

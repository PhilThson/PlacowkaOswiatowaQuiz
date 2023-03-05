using PlacowkaOswiatowaQuiz.Controllers;
using PlacowkaOswiatowaQuiz.Helpers;
using PlacowkaOswiatowaQuiz.Helpers.Options;
using PlacowkaOswiatowaQuiz.Interfaces;
using PlacowkaOswiatowaQuiz.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
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
        client.BaseAddress = new Uri(apiUrl.Host + '/' +
            apiSettings.MainController + '/');
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.Clear();
    });

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

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
//builder.Services.AddHttpClient<IHttpClientService, HttpClientService>((provider, client) =>
//{
//    var apiSettings = provider.GetRequiredService<QuizApiSettings>();
//    var apiUrl = provider.GetRequiredService<QuizApiUrl>();
//    client.BaseAddress = new Uri(apiUrl.Host + '/' + apiSettings.MainController + '/');
//});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using PlacowkaOswiatowaQuiz.Helpers.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(
    builder.Configuration
        .GetSection("QuizApiSettings").Get<QuizApiSettings>());
builder.Services.AddHttpClient(
    builder.Configuration.GetValue<string>("QuizApiSettings:ClientName"),
    (provider, client) =>
    {
        var apiSettings = provider.GetRequiredService<QuizApiSettings>();
        client.BaseAddress = new Uri(apiSettings.Host + '/' +
            apiSettings.MainController + '/');
        client.Timeout = TimeSpan.FromSeconds(30);
        client.DefaultRequestHeaders.Clear();
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

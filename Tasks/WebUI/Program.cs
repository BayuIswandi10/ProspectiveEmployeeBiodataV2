using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Auth
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());

// HTTP Client -> API
builder.Services.AddScoped(sp =>
{
    var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
        ?? "https://localhost:7000";
    return new HttpClient { BaseAddress = new Uri(apiBaseUrl) };
});

// Services
builder.Services.AddScoped<AuthApiService>();
builder.Services.AddScoped<BiodataApiService>();
builder.Services.AddScoped<AdminApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

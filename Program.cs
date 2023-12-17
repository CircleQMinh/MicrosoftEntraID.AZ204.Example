using CircleCat.MicrosoftEntraID.AZ204.Helpers;
using CircleCat.MicrosoftEntraID.AZ204.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
IEnumerable<string>? initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ');
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    // Handling SameSite cookie according to https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1
    options.HandleSameSiteCookieCompatibility();
});
builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd")
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        .AddDownstreamWebApi("DownstreamApi", builder.Configuration.GetSection("DownstreamApi"))
        .AddInMemoryTokenCaches();
//policy
builder.Services.AddAuthorization(options => {
    options.AddPolicy(AppPolicy.Admin, 
        policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == AppRole.Admin)));
    options.AddPolicy(AppPolicy.HighLevelUserOnly,
        policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == AppRole.Admin || c.Type == AppRole.Manager)));
    options.AddPolicy(AppPolicy.User,
        policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == AppRole.User)));
});
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

//role for user
builder.Services.AddScoped<IUserServices,UserService>();
builder.Services.AddTransient<IClaimsTransformation, ClaimTranformer>();

builder.Services.AddRazorPages();
     
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});
app.Run();

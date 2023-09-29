using Microsoft.EntityFrameworkCore;
using Portfolio.Business.Repositories.Abstract;
using Portfolio.Core.DependencyResolvers;
using Portfolio.Core.Extensions;
using Portfolio.Core.Utilities.IoC;
using Portfolio.DataAccess.Abstract;
using Portfolio.DataAccess.Concrete;
using Portfolio.DataAccess.Concrete.EntityFramework.Contexts;
using Portfolio.Web.Constraints;
using Portfolio.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    //options.Conventions.Add(new LowercaseAreaRouteConvention()); // Controller ismi buradan da y�netilebilinir
    //options.Conventions.Add(new LowercaseControllerRouteConvention());
    options.Conventions.Add(new CustomRouteConvention()); // path isimleri burada k���k harfe �evrildi
});

builder.Services.AddDbContext<PortfolioContext>(options =>
{
    options
    .UseSqlServer(builder.Configuration.GetConnectionString("LocalDb"))
    .EnableSensitiveDataLogging();
    #region EnableSensitiveDataLogging
    // Entity Framework Core taraf�ndan sa�lanan bir se�enektir ve Entity Framework Core'un veritaban� i�lemleri s�ras�nda olu�an sorgular�n ve verilerin hassas bilgilerini (�rne�in, veritaban� parolalar� veya hassas ki�isel bilgiler) kaydetmeye veya g�nl��e almaya yarar.  Bu se�ene�i kullanmak, genellikle geli�tirme ve hata ay�klama a�amas�nda yararl�d�r.Hassas bilgilerin g�nl��e al�nmas�, veritaban� i�lemleri s�ras�nda neyin olup bitti�ini daha ayr�nt�l� bir �ekilde anlamak ve hata ay�klamak i�in kullan��l�d�r.Ancak, bu �zellik prod�ksiyon uygulamalarda etkinle�tirilmemelidir, ��nk� hassas bilgilerin g�nl��e al�nmas� g�venlik sorunlar�na yol a�abilir ve veri gizlili�ini tehlikeye atabilir. Bu nedenle, EnableSensitiveDataLogging se�ene�ini yaln�zca geli�tirme veya hata ay�klama ama�lar� i�in kullanmal� ve prod�ksiyon uygulamalar�nda devre d��� b�rakmal�s�n�z.Bir uygulama prod�ksiyon ortam�nda �al��t�r�ld���nda, bu se�ene�i kullanmamal�s�n�z ve hassas bilgilerin g�nl��e al�nmamas�n� sa�lamal�s�n�z.Bu, uygulama g�venli�ini ve kullan�c� verilerinin gizlili�ini korumak i�in �nemlidir.
    #endregion

    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


builder.Services.AddHttpClient();
//builder.Services.AddScoped<GithubApiService>(); // GithubApiService'yi ekledik

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IService, Portfolio.Business.Repositories.Concrete.Base.Service>();
builder.Services.AddAutoMapper(typeof(Portfolio.Business.AssemblyRefence).Assembly);


builder.Host.ConfigureAutofac();


builder.Services.AddDependencyResolvers(new ICoreModule[]
{
    new CoreModule()
});



var app = builder.Build();


app.UseStaticFiles();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
app.MapDefaultControllerRoute();

app.Run();
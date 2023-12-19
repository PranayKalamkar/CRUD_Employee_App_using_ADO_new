//using EmployeeApp.EmployeeDataManager.DAL;
//using EmployeeApp.EmployeeDataManager.IDAL;
//using MySql.Data.MySqlClient;
//using System.Data.Common;

//namespace EmployeeApp
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
using EmployeeApp.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
//builder.Services.AddDBManager();//System.Text.Json.JsonNamingPolicy.CamelCase);
builder.Services.AddAppSetting(); //new
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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Index}/{id?}");

app.Run();
//        }
//        public static IServiceCollection AddAppSetting(IServiceCollection services)
//        {
//            services.AddScoped<IDBManager>(AddDBManager);

//            return services;
//        }
//        internal static IDBManager AddDBManager(IServiceProvider serviceProvider)
//        {
//            IConfiguration Configuration = serviceProvider.GetRequiredService<IConfiguration>();

//            string dbconstr = Configuration.GetConnectionString("DBConnectionString");
//            return GetDBManager(dbconstr);


//        }
//        public static IDBManager GetDBManager(string connectionString)
//        {
//            DbConnection dbconn = new MySqlConnection(connectionString);
//            return new DBManager(dbconn);
//        }
//    }
//}

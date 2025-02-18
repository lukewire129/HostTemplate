using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Views;

namespace Sample
{    public class Program
    {
        [STAThread]  // UI 스레드를 STA 모드로 설정
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder (args);

            builder.Services.AddSingleton<App> ();
            builder.Services.AddSingleton<MainWindow> ();
            builder.Services.AddSingleton<MainContent> ();

            var host = builder.Build ();
            host.RunWithApp ();
        }
    }

    public static class HostTemplateExtention
    {
        public static void RunWithApp(this IHost host)
        {
            var app = host.Services.GetRequiredService<App> ();
            var window = host.Services.GetRequiredService<MainWindow> ();

            // WPF 애플리케이션 실행
            app.Run (window);

            host.RunAsync ().GetAwaiter ().GetResult ();
        }
    }
}
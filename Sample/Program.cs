using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sample.Views;

namespace Sample
{   
    public class Program
    {
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
            // 백그라운드 서비스 실행 (MTA 스레드)
            Task.Run (() => host.StartAsync ());

            // UI 스레드 실행 (STA)
            var uiThread = new Thread (() =>
            {
                var app = host.Services.GetRequiredService<App> ();
                var window = host.Services.GetRequiredService<MainWindow> ();
                app.Run (window);
            });

            uiThread.SetApartmentState (ApartmentState.STA);
            uiThread.Start ();
            uiThread.Join ();

            // 애플리케이션 종료 시 백그라운드 서비스 정리
            host.StopAsync ().GetAwaiter ().GetResult ();
        }
    }
}
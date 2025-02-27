using Microsoft.Extensions.DependencyInjection;

namespace AgeCalculatorPlus
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Captura erros globais
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                File.WriteAllText("error.log", e.ExceptionObject.ToString());
                MessageBox.Show("Erro crítico! Verifique error.log");
            };

            // Configuração da injeção de dependência
            var services = new ServiceCollection();
            services.AddMemoryCache();
            services.AddHttpClient<HolidayServices>(); // 🔹 Registra o HttpClient corretamente

            var serviceProvider = services.BuildServiceProvider();
            var holidayService = serviceProvider.GetRequiredService<HolidayServices>();

            // Inicia a aplicação passando a instância do serviço
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1(holidayService));
        }
    }
}


// Implementar mapa de feriados
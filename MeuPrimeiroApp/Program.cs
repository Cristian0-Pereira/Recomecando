using System;

class Program 
{
    static void Main() 
    {
        Console.WriteLine("⭐Calculadora de Idade Avançado - Digite 0 para sair⭐");

        while(true)
        {
            // Captura do Ano
            int year;
            while (true)
            {
                Console.Write("\nDigite o ano de nascimento (ou 0 para sair): ");
                string imput = Console.ReadLine() ?? "";

                if (imput == "0") 
                {   
                    // Sai do programa se digitar 0
                    Console.WriteLine("Encerrando o programa...");
                    return;
                }

                // Validação do imput
                if (!int.TryParse(imput, out year) || year < 1900 || year > DateTime.Now.Year)
                {
                    Console.WriteLine($"❌ Ano inválido! Digite um número entre 1900 e {DateTime.Now.Year} (ano atual).");
                    continue;
                }
                break;
            }

            // Captura do mês
            int month;
            while (true)
            {
                Console.Write("Digite o mês de nascimento (1-12): ");
                string imput = Console.ReadLine() ?? "";

                if(!int.TryParse(imput, out month) || month < 1 || month > 12)
                {
                    Console.WriteLine("❌ Mês inválido! Digite um número entre 1 e 12.");
                    continue;
                }
                break;
            }

            // Captura do dia
            int maxDay = DateTime.DaysInMonth(year, month);
            int day;
            while (true)
            {
                Console.Write($"Digite o dia de nascimento (1-{maxDay}): ");
                string imput = Console.ReadLine() ?? "";

                if (!int.TryParse(imput, out day) || day < 1 || day > maxDay)
                {
                    Console.WriteLine($"❌ Dia inválido! Digite um número entre 1 e {maxDay}.");
                    continue;
                }
                break;
            }

            // Cálculo preciso da idade
            DateTime nascimento = new(year, month, day);
            DateTime hoje = DateTime.Today;

            int idade = hoje.Year - nascimento.Year;
            if (nascimento.Date > hoje.AddYears(-idade)) idade--;

            // Exibição detalhada
            Console.WriteLine($"\n📅 Data de nascimento: {nascimento:dd/MM/yyyy}");
            Console.WriteLine($"🕒 Data Atual: {hoje:dd/MM/yyyy}");
            Console.WriteLine($"✅ Você tem {idade} anos.");
            Console.WriteLine("──────────────────────────────────");
        }
    }
}

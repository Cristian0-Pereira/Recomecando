using System;
using System.Drawing;
using System.Net.Http;
using System.Windows;
using System.Collections.Generic;

namespace AgeCalculatorPlus
{
    public partial class MainForm : Form
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public MainForm()
        {
            InitializeComponent();
            ConfigureUI();
        }

        private void ConfigureUI()
        {
            // Configuração da Janela
            this.Text = "Calculadora de Idade Avançado";
            this.Size = new System.Drawing.Size(850, 500);

            // Controles
            dtpBirthDate = new DateTimePicker
            {
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(200, 20),
                Format = DateTimePickerFormat.Short 
            };

            btnCalculate = new Button
            {
                Text = "Calcular",
                Location = new System.Drawing.Point(230, 20),
                Size = new System.Drawing.Size(100, 25)
            };

            lblResult = new Label
            {
                Location = new System.Drawing.Point(20, 60),
                AutoSize = true
            };

            lstHolidays = new ListBox
            {
                Location = new System.Drawing.Point(20, 100),
                Size = new System.Drawing.Size(790, 350)
            };

            // Eventos
            btnCalculate.Click += BtnCalculate_Click;

            // Add Controles
            this.Controls.Add(dtpBirthDate);
            this.Controls.Add(btnCalculate);
            this.Controls.Add(lblResult);
            this.Controls.Add(lstHolidays);
        }

        private async void BtnCalculate_Click(object sender, EventArgs e)
        {
            var birthDate = dtpBirthDate.Value;

            // Cálculo de idade
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;

            // Dias para próximo aniversário
            var nextBirthDay = birthDate.AddYears(today.Year - birthDate.Year);
            if (nextBirthDay < today) nextBirthDay = nextBirthDay.AddYears(1);
            var daysToNextBirthDay = (nextBirthDay - today).Days;

            // Exibir resultados
            lblResult.Text = $"Você tem {age} anos e faltam {daysToNextBirthDay} dias para o seu próximo aniversário.";

            // Feriados
            await LoadHolidays(birthDate.Year);
        }

        private async Task LoadHolidays(int year)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                $"https://brasilapi.com.br/api/feriados/v1/{year}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var holidays = JsonContent.DeserializeObject<List<Holiday>>(json);

                    lstHolidays.Items.Clear();
                    foreach (var holiday in holidays)
                    {
                        lstHolidays.Items.Add($"{holiday.Date:dd/MM} - {holiday.name} ({holiday.Type})");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar feriados: {ex.Message}");
            }
        }

        // Controles
        private DateTimePicker dtpBirthDate;
        private Button btnCalculate;
        private Label lblResult;
        private ListBox lstHolidays;

        public class Holiday
        {
            public DateTime Date { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
        }
    }
}



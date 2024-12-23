using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AITest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string key = "sk-or-vv-0dfbe36091f6cb5989322569d3878d26021399798fa8f09be885b1803e747c9c";
        string url = "https://api.vsegpt.ru/v1/";

        HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonS_Click(object sender, RoutedEventArgs e)
        {
            ButtonS.IsEnabled = false;
            LabelS.Content = "Загрузка...";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

            string promt = BoxS.Text;

            List<dynamic> messages = new List<dynamic>();
            messages.Add(new { role = "user", content = promt});
            var requestData = new
            {
                model = "openai/gpt-3.5-turbo",
                messages = messages,
                temperature = 0.7,
                n = 1,
                max_tokens = 3000
            };

            var jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url + "chat/completions", content);

            string result;
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
                string responseContent = responseData.choices[0].message.content;
                result = responseContent;
            }
            else {
                result = "Ошибка: " + response.ReasonPhrase;
            }

            LabelS.Content = result;
            ButtonS.IsEnabled = true;
        }
    }
}
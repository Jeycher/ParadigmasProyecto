using System.Text; 
using System.Text.Json; 


namespace AsistenteProductividad.Services 

    //Les comento esta clase lo que hace es que nos ayuda a comunicarnos con OpenIA y nos de la informacion
{
    public class OpenAIService 
    {
        private readonly HttpClient _httpClient; 

        public OpenAIService(string apiKey)
        {
            // Con este If lo que  hacemos es validar que la API exista
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API Key no puede ser null o vacía.", nameof(apiKey));

            _httpClient = new HttpClient(); 
            // aca agregamos la apiKey al header de las solictudes realizadas, para que OpenAI nos reconozca
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        // Con este prompt le escribimos a la IA y nos devuelve la respuesta de la IA
        public async Task<string> EnviarPromptAsync(string prompt)
        {
            
            var requestData = new
            {
                model = "gpt-4o-mini", // el modelo que usamos, mas ligero que el GPT-4 completo
                messages = new[]
                {
                    new { role = "user", content = prompt } // aca ponemos el mensaje del usuario
                }
            };

            // en esta parte convertimos el objeto a JSON 
            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            // Y aca mandamos la peticion POST a la API de OpenIA  y esperamos la respuesta
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            // En esta parte nos mostrara la respuesta la API
            var responseBody = await response.Content.ReadAsStringAsync();

            // si llega a pasar algun error, mostramos un mensaje de error 
            if (!response.IsSuccessStatusCode)
            {
                return $"Error {response.StatusCode}: {responseBody}";
            }

            
            using var doc = JsonDocument.Parse(responseBody);
            var choices = doc.RootElement.GetProperty("choices"); 

            
            if (choices.GetArrayLength() > 0)
            {
                return choices[0].GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
            }

            
            return "Error";
        }
    }
}

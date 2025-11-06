using Microsoft.AspNetCore.Mvc; 
using AsistenteProductividad.Services;  


namespace AsistenteProductividad.Controllers 
{
    public class ChatController : Controller
    {
        private readonly OpenAIService _openAIService; 
       
        public ChatController(OpenAIService openAIService)
        {
            
            _openAIService = openAIService;
        }

       
        public IActionResult Index()
        {
            return View(); 
        }

        // POST 
        [HttpPost]
        public async Task<IActionResult> Enviar(string prompt)
        {
            
            if (string.IsNullOrWhiteSpace(prompt))
            {
                // devolvemos un mensaje directo en JSON para decir que escriba algo
                return Json(new { respuesta = "Por favor, escribe algo antes de enviar." });
            }

            
            var respuesta = await _openAIService.EnviarPromptAsync(prompt);

            // aca mostramos la respuesta que de la IA y se muestra con el formato de Json y se muestra en el chat
            
            return Json(new { respuesta });
        }
    }
}

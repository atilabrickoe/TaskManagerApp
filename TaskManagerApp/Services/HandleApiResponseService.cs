using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TaskManagerApp.Services.Responses;

namespace TaskManagerApp.Services
{
    public class HandleApiResponseService<T> : IHandleApiResponseService<T>
    {
        public async Task<Response<T>> HandleAsync(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new Response<T>
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.UNAUTHORIZED,
                        Message = "Acesso negado, favor efetuar log-out/login e tentar novamente."
                    };
                }

                return new Response<T>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                    Message = "Erro desconhecido. O servidor não retornou dados."
                };
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                // Needed because your API sends enum names as strings
                //options.Converters.Add(new JsonStringEnumConverter());

                var apiResponse = JsonSerializer.Deserialize<Response<T>>(json, options);

                if (apiResponse == null)
                {
                    return new Response<T>
                    {
                        Success = false,
                        ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                        Message = "Erro ao processar resposta do servidor."
                    };
                }

                return apiResponse;
            }
            catch (JsonException)
            {
                return new Response<T>
                {
                    Success = false,
                    ErrorCode = ErrorCodes.INTERNAL_SERVER_ERROR,
                    Message = "Erro inesperado ao interpretar resposta do servidor."
                };
            }
        }
    }
}
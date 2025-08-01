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

                if (!apiResponse.Success)
                {
                    apiResponse.Message = GetMessageFromErrorCode(apiResponse.ErrorCode);
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

        private string GetMessageFromErrorCode(ErrorCodes errorCode)
        {
            return errorCode switch
            {
                ErrorCodes.USER_NOT_FOUND => "Usuário não encontrado.",
                ErrorCodes.USER_ALREADY_EXISTS => "Este nome de usuário já existe.",
                ErrorCodes.LOGIN_WITH_WRONG_PASSWORD => "Senha incorreta.",
                ErrorCodes.MISSING_INFORMATION => "Informações obrigatórias ausentes.",
                ErrorCodes.TASK_NOT_FOUND => "Tarefa não encontrada.",
                ErrorCodes.TASK_TITLE_ALREADY_EXISTS => "Já existe uma tarefa com esse título.",
                _ => "Ocorreu um erro inesperado. Tente novamente."
            };
        }
    }
}
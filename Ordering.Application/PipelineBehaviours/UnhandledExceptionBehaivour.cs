using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.PipelineBehaviours
{
    public class UnhandledExceptionBehaivour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> //Mediatr üzerinden send methodunu calıstıdıgmızda try catch methodu ile encapsule ederek herhangi bir hata durumunda kontrollü bir şekilde yönetilmesini sağlayacağız.
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehaivour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();  //try icinde delegate edip bir hata ile karsılasırsak catch'e girip log tutacağız.
            }
            catch (Exception ex)
            {

                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "CleanArchitecture Request: Undhandled Exception for Request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}

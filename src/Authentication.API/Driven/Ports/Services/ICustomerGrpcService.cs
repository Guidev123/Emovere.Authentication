using Authentication.API.Application.Commands.Users.Register;
using Emovere.SharedKernel.Responses;

namespace Authentication.API.Driven.Ports.Services
{
    public interface ICustomerGrpcService
    {
        Task<Response> CreateAsync(RegisterUserCommand command, Guid userId);
    }
}
using MediatR;

namespace Identity.Application.Authenticates.Commands.Logins;

public class LoginCommand : IRequest<GenericResponse<LoginResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, GenericResponse<LoginResponse>>
{
    public Task<GenericResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
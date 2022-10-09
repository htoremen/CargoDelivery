using MediatR;

namespace Identity.Application.Authenticates.Commands.Authenticates;

public class AuthenticateCommand : IRequest<GenericResponse<AuthenticateResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, GenericResponse<AuthenticateResponse>>
{
    public Task<GenericResponse<AuthenticateResponse>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
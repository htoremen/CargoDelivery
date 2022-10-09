using Grpc.Core;
using Identity.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Identity.Application.Authenticates.Commands.Logins;

public class LoginCommand : IRequest<GenericResponse<LoginResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, GenericResponse<LoginResponse>>
{
    private readonly IApplicationDbContext _context;

    public LoginCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username);

        // validate
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
           return GenericResponse<LoginResponse>.NotFoundException("kullanıcı bulunamadı", 404);

        return GenericResponse<LoginResponse>.Success(200);
    }
}
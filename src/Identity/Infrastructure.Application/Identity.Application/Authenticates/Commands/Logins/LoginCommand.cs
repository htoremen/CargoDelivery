using Grpc.Core;
using Identity.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Identity.Application.Extensions;
using System.Net;
using Identity.Domain.Entities;

namespace Identity.Application.Authenticates.Commands.Logins;

public class LoginCommand : IRequest<GenericResponse<LoginResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string IpAddress { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, GenericResponse<LoginResponse>>
{
    private readonly IApplicationDbContext _context;
    private IJwtUtils _jwtUtils;

    public LoginCommandHandler(IApplicationDbContext context, IJwtUtils jwtUtils)
    {
        _context = context;
        _jwtUtils = jwtUtils;
    }

    public async Task<GenericResponse<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(x => x.RefreshTokens).FirstOrDefaultAsync(x => x.Username == request.Username);

        // validate
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
           return GenericResponse<LoginResponse>.NotFoundException("kullanıcı bulunamadı", 404);

        var jwtToken = _jwtUtils.GenerateJwtToken(user);
        var refreshToken = _jwtUtils.GenerateRefreshToken(request.IpAddress);
        user.RefreshTokens.Add(refreshToken);
        user.RefreshTokens.RemoveAll(x => !x.IsActive && x.Created.AddDays(2) <= DateTime.UtcNow);

        // save changes to db
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return GenericResponse<LoginResponse>.Success(new LoginResponse(user, jwtToken, refreshToken.Token), 200);
    }

}
using Identity.Application.Common.Interfaces;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Authenticates.Commands.CreateUsers;

public class CreateUserCommand : IRequest<GenericResponse<CreateUserResponse>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, GenericResponse<CreateUserResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GenericResponse<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var check = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == request.Username.ToLower());
        if (check == null)
        {
            var newUser = new User
            {
                UserId = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);
        }
        return GenericResponse<CreateUserResponse>.Success(200);
    }
}

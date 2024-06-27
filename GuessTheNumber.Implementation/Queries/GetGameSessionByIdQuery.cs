using GuessTheNumber.Application.Models.Responses;
using GuessTheNumber.Dal.Abstractions.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GuessTheNumber.Implementation.Queries;

public sealed record GetGameSessionByIdQuery(Guid SessionId) : IRequest<GetGameSessionResponse?>;

public class GetGameSessionByIdQueryHandler : IRequestHandler<GetGameSessionByIdQuery, GetGameSessionResponse?>
{
    private readonly IGameSessionReadContext _context;

    public GetGameSessionByIdQueryHandler(IGameSessionReadContext context)
    {
        _context = context;
    }

    public async Task<GetGameSessionResponse?> Handle(GetGameSessionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var session = await _context.GameSessions.FirstOrDefaultAsync(x => x.SessionId == request.SessionId,
            cancellationToken);

        return session == null ? null : new GetGameSessionResponse(session.TargetValue, session.AttemptCount);
    }
}
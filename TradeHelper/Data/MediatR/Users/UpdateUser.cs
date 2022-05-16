using MediatR;
using Remora.Rest.Core;

namespace TradeHelper.Data.MediatR.Users;

public static class UpdateUser
{
    public record Request(Snowflake Repper, Snowflake UserID, bool? PositiveRep) : IRequest;
    
    internal class Handler : IRequestHandler<Request>
    {
        private readonly TradeContext _db;
        public Handler(TradeContext db) => _db = db;

        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FindAsync(request.UserID);
            var repper = await _db.Users.FindAsync(request.Repper);
            
            if (user is null || repper is null)
                return Unit.Value;

            if (request.PositiveRep is { } definedPositiveRep)
            {
                repper.LastRepTime = DateTime.UtcNow;
                _ = definedPositiveRep ? user.Reputation++ : user.Reputation--;
            }

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
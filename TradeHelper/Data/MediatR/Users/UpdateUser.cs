using MediatR;
using Remora.Rest.Core;

namespace TradeHelper.Data.MediatR.Users;

public static class UpdateUser
{
    public record Request(Snowflake userID, bool? Rep) : IRequest;
    
    internal class Handler : IRequestHandler<Request>
    {
        private readonly TradeContext _db;

        public Handler(TradeContext db) => _db = db;

        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FindAsync(request.userID);

            if (user is null)
                return Unit.Value;
            
            if (request.Rep is { } definedRep) 
                _ = definedRep ? user.Reputation++ : user.Reputation--;

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
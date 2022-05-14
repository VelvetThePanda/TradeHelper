using MediatR;
using Remora.Rest.Core;
using TradeHelper.Data.Models;

namespace TradeHelper.Data.MediatR.Users;

public static class CreateUser
{
    public record Request(Snowflake ID) : IRequest;
    
    internal class Handler : IRequestHandler<Request>
    {
        private readonly TradeContext _db;

        public Handler(TradeContext db) => _db = db;

        public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = new TradeUser() { ID = request.ID };
            _db.Add(user);
            
            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch { /* Ignore */ }
            
            return Unit.Value;
        }
    }
}
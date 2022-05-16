using MediatR;
using Remora.Rest.Core;
using TradeHelper.Data.DTOs;
using TradeHelper.Data.Models;

namespace TradeHelper.Data.MediatR.Users;

public static class CreateUser
{
    public record Request(Snowflake ID) : IRequest<TradeUserDTO>;
    
    internal class Handler : IRequestHandler<Request, TradeUserDTO>
    {
        private readonly TradeContext _db;

        public Handler(TradeContext db) => _db = db;

        public async Task<TradeUserDTO> Handle(Request request, CancellationToken cancellationToken)
        {
            var existingUser = await _db.Users.FindAsync(request.ID);
            
            if (existingUser is not null)
                return existingUser.ToDTO();

            var user = new TradeUser() { ID = request.ID };
            _db.Add(user);
            
            try
            {
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch { /* Ignore */ }
            
            return user.ToDTO();
        }
    }
}
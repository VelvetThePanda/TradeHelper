using MediatR;
using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;

namespace TradeHelper.Data.MediatR;

public static class UserExists
{
    public record Request(Snowflake UserID) : IRequest<bool>;
    
    internal class Handler : IRequestHandler<Request, bool>
    {
        private readonly TradeContext _database;
        
        public Handler(TradeContext database) => _database = database;

        public Task<bool> Handle(Request request, CancellationToken cancellationToken) 
            => _database.Users.AnyAsync(x => x.ID == request.UserID, cancellationToken);
    }
}
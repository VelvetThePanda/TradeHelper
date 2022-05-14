using MediatR;
using Microsoft.EntityFrameworkCore;
using Remora.Rest.Core;
using TradeHelper.Data.DTOs;

namespace TradeHelper.Data.MediatR.Users;

public static class GetUser
{
    public record Request(Snowflake ID) : IRequest<TradeUserDTO?>;
    
    internal class Handler : IRequestHandler<Request, TradeUserDTO?>
    {
        private readonly TradeContext _context;

        public Handler(TradeContext context)
        {
            _context = context;
        }

        public async Task<TradeUserDTO?> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.TradeOffers)
                .Include(u => u.ClaimedTrades)
                .FirstOrDefaultAsync(u => u.ID == request.ID);
            
            return user?.ToDTO();
        }
    }
}
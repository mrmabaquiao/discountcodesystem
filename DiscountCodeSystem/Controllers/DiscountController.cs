using DiscountCodeSystem.Data;
using DiscountCodeSystem.Middleware;
using DiscountCodeSystem.Models;
using DiscountCodeSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;





namespace DiscountCodeSystem.Controllers
{
    [ApiController]
    [Route("api/discount")]
    public class DiscountController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly DiscountCodeGenerator _generator;
        private readonly WebSocketHandler _wsHandler;

        public DiscountController(AppDbContext db, DiscountCodeGenerator generator, WebSocketHandler wsHandler)
        {
            _db = db;
            _generator = generator;
            _wsHandler = wsHandler;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<bool>> Generate( [FromQuery] int count = 1, [FromQuery] byte length = 7)
            
        {
            if (length < 7 || length > 8) return BadRequest(false);
            if (count < 1 || count > 2000) return BadRequest(false);

            var codes = _generator.GenerateUniqueCodes(count,length);
            // constant user for now - TestDiscountUser
            var entities = codes.Select(code => new DiscountCode { Code = code, User = "TestDiscountUser" }).ToList();

            await _db.DiscountCodes.AddRangeAsync(entities);
            await _db.SaveChangesAsync();

            foreach (var code in entities)
            {
                await _wsHandler.BroadcastAsync($"{code.Code} with length:{length.ToString()}");
            }

            return true;
        }

        [HttpPost("usecode")]
        public async Task<ActionResult<byte>> Use([FromQuery] string code)
        {
            if (code.Length < 7 || code.Length > 8) 
                return BadRequest(false);
            if (string.IsNullOrWhiteSpace(code)) 
                return BadRequest(false);

            code = code.Trim();

            var entry = await _db.DiscountCodes.FirstOrDefaultAsync(c => c.Code == code);
            if (entry == null || entry.IsRedeemed) return 0;

            entry.IsRedeemed = true;
            await _db.SaveChangesAsync();
            return 1;
        }
    }

}

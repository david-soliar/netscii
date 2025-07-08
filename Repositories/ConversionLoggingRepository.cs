using netscii.Models.Entities;
using netscii.Models;
using netscii.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace netscii.Repositories
{
    public class ConversionLoggingRepository
    {
        private readonly NetsciiContext _context;

        public ConversionLoggingRepository(NetsciiContext context)
        {
            _context = context;
        }

        public async Task<List<LogDto>> GetLogsAsync(TimeSpan period)
        {
            var cutoff = DateTime.UtcNow - period;


            return await _context.ConversionActivities
                .Where(ca => ca.Timestamp >= cutoff)
                .OrderByDescending(ca => ca.Timestamp)
                .Include(ca => ca.ConversionParameters)
                .Select(ca => new LogDto
                {
                    Activity = ca,
                    Parameters = ca.ConversionParameters
                })
                .ToListAsync();
        }

        public async Task LogConversionAsync(ConversionActivity activity, ConversionParameters parameters)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var existingParams = await _context.ConversionParameters.FirstOrDefaultAsync(p =>
                    p.Characters == parameters.Characters &&
                    p.Font == parameters.Font &&
                    p.Background == parameters.Background &&
                    p.Scale == parameters.Scale &&
                    p.Invert == parameters.Invert &&
                    p.Platform == parameters.Platform);

                if (existingParams != null)
                {
                    parameters = existingParams;
                }
                else
                {
                    _context.ConversionParameters.Add(parameters);
                    await _context.SaveChangesAsync();
                }

                activity.ConversionParametersId = parameters.Id;
                _context.ConversionActivities.Add(activity);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

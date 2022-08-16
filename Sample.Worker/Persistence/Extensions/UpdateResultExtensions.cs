using MongoDB.Driver;

namespace Sample.Worker.Persistence.Extensions
{
    public static class UpdateResultExtensions
    {
        public static bool IsSuccess(this UpdateResult result) =>
            result.IsAcknowledged && (result.ModifiedCount > 0 || result.MatchedCount > 0);
    }
}
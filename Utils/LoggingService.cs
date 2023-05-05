using Store_Core7.Model;
using Store_Core7.Repository;

namespace Store_Core7.Utils
{
    public class LoggingService
    {
        private readonly IRepository<LogModel> _repository;
        public LoggingService(IRepository<LogModel> repository)
        {
            _repository = repository;
        }
        public static void Log(LogModel logModel)
        {

        }
    }
}

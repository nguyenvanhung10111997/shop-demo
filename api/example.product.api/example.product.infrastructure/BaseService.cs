using example.domain.Interfaces;

namespace example.infrastructure
{
    public abstract class BaseService
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;
        protected IUnitOfWork UnitOfWork => _unitOfWork.Value;

        protected BaseService(Lazy<IUnitOfWork> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_unitOfWork != null && _unitOfWork.Value != null)
                {
                    _unitOfWork.Value.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~BaseService() { Dispose(false); }
    }
}

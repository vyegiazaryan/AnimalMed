using AnimalMed.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalMed.BLL.Services
{
    public class ServiceBase
    {
        protected readonly AnimalMedDbContext _db;

        public ServiceBase(AnimalMedDbContext db)
        {
            _db = db;
        }

        #region __ IDisposable Support __

        private bool isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}

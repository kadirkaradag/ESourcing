using Ordering.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ordering.Domain.Repositories.Base
{
    public interface IRepository<T> where T : Entity    //oluşturduğumuz tüm repositorylerde ortak kullanacağımız methodlar
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate); //linq expression ile x=> x.id == 1 şeklinde alabilmek için
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>,IOrderedQueryable<T>> orderBy = null, //order işlemleri icin
                                        string includeString=null, // eager ve lazy loading ile mesela user tablosunu cektigimiz zaman 1'e N şeklinde adresler tablosuna da bağlıysa adresler tablosunu da cekmemizi sağlayacak.
                                        bool disableTracking = true //Entity framework AsNoTracking özelliği ile aslında nesne değişikliklerini takip etmemesini söyleyebiliriz yani get ile cagırıp update etmeyeceksek bu değişimi takip etmemesini isteyerek performansta artış sağlayabiliriz.
            );

        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}

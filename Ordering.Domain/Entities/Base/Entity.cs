using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities.Base
{
    public abstract class Entity : IEntityBase
    {
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; protected set; }

        public Entity Clone()  // ilgili entity ye eriştiğimizde örneğin order.clone dedğimizde yeni bir order nesnesi olustururuz. örnek amaclı. Entity den türeyen tüm sınıvlara bu method uygulanabilir.
        {
            return (Entity)this.MemberwiseClone();
        }
    }
}

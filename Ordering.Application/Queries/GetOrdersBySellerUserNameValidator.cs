using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Queries
{
    public class GetOrdersBySellerUserNameValidator : AbstractValidator<GetOrdersBySellerUserNameQuery>
    {
        public GetOrdersBySellerUserNameValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().EmailAddress();
        }
    }
}

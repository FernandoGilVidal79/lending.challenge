using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LendingService.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace LendingService.Infrastructure.Context
{
    internal class ApplicationDbContext : DbContext, IApplicationDbContext
    {
    }
}

using shitfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shitfo.Services
{
    public class LayoutViewModelService
    {
        private readonly AppDbContext _context;
       
        public LayoutViewModelService(AppDbContext context)
        {
            _context = context;
            
        }
        public List<Setting> GetSettings()
        {
            return _context.Settings.ToList();
        }
        
    }
}

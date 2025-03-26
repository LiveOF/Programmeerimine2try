using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ServicesService : IServicesService
    {
        private readonly ApplicationDbContext _context;

        public ServicesService(ApplicationDbContext context)
        {
            _context = context;
        }

         
        public async Task<PagedResult<Service>> List(int page, int pageSize, ServicesSearch search)
        {
            var query = _context.Service.AsQueryable();

            // Apply filtering if a keyword is provided
            if (!string.IsNullOrEmpty(search?.Keyword))
            {
                query = query.Where(s => Convert.ToString(s.Id).Contains(search.Keyword)); // Adjust property as needed
            }

            return await query.GetPagedAsync(page, pageSize); // Use pagination after filtering
        }
        public async Task<Service> Get(int id)
        {
            return await _context.Service.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task Save(Service list)
        {
            if (list.Id == 0)
            {
                _context.Add(list);
            }
            else
            {
                _context.Update(list);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var service = await _context.Service.FindAsync(id);
            if (service != null)
            {
                _context.Service.Remove(service);
                await _context.SaveChangesAsync();
            }
        }
    }
}

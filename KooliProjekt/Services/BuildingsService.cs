﻿using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class BuildingsService : IBuildingsService
    {
        private readonly ApplicationDbContext _context;

        public BuildingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var building = await _context.Building.FindAsync(id);
            if (building == null)
            {
                return;
            }

            _context.Building.Remove(building);
            await _context.SaveChangesAsync();
        }

        public async Task<Building> Get(int id)
        {
            return await _context.Building.FindAsync(id);
        }

        public async Task<PagedResult<Building>> List(int page, int pageSize, BuildingsSearch search = null)
        {
            var query = _context.Building.AsQueryable();

            search = search ?? new BuildingsSearch();

            if (!string.IsNullOrWhiteSpace(search.Keyword))
            {
                query = query.Where(list => list.Location.Contains(search.Keyword));
            }

            return await query
                .OrderBy(list => list.Location)
                .GetPagedAsync(page, pageSize);
        }
        public async Task<IEnumerable<Building>> GetAll()
        {
            // Assuming you have a DbContext named _context
            return await _context.Building.ToListAsync();
        }
        public async Task Save(Building list)
        {
            if (list.Id == 0)
            {
                _context.Building.Add(list);
            }
            else
            {
                _context.Building.Update(list);
            }

            await _context.SaveChangesAsync();
        }
    }
}

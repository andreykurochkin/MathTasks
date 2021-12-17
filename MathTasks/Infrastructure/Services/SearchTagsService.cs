using MathTasks.Contracts;
using MathTasks.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathTasks.Infrastructure.Services
{
    public class SearchTagsService : ISearchTagsService
    {
        private readonly ApplicationDbContext _context;

        public SearchTagsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<string> SearchTags(string term)
        {
            var items = _context!.Tags!
                .AsNoTracking()
                .Where(x => x.Name.ToLower().StartsWith(term.ToLower()))
                .Select(x => x.Name)
                .ToList();
            return items;
        }
    }
}

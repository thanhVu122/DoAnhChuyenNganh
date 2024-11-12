using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Vfurniture.Reponsitory.Components
{
    public class DanhMucsViewComponent : ViewComponent
    {
        private readonly DataContext _dataContext;
        public DanhMucsViewComponent(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.DanhMucs.ToListAsync());
    }
}

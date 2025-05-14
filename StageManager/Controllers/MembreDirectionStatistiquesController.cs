using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StageManager.DTO;
using StageManager.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using TestRestApi.Data;

namespace StageManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "MembreDirection")]
    public class MembreDirectionStatistiquesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MembreDirectionStatistiquesController(AppDbContext context)
        {
            _context = context;
        }

    }
}
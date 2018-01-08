using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingAPI.Controllers.Response;
using DatingAPI.Core;
using DatingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository datingRepository, IMapper mapper)
        {
            _datingRepository = datingRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _datingRepository.GetUsers();

            var userListResponse = _mapper.Map<IEnumerable<UserListResponse>>(users);
            
            return Ok(userListResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _datingRepository.GetUser(id);

            if (user == null)
                return NotFound();


            var userDetailResponse = _mapper.Map<UserDetailResponse>(user);
            
            return Ok(userDetailResponse);
        }
    }
}
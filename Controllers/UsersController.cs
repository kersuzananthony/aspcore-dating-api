using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingAPI.Controllers.Queries;
using DatingAPI.Controllers.Requests;
using DatingAPI.Controllers.Response;
using DatingAPI.Core;
using DatingAPI.Helpers;
using DatingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository datingRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _datingRepository = datingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(UserQuery query)
        {
            var users = await _datingRepository.GetUsersAsync(query);

            var userListResponse = _mapper.Map<QueryResultResponse<UserListResponse>>(users);
            
            return Ok(userListResponse);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _datingRepository.GetUserAsync(id);

            if (user == null)
                return NotFound();


            var userDetailResponse = _mapper.Map<UserDetailResponse>(user);
            
            return Ok(userDetailResponse);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _datingRepository.GetUserAsync(id);

            if (user == null)
                return NotFound($"Could not find user with an ID of {id}");

            if (user.Id != currentUserId)
                return Forbid("You cannot update data from another user.");

            _mapper.Map(request, user);

            if (await _unitOfWork.CompleteAsync())
            {
                return NoContent();   
            }
            
            throw new Exception($"Updating user ${id} failed on save.");
        }
    }
}
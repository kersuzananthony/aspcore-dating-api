﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingAPI.Controllers.Queries;
using DatingAPI.Controllers.Requests;
using DatingAPI.Controllers.Response;
using DatingAPI.Core;
using DatingAPI.Extensions;
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
            var currentUser = await this.GetCurrentUser();

            if (currentUser == null)
                return Unauthorized();

            query.UserId = currentUser.Id;

            if (string.IsNullOrEmpty(query.Gender))
                query.Gender = currentUser.Gender == "female" ? "male" : "female";

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

            var currentUserId = this.GetCurrentUserId();
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

        [HttpPost("{userId}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int userId, int recipientId)
        {
            var currentUserId = this.GetCurrentUserId();

            if (currentUserId != userId)
                return Forbid();

            if (userId == recipientId)
                return BadRequest("You cannot like yourself.");

            var like = await _datingRepository.GetLikeAsync(userId, recipientId);

            if (like != null)
                return BadRequest("You alreay liked this user");

            if (await _datingRepository.GetUserAsync(recipientId) == null)
                return NotFound();

            var newLike = new Like
            {
                LikeeId = recipientId,
                LikerId = userId
            };
            
            _datingRepository.Add(newLike);

            if (await _unitOfWork.CompleteAsync())
            {
                return Ok();
            }

            return BadRequest("Failed to like user.");
        }
    }
}
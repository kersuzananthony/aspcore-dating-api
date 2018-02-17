using System;
using System.Collections.Generic;
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
    [Route("api/users/{userId}/[controller]")]
    public class MessagesController : Controller
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessagesController(IDatingRepository datingRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _datingRepository = datingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(int userId, MessageQuery query)
        {
            if (userId != this.GetCurrentUserId())
            {
                return Unauthorized();
            }

            var messages = await _datingRepository.GetMessagesForUserAsync(query);

            return Ok(_mapper.Map<QueryResultResponse<MessageResponse>>(messages));
        }

        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId, string id)
        {
            if (userId != this.GetCurrentUserId())
            {
                return Unauthorized();
            }

            var message = await _datingRepository.GetMessageAsync(Guid.Parse(id));

            if (message == null)
                return NotFound();

            return Ok(_mapper.Map<MessageRequest>(message));
        }

        [HttpGet("thread/{recipentId}")]
        public async Task<IActionResult> GetThreadMessages(int userId, int recipientId)
        {
            if (userId != this.GetCurrentUserId())
                return Unauthorized();

            var messages = await _datingRepository.GetMessagesThreadAsync(userId, recipientId);

            return Ok(_mapper.Map<IEnumerable<MessageResponse>>(messages));
        }
        

        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, [FromBody] MessageRequest request)
        {
            if (userId != this.GetCurrentUserId())
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            request.SenderId = userId;

            var recipient = await _datingRepository.GetUserAsync(request.RecipentId);
            if (recipient == null)
            {
                ModelState.AddModelError("RecipentId", "Recipent does not exist");
                return BadRequest(ModelState);
            }

            var message = _mapper.Map<Message>(request);
            
            _datingRepository.Add(message);

            if (await _unitOfWork.CompleteAsync())
                return CreatedAtRoute("GetMessage", new { id = message.Id }, message);

            return BadRequest("An error happened during the creation of the message");
        }
    }
}
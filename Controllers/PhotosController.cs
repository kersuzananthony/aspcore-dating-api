using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingAPI.Controllers.Requests;
using DatingAPI.Controllers.Response;
using DatingAPI.Core;
using DatingAPI.Helpers;
using DatingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingAPI.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    public class PhotosController : Controller
    {
        private readonly IDatingRepository _datingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository datingRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            IOptions<CloudinarySettings> cloudinarySettings)
        {
            _datingRepository = datingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinarySettings = cloudinarySettings;
            
            var account = new Account(
                _cloudinarySettings.Value.CloudName,
                _cloudinarySettings.Value.ApiKey,
                _cloudinarySettings.Value.ApiSecret);
            
            _cloudinary = new Cloudinary(account);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int userId, string id)
        {
            var photoFromRepo = await _datingRepository.GetPhotoAsync(id);

            if (photoFromRepo == null)
                return NotFound();

            return Ok(_mapper.Map<PhotoDetailResponse>(photoFromRepo));
        }
        
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, PhotoCreationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var user = await _datingRepository.GetUserAsync(userId);

            if (user == null)
                return BadRequest("Cannot find user.");

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (user.Id != currentUserId)
                return Forbid();

            var file = request.File;
   
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            request.PublicId = uploadResult.PublicId;
            request.Url = uploadResult.Uri.ToString();

            var photo = _mapper.Map<Photo>(request);
            photo.User = user;

            if (!user.Photos.Any(p => p.IsMain))
                photo.IsMain = true;

            user.Photos.Add(photo);

            if (await _unitOfWork.CompleteAsync())
            {
                return CreatedAtRoute("GetPhoto", new {id = photo.Id.ToString()}, _mapper.Map<PhotoDetailResponse>(photo));
            }

            return BadRequest("Could not save the photo.");
        }

        [HttpPost("{id}/main")]
        public async Task<IActionResult> setMainPhoto(int userId, string id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Forbid("You cannot update this data.");

            var photo = await this._datingRepository.GetPhotoAsync(id);

            if (photo == null)
                return NotFound();

            if (photo.IsMain)
                return BadRequest("Photo is already main.");

            photo.IsMain = true;

            var currentMainPhoto = await _datingRepository.GetMainPhotoForUserAsync(userId);

            if (currentMainPhoto != null)
                currentMainPhoto.IsMain = false;

            if (await _unitOfWork.CompleteAsync())
            {
                return NoContent();
            }

            return BadRequest("Could not save the photo");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, string id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Forbid("You cannot update this data.");

            var photo = await this._datingRepository.GetPhotoAsync(id);

            if (photo == null)
                return NotFound();
            
            if (photo.IsMain)
                return BadRequest("You cannot delete your main photo.");
            
            var deleteParams = new DeletionParams(photo.PublicId);

            var result = _cloudinary.Destroy(deleteParams);
            
            if (result.Result == "ok")
                _datingRepository.Remove(photo);

            if (await _unitOfWork.CompleteAsync())
                return NoContent();

            return BadRequest("Could not delete the photo");
        }
    }
}
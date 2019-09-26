using AutoMapper;
using CompositeKey.Core.Enums;
using CompositeKey.Domain.ViewModels;
using CompositeKey.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CompositeKey.API.Base
{
    public class CommonController : ApiController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<CommonController> _logger;

        public CommonController(
            IConfiguration configuration,
            IMapper mapper,
            ILogger<CommonController> logger)
            : base(configuration)
        {
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        protected OkResult OkCreateResponse(User user, string name)
        {
            _logger.LogInformation($"Created Area with Name = {name} by user {user.Id}: {user.UserName}");

            return Ok();
        }

        protected OkResult OkUpdateResponse(User user, string areaId)
        {
            _logger.LogInformation($"Updated Area with id = {areaId} by user {user.Id}: {user.UserName}");

            return Ok();
        }

        protected OkResult OkDeleteResponse(User user, string areaId)
        {
            _logger.LogInformation($"Deleted Area with id = {areaId} by user {user.Id}: {user.UserName}");

            return Ok();
        }

        protected BadRequestObjectResult AlreadyExist(int areaId)
        {
            _logger.LogError($"Area with id = {areaId} has already existed");

            return BadRequest
            (
                GetErrorResponseViewModel(ErrorCode.AlreadyExist, $"Area with id = {areaId} has already existed")
            );
        }

        protected BadRequestObjectResult NotExist(string areaId)
        {
            _logger.LogError($"Area with id = {areaId} doesn't exist");

            return BadRequest
            (
                GetErrorResponseViewModel(ErrorCode.NotExist, $"Area with id = {areaId} doesn't exist")
            );
        }

        protected BadRequestObjectResult UserNotFound()
        {
            _logger.LogError($"User not found");

            return BadRequest
            (
                GetErrorResponseViewModel(ErrorCode.UserNotExist, $"User not found")
            );
        }

        protected ErrorResponseViewModel GetErrorResponseViewModel(ErrorCode errorCode, string description)
        {
            return new ErrorResponseViewModel
            {
                ErrorCode = errorCode.ToString(),
                Description = description
            };
        }

    }
}

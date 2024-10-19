using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TCC.Biometric.Payment.Config;
using TCC.Biometric.Payment.DTOs;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;
using TCC.Payment.Data.Interfaces;


using ILogger = Serilog.ILogger;

namespace TCC.Biometric.Payment.Controllers
{
    [Route("api/trigger")]
    [ApiController]
    public class TriggerController : Controller
    {
        private readonly ITriggerRepository _triggerRepository;
      
        private readonly IMapper _autoMapper;
        private readonly ILogger _logger;

        public TriggerController(ITriggerRepository triggerRepository,           
            IMapper autoMapper, 
            ILogger logger)
        {           
            _triggerRepository = triggerRepository;
            _autoMapper = autoMapper;
            _logger = logger;
        }


        [Route("getlive")]
        [HttpGet]
        //[OpenApiTags("Transaction")]  
        [ProducesResponseType(typeof(ResultDto<TriggerResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<TriggerResponseDto>))]
        public async Task<IActionResult> GetLive(string Id, CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();
            var response = new ResultDto<TriggerResponseDto>();

            var trigger = _triggerRepository.GetLive(Id).Result;

            if (trigger!=null)
            {
                response.data = _autoMapper.Map<TriggerResponseDto>(trigger);
                response.success = true;
            }

            return Ok(response);

        }
        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<TriggerResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<TriggerResponseDto>))]
        public async Task<IActionResult> create(TriggerRequestDto request, CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<TriggerResponseDto>();
            await _triggerRepository.DiscardOldPendingTriggerAsync(request.device_ID);

            var trigger = _autoMapper.Map<Trigger>(request);
            trigger.createdDate = DateTime.Now;
            trigger.status = TriggerStatus.pending;

            var result = (await _triggerRepository.AddAsync(trigger));
            _triggerRepository.SaveChanges();           

            response.data = _autoMapper.Map<TriggerResponseDto>(result.Entity);
            response.success = true;

            return Ok(response);

        }
    }

}

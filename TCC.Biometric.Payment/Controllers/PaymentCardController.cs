using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using TCC.Biometric.Payment.DTOs;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;
using TCC.Payment.Data.Interfaces;
using TCC.Payment.Data.Repositories;
using ILogger = Serilog.ILogger;

namespace TCC.Biometric.Payment.Controllers
{

    [Route("api/paymentCard")]
    [ApiController]
    public class PaymentCardController : Controller
    {
        private readonly IPaymentCardRepository _paymentCardRepository;

        private readonly IMapper _autoMapper;
        private readonly ILogger _logger;

        public PaymentCardController(IPaymentCardRepository paymentCardRepository,
            IMapper autoMapper, IAuthenticationService authenticationService,
            ILogger logger)
        {
            _paymentCardRepository = paymentCardRepository;
            _autoMapper = autoMapper;
            _logger = logger;
        }


        [Route("get")]
        [HttpGet]       
        //[OpenApiTags("OnboardingPaymentCard")]  
        [ProducesResponseType(typeof(ResultDto<PaymentCardResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<PaymentCardResponseDto>))]
        public async Task<IActionResult> GetPaymentCard(Guid Id, CancellationToken cancellationToken = default)
        {  
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<PaymentCardResponseDto>();

            var paymentCard = _paymentCardRepository.GetByID(Id);


            if (paymentCard == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "PaymentCard Not Found";
                //response.error.errorDetails = "digital ID PaymentCard Not Found";

                return Conflict(response);
            }

            response.data = _autoMapper.Map<PaymentCardResponseDto>(paymentCard);


            response.success = true;
            return Ok(response);


        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<PaymentCardResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<PaymentCardResponseDto>))]
        public async Task<IActionResult> create(PaymentCardRequestDto request, CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<PaymentCardResponseDto>();


            var paymentCard = _autoMapper.Map<PaymentCard>(request);
            paymentCard.createdDate = DateTime.Now;
            paymentCard.status = CardStatus.pending;

            var result = (await _paymentCardRepository.AddAsync(paymentCard));
            _paymentCardRepository.SaveChanges();

            response.data = _autoMapper.Map < PaymentCardResponseDto > (result.Entity);
            response.success = true;

            return Ok(response);

        }

    }
}

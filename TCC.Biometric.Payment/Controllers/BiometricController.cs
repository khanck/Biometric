using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using System.Text.Json.Serialization;
using TCC.Biometric.Payment.Config;
using TCC.Biometric.Payment.DTOs;
using TCC.Biometric.Payment.Handlers;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Interfaces;
using TCC.Payment.Data.Repositories;
using TCC.Payment.Integration.Biometric;
using TCC.Payment.Integration.Interfaces;

using ILogger = Serilog.ILogger;

namespace TCC.Biometric.Payment.Controllers
{
    [Route("api/biometric")]
    [ApiController]
    public class BiometricController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBiometricVerificationRepository _biometricVerificationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPaymentCardRepository _paymentCardRepository;
        private readonly IAlpetaServer _alpetaServer;
      //  private readonly WebSocketHandler _webSocketHandler;

        private readonly IMapper _autoMapper;
        private readonly ILogger _logger;

        public BiometricController(ITransactionRepository transactionRepository,
            IBiometricVerificationRepository biometricVerificationRepository,
            ICustomerRepository customerRepository,
            IPaymentCardRepository paymentCardRepository,
            IAlpetaServer alpetaServer,
            IMapper autoMapper, IAuthenticationService authenticationService,
           // WebSocketHandler webSocketHandler,
            ILogger logger)
        {
            _transactionRepository = transactionRepository;
            _biometricVerificationRepository = biometricVerificationRepository;
            _customerRepository = customerRepository;
            _paymentCardRepository = paymentCardRepository;
            _alpetaServer = alpetaServer;
            _autoMapper = autoMapper;
           // _webSocketHandler = webSocketHandler;
            _logger = logger;
        }

        [Route("testsocket")]
        [HttpGet]
        //[OpenApiTags("Transaction")]  
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<CustomerResponseDto>))]
        [Produces(typeof(ResultDto<CustomerResponseDto>))]
        public async Task<IActionResult> TestSocket(CancellationToken cancellationToken = default)
        {
         
            // var webSocketHandler = new ClientWebSocket();
            //await webSocketHandler.HandleWebSocketConnection(context);

            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();


            return Ok();

        }


        [Route("verifycustomer")]
        [HttpGet]
        //[OpenApiTags("Transaction")]  
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<CustomerResponseDto>))]
        [Produces(typeof(ResultDto<CustomerResponseDto>))]
        public async Task<IActionResult> VerifyCustomer(CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();
            var response = new ResultDto<CustomerResponseDto>();


            var verification = _alpetaServer.GetCurrentUserBiometric().Result;

            if (verification.AuthLogList.IsNullOrEmpty())
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "Biometric not verified";
                response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);
            }

            var verificationDetail = _alpetaServer.GetVerificationDetails(verification.AuthLogList.FirstOrDefault().IndexKey).Result;

            if (verificationDetail.AuthLogDetail == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "Biometric not verified";
                response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);
            }

            var customer = _customerRepository.GetByCustomerID(Convert.ToInt32(verificationDetail.AuthLogDetail.UserID)).Result;

            if (customer == null) 
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "customer not found";
                //response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);
            }
            response.data = _autoMapper.Map<CustomerResponseDto>(customer);
            response.success = true;

            return Ok(response);

        }

    }

}

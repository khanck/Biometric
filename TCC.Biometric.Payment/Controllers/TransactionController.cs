using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using TCC.Biometric.Payment.DTOs;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;
using TCC.Payment.Data.Interfaces;
using TCC.Payment.Data.Repositories;
using TCC.Payment.Integration.Biometric;
using TCC.Payment.Integration.Interfaces;
using ILogger = Serilog.ILogger;

namespace TCC.Biometric.Payment.Controllers
{

    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBiometricVerificationRepository _biometricVerificationRepository;
        private readonly IAlpetaServer _alpetaServer;

        private readonly IMapper _autoMapper;
        private readonly ILogger _logger;

        public TransactionController(ITransactionRepository transactionRepository,
            IBiometricVerificationRepository biometricVerificationRepository,
            IAlpetaServer alpetaServer,
            IMapper autoMapper, IAuthenticationService authenticationService,
            ILogger logger)
        {
            _transactionRepository = transactionRepository;
            _biometricVerificationRepository = biometricVerificationRepository;
            _alpetaServer = alpetaServer;
            _autoMapper = autoMapper;
            _logger = logger;
        }

        
        [Route("get")]
        [HttpGet]       
        //[OpenApiTags("Transaction")]  
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<TransactionResponseDto>))]
        [Produces(typeof(ResultDto<TransactionResponseDto>))]
        public async Task<IActionResult> GetTransaction(Guid Id, CancellationToken cancellationToken = default)
        {  //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
           //    return Unauthorized();


            var data = _alpetaServer.Login();
            var res = _alpetaServer.GetCurrentUserBiometric().Result;

            var details = _alpetaServer.GetVerificationDetails(res.AuthLogList.FirstOrDefault().IndexKey);



            var response = new ResultDto<TransactionResponseDto>();

            var transaction = _transactionRepository.GetByID(Id);


            if (transaction == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "Transaction Not Found";
                //response.error.errorDetails = "digital ID Transaction Not Found";

                return Conflict(response);
            }

            response.data = _autoMapper.Map<TransactionResponseDto>(transaction);


            response.success = true;
            return Ok(response);


        }

        [Route("getbycustomer")]
        [HttpGet]
        //[OpenApiTags("OnboardingTransaction")]  
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<List<TransactionResponseDto>>))]
        [Produces(typeof(ResultDto<List<TransactionResponseDto>>))]
        public async Task<IActionResult> GetByCustomer(Guid Id, CancellationToken cancellationToken = default)
        {  //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<List<TransactionResponseDto>>();

            var result = _transactionRepository.GetAllByCustomerID(Id).Result;


            if (result == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "Transactions Not Found";
                //response.error.errorDetails = "digital ID Transaction Not Found";

                return Conflict(response);
            }

            response.data = _autoMapper.Map<List<TransactionResponseDto>>(result);


            response.success = true;
            return Ok(response);


        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<TransactionResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<TransactionResponseDto>))]
        public async Task<IActionResult> create(TransactionRequestDto request, CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

        


            var response = new ResultDto<TransactionResponseDto>();


            var biometricVerification = _autoMapper.Map<BiometricVerification>(request.biometricVerification);
            biometricVerification.createdDate = DateTime.Now;
            biometricVerification.verificationStatus = VerificationStatus.pending;
            biometricVerification.verificationResponse = "pending";
            var Verificationresult = (await _biometricVerificationRepository.AddAsync(biometricVerification));
            _biometricVerificationRepository.SaveChanges();

            var transaction = _autoMapper.Map<Transaction>(request);
            transaction.biometricVerification_ID = biometricVerification.Id;
            transaction.createdDate = DateTime.Now;
            transaction.status = TransactionStatus.pending;
            transaction.transactionNumber= (Random.Shared.Next(1000, 10000)).ToString(); 

            var result = (await _transactionRepository.AddAsync(transaction));
            _transactionRepository.SaveChanges();

            response.data = _autoMapper.Map < TransactionResponseDto > (result.Entity);
            response.success = true;

            return Ok(result);

        }


    }
}

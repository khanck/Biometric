using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using TCC.Biometric.Payment.DTOs;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;
using TCC.Payment.Data.Interfaces;
using TCC.Payment.Data.Repositories;
using TCC.Payment.Integration.Biometric;
using TCC.Payment.Integration.Config;
using TCC.Payment.Integration.Interfaces;
using ILogger = Serilog.ILogger;
using TCC.Payment.Integration.Models.Innovatrics;
using TCC.Payment.Integration.Models;

namespace TCC.Biometric.Payment.Controllers
{

    [Route("api/transaction")]
    [ApiController]
    public class TransactionController : Controller
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly IBiometricVerificationRepository _biometricVerificationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPaymentCardRepository _paymentCardRepository;
        private readonly IBiometricRepository _biometricRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IBusinessRepository _businessRepository;

        private readonly IAlpetaServer _alpetaServer;
        private readonly IInnovatricsAbis _innovatricsAbis;

        private readonly IMapper _autoMapper;
        private readonly ILogger _logger;

        public TransactionController(ITransactionRepository transactionRepository,
            IBiometricVerificationRepository biometricVerificationRepository,
            ICustomerRepository customerRepository,
            IPaymentCardRepository paymentCardRepository,
            IBiometricRepository biometricRepository,
            IAccountRepository accountRepository,
            IBusinessRepository businessRepository,
        IAlpetaServer alpetaServer,
        IInnovatricsAbis innovatricsAbis,
            IMapper autoMapper, IAuthenticationService authenticationService,
            ILogger logger)
        {
            _transactionRepository = transactionRepository;
            _biometricVerificationRepository = biometricVerificationRepository;
            _customerRepository = customerRepository;
            _paymentCardRepository = paymentCardRepository;
            _biometricRepository = biometricRepository;
            _businessRepository = businessRepository;
            _accountRepository = accountRepository;
            _alpetaServer = alpetaServer;
            _innovatricsAbis = innovatricsAbis;
            _autoMapper = autoMapper;
            _logger = logger;
        }


        [Route("get")]
        [HttpGet]
        //[OpenApiTags("Transaction")]  
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<TransactionResponseDto>))]
        [Produces(typeof(ResultDto<TransactionResponseDto>))]
        public async Task<IActionResult> GetTransaction(Guid Id, CancellationToken cancellationToken = default)
        { 
           //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
           //    return Unauthorized();
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

            //var customer = _customerRepository.GetByCustomerID(Convert.ToInt32(verificationDetail.AuthLogDetail.UserID)).Result;

            //response.data.customer = _autoMapper.Map<CustomerResponseDto>(customer);
            //response.data.paymentCard = _autoMapper.Map<PaymentCardResponseDto>(paymentCard);
            //response.data.biometricVerification = _autoMapper.Map<BiometricVerificationResponseDto>(biometricVerification);



            response.success = true;
            return Ok(response);


        }

     
        [Route("getbycustomer")]
        [HttpGet]
        //[OpenApiTags("OnboardingTransaction")]  
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<List<CustomerTransactionResponseDto>>))]
        [Produces(typeof(ResultDto<List<CustomerTransactionResponseDto>>))]
        public async Task<IActionResult> GetByCustomer(Guid Id, CancellationToken cancellationToken = default)
        {  //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<List<CustomerTransactionResponseDto>>();

            var result = _transactionRepository.GetAllByCustomerID(Id).Result;


            if (result == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "Transactions Not Found";
                //response.error.errorDetails = "digital ID Transaction Not Found";

                return Conflict(response);
            }


            var transactions = _autoMapper.Map<List<CustomerTransactionResponseDto>>(result);
            response.data=new List<CustomerTransactionResponseDto>();
            foreach (var transaction in transactions)
            {
               var account = _autoMapper.Map<AccountResponseDto>(_accountRepository.GetByID(transaction.account_ID));
                transaction.business = _autoMapper.Map<BusinessResponseDto>(_businessRepository.GetByID(account.business_ID));
                response.data.Add(transaction);
            }


            response.success = true;
            return Ok(response);

        }

        [Route("getbybusiness")]
        [HttpGet]
        //[OpenApiTags("OnboardingTransaction")]  
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<List<TransactionResponseDto>>))]
        [Produces(typeof(ResultDto<List<TransactionResponseDto>>))]
        public async Task<IActionResult> GetByBusiness(Guid Id, CancellationToken cancellationToken = default)
        {  //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<List<TransactionResponseDto>>();

            var result = _transactionRepository.GetAllByBusinessID(Id).Result;


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

            return Ok(response);

        }

        [Route("biometricpayment")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<TransactionResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<TransactionResponseDto>))]
        public async Task<IActionResult> BiometricPayment(BiometricPaymentRequestDto request, CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();


            var response = new ResultDto<TransactionResponseDto>();


            var verification = _alpetaServer.GetCurrentUserBiometric().Result;

            for (int i = 0; i < 3; i++)
            {
                if (verification.AuthLogList.IsNullOrEmpty())
                {
                    await Task.Delay(2500);
                    verification = _alpetaServer.GetCurrentUserBiometric().Result;
                }else
                    break;
            }

            if (verification.AuthLogList.IsNullOrEmpty())
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "Biometric not verified";
                response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);
            }
            
            var verificationDetail = _alpetaServer.GetVerificationDetails(verification.AuthLogList.FirstOrDefault().IndexKey).Result;

            if (verificationDetail.AuthLogDetail==null)
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
            var paymentCard=  _paymentCardRepository.GetByCustomerID(Convert.ToInt32(verificationDetail.AuthLogDetail.UserID)).Result;

            if (paymentCard == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "payment Card not found";
                //response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);
            }

            var biometricVerification = _autoMapper.Map<BiometricVerification>(verificationDetail.AuthLogDetail);
            biometricVerification.customer_ID = paymentCard.customer_ID;
            //biometricVerification.verificationStatus = VerificationStatus.pending;
            //biometricVerification.verificationResponse = "pending";
            var Verificationresult = (await _biometricVerificationRepository.AddAsync(biometricVerification));
            _biometricVerificationRepository.SaveChanges();

            var transaction = _autoMapper.Map<Transaction>(request);
            transaction.biometricVerification_ID = biometricVerification.Id;
            transaction.paymentCard_ID = paymentCard.Id;
            transaction.createdDate = DateTime.Now;
            transaction.status = TransactionStatus.pending;
            transaction.transactionNumber = (Random.Shared.Next(1000, 10000)).ToString();

            var result = (await _transactionRepository.AddAsync(transaction));
            _transactionRepository.SaveChanges();

            response.data = _autoMapper.Map<TransactionResponseDto>(result.Entity);

            //var biometric = _biometricRepository.GetByCustomerID(customer.Id).Result;
            
            response.data.customer = _autoMapper.Map<CustomerResponseDto>(customer);
            //response.data.paymentCard = _autoMapper.Map<PaymentCardResponseDto>(paymentCard);
            response.data.biometricVerification = _autoMapper.Map<BiometricVerificationResponseDto>(biometricVerification);
            //response.data.customer.biometric.Add( _autoMapper.Map<BiometricResponseDto>(biometric));

            response.success = true;

            return Ok(response);

        }

        [Route("biometricpaymentbyuser")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<TransactionResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<TransactionResponseDto>))]
        public async Task<IActionResult> BiometricPaymentByUser(UserPaymentRequestDto request, CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();


            var response = new ResultDto<TransactionResponseDto>();

            var customer = _customerRepository.GetByID(request.customer_ID);

            var verification = _alpetaServer.GetCurrentUserBiometric(customer.TerminalUserId.ToString()).Result;  //need to change to user id

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

            var paymentCard = _paymentCardRepository.GetByCustomerID(Convert.ToInt32(verificationDetail.AuthLogDetail.UserID)).Result;

            if (paymentCard == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "customer not found";
                //response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);
            }

            var biometricVerification = _autoMapper.Map<BiometricVerification>(verificationDetail.AuthLogDetail);
            biometricVerification.customer_ID = paymentCard.customer_ID;
            //biometricVerification.verificationStatus = VerificationStatus.pending;
            //biometricVerification.verificationResponse = "pending";
            var Verificationresult = (await _biometricVerificationRepository.AddAsync(biometricVerification));
            _biometricVerificationRepository.SaveChanges();

            var transaction = _autoMapper.Map<Transaction>(request);
            transaction.biometricVerification_ID = biometricVerification.Id;
            transaction.paymentCard_ID = paymentCard.Id;
            transaction.createdDate = DateTime.Now;
            transaction.status = TransactionStatus.pending;
            transaction.transactionNumber = (Random.Shared.Next(1000, 10000)).ToString();

            var result = (await _transactionRepository.AddAsync(transaction));
            _transactionRepository.SaveChanges();

            response.data = _autoMapper.Map<TransactionResponseDto>(result.Entity);

            response.data.customer= _autoMapper.Map<CustomerResponseDto>(customer);
            response.data.paymentCard = _autoMapper.Map<PaymentCardResponseDto>(paymentCard);
            //response.data.biometricVerification = _autoMapper.Map<BiometricVerificationResponseDto>(biometricVerification);

            response.success = true;

            return Ok(response);

        }
        [Route("biometric-verification-and-payment")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<TransactionResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<TransactionResponseDto>))]
        public async Task<IActionResult> BiometricVerificationAndPayment(VerificationAndPaymentRequestDto request, CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<TransactionResponseDto>();

            Identification identification = new Identification();
            identification.gallery = "default";
            identification.identificationParameters = new IdentificationParameter();
            identification.probe=new Probe();
            identification.probe.faces = new List<AbisImage>();
            identification.probe.faces.Add( new AbisImage() { dataBytes= request.biometric.biometricData });

            var verification = _innovatricsAbis.IdentifyByFace(identification).Result;

            if (verification.IsNullOrEmpty())
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "Biometric not verified";
                response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);
            }

            var customer = _customerRepository.GetByID(new Guid (verification.FirstOrDefault().externalId));

            if (customer == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "customer not found";
                //response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);

            }

            if (customer.pin!=request.pin.ToString())
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "customer not verified";
                response.error.errorDetails = " Please do Verification again";

                return NotFound(response);
            }
            var paymentCard = _paymentCardRepository.GetByCustomerID(new Guid(verification.FirstOrDefault().externalId)).Result;

            if (paymentCard == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "payment Card not found";
                //response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);
            }

            var biometricVerification = _autoMapper.Map<BiometricVerification>(request.biometric);
            biometricVerification.customer_ID = paymentCard.customer_ID;
            biometricVerification.createdDate = DateTime.Now;
            biometricVerification.verificationStatus = VerificationStatus.success;
            biometricVerification.verificationResponse = JsonConvert.SerializeObject(verification.FirstOrDefault());
            biometricVerification.verificationID = verification.FirstOrDefault().score.ToString();
            var Verificationresult = (await _biometricVerificationRepository.AddAsync(biometricVerification));
            _biometricVerificationRepository.SaveChanges();

            var transaction = _autoMapper.Map<Transaction>(request);
            transaction.biometricVerification_ID = biometricVerification.Id;
            transaction.paymentCard_ID = paymentCard.Id;
            transaction.createdDate = DateTime.Now;
            transaction.status = TransactionStatus.success;
            transaction.transactionNumber = (Random.Shared.Next(1000, 10000)).ToString();

            var result = (await _transactionRepository.AddAsync(transaction));
            _transactionRepository.SaveChanges();

            response.data = _autoMapper.Map<TransactionResponseDto>(result.Entity);

            //var biometric = _biometricRepository.GetByCustomerID(customer.Id).Result;

            response.data.customer = _autoMapper.Map<CustomerResponseDto>(customer);
            //response.data.paymentCard = _autoMapper.Map<PaymentCardResponseDto>(paymentCard);
            response.data.biometricVerification = _autoMapper.Map<BiometricVerificationResponseDto>(biometricVerification);
            //response.data.customer.biometric.Add( _autoMapper.Map<BiometricResponseDto>(biometric));

            response.success = true;


            return Ok(response);
        }

        }
}

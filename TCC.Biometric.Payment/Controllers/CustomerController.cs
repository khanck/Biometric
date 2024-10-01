using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using TCC.Biometric.Payment.DTOs;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;
using TCC.Payment.Data.Interfaces;
using TCC.Payment.Data.Repositories;
using TCC.Payment.Integration.Interfaces;
using TCC.Payment.Integration.Models;
using ILogger = Serilog.ILogger;

namespace TCC.Biometric.Payment.Controllers
{

    [Route("api/customer")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IBiometricRepository _biometricRepository;
        private readonly IPaymentCardRepository _paymentCardRepository;

        private readonly IMapper _autoMapper;
        private readonly ILogger _logger;
        private readonly IAlpetaServer _alpetaServer;

        public CustomerController(ICustomerRepository customerRepository,
            IBiometricRepository biometricRepository,
            IPaymentCardRepository paymentCardRepository,
            IMapper autoMapper, IAuthenticationService authenticationService,
            ILogger logger,
            IAlpetaServer alpetaServe)
        {
            _customerRepository = customerRepository;
            _biometricRepository = biometricRepository;
            _paymentCardRepository = paymentCardRepository;
            _autoMapper = autoMapper;
            _logger = logger;
            _alpetaServer = alpetaServe;
        }

        [Route("Health")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> Health()
        {
            return Ok("Services are running");
        }


        [Route("get")]
        [HttpGet]     
        //[OpenApiTags("OnboardingCustomer")]  
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<CustomerResponseDto>))]
        [Produces(typeof(ResultDto<CustomerResponseDto>))]
        public async Task<IActionResult> GetCustomer(Guid Id, CancellationToken cancellationToken = default)
        {  //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<CustomerResponseDto>();

            var customer = _customerRepository.GetByID(Id);


            if (customer == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "Customer Not Found";
                //response.error.errorDetails = "digital ID Customer Not Found";

                return Conflict(response);
            }

            response.data = _autoMapper.Map<CustomerResponseDto>(customer);


            response.success = true;
            return Ok(response);


        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<CustomerResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<CustomerResponseDto>))]
        public async Task<IActionResult> create(CustomerRequestDto request, CancellationToken cancellationToken = default)
        {

            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            _logger.Information("request image " + request.biometric.FirstOrDefault().biometricData);
            await _alpetaServer.Login();
            var response = new ResultDto<CustomerResponseDto>();

            var customer = _autoMapper.Map<Customer>(request);
            customer.createdDate = DateTime.Now;
            customer.status = CustomerStatus.pending;

            var result = (await _customerRepository.AddAsync(customer));
            _customerRepository.SaveChanges();


            var biometric = _autoMapper.Map<Biometrics>(request.biometric.FirstOrDefault());
            biometric.customer_ID = customer.Id;
            biometric.createdDate = DateTime.Now;
            biometric.status = BiometricStatus.pending;
            biometric.abisReferenceID = customer.TerminalUserId.ToString();

            var biometricResult = (await _biometricRepository.AddAsync(biometric));
            _biometricRepository.SaveChanges();


            // add dummy card will remove soon after mobile app 

            _paymentCardRepository.Add(new PaymentCard() {customer_ID= customer.Id,nameOnCard=customer.firstName,
                                cardNumber = (Random.Shared.Next(2, 1000000000)).ToString(),cardType="VISA",expiryYear="2026",
                expiryMonth="9",cvv="099",status= CardStatus.active});
            _paymentCardRepository.SaveChanges();


            // var customer = _autoMapper.Map<Customer>(request);
            //customer.Id = biometricResult.Entity.Id;
            customer.createdDate = DateTime.Now;
            customer.status = CustomerStatus.pending;
            CreateUserRequestDTO createUserRequestDTO = new CreateUserRequestDTO();
            createUserRequestDTO.UpdateUserID(customer.TerminalUserId);
            createUserRequestDTO.UserInfo.ID = customer.TerminalUserId.ToString();
            createUserRequestDTO.UserInfo.UniqueID = customer.TerminalUserId.ToString();
            createUserRequestDTO.UserInfo.Email = customer.email;
            createUserRequestDTO.UserInfo.Name = customer.firstName + ' ' + customer.lastName;
            createUserRequestDTO.UserInfo.Phone = customer.mobile;
            createUserRequestDTO.UserInfo.Password = customer.pin;
            createUserRequestDTO.UserInfo.LoginPW = customer.password;

            UserFaceInfo userFaceInfo = new UserFaceInfo();
            UserFaceWTInfo userFaceWTInfo = new UserFaceWTInfo();
            userFaceWTInfo.UserID = customer.TerminalUserId;
            userFaceWTInfo.TemplateData = request.biometric.FirstOrDefault().biometricData;
            userFaceWTInfo.TemplateType = 1;
            userFaceWTInfo.TemplateSize = GetPictureSizeInKB(userFaceWTInfo.TemplateData);



            userFaceInfo.UserID = customer.TerminalUserId;
            userFaceInfo.TemplateData = request.biometric.FirstOrDefault().biometricData;
            userFaceInfo.TemplateSize = biometric.biometricData.Length;
            // createUserRequestDTO.UserFaceInfo.Add(userFaceInfo);
            createUserRequestDTO.UserFaceWTInfo.Add(userFaceWTInfo);

            var user = _alpetaServer.CreateUser(createUserRequestDTO).Result;
            if (user == null || user.Result.ResultCode != "0")
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_50";
                response.error.errorMessage = "Error in biometric registry";
                response.error.errorDetails = JsonConvert.SerializeObject(user);

                return Conflict(response);
            }

            DownloadInfo downloadInfo = new DownloadInfo();
            downloadInfo.Offset = 1;
            downloadInfo.Total = 1;
            await _alpetaServer.SaveUserToTerminal(new SaveUserToTerminalDto
            {
                TerminalId = 1,
                UserId = customer.TerminalUserId,
                DownloadInfo = downloadInfo
            });

            response.data = _autoMapper.Map<CustomerResponseDto>(customer);
            response.success = true;

            return Ok(response);

        }

        [Route("push-user-biometric")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<CustomerResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<CustomerResponseDto>))]
        public async Task<IActionResult> PushUserBiometric(PushUserBiometricRequestDto request, CancellationToken cancellationToken = default)
        {

            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

           
 
            var response = new ResultDto<CustomerResponseDto>();

            var customer = _customerRepository.GetByID(request.customer_ID);
            var biometric = _biometricRepository.GetByCustomerID(request.customer_ID).Result;

            // var customer = _autoMapper.Map<Customer>(request);
            //customer.Id = biometricResult.Entity.Id;     
            CreateUserRequestDTO createUserRequestDTO = new CreateUserRequestDTO();
            createUserRequestDTO.UpdateUserID(customer.TerminalUserId);
            createUserRequestDTO.UserInfo.ID = customer.TerminalUserId.ToString();
            createUserRequestDTO.UserInfo.UniqueID = customer.TerminalUserId.ToString();
            createUserRequestDTO.UserInfo.Email = customer.email;
            createUserRequestDTO.UserInfo.Name = customer.firstName + ' ' + customer.lastName;
            createUserRequestDTO.UserInfo.Phone = customer.mobile;
            createUserRequestDTO.UserInfo.Password = customer.pin;
            createUserRequestDTO.UserInfo.LoginPW = customer.password;

            UserFaceInfo userFaceInfo = new UserFaceInfo();
            UserFaceWTInfo userFaceWTInfo = new UserFaceWTInfo();
            userFaceWTInfo.UserID = customer.TerminalUserId;
            userFaceWTInfo.TemplateData = biometric.biometricData;
            userFaceWTInfo.TemplateType = 1;
            userFaceWTInfo.TemplateSize = GetPictureSizeInKB(userFaceWTInfo.TemplateData);



            userFaceInfo.UserID = customer.TerminalUserId;
            userFaceInfo.TemplateData = biometric.biometricData;
            userFaceInfo.TemplateSize = biometric.biometricData.Length;
            // createUserRequestDTO.UserFaceInfo.Add(userFaceInfo);
            createUserRequestDTO.UserFaceWTInfo.Add(userFaceWTInfo);

            var user = _alpetaServer.CreateUser(createUserRequestDTO).Result;
            if (user == null || user.Result.ResultCode != "0")
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_50";
                response.error.errorMessage = "Error in biometric registry";
                response.error.errorDetails = user.ToString();

                return Conflict(response);
            }

            DownloadInfo downloadInfo = new DownloadInfo();
            downloadInfo.Offset = 1;
            downloadInfo.Total = 1;
            await _alpetaServer.SaveUserToTerminal(new SaveUserToTerminalDto
            {
                TerminalId = request.TerminalId,
                UserId = customer.TerminalUserId,
                DownloadInfo = downloadInfo
            });

            return Ok(response);

        }

        [Route("update-user-picture")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDto<CustomerResponseDto>))]
        [Produces(typeof(ResultDto<CustomerResponseDto>))]
        public async Task<IActionResult> UpdateUserPicture(UpdateUserPictureReqDTO updateUserPictureReqDTO, CancellationToken cancellationToken = default)
        {  
            var response = new ResultDto<UpdateUserPictureReqDTO>();

            await _alpetaServer.Login();
            await _alpetaServer.UpdateUserPicture(updateUserPictureReqDTO);

            response.success = true;
            return Ok(response);


        }

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<CustomerResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<CustomerResponseDto>))]
        public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<CustomerResponseDto>();

            var customer = _customerRepository.Login(request.email, request.password).Result;


            if (customer == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "Invalid user";
                //response.error.errorDetails = "digital ID Customer Not Found";

                return Conflict(response);
            }

            response.data = _autoMapper.Map<CustomerResponseDto>(customer);


            response.success = true;
            return Ok(response);
        }
        private static int GetPictureSizeInKB(string base64Picture)
        {
            // Remove any data URL prefix (if present), e.g., "data:image/png;base64,"
            string base64Data = base64Picture.Contains(",") ? base64Picture.Split(',')[1] : base64Picture;

            // Convert the base64 string to a byte array
            byte[] pictureBytes = Convert.FromBase64String(base64Data);

            // Get the size in bytes
            int sizeInBytes = pictureBytes.Length;

            // Convert bytes to kilobytes (1 KB = 1024 bytes)
            int sizeInKB = sizeInBytes;

            return pictureBytes.Length;
        }

    }
    }

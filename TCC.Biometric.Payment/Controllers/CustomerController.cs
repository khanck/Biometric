using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TCC.Biometric.Payment.DTOs;
using TCC.Payment.Data.Entities;
using TCC.Payment.Data.Enums;
using TCC.Payment.Data.Interfaces;
using TCC.Payment.Data.Repositories;
using TCC.Payment.Integration.Interfaces;
using TCC.Payment.Integration.Models;
using TCC.Payment.Integration.Models.Innovatrics;
using static System.Net.Mime.MediaTypeNames;
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
        private readonly IInnovatricsAbis _innovatricsAbis;

        public CustomerController(ICustomerRepository customerRepository,
            IBiometricRepository biometricRepository,
            IPaymentCardRepository paymentCardRepository,
            IMapper autoMapper, IAuthenticationService authenticationService,
            ILogger logger,
            IAlpetaServer alpetaServe,
             IInnovatricsAbis innovatricsAbis)
        {
            _customerRepository = customerRepository;
            _biometricRepository = biometricRepository;
            _paymentCardRepository = paymentCardRepository;
            _autoMapper = autoMapper;
            _logger = logger;
            _alpetaServer = alpetaServe;
            _innovatricsAbis=innovatricsAbis;
        }

        [Route("Health")]
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Health()
        {
            return Ok("Services are running");
        }


        [Route("get")]
        [HttpGet]     
        //[OpenApiTags("OnboardingCustomer")]  
        [ProducesResponseType(typeof(ResultDto<CustomerResponseDto>), StatusCodes.Status200OK)]
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

            //_logger.Information("request image " + request.biometric.FirstOrDefault().biometricData);
            

            var response = new ResultDto<CustomerResponseDto>();

            var userEmail = _customerRepository.GetByEmail(request.email).Result;
            if (userEmail != null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_035";
                response.error.errorMessage = "User is already existing";
                response.error.errorDetails = "User is already existing";

                return Conflict(response);
            }

            Identification identification = new Identification();
            identification.gallery = "default";
            identification.identificationParameters = new IdentificationParameter();
            identification.probe = new Probe();
            identification.probe.faces = new List<AbisImage>();
            identification.probe.faces.Add(new AbisImage() { dataBytes = request.biometric.FirstOrDefault().biometricData });

            var userExists = _innovatricsAbis.IdentifyByFace(identification).Result;
            if (!userExists.IsSuccess)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_031";
                response.error.errorMessage = "issue in Biometric face Verification";
                //response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);
            }

            if (!userExists.searchResult.IsNullOrEmpty())
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_030";
                response.error.errorMessage = "User is already existing in ABIS";
                response.error.errorDetails = " User is already existing in ABIS ";

                return Conflict(response);
            }


            var customer = _autoMapper.Map<Customer>(request);
            customer.createdDate = DateTime.Now;
            customer.status = CustomerStatus.pending;

            var result = (await _customerRepository.AddAsync(customer));
            _customerRepository.SaveChanges();


            var biometric = _autoMapper.Map<Biometrics>(request.biometric.FirstOrDefault());
            biometric.customer_ID = customer.Id;
            biometric.createdDate = DateTime.Now;
            biometric.status = BiometricStatus.pending;
            biometric.abisReferenceID = customer.Id.ToString();

            var biometricResult = (await _biometricRepository.AddAsync(biometric));
            _biometricRepository.SaveChanges();


            // add dummy card will remove soon after mobile app 

            //_paymentCardRepository.Add(new PaymentCard() {customer_ID= customer.Id,nameOnCard=customer.firstName,
            //                    cardNumber = (Random.Shared.Next(2, 1000000000)).ToString(),cardType="VISA",expiryYear="2026",
            //    expiryMonth="9",cvv="099",status= CardStatus.active});
            //_paymentCardRepository.SaveChanges();


            // var customer = _autoMapper.Map<Customer>(request);
            //customer.Id = biometricResult.Entity.Id;
            customer.createdDate = DateTime.Now;
            customer.status = CustomerStatus.active;
           

            //add user to Alpeta server
            /*
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

            */ 
            //End


            // send To Abis           
            var abisResponse =await EnrollUserToAbis(customer, biometric);
            if (!abisResponse.IsSuccess)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_031";
                response.error.errorMessage = "issue in ABIS Enrolment ";
                //response.error.errorDetails = " Please do Biometric Verification";

                return NotFound(response);
            }

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
                response.error.errorDetails = JsonConvert.SerializeObject(user);

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



        [Route("push-user-to-terminal")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<CustomerResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<CustomerResponseDto>))]
        public async Task<IActionResult> PushUserToTerminal(PushUserToTerminalRequestDto request, CancellationToken cancellationToken = default)
        {

            var response = new ResultDto<CustomerResponseDto>();

            var customer = _customerRepository.GetByID(request.customer_ID);


            if (customer == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "Customer Not Found";
                //response.error.errorDetails = "digital ID Customer Not Found";

                return Conflict(response);
            }

            DownloadInfo downloadInfo = new DownloadInfo();
            downloadInfo.Offset = 1;
            downloadInfo.Total = 1;
            var result = _alpetaServer.SaveUserToTerminal(new SaveUserToTerminalDto
            {
                TerminalId = request.TerminalId,
                UserId = customer.TerminalUserId,
                DownloadInfo = downloadInfo
            }).Result;

            if (result.Result.ResultCode != "0")
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_400";
                response.error.errorMessage = "Error in Save User To Terminal";
                response.error.errorDetails = JsonConvert.SerializeObject(result);

                return Conflict(response);
            }
            response.data = _autoMapper.Map<CustomerResponseDto>(customer);
            response.success = true;
            return Ok(response);

        }


        [Route("update-user-picture")]
        [HttpPut]
        [ProducesResponseType(typeof(ResultDto<CustomerResponseDto>), StatusCodes.Status200OK)]
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
            var biometric = _biometricRepository.GetByCustomerID(customer.Id).Result;
            response.data.biometric.Add(_autoMapper.Map<BiometricResponseDto>(biometric));

            response.success = true;
            return Ok(response);
        }

        [Route("push-user-to-abis")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<CustomerResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<CustomerResponseDto>))]
        public async Task<IActionResult> PushUserToAbis(PushUserToAbisRequestDto request, CancellationToken cancellationToken = default)
        {

            var response = new ResultDto<CustomerResponseDto>();

            var customer = _customerRepository.GetByID(request.customer_ID);
            

            if (customer == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "Customer Not Found";
                //response.error.errorDetails = "digital ID Customer Not Found";

                return Conflict(response);
            }
            var image = _biometricRepository.GetByCustomerID(request.customer_ID).Result;

            if (image == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "Customer face image Not Found";
                //response.error.errorDetails = "digital ID Customer Not Found";

                return Conflict(response);
            }

            AbisEnrollUser person=new AbisEnrollUser();
           
            person.enrolledAt=DateTime.Now;
            person.externalId= customer.Id.ToString();
            person.enrollAction = new EnrollAction (){ enrollActionType = request.enrollActionType, referenceExternalId= customer.Id.ToString() };
            person.customDetails = new PersonInfo() { givenNames = customer.firstName, surname = customer.lastName, email = customer.email };
            person.faceModality = new Modality();
            person.faceModality.faces = new List<Face>();

            AbisImage faceImage=new AbisImage() { captureDevice="user mobile", dataBytes= image.biometricData};
            Face face=new Face();
            face.image= faceImage;

            person.faceModality.faces.Add(face);

            var result = _innovatricsAbis.EnrollPerson(person);

            if (result.Id == 0)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_400";
                response.error.errorMessage = "Error in Save User To Terminal";
                response.error.errorDetails = JsonConvert.SerializeObject(result);

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

        private Task<AbisResponse> EnrollUserToAbis(Customer customer, Biometrics image)
        {
            AbisEnrollUser person = new AbisEnrollUser();

            person.enrolledAt = DateTime.Now;
            person.externalId = customer.Id.ToString();
            person.enrollAction = new EnrollAction() { enrollActionType = "None", referenceExternalId = customer.Id.ToString() };
            person.customDetails = new PersonInfo() { givenNames = customer.firstName, surname = customer.lastName, email = customer.email };
            person.faceModality = new Modality();
            person.faceModality.faces = new List<Face>();

            AbisImage faceImage = new AbisImage() { captureDevice = "user mobile", dataBytes = image.biometricData };
            Face face = new Face();
            face.image = faceImage;

            person.faceModality.faces.Add(face);

            var result = _innovatricsAbis.EnrollPerson(person);

            return result;
        }
    }
    }

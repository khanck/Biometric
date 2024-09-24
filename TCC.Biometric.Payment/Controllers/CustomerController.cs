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

    [Route("api/customer")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IBiometricRepository _biometricRepository;

        private readonly IMapper _autoMapper;
        private readonly ILogger _logger;

        public CustomerController(ICustomerRepository customerRepository,
            IBiometricRepository biometricRepository,
            IMapper autoMapper, IAuthenticationService authenticationService,
            ILogger logger)
        {
            _customerRepository = customerRepository;
            _biometricRepository = biometricRepository;
            _autoMapper = autoMapper;
            _logger = logger;
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
            biometric.abisReferenceID = "abis ID";
            var biometricResult = (await _biometricRepository.AddAsync(biometric));
            _biometricRepository.SaveChanges();
          

            response.data = _autoMapper.Map < CustomerResponseDto > (result.Entity);
            response.success = true;

            return Ok(response);

        }

    }
}

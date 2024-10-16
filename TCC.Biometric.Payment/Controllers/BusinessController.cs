﻿using AutoMapper;
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

    [Route("api/business")]
    [ApiController]
    public class BusinessController : Controller
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IAccountRepository _accountRepository;

        private readonly IMapper _autoMapper;
        private readonly ILogger _logger;

        public BusinessController(IBusinessRepository businessRepository,
            IAccountRepository accountRepository,
            IMapper autoMapper, IAuthenticationService authenticationService,
            ILogger logger)
        {
            _businessRepository = businessRepository;
            _accountRepository=accountRepository;
            _autoMapper = autoMapper;
            _logger = logger;
        }

       
        [Route("get")]
        [HttpGet]       
        //[OpenApiTags("OnboardingBusiness")]  
        [ProducesResponseType( typeof(ResultDto<BusinessResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<BusinessResponseDto>))]
        public async Task<IActionResult> GetBusiness(Guid Id, CancellationToken cancellationToken = default)
        {  //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<BusinessResponseDto>();

            var business = _businessRepository.GetByID(Id);


            if (business == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "Business Not Found";
                //response.error.errorDetails = "digital ID Business Not Found";

                return Conflict(response);
            }

            response.data = _autoMapper.Map<BusinessResponseDto>(business);

            var account = _accountRepository.GetByBusinessID(business.Id).Result;
            response.data.account = _autoMapper.Map<AccountResponseDto>(account);

            response.success = true;
            return Ok(response);


        }

        [Route("create")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<BusinessResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<BusinessResponseDto>))]
        public async Task<IActionResult> create(BusinessRequestDto request, CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<BusinessResponseDto>();          


            var business = _autoMapper.Map<Business>(request);
            business.createdDate = DateTime.Now;
            business.status = BusinessStatus.pending;

            var result = (await _businessRepository.AddAsync(business));
            _businessRepository.SaveChanges();


            var account = _autoMapper.Map<Account>(request.account);
            account.business_ID = business.Id;
            account.createdDate = DateTime.Now;
            account.status = AccountStatus.pending;

            var accountResult = (await _accountRepository.AddAsync(account));
            _accountRepository.SaveChanges();

            response.data = _autoMapper.Map<BusinessResponseDto>(result.Entity);
            response.success = true;

            return Ok(response);

        }

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(typeof(ResultDto<BusinessResponseDto>), StatusCodes.Status200OK)]
        [Produces(typeof(ResultDto<BusinessResponseDto>))]
        public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken cancellationToken = default)
        {
            //if ((Request.Headers["Authorization"].Count == 0) || (!_authenticationService.IsValidUser(AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]))))
            //    return Unauthorized();

            var response = new ResultDto<BusinessResponseDto>();

            var business = _businessRepository.Login(request.email, request.password).Result;


            if (business == null)
            {
                response.error = new ErrorDto();
                response.error.errorCode = "BP_001";
                response.error.errorMessage = "Invalid user";
                //response.error.errorDetails = "digital ID Customer Not Found";

                return Conflict(response);
            }
           

            response.data = _autoMapper.Map<BusinessResponseDto>(business);

            var account = _accountRepository.GetByBusinessID(business.Id).Result;
            response.data.account= _autoMapper.Map<AccountResponseDto>(account);

            response.success = true;
            return Ok(response);
        }

    }
}

using AutoMapper;
using TCC.Payment.Data.Entities;
using TCC.Biometric.Payment.DTOs;
using TCC.Payment.Integration.Models;
using TCC.Payment.Data.Enums;

namespace TCC.Biometric.Payment.Profiles
{
    public class EntityProfile:Profile
    {
        public EntityProfile()
        {
            //Source-> Dest
            CreateMap<Customer, CustomerRequestDto>();
            CreateMap<CustomerRequestDto, Customer>();
            CreateMap<Customer, CustomerResponseDto>();
            CreateMap<CustomerResponseDto, Customer>();

            CreateMap<PaymentCard, PaymentCardRequestDto>();
            CreateMap<PaymentCardRequestDto, PaymentCard>();
            CreateMap<PaymentCard, PaymentCardResponseDto>();
            CreateMap<PaymentCardResponseDto, PaymentCard>();

            CreateMap<Account, AccountRequestDto>();
            CreateMap<AccountRequestDto, Account>();
            CreateMap<Account, AccountResponseDto>();
            CreateMap<AccountResponseDto, Account>();


            CreateMap<Business, BusinessRequestDto>();
            CreateMap<BusinessRequestDto, Business>();
            CreateMap<Business, BusinessResponseDto>();
            CreateMap<BusinessResponseDto, Business>();

            CreateMap<Transaction, TransactionRequestDto>();
            CreateMap<TransactionRequestDto, Transaction>()
             .ForMember(dest => dest.biometricVerification, src => src.Ignore());
            CreateMap<Transaction, TransactionResponseDto>();
            CreateMap<TransactionResponseDto, Transaction>();
            CreateMap<Transaction, BusinessTransactionResponseDto>();
            CreateMap<Transaction, CustomerTransactionResponseDto>();

            CreateMap<BiometricPaymentRequestDto, Transaction>()
                .ForMember(dest => dest.TransactionType, src => src.MapFrom(src => TransactionTypes.payment));

            CreateMap<BiometricVerification, BiometricVerificationRequestDto>();
            CreateMap<BiometricVerificationRequestDto, BiometricVerification>();
            CreateMap<BiometricVerification, BiometricVerificationResponseDto>();
            CreateMap<BiometricVerificationResponseDto, BiometricVerification>();

            CreateMap<BiometricAuthenticationDetail,BiometricVerification>()
                 .ForMember(dest => dest.biometricType, src => src.MapFrom(src => BiometricTypes.face))
                  .ForMember(dest => dest.biometricData, src => src.MapFrom(src => src.LogImage))
                   .ForMember(dest => dest.createdDate, src => src.MapFrom(src => DateTime.Now))
                   .ForMember(dest => dest.verificationID, src => src.MapFrom(src => src.IndexKey.ToString()))
                     .ForMember(dest => dest.verificationResponse, src => src.MapFrom(src => src.AuthResult.ToString()))
                   .ForMember(dest => dest.verificationStatus, src =>  src.MapFrom(src => (src.AuthResult == 0) ? VerificationStatus.success: VerificationStatus.failed))
                 ;
            

            CreateMap<Biometrics, BiometricRequestDto>();
            CreateMap<BiometricRequestDto, Biometrics>();
            CreateMap<Biometrics, BiometricResponseDto>();
            CreateMap<BiometricResponseDto, Business>();



            //CreateMap<Application, InvitationStatusDto>()
            //     .ForMember(dest => dest.Payment, src => src.MapFrom(src => src.PersonId));
        }
    }
}

using AutoMapper;
using TCC.Payment.Data.Entities;
using TCC.Biometric.Payment.DTOs;

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

            CreateMap<BiometricVerification, BiometricVerificationRequestDto>();
            CreateMap<BiometricVerificationRequestDto, BiometricVerification>();
            CreateMap<BiometricVerification, BiometricVerificationResponseDto>();
            CreateMap<BiometricVerificationResponseDto, BiometricVerification>();

            CreateMap<Biometrics, BiometricRequestDto>();
            CreateMap<BiometricRequestDto, Biometrics>();
            CreateMap<Biometrics, BiometricResponseDto>();
            CreateMap<BiometricResponseDto, Business>();



            //CreateMap<Application, InvitationStatusDto>()
            //     .ForMember(dest => dest.Payment, src => src.MapFrom(src => src.PersonId));
        }
    }
}

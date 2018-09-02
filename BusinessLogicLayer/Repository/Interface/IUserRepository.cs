using Entities;

namespace Repository
{
    public interface IUserRepository
    {
        bool IsUserValid(User objUser);
        int RegisterUser(Registration objRegister);
        bool UnlockUserAccount(User objUser);
        bool ChangePassword(ChangePassword changePassword);
        //int UpdateAddressDetails(AddressDetails objAddress);
        //int UpdateCompanyDetails(CompanyDetails objCompany);
    }
}

using System;
using DataAccessLayer;
using System.Data.SqlClient;
using System.Data;
using Repository;
using Entities;
using static Enum.Enumeration;

namespace BusinessLogicLayer.Repository
{
    public class UserRepository : IUserRepository
    {
        private CryptographyRepository _objRepo = CryptographyRepository.GetInstance;
        public UserRepository() { }

        public bool ChangePassword(ChangePassword changePassword)
        {
            return true;
        }

        public bool IsUserValid(User objUser)
        {
            bool IsValidUser = false;
            try
            {
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter { ParameterName = "@MobileNumber", Value = objUser.MobileNumber };

                var dtUser = SqlHelper.GetTableFromSP("Usp_ValidateUser", parameter);
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    if (Convert.ToString(dtUser.Rows[0][0]) == "201")
                    {
                        IsValidUser = false;
                    }
                    else
                    {
                        var decryptedPassword = _objRepo.Decrypt(Convert.ToString(dtUser.Rows[0][1]));
                        var username = Convert.ToString(dtUser.Rows[0][0]);
                        var isLocked = Convert.ToInt32(dtUser.Rows[0][2]);
                        var loginAttempts = Convert.ToInt32(dtUser.Rows[0][3]);
                        if ((username.Equals(objUser.MobileNumber) && decryptedPassword.Equals(objUser.Password))) {
                            IsValidUser = true;
                            if (isLocked == 0) {
                                //reset login attempts and is locked column
                                LoginDetails loginDetails = new LoginDetails()
                                {
                                    Username = objUser.MobileNumber,
                                    IsLocked = true,
                                    LoginAttempts = 3
                                };
                                UpdateLoginDetails(loginDetails);
                            }
                            else {
                                //return account suspended status
                                IsValidUser = false;
                            }
                        }
                        else if (String.IsNullOrEmpty(objUser.Password) || decryptedPassword != objUser.Password)
                        {
                            //decrese login attempts
                            //if attempts greater than zero set account status active and decrease attempts
                            loginAttempts = loginAttempts - 1;
                            if (loginAttempts >= 0)
                            {
                                LoginDetails loginDetails = new LoginDetails()
                                {
                                    Username = objUser.MobileNumber,
                                    IsLocked = true,
                                    LoginAttempts = 3
                                };
                                UpdateLoginDetails(loginDetails);
                                //update attempts left and return status as wrong password
                            }
                            else
                            {
                                LoginDetails loginDetails = new LoginDetails()
                                {
                                    Username = objUser.MobileNumber,
                                    IsLocked = false,
                                    LoginAttempts = loginAttempts
                                };
                                UpdateLoginDetails(loginDetails);
                                //update if attempts equal to zero or less suspend the account and set attempts left to zero
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //WRITE ERROR LOG
                LogManager.WriteLog(ex, ErrorLevel.Critical);
            }
            return IsValidUser;
        }
        public int RegisterUser(Registration objRegister)
        {
            int returnValue = 0;
            try
            {
                SqlParameter[] parameter = new SqlParameter[16];
                parameter[0] = new SqlParameter { ParameterName = "@MobileNum", Value = objRegister.MobileNumber };
                parameter[1] = new SqlParameter { ParameterName = "@Password", Value = _objRepo.Encrypt(objRegister.Password) };
                parameter[2] = new SqlParameter { ParameterName = "@EmailId", Value = objRegister.EmaillAddress };
                parameter[3] = new SqlParameter { ParameterName = "@Name", Value = objRegister.FullName };
                parameter[4] = new SqlParameter { ParameterName = "@BuildingName", Value = objRegister.Building_Name };
                parameter[5] = new SqlParameter { ParameterName = "@Locality", Value = objRegister.Locality };
                parameter[6] = new SqlParameter { ParameterName = "@Pincode", Value = objRegister.PinCode };
                parameter[7] = new SqlParameter { ParameterName = "@City", Value = objRegister.City };
                parameter[8] = new SqlParameter { ParameterName = "@State", Value = objRegister.State };
                parameter[9] = new SqlParameter { ParameterName = "@Landmark", Value = objRegister.Landmark };
                parameter[10] = new SqlParameter { ParameterName = "@AddressType", Value = objRegister.AddressType };
                parameter[11] = new SqlParameter { ParameterName = "@CompanyName", Value = objRegister.CompanyName };
                parameter[12] = new SqlParameter { ParameterName = "@GSTNo", Value = objRegister.GST_No };
                parameter[13] = new SqlParameter { ParameterName = "@Category", Value = objRegister.Category };
                parameter[14] = new SqlParameter { ParameterName = "@BusinessType", Value = objRegister.Businees_Type };
                parameter[15] = new SqlParameter { ParameterName = "@RoleID", Value = (int)objRegister.RoleId };
                DataTable dt = SqlHelper.GetTableFromSP("Usp_RegisterUser", parameter);
                returnValue = Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception ex)
            {
                //WRITE ERROR LOG
                LogManager.WriteLog(ex, ErrorLevel.Important);
            }
            return returnValue;
        }
        public bool UnlockUserAccount(User objUser)
        {
            bool IsUnlocked=false;
            try
            {
                LoginDetails loginDetails = new LoginDetails()
                {
                    Username = objUser.MobileNumber,
                    IsLocked = true,
                    LoginAttempts = 3
                };
                var returnValue = UpdateLoginDetails(loginDetails);
                if (returnValue == 1) { IsUnlocked = true; }
            }
            catch(Exception ex) { throw ex; }
            return IsUnlocked;
        }
        private int UpdateLoginDetails(LoginDetails loginDetails)
        {
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter { ParameterName = "@MobileNumber", Value = loginDetails.Username};
            param[1] = new SqlParameter { ParameterName = "@IsLocked", Value = loginDetails.IsLocked };
            param[2] = new SqlParameter { ParameterName = "@LoginAttempts", Value = loginDetails.LoginAttempts };
            return SqlHelper.ExecuteNonQuery("Usp_UpdateLoginDetails", param);
        } 
        //public int UpdateAddressDetails(AddressDetails objAddress)
        //{
        //    int returnValue = 0;
        //    try
        //    {
        //        SqlParameter[] parameter = new SqlParameter[8];
        //        parameter[0] = new SqlParameter { ParameterName = "@USR_ID", Value = objAddress.UserId };
        //        parameter[1] = new SqlParameter { ParameterName = "@BUILDING_NAME", Value = objAddress.Building_Name };
        //        parameter[2] = new SqlParameter { ParameterName = "@LOCALITY", Value = objAddress.Locality };
        //        parameter[3] = new SqlParameter { ParameterName = "@PINCODE", Value = objAddress.PinCode };
        //        parameter[4] = new SqlParameter { ParameterName = "@CITY", Value = objAddress.City };
        //        parameter[5] = new SqlParameter { ParameterName = "@STATE", Value = objAddress.State };
        //        parameter[6] = new SqlParameter { ParameterName = "@LANDMARK", Value = objAddress.Landmark };
        //        parameter[7] = new SqlParameter { ParameterName = "@ADDRESS_TYPE", Value = objAddress.AddressType };
        //        returnValue = SqlHelper.ExecuteNonQuery("USP_UPDATE_ADDRESS", parameter);
        //    }
        //    catch (Exception ex)
        //    {
        //        //WRITE ERROR LOG
        //        LogManager.WriteLog(ex);
        //    }
        //    return returnValue;
        //}

        //public int UpdateCompanyDetails(CompanyDetails objCompany)
        //{
        //    int returnValue = 0;
        //    try
        //    {
        //        SqlParameter[] parameter = new SqlParameter[5];
        //        parameter[0] = new SqlParameter { ParameterName = "@USR_ID", Value = objCompany.UserId };
        //        parameter[1] = new SqlParameter { ParameterName = "@COMPANY_NAME", Value = objCompany.CompanyName };
        //        parameter[2] = new SqlParameter { ParameterName = "@GST_NO", Value = objCompany.GST_No };
        //        parameter[3] = new SqlParameter { ParameterName = "@CATEGORY", Value = objCompany.Category };
        //        parameter[4] = new SqlParameter { ParameterName = "@BUSINESS_TYPE", Value = objCompany.Businees_Type};
        //        returnValue = SqlHelper.ExecuteNonQuery("USP_INSERT_COMPANY_DETAILS", parameter);
        //    }
        //    catch (Exception ex)
        //    {
        //        //WRITE ERROR LOG
        //        LogManager.WriteLog(ex);
        //    }
        //    return returnValue;
        //}
    }
}

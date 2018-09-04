using System;
using DataAccessLayer;
using System.Data.SqlClient;
using System.Data;
using Repository;
using Entities;
using static Enum.Enums;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private CryptographyRepository _objRepo = CryptographyRepository.GetInstance;

        public UserRepository() { }

        public ServiceRes ChangePassword(ChangePassword changePassword)
        {
            ServiceRes serviceRes = new ServiceRes();
            return serviceRes;
        }

        public ServiceRes IsUserValid(User objUser)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter { ParameterName = "@MobileNumber", Value = objUser.MobileNumber };

                var dtUser = SqlHelper.GetTableFromSP("Usp_ValidateUser", parameter);
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    if (Convert.ToString(dtUser.Rows[0][0]) == "201")
                    {
                        serviceRes.IsSuccess = false;
                        serviceRes.ReturnCode = "201";
                        serviceRes.ReturnMsg = "User not found";
                    }
                    else
                    {
                        string decryptedPassword = _objRepo.Decrypt(Convert.ToString(dtUser.Rows[0][1]));
                        string username = Convert.ToString(dtUser.Rows[0][0]);
                        int isLocked = Convert.ToInt32(dtUser.Rows[0][2]);
                        int loginAttempts = Convert.ToInt32(dtUser.Rows[0][3]);

                        if ((username.Equals(objUser.MobileNumber) && decryptedPassword.Equals(objUser.Password))) {
                            if (isLocked == 0) {
                                //reset login attempts and is locked column
                                LoginDetails loginDetails = new LoginDetails()
                                {
                                    Username = objUser.MobileNumber,
                                    IsLocked = true,
                                    LoginAttempts = 3
                                };
                                UpdateLoginDetails(loginDetails);
                                serviceRes.IsSuccess = true;
                                serviceRes.ReturnCode = "200";
                                serviceRes.ReturnMsg = "User account is valid";
                            }
                            else {
                                //return account suspended status
                                serviceRes.IsSuccess = false;
                                serviceRes.ReturnCode = "403";
                                serviceRes.ReturnMsg = "Account suspended";
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
                                serviceRes.IsSuccess = false;
                                serviceRes.ReturnCode = "403";
                                serviceRes.ReturnMsg = "Wrong Password ";
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
                                serviceRes.IsSuccess = true;
                                serviceRes.ReturnCode = "";
                                serviceRes.ReturnMsg = "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Critical);
            }
            return serviceRes;
        }

        public ServiceRes RegisterUser(Registration objRegister)
        {
            ServiceRes serviceRes = new ServiceRes();
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
                int returnValue = Convert.ToInt32(dt.Rows[0][0]);
                if(returnValue > 0)
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "201";
                    serviceRes.ReturnMsg = "Something went wrong";
                }
                else
                {
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "User registered";
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Important);
            }
            return serviceRes;
        }

        public ServiceRes UnlockUserAccount(User objUser)
        {
            ServiceRes serviceRes = new ServiceRes();
            try
            {
                LoginDetails loginDetails = new LoginDetails()
                {
                    Username = objUser.MobileNumber,
                    IsLocked = true,
                    LoginAttempts = 3
                };
                var returnValue = UpdateLoginDetails(loginDetails);
                if (returnValue == 1) {
                    serviceRes.IsSuccess = true;
                    serviceRes.ReturnCode = "200";
                    serviceRes.ReturnMsg = "Login details updated";
                }
                else
                {
                    serviceRes.IsSuccess = false;
                    serviceRes.ReturnCode = "400";
                    serviceRes.ReturnMsg = "Login details not updated";
                }
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Important);
            }
            return serviceRes;
        }

        private int UpdateLoginDetails(LoginDetails loginDetails)
        {
            int returnValue=0;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter { ParameterName = "@MobileNumber", Value = loginDetails.Username };
                param[1] = new SqlParameter { ParameterName = "@IsLocked", Value = loginDetails.IsLocked };
                param[2] = new SqlParameter { ParameterName = "@LoginAttempts", Value = loginDetails.LoginAttempts };
                returnValue = SqlHelper.ExecuteNonQuery("Usp_UpdateLoginDetails", param);
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Critical);
            }
            return returnValue;
        } 

    }
}

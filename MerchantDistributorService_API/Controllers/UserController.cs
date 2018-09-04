using BusinessLogicLayer.Repository;
using BusinessObjects.Entities;
using Entities;
using MerchantDistributorService_API.Common;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static Enum.Enums;

namespace MerchantDistributorService_API.Controllers
{
    public class UserController : ApiController
    {
        private IUserRepository _userRepository;
        private ICommonRepository _commonRepository;
        public UserController(IUserRepository userRepo,ICommonRepository commonRepo)
        {
            this._userRepository = userRepo;
            this._commonRepository = commonRepo;
        }

        [HttpPost]
        public HttpResponseMessage ValidateUser(User request)
        {
            try
            {
                    //AuthenticationRepository authRepo = AuthenticationRepository.GetInstance;
                    //authRepo.GenerateToken(request.MobileNumber, 12);
                    return Request.CreateResponse(HttpStatusCode.OK, _userRepository.IsUserValid(request));
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Important);
                HttpError httpError = new HttpError(ex, true) { { "IsSuccess", false } };
                return Request.CreateErrorResponse(HttpStatusCode.OK, httpError);
            }
        }

        [HttpPost]
        public HttpResponseMessage RegisterUser(Registration request) {
            try
            {
                return Request.CreateResponse(HttpStatusCode.Ambiguous, _userRepository.RegisterUser(request));
                
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex,SeverityLevel.None);
                HttpError httpError = new HttpError(ex, true) { { "IsSuccess", false } };
                return Request.CreateResponse(HttpStatusCode.OK, httpError);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetStates()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _commonRepository.GetStates());
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Important);
                HttpError httpError = new HttpError(ex, true) { { "IsSuccess", false } };
                return Request.CreateResponse(HttpStatusCode.OK, httpError);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetGender()
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _commonRepository.GetGenders());
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Important);
                HttpError httpError = new HttpError(ex, true) { { "IsSuccess", false } };
                return Request.CreateResponse(HttpStatusCode.OK, httpError);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCityByStates(States request)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, _commonRepository.GetCitiesByState(request));
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex, SeverityLevel.Important);
                HttpError httpError = new HttpError(ex, true) { { "IsSuccess", false } };
                return Request.CreateResponse(HttpStatusCode.OK, httpError);
            }
        }

        //[HttpPost]
        //public HttpResponseMessage UpdateAddressDetails([FromBody]AddressDetails request)
        //{
        //    try
        //    {
        //        var response = _userRepository.UpdateAddressDetails(request);
        //        if (response == 1)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, $"Updated {request.UserId}");
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.Ambiguous, $"Updated {request.UserId}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.WriteLog(ex);
        //        HttpError httpError = new HttpError(ex, true) { { "IsSuccess", false } };
        //        return Request.CreateResponse(HttpStatusCode.OK, httpError);
        //    }
        //}

        //[HttpPost]
        //public HttpResponseMessage UpdateCompanyDetails([FromBody]CompanyDetails request)
        //{
        //    try
        //    {
        //        var response = _userRepository.UpdateCompanyDetails(request);
        //        if (response == 1)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.OK, $"Updated {request.UserId}");
        //        }
        //        else
        //        {
        //            return Request.CreateResponse(HttpStatusCode.Ambiguous, $"Failed {request.UserId}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.WriteLog(ex);
        //        HttpError httpError = new HttpError(ex, true) { { "IsSuccess", false } };
        //        return Request.CreateResponse(HttpStatusCode.OK, httpError);
        //    }
        //}
    }
}

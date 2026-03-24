using MARN_API.DTOs.Lookup;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Enums.Property;
using MARN_API.Enums.RoommatePreferences;
using MARN_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MARN_API.Controllers
{
    /// <summary>
    /// Controller for retrieving enum values used throughout the application.
    /// Useful for dropdowns, toggle menus, and filters in the frontend.
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : BaseController
    {
        private List<EnumValueDto> GetEnumValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new EnumValueDto
                {
                    Id = Convert.ToInt32(e),
                    Name = e.ToString()
                })
                .ToList();
        }

        /// <summary>
        /// Retrieves all lookup enums in a single request.
        /// </summary>
        [HttpGet("all")]
        public IActionResult GetAllEnums()
        {
            var data = new
            {
                Genders = GetEnumValues<Gender>(),
                Countries = GetEnumValues<Country>(),
                Languages = GetEnumValues<Language>(),
                PropertyTypes = GetEnumValues<PropertyType>(),
                AmenityTypes = GetEnumValues<AmenityType>(),
                RentalUnits = GetEnumValues<RentalUnit>(),
                PropertyStatuses = GetEnumValues<PropertyStatus>(),
                EducationLevels = GetEnumValues<EducationLevel>(),
                FieldsOfStudy = GetEnumValues<FieldOfStudy>(),
                GuestsFrequencies = GetEnumValues<GuestsFrequency>(),
                SharingLevels = GetEnumValues<SharingLevel>(),
                SleepSchedules = GetEnumValues<SleepSchedule>(),
                WorkSchedules = GetEnumValues<WorkSchedule>(),
                BookingRequestStatuses = GetEnumValues<BookingRequestStatus>(),
                ContractStatuses = GetEnumValues<ContractStatus>(),
                PaymentStatuses = GetEnumValues<PaymentStatus>(),
                PaymentFrequencies = GetEnumValues<PaymentFrequency>(),
                ReportStatuses = GetEnumValues<ReportStatus>(),
                ReportableTypes = GetEnumValues<ReportableType>()
            };

            return HandleServiceResult(ServiceResult<object>.Ok(data));
        }

        [HttpGet("genders")]
        public IActionResult GetGenders() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<Gender>()));

        [HttpGet("countries")]
        public IActionResult GetCountries() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<Country>()));

        [HttpGet("languages")]
        public IActionResult GetLanguages() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<Language>()));

        [HttpGet("property-types")]
        public IActionResult GetPropertyTypes() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<PropertyType>()));

        [HttpGet("amenity-types")]
        public IActionResult GetAmenityTypes() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<AmenityType>()));

        [HttpGet("rental-units")]
        public IActionResult GetRentalUnits() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<RentalUnit>()));

        [HttpGet("property-statuses")]
        public IActionResult GetPropertyStatuses() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<PropertyStatus>()));

        [HttpGet("education-levels")]
        public IActionResult GetEducationLevels() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<EducationLevel>()));

        [HttpGet("fields-of-study")]
        public IActionResult GetFieldsOfStudy() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<FieldOfStudy>()));

        [HttpGet("guests-frequencies")]
        public IActionResult GetGuestsFrequencies() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<GuestsFrequency>()));

        [HttpGet("sharing-levels")]
        public IActionResult GetSharingLevels() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<SharingLevel>()));

        [HttpGet("sleep-schedules")]
        public IActionResult GetSleepSchedules() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<SleepSchedule>()));

        [HttpGet("work-schedules")]
        public IActionResult GetWorkSchedules() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<WorkSchedule>()));

        [HttpGet("booking-request-statuses")]
        public IActionResult GetBookingRequestStatuses() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<BookingRequestStatus>()));

        [HttpGet("contract-statuses")]
        public IActionResult GetContractStatuses() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<ContractStatus>()));

        [HttpGet("payment-statuses")]
        public IActionResult GetPaymentStatuses() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<PaymentStatus>()));

        [HttpGet("payment-frequencies")]
        public IActionResult GetPaymentFrequencies() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<PaymentFrequency>()));

        [HttpGet("report-statuses")]
        public IActionResult GetReportStatuses() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<ReportStatus>()));

        [HttpGet("reportable-types")]
        public IActionResult GetReportableTypes() => HandleServiceResult(ServiceResult<List<EnumValueDto>>.Ok(GetEnumValues<ReportableType>()));
    }
}

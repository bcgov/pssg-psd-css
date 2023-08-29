using Gov.Pssg.Css.Interfaces.DynamicsAutorest;
using Gov.Pssg.Css.Interfaces.DynamicsAutorest.Models;
using Gov.Pssg.Css.Public.ViewModels;
using System;
using System.Threading.Tasks;

namespace Gov.Pssg.Css.Public.Utility
{
    public static class DynamicsUtility
    {
        public static async Task<MicrosoftDynamicsCRMcsuComplaints> CreateComplaintAsync(IDynamicsClient dynamicsClient, Complaint complaint)
        {
            var entity = new MicrosoftDynamicsCRMcsuComplaints
            {
                Statuscode = Constants.ComplaintStatusTypeReceived,
                CsuReportdataandtime = DateTimeOffset.UtcNow,
                CsuSourcetype = Constants.SourceTypeWebPortal,
                CsuComplaintreportdetail = complaint.Details.Problems,
            };

            if (complaint.LegislationType == Constants.LegislationTypeCSA)
            {
                entity.CsuLegislation = Constants.LegislationTypeCSA;
                entity.CsuComplainttype = Constants.ComplaintTypeIndividual;
                entity.CsuPropertydetails = complaint.Details.Description;
            }
            else if (complaint.LegislationType == Constants.LegislationTypeCCLA)
            {
                entity.CsuLegislation = Constants.LegislationTypeCCLA;

                if (string.IsNullOrWhiteSpace(complaint.Complainant?.GovernmentAgency))
                {
                    entity.CsuComplainttype = Constants.ComplaintTypeIndividual;
                }
                else
                {
                    entity.CsuComplainttype = Constants.ComplaintTypeOtherAgency;
                    entity.Csu106report = Constants.Complaint106ReportTypeNo;
                    entity.CsuAgencyfilenumber = "To be confirmed";
                    entity.CsuOtheragency = complaint.Complainant.GovernmentAgency;
                }
            }

            var complainant = complaint.Complainant;
            if (complainant != null)
            {
                entity.CsuFirstname = complainant.FirstName;
                entity.CsuMiddlename = complainant.MiddleName;
                entity.CsuLastname = complainant.LastName;
                entity.CsuFax = complainant.Fax;
                entity.CsuPrimaryphone = complainant.Phone;
                entity.CsuEmail = complainant.Email;

                entity.CsuAddress1 = complainant.Address.Line1;
                entity.CsuAddress2 = complainant.Address.Unit;
                entity.CsuCity = complainant.Address.City;
                entity.CsuCountry = complainant.Address.Country;
                if (string.Equals(complainant.Address.Country, "Canada", StringComparison.InvariantCultureIgnoreCase))
                {
                    entity.CsuProvince = complainant.Address.Province;
                }
                else
                {
                    entity.CsuStateprovince = complainant.Address.ProvinceState;
                }
                entity.CsuZippostalcode = complainant.Address.ZipPostalCode;
            }

            entity = await dynamicsClient.Csucomplaintses.CreateAsync(entity);

            await CreateSubjectOfComplaintAsync(dynamicsClient, complaint, entity.CsuComplaintsid);

            return entity;
        }

        public static async Task<MicrosoftDynamicsCRMcsuSubjectofcomplaint> CreateSubjectOfComplaintAsync(
            IDynamicsClient dynamicsClient, Complaint complaint, string complaintId)
        {
            string dynamicsComplaint = dynamicsClient.GetEntityURI("csu_complaintses", complaintId);

            var address = complaint.Details.Address;
            var entity = new MicrosoftDynamicsCRMcsuSubjectofcomplaint
            {
                ComplaintODataBind = dynamicsComplaint,

                CsuAddressdescription = complaint.Details.Name,

                CsuAddress1 = address.Line1,
                CsuAddress2 = address.Unit,
                CsuCity = address.City,
                CsuProvince = address.ProvinceState,
                CsuCountry = address.Country,
                CsuZippostalcode = address.ZipPostalCode,
            };

            if (complaint.LegislationType == Constants.LegislationTypeCSA)
            {
                entity.CsuPropertytype = complaint.Details.PropertyType;
                entity.CsuOtherpropertytype = complaint.Details.OtherPropertyType;
                entity.CsuNameofoccupants = complaint.Details.OccupantName;
                entity.CsuNameofowners = complaint.Details.OwnerName;

                if (complaint.Details.PropertyType == Constants.PropertyTypeCommercial)
                {
                    entity.CsuSubjecttype = Constants.SubjectTypeBusiness;
                }
                else if (complaint.Details.PropertyType == Constants.PropertyTypeOther)
                {
                    entity.CsuSubjecttype = Constants.SubjectTypeLocation;
                }
                else
                {
                    entity.CsuSubjecttype = Constants.SubjectTypeResidence;
                }
            }
            else if (complaint.LegislationType == Constants.LegislationTypeCCLA)
            {
                entity.CsuSubjecttype = Constants.SubjectTypeResidence;
            }

            entity = await dynamicsClient.Csusubjectofcomplaints.CreateAsync(entity);
            return entity;
        }
    }
}

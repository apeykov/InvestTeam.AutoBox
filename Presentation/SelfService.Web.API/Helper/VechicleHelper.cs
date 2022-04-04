using InvestTeam.AutoBox.Application.AppServices;
using InvestTeam.AutoBox.Application.Common;
using InvestTeam.AutoBox.Domain.Entities;
using System.Threading.Tasks;

namespace InvestTeam.AutoBox.SelfService.Web.API.Helper
{
    public class VechicleHelper
    {
        private VechicleService service;

        public VechicleHelper(VechicleService service)
        {
            this.service = service;
        }

        public async Task<QueryOperationResult<Vechicle>> GetVechicles(string number)
        {
            QueryOperationResult<Vechicle> result = new QueryOperationResult<Vechicle>();

            if (string.IsNullOrEmpty(number))
                result = service.GetVechicles(v => true);
            else
                result = service.GetVechicles(v => v.Identity == number);

            return await Task.FromResult(result);
        }
    }
}

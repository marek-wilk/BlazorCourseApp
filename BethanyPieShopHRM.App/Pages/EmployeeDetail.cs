using BethanyPieShopHRM.App.Services;
using BethanyPieShopHRM.Shared;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BethanyPieShopHRM.App.Pages
{
    public partial class EmployeeDetail
    {
		[Parameter]
		public string EmployeeId { get; set; }

		public Employee Employee { get; set; } = new Employee();

		[Inject]
		public IEmployeeDataService EmployeeDataService { get; set; }

        protected override async Task OnInitializedAsync()
		{
            Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId));
        }
    }
}

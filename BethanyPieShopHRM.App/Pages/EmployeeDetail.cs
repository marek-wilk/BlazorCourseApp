using BethanyPieShopHRM.App.Services;
using BethanyPieShopHRM.Shared;
using BethanysPieShopHRM.ComponentsLibrary.Map;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BethanyPieShopHRM.App.Pages
{
    public partial class EmployeeDetail
    {
        [Parameter]
        public string EmployeeId { get; set; }

        public Employee Employee { get; set; } = new Employee();

        public List<Marker> MapMarkers { get; set; } = new List<Marker>();

        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId));
            MapMarkers = new List<Marker>
            {
                new Marker
                {
                    Description = $"{Employee.FirstName} {Employee.LastName}", 
                    ShowPopup = false,
                    X = Employee.Longitude, 
                    Y = Employee.Latitude
                }
            };
        }
    }
}

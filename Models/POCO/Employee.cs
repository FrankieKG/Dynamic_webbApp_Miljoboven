using System.ComponentModel.DataAnnotations;

namespace uppgift5.Models
{
  //Hämtar data och updaterar data
  public class Employee
	{
		[Key]
		public String EmployeeId { get; set; }
		public String EmployeeName { get; set; }
		public String RoleTitle { get; set; }
		public String DepartmentId { get; set; }
	}
}

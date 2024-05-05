using System.ComponentModel.DataAnnotations;

namespace uppgift5.Models
{
	//Hämtar data och updaterar data
	public class Department
	{
		[Key]
		public string DepartmentId { get; set; }
		public string DepartmentName { get; set; }
	}
}

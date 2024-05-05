using System.ComponentModel.DataAnnotations;

namespace uppgift5.Models
{
  //Hämtar data och updaterar data

  public class ErrandStatus
	{
		[Key]
		public String StatusId { get; set; }
		public String StatusName { get; set; }
	}
}

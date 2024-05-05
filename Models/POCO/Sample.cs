using System.ComponentModel.DataAnnotations;

namespace uppgift5.Models
{
  //Hämtar data och updaterar data

  public class Sample
	{
		[Key]
		public int SampleId { get; set; }
		public String SampleName { get; set; }
		public int ErrandId { get; set; }
	}
}

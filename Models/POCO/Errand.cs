using System.ComponentModel.DataAnnotations;

namespace uppgift5.Models
{
  //Hämtar data och updaterar data

  public class Errand
	{
		[Key]
		public int ErrandId { get; set; }
		public String RefNumber { get; set; }
		[Display(Name = "Var har brottet skett någonstans?")]
		[Required(ErrorMessage = "Du måste fylla i plats")]
		public String Place { get; set; }
		[Display(Name = "Vilket typ av brott?")]
		[Required(ErrorMessage = "Du måste fylla i typ av brott")]
		public String TypeOfCrime { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		[Display(Name = "När skedde brottet?")]
		[Required(ErrorMessage = "Du måste fylla i datum och tid för brottet")]
		[DataType(DataType.Date)]
		public DateTime DateOfObservation { get; set; }
		[Display(Name = "Ditt namn (för- och efternamn):")]
		[Required(ErrorMessage = "Du måste fylla i ditt namn")]
		public String InformerName { get; set; }
		[Display(Name = "Din telefon:")]
		[RegularExpression(@"^[0]{1}[0-9]{1,3}-[0-9]{5,9}$", ErrorMessage = "Formatet är riktnummer-telefonnummer")]
		[Required(ErrorMessage = "Du måste fylla i ditt telefonnummer")]
		public String InformerPhone { get; set; }
		public String Observation { get; set; }
		public String InvestigatorInfo { get; set; }
		public String InvestigatorAction { get; set; }
		public String StatusId { get; set; }
		public String DepartmentId { get; set; }
		public String EmployeeId { get; set; }
		public ICollection<Sample> Samples { get; set; }
		public ICollection<Picture> Pictures { get; set; }
	}
}

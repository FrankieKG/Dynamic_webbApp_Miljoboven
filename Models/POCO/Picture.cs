using System.ComponentModel.DataAnnotations;

namespace uppgift5.Models
{
  //Hämtar data och updaterar data

  public class Picture
	{
		[Key]
		public int PictureId { get; set; }
		public String PictureName { get; set; }
		public int ErrandId { get; set; }
	}
}

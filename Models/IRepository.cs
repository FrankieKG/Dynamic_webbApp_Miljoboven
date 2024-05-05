namespace uppgift5.Models
{
	public interface IRepository
	{
		//Interface som påverkar EFRepository
		IQueryable<Department> Departments { get; }
		IQueryable<Employee> Employees { get; }
		IQueryable<ErrandStatus> ErrandStatuses { get; }
		IQueryable<Errand> Errands { get; }
		IQueryable<Picture> Pictures { get; }
		IQueryable<Sample> Samples { get; }
		IQueryable<Sequence> Sequences { get; }

		//Skapar och/eller uppdaterar
		void SaveErrand(Errand errand);
		void UpdateDepartment(int errandid, string choosenDepartment);
		void UpdateEmployeeId(int errandid, string investigator);
		void UpdateReason(int errandid, string reason);
		void CreateInvestigatorInfo(int errandid, string information);
		void CreateInvestigatorEvent(int errandid, string events);
		void ChangeStatus(int errandid, string status);
		Task UploadPicture(int errandid, IFormFile loadImage);
		Task UploadSample(int errandid, IFormFile loadSample);
		String GetDepFromEmp(String user);

    //Läser in data eller hämtar data
    IQueryable<MyErrand> GetErrandListCoordinator();
		IQueryable<MyErrand> GetErrandListManager(String userName);
		IQueryable<MyErrand> GetErrandListInvestigator(String userName);
    Task<Errand> GetErrandDetails(int id);
    Employee GetEmployee(String userName);

		//Raderar data
		void DeleteErrand(int errandId);

	}

}


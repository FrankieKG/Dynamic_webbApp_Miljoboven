using Microsoft.EntityFrameworkCore;

namespace uppgift5.Models
{
  //Innehåller logiken för metoder som används i programmet

  public class EFRepository : IRepository
  {
    private IWebHostEnvironment environment;
    private readonly ApplicationDbContext context;
    private IHttpContextAccessor contextAcc;

    public EFRepository(IWebHostEnvironment env, ApplicationDbContext ctx, IHttpContextAccessor cont)
    {
      environment = env;
      context = ctx;
      contextAcc = cont;
    }

    public IQueryable<Department> Departments => context.Departments;

    public IQueryable<Employee> Employees => context.Employees;

    public IQueryable<Errand> Errands => context.Errands.Include(e => e.Samples).Include(e => e.Pictures);

    public IQueryable<ErrandStatus> ErrandStatuses => context.ErrandStatuses;

    public IQueryable<Picture> Pictures => context.Pictures;

    public IQueryable<Sample> Samples => context.Samples;

    public IQueryable<Sequence> Sequences => context.Sequences;

    //Hämtar ärendedetaljer från det valda ärende 
    public Task<Errand> GetErrandDetails(int id)
    {
      return Task.Run(() =>
      {
        var errandDetails = Errands.Where(er => er.ErrandId == id).First();

        return errandDetails;
      }
      );
    }

    //Sparar ärendet med ett specifikt StatusId och ger det ett referensnummer med hjälp av sequence klassen
    public void SaveErrand(Errand errand)
    {
      if (errand.ErrandId == 0)
      {
        var se = context.Sequences.FirstOrDefault(sq => sq.Id == 1);
        errand.StatusId = "S_A";
        errand.RefNumber = "2020-45-" + se.CurrentValue;
        context.Errands.Add(errand);
        se.CurrentValue++;
      }

      context.SaveChanges();
    }

    //Raderar valda ärendet
    public void DeleteErrand(int id)
    {
      Errand dbEntry = context.Errands.FirstOrDefault(ec => ec.ErrandId == id);
      if (dbEntry != null)
      {
        context.Errands.Remove(dbEntry);
      }

      context.SaveChanges();
    }

    //Updaterar vilken avdelning ärendet hör till
    public void UpdateDepartment(int errandid, string choosenDepartment)
    {
      Errand dbEntry = context.Errands.FirstOrDefault(ec => ec.ErrandId == errandid);

      if (dbEntry != null)
      {
        dbEntry.DepartmentId = choosenDepartment;
      }

      context.SaveChanges();
    }

    //Updaterar EmployeeId för det valda ärendet
    public void UpdateEmployeeId(int errandid, string investigator)
    {
      foreach (Employee em in Employees)
      {
        if (em.EmployeeName == investigator)
        {
          investigator = em.EmployeeId;
        }
      }

      Errand dbEntry = context.Errands.FirstOrDefault(ec => ec.ErrandId == errandid);

      if (dbEntry != null)
      {
        dbEntry.EmployeeId = investigator;
      }

      context.SaveChanges();
    }

    //Updaterar innehållet för reason samt ändrar StatusID till en specifik sträng
    public void UpdateReason(int errandid, string reason)
    {
      Errand dbEntry = context.Errands.FirstOrDefault(ec => ec.ErrandId == errandid);

      if (dbEntry != null)
      {
        dbEntry.InvestigatorInfo = reason;
        dbEntry.StatusId = "S_B";
      }

      context.SaveChanges();
    }

    //Uppdaterar innehållet för InvestigatorInfo för ett specifikt ärende
    public void CreateInvestigatorInfo(int errandid, string information)
    {

      Errand dbEntry = context.Errands.FirstOrDefault(ec => ec.ErrandId == errandid);

      if (information != null)
      {
        dbEntry.InvestigatorInfo = dbEntry.InvestigatorInfo + " " + information;
      }

      context.SaveChanges();
    }

    //Uppdaterar InvestigatorAction för ett specifikt ärende
    public void CreateInvestigatorEvent(int errandid, string events)
    {
      Errand dbEntry = context.Errands.FirstOrDefault(ec => ec.ErrandId == errandid);

      if (events != null)
      {
        dbEntry.InvestigatorAction = dbEntry.InvestigatorAction + " " + events;
      }

      context.SaveChanges();
    }

    //Uppdaterar StatusId för ett specifikt ärende
    public void ChangeStatus(int errandid, string status)
    {
      foreach (ErrandStatus em in ErrandStatuses)
      {
        if (em.StatusName == status)
        {
          status = em.StatusId;
        }
      }

      Errand dbEntry = context.Errands.FirstOrDefault(ec => ec.ErrandId == errandid);

      if (dbEntry != null)
      {
        dbEntry.StatusId = status;
      }

      context.SaveChanges();
    }

    //Metod för att ladda upp filer, ge de unika namn och koppla filerna till Pictures tabellen
    public async Task UploadPicture(int errandid, IFormFile loadImage)
    {
      var tempPath = Path.GetTempFileName();

      if (loadImage.Length > 0)
      {
        using (var streamp = new FileStream(tempPath, FileMode.Create))
        {
          await loadImage.CopyToAsync(streamp);
        }
      }

      string uniquePictureName = Guid.NewGuid().ToString() + "-" + loadImage.FileName;
      var pathImage = Path.Combine(environment.WebRootPath, "Picture", uniquePictureName);

      System.IO.File.Move(tempPath, pathImage);

      Picture pic = new Picture();

      pic.PictureName = uniquePictureName;
      pic.ErrandId = errandid;
      context.Pictures.Add(pic);

      context.SaveChanges();
    }

    //Metod för att ladda upp filer, ge de unika namn och koppla filerna till Samples tabellen
    public async Task UploadSample(int errandid, IFormFile loadSample)
    {
      {
        var tempPath = Path.GetTempFileName();
        if (loadSample.Length > 0)
        {
          using (var stream = new FileStream(tempPath, FileMode.Create))
          {
            await loadSample.CopyToAsync(stream);
          }
        }

        string uniqueSampleName = Guid.NewGuid().ToString() + "-" + loadSample.FileName;

        var pathSample = Path.Combine(environment.WebRootPath, "Sample", uniqueSampleName);

        System.IO.File.Move(tempPath, pathSample);

        Sample smp = new Sample();

        smp.SampleName = uniqueSampleName;
        smp.ErrandId = errandid;
        context.Samples.Add(smp);

        context.SaveChanges();

      }
    }

    //Metod för att skapa ett MyErrand object som ska inehålla data från förenade tabeller via join
    public IQueryable<MyErrand> GetErrandListCoordinator()
    {
      var errandList = from err in Errands
                       join stat in ErrandStatuses on err.StatusId equals stat.StatusId
                       join dep in Departments on err.DepartmentId equals dep.DepartmentId
                       into departmentErrand
                       from deptE in departmentErrand.DefaultIfEmpty()

                       join em in Employees on err.EmployeeId equals em.EmployeeId into employeeErrand
                       from empE in employeeErrand.DefaultIfEmpty()
                       orderby err.RefNumber descending

                       select new MyErrand
                       {
                         DateOfObservation = err.DateOfObservation,
                         ErrandId = err.ErrandId,
                         RefNumber = err.RefNumber,
                         TypeOfCrime = err.TypeOfCrime,
                         StatusName = stat.StatusName,
                         DepartmentName = (err.DepartmentId == null ? "ej tillsatt" : deptE.DepartmentName),
                         EmployeeName = (err.EmployeeId == null ? "ej tillsatt" : empE.EmployeeName)
                       };
      return errandList;
    }

    //Metod för att skapa ett MyErrand object som ska inehålla data från förenade tabeller via join som vi specifikt söker för Manager
    public IQueryable<MyErrand> GetErrandListManager(String userName)
    {
      Employee emp = GetEmployee(userName);


      var errandList = from err in Errands

                       join stat in ErrandStatuses on err.StatusId equals stat.StatusId
                       join dep in Departments on err.DepartmentId equals dep.DepartmentId
                       into departmentErrand
                       from deptE in departmentErrand
                       where deptE.DepartmentId == emp.DepartmentId

                       join em in Employees on err.EmployeeId equals em.EmployeeId into employeeErrand
                       from empE in employeeErrand.DefaultIfEmpty()
                       orderby err.RefNumber descending

                       select new MyErrand
                       {
                         DateOfObservation = err.DateOfObservation,
                         ErrandId = err.ErrandId,
                         RefNumber = err.RefNumber,
                         TypeOfCrime = err.TypeOfCrime,
                         StatusName = stat.StatusName,
                         DepartmentName = (err.DepartmentId == null ? "ej tillsatt" : deptE.DepartmentName),
                         EmployeeName = (err.EmployeeId == null ? "ej tillsatt" : empE.EmployeeName)
                       };
      return errandList;

    }
    
    //Metod för att skapa ett MyErrand object som ska inehålla data från förenade tabeller via join som vi specifikt söker för Investigator
    public IQueryable<MyErrand> GetErrandListInvestigator(String userName)
    {
      Employee emp = GetEmployee(userName);

      var errandList = from err in Errands

                       join stat in ErrandStatuses on err.StatusId equals stat.StatusId
                       join dep in Departments on err.DepartmentId equals dep.DepartmentId
                       into departmentErrand
                       from deptE in departmentErrand.DefaultIfEmpty()

                       join em in Employees on err.EmployeeId equals em.EmployeeId into employeeErrand
                       from empE in employeeErrand
                       where empE.EmployeeId == emp.EmployeeId
                       orderby err.RefNumber descending

                       select new MyErrand
                       {
                         DateOfObservation = err.DateOfObservation,
                         ErrandId = err.ErrandId,
                         RefNumber = err.RefNumber,
                         TypeOfCrime = err.TypeOfCrime,
                         StatusName = stat.StatusName,
                         DepartmentName = (err.DepartmentId == null ? "ej tillsatt" : deptE.DepartmentName),
                         EmployeeName = (err.EmployeeId == null ? "ej tillsatt" : empE.EmployeeName)
                       };
      return errandList;

    }

    //Metod som söker efter ett specifikt employee objekt, som sedan kopieras på ett nytt employee objekt som sedan returneras
    public Employee GetEmployee(String userName)
    {
      Employee emp = new Employee();
      foreach (Employee em in Employees)
      {
        if (em.EmployeeId == userName)
        {
          emp = em;
        }
      }
      return emp;

    }

    //Hämtar avdelningsid

    public String GetDepFromEmp(String user)
    {
      Employee em = new Employee();

      foreach (Employee em2 in Employees)
      {
        if (em2.EmployeeId == user)
        {
          em = em2;
        }
      }
      String depart = em.DepartmentId;
      return depart;
      
      
    }
  }
}



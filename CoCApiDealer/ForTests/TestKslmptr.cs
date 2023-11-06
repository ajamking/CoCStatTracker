namespace CoCApiDealer.ForTests;
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Emploee> Staff { get; set; }

    public Company()
    {
        Staff = new HashSet<Emploee>();
    }
}

public class Emploee
{
    public int Id { set; get; }

    public int CompanyId { get; set; }

    public virtual Company Company { get; set; }
    public string Name { get; set; }
}
namespace MyWebApi.Domain.Entities
{
    public class Period
    {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public Guid UserId { get; set; }
        }
    }




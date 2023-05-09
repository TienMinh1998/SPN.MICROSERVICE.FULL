using DatabaseCore.Domain.Entities.Base;
namespace DatabaseCore.Domain.Entities.Normals;

public class ReadingQuestion : BaseEntity<int>
{
    public int Fk_reding_id { get; set; }
    public string Content { get; set; }
    public string Answer { get; set; }
}

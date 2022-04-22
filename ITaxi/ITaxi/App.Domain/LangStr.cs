using Base.Domain;

namespace App.Domain;

public class LangStr: DomainEntityId
{
    public ICollection<Translation>? Translations { get; set; }
}
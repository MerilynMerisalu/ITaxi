using Base.Contracts.Domain;

namespace Base.Domain;

public class LangStr : LangStr<Guid>
{
    public LangStr(): this("")
    {
        
    }

    public LangStr(string value, string? culture = null): base(value, culture)
    {
        SetTranslation(value, culture);
    }
    public static implicit operator string(LangStr? langStr) => langStr?.ToString() ?? "null";
    public static implicit operator LangStr(string value) => new LangStr(value);

}

public class LangStr<TKey>: DomainEntityId<TKey> 
where TKey: IEquatable<TKey>
{
    private static string _defaultCulture = "en";

    public virtual ICollection<Translation>? Translations { get; set; }

    public LangStr(): this("")
    {
        
    }

    public LangStr(string value,string? culture = null )
    {
        SetTranslation(value, culture);
    }
    
    public virtual void SetTranslation(string value, string? culture = null)
    {
        culture ??= Thread.CurrentThread.CurrentUICulture.Name;
        culture ??= _defaultCulture;
        
        if (Translations == null)
        {

            if (Id.Equals(default(TKey)) && Translations == null)
            {
                Translations ??= new List<Translation>();
            }
            else
            {
                throw new NullReferenceException("Translations cannot be null. Did you forgot to do an include?");
            }
            
        }
        var translation = Translations.FirstOrDefault(t => t.Culture == culture);
        if (translation == null)
        {
            Translations.Add(new Translation()
            {
                Culture = culture,
                Value = value
            });
            
        }
        else
        {
            translation.Value = value;
        }
        
       
        
    }

    public string? Translate(string? culture = null)
    {
        if (Translations == null)
        {
            #warning Should I put the code into a method to make it more reusable
            if (Id.Equals(default(TKey)) && Translations == null)
            {
                return null;
            }
            
            culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;

            if (Translations != null)
            {
                // Do we have an exact match
                var translation = Translations.FirstOrDefault(t => t.Culture == culture);
                if (translation != null)
                {
                    return translation.Value;
                }

                // Do we have a match without a region
                translation = Translations.FirstOrDefault(t => culture.StartsWith(t.Culture));
                if (translation != null)
                {
                    return translation.Value;
                }
            
                // Do we have a default culture string
                translation = Translations.FirstOrDefault(t => _defaultCulture.StartsWith(_defaultCulture));
                if (translation != null)
                {
                    return translation.Value;
                }
                
                // just return the first one from the list or null
            }
        }
        
        return Translations!.FirstOrDefault()?.Value;
        
    }

    public override string ToString()
    {
        return Translate() ?? "?????";
    }
    public static implicit operator string(LangStr<TKey>? langStr) => langStr?.ToString() ?? "null";
    public static implicit operator LangStr<TKey>(string value) => new LangStr<TKey>(value);

}
    
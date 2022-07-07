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
    public const string DefaultCulture = "en";

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
        culture ??= DefaultCulture;
        
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
                return "";
            }
            
            culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;
            
            var translation = Translations.FirstOrDefault(t => t.Culture == culture);
            if (translation != null)
            {
                return translation.Value;
            }

            translation = Translations.FirstOrDefault(t => t.Culture == culture);
            if (translation != null)
            {
                return translation.Value;
            }
            
            translation = Translations.FirstOrDefault(t => DefaultCulture.StartsWith(culture));
            if (translation != null)
            {
                return translation.Value;
            }

            
        }
        
        return Translations.FirstOrDefault()?.Value;
        
    }

    public override string ToString()
    {
        return Translate() ?? "?????";
    }
    public static implicit operator string(LangStr<TKey>? langStr) => langStr?.ToString() ?? "null";
    public static implicit operator LangStr<TKey>(string value) => new LangStr<TKey>(value);

}
    
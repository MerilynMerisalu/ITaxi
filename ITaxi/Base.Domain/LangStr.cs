namespace Base.Domain;

public class LangStr : LangStr<Guid>
{
    public LangStr()
    {
    }

    public LangStr(string value, string? culture = null) : base(value, culture)
    {
        SetTranslation(value, culture);
    }

    public static implicit operator string(LangStr? langStr)
    {
        return langStr?.ToString() ?? "null";
    }

    public static implicit operator LangStr(string value)
    {
        return new LangStr(value);
    }
}

public class LangStr<TKey> : DomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    private static readonly string _defaultCulture = "en";
    public LangStr()
    {
    }

    public LangStr(string value, string? culture = null)
    {
        SetTranslation(value, culture);
    }

    public virtual ICollection<Translation>? Translations { get; set; }

    public virtual void SetTranslation(string value, string? culture = null)
    {
        culture ??= Thread.CurrentThread.CurrentUICulture.Name;
        culture ??= _defaultCulture;

        if (Translations == null)
        {
            if (Id.Equals(default) && Translations == null)
                Translations ??= new List<Translation>();
            else
                throw new NullReferenceException("Translations cannot be null. Did you forgot to do an include?");
        }

        var translation = Translations.FirstOrDefault(t => t.Culture == culture);
        if (translation == null)
            Translations.Add(new Translation
            {
                Culture = culture,
                Value = value
            });
        else
            translation.Value = value;
    }

    public string? Translate(string? culture = null)
    {
        if (Translations == null)
        {
            if (Id.Equals(default)) return null;
            throw new NullReferenceException("Translations cannot be null. Did you forgot to do .include?");
        }
        
        culture = culture?.Trim() ?? Thread.CurrentThread.CurrentUICulture.Name;

        /*
         cultures in db
         en, en-GB
         in query
         ru, en, en-US, en-GB
         */

        // do we have exact match
        var translation = Translations.FirstOrDefault(t => t.Culture == culture);
        if (translation != null) return translation.Value;

        // do we have match without region - match en-XX or en
        translation = Translations.FirstOrDefault(t => culture.StartsWith(t.Culture));
        if (translation != null) return translation.Value;

        // do we have the default culture string
        // exact match
        translation = Translations.FirstOrDefault(t => t.Culture == _defaultCulture);
        if (translation != null) return translation.Value;
        // starts with
        translation = Translations.FirstOrDefault(t => _defaultCulture.StartsWith(t.Culture));
        if (translation != null) return translation.Value;

        // just return the first one or null
        return Translations.FirstOrDefault()?.Value;
    }


    public override string ToString()
    {
        return Translate() ?? "?????";
    }

    public static implicit operator string(LangStr<TKey>? langStr)
    {
        return langStr?.ToString() ?? "null";
    }

    [Obsolete("Only use this for New entries, not for editing existing entries")]
    public static implicit operator LangStr<TKey>(string value)
    {
        return new LangStr<TKey>(value);
    }
}
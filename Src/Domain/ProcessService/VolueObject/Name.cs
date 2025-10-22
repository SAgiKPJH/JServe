using System.Text.RegularExpressions;

namespace ProcessService.VolueObject;

public class Name(string value)
{
    public string Value => value;

    public static Name Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Name cannot be null, empty, or whitespace.", nameof(input));

            // 대문자를 소문자로 변환
        string normalizedInput = input.ToLower();

            // 길이 체크 (10자 이하)
        if (normalizedInput.Length > 10)
            throw new ArgumentException("Name cannot exceed 10 characters.", nameof(input));

            // 허용되는 특수문자: 하이픈(-), 언더스코어(_), 마침표(.), 콜론(:), 슬래시(/)
            // 나머지 특수문자는 금지 (공백 불허용)
        string invalidCharsPattern = @"[^\w\-.:/]";

        if (Regex.IsMatch(normalizedInput, invalidCharsPattern))
            throw new ArgumentException("Name contains invalid special characters. Only hyphens (-), underscores (_), periods (.), colons (:), and forward slashes (/) are allowed as special characters.", nameof(input));
        
        return new Name(normalizedInput);
    }

    public override string ToString() => value;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        var other = (Name)obj;
        return string.Equals(value, other.Value, StringComparison.Ordinal);
    }

    public override int GetHashCode() => value.GetHashCode();

    public static bool operator ==(Name left, Name right)
    {
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        if (ReferenceEquals(right, null)) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Name left, Name right) => !(left == right);

    // 암시적 변환 연산자: Name -> string
    public static implicit operator string(Name name) => name.Value;

    // 명시적 변환 연산자: string -> Name (선택사항)
    public static explicit operator Name(string value) => Create(value);
}
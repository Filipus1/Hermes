namespace Hermes.Infrastructure.FormatSerializer;
public interface IFormatSerializer
{
    public string JsonToFormat(string input);
    public string FormatToJson(string input);
}
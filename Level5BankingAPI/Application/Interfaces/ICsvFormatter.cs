namespace Application.Interfaces;

public interface ICsvFormatter
{
    string FormatCsv();
    string CreateHeader();
}
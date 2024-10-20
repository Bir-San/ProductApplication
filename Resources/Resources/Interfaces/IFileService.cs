namespace Resources.Interfaces;

public interface IFileService
{
    string GetFromFile();
    bool SaveToFile(string content);
}
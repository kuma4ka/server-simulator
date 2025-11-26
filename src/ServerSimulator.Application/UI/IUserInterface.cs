namespace ServerSimulator.Application.UI;

public interface IUserInterface
{
    void ShowHeader();
    void ShowInfo(string message);
    void ShowError(string message);
    string PromptString(string message, string defaultValue);
    int PromptInt(string message, int defaultValue);
    void WaitForKeyPress();
}
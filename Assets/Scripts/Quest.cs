[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    public bool isComplete;

    public Quest(string title, string description)
    {
        this.title = title;
        this.description = description;
        isComplete = false;
    }
}

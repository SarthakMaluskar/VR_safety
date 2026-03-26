using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Choice
{
    public string text;
    public string next;
}

[System.Serializable]
public class Node
{
    public string id;
    public string text;
    public Choice[] choices;
    public string sceneTag; // Matches the sceneTag in your JSON
}

[System.Serializable]
public class Story
{
    public Node[] nodes;
}

public class StoryManager : MonoBehaviour
{
    public TextMeshProUGUI storyText;
    public Button[] choiceButtons;
    public SceneController sceneController; // 👈 Added SceneController reference

    private Dictionary<string, Node> storyDict;
    private string currentNode;

    void Start()
    {
        LoadStory();
        currentNode = "Start"; 
        ShowNode();
    }

    void LoadStory()
    {
        TextAsset json = Resources.Load<TextAsset>("story");
        Story story = JsonUtility.FromJson<Story>(json.text);

        storyDict = new Dictionary<string, Node>();

        foreach (Node node in story.nodes)
        {
            storyDict[node.id] = node;
        }
    }   

    string CleanText(string text)
    {
        // Remove [[...]] patterns (Twine links)
        while (text.Contains("[["))
        {
            int start = text.IndexOf("[[");
            int end = text.IndexOf("]]") + 2;

            if (start >= 0 && end > start)
            {
                text = text.Remove(start, end - start);
            }
            else break;
        }

        return text.Trim();
    }

    void ShowNode()
    {
        if (!storyDict.ContainsKey(currentNode)) return;

        Node node = storyDict[currentNode];

        // 👈 Trigger the visual scene change based on the node's tag
        if (sceneController != null)
        {
            sceneController.ShowScene(node.sceneTag);
        }

        storyText.text = CleanText(node.text);

        // Reset all buttons
        foreach (Button btn in choiceButtons)
        {
            btn.onClick.RemoveAllListeners();
            btn.gameObject.SetActive(false);
        }

        // Set buttons dynamically
        for (int i = 0; i < node.choices.Length; i++)
        {
            if (i >= choiceButtons.Length) break;

            Button btn = choiceButtons[i];
            btn.gameObject.SetActive(true);

            btn.GetComponentInChildren<TextMeshProUGUI>().text = node.choices[i].text;

            string nextNode = node.choices[i].next;

            // Using a temporary variable to capture the correct nextNode for the listener
            btn.onClick.AddListener(() =>
            {
                currentNode = nextNode;
                ShowNode();
            });
        }
    }
}
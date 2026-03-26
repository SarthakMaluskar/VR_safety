using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject park;
    public GameObject interaction;
    public GameObject cpr;
    public GameObject recovery;
    public GameObject warning;

    public void ShowScene(string sceneTag)
    {
        // Turn OFF all
        park.SetActive(false);
        interaction.SetActive(false);
        cpr.SetActive(false);
        recovery.SetActive(false);
        warning.SetActive(false);

        // Turn ON based on tag
        switch (sceneTag)
        {
            case "park":
                park.SetActive(true);
                break;

            case "interaction":
                interaction.SetActive(true);
                break;

            case "cpr":
                cpr.SetActive(true);
                break;

            case "recovery":
                recovery.SetActive(true);
                break;

            case "warning":
                warning.SetActive(true);
                break;
        }
    }
}
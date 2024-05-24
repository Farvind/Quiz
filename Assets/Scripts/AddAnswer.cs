using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAnswer : MonoBehaviour
{
	public static AddAnswer Instance;
    [SerializeField] GameObject[] AnswerPanels;
	int currentIndex = 0;

	private void Start()
	{
		if (Instance != null) 
		{
			Instance = this;
		}
	}
    public void DisplayAnswerPanel()
    {
        if (AnswerPanels.Length == 0)
        {
            Debug.Log("No answer panels set in the AnswerPanels array.");
            return;
        }

        // Find the next inactive answer panel
        for (int i = 0; i < AnswerPanels.Length; i++)
        {
            int nextIndex = (currentIndex + i) % AnswerPanels.Length;
            if (!AnswerPanels[nextIndex].activeSelf)
            {
                // Activate the next inactive panel
                AnswerPanels[nextIndex].SetActive(true);
                currentIndex = nextIndex;
                return;
            }
        }

        // If all panels are already active, deactivate them all and activate the first one
        foreach (var panel in AnswerPanels)
        {
            panel.SetActive(false);
        }
        AnswerPanels[0].SetActive(true);
        currentIndex = 0;
    }
    public void HideAnswerPanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}

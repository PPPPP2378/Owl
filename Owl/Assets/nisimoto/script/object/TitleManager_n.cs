using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager_n : MonoBehaviour
{
    private int selectID = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            selectID--;

            if (selectID < 0)
                selectID = 2;

            Debug.Log("‘I‘ð’† : " + selectID);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            selectID++;

            if (selectID > 2)
                selectID = 0;

            Debug.Log("‘I‘ð’† : " + selectID);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            switch (selectID)
            {
                case 0:
                    Debug.Log("START");
                    break;

                case 1:
                    Debug.Log("MENU");
                    break;

                case 2:
                    Debug.Log("EXIT");
                    break;
            }
        }
    }
}

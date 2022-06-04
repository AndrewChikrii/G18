using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintDisplay : MonoBehaviour
{
    [SerializeField] private Text interactionsUI;
    public Text interaction;
    private float maxAlpha = 1f;

    void Start()
    {
        interaction = interactionsUI.GetComponent<Text>();
    }

    void Update()
    {
        interaction.color = new Color(interaction.color.r, interaction.color.g, interaction.color.b,
                Mathf.Lerp(interaction.color.a, maxAlpha, 5f * Time.deltaTime));
    }

    public IEnumerator HintCoroutine()
    {
        maxAlpha = 1f;
        yield return new WaitForSeconds(1f);
        maxAlpha = 0f;
    }
}
using UnityEngine;

[CreateAssetMenu(menuName = "Dianlogue/New Dialogue Container")]
public class DialogueContainer : ScriptableObject
{
    public int[] speakerID;
    [TextArea(5, 10)]
    public string[] paragraphs;
}

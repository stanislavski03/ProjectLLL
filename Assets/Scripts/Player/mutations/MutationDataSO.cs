using UnityEngine;
[CreateAssetMenu(fileName = "New Mutation Data", menuName = "Mutations/Mutation Data")]
public class MutationDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public string mutationTitle;
    public string description;
    public Sprite icon;

    public Chest chest;

    [Header("Base Stats")]
    public float bonus;

    

    public virtual void OnPick()
    {
        Debug.Log("мутация взята");
    }

    public virtual void OnDelete()
    {
        Debug.Log("мутация удалена");
    }

}

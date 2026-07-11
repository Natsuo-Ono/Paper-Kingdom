using UnityEngine;

[CreateAssetMenu(fileName = "Element Database", menuName = "Paper Kingdom/Element Database")]
public class ElementDatabase : ScriptableObject
{
    public ElementData[] elements;

    public ElementData Get(Element element)
    {
        foreach (var e in elements)
        {
            if (e.element == element)
                return e;
        }

        return null;
    }
}
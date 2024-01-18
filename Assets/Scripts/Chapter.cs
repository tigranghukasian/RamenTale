using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chapter")]
public class Chapter : ScriptableObject
{
    [SerializeField] private List<ChapterPart> chapterParts;
    public List<ChapterPart> ChapterParts => chapterParts;

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DescriptionDFS", menuName = "DFS/Description")]
public class SO_DFS : ScriptableObject
{
    public List<DFS_Description> DfsDes;

    public void ResetCheck()
    {
        foreach (DFS_Description dfsDes in DfsDes)
        {
            dfsDes.UnCheck();
        }
    }
    public void FullFill()
    {
        foreach (DFS_Description dfsDes in DfsDes)
        {
            if (!dfsDes.Checked)
                return;
        }
        Manager.Instance.ReloadBtnAppear();
    }
    [Serializable]
    public class DFS_Description
    {
        public Sprite Img;
        [TextArea] public string Description;
        public bool Checked = false;
        public string GetDescription()
        {
            return Description;
        }
        public void UnCheck()
        {
            Checked = false;
        }
        public void Check()
        {
            Checked = true;
        }
    }
}

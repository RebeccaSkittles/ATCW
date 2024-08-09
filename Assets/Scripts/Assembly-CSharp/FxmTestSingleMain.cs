using UnityEngine;
using UnityEngine.UI; // Add this for the Text component

public class FxmTestSingleMain : MonoBehaviour
{
    public GameObject[] m_EffectPrefabs = new GameObject[1];

    public Text m_EffectUIText; // Change this from GUIText to Text

    public int m_nIndex;

    public float m_fCreateScale = 1f;

    public int m_nCreateCount = 1;

    public float m_fRandomRange = 1f;

    private void Start()
    {
        Resources.UnloadUnusedAssets();
        Invoke("CreateEffect", 1f);
    }

    private void CreateEffect()
    {
        if (!(m_EffectPrefabs[m_nIndex] == null))
        {
            if (m_EffectUIText != null)
            {
                m_EffectUIText.text = m_EffectPrefabs[m_nIndex].name;
            }
            float num = 0f;
            if (1 < m_nCreateCount)
            {
                num = m_fRandomRange;
            }
            for (int i = 0; i < GetInstanceRoot().transform.childCount; i++)
            {
                Destroy(GetInstanceRoot().transform.GetChild(i).gameObject);
            }
            for (int j = 0; j < m_nCreateCount; j++)
            {
                GameObject gameObject = Instantiate(m_EffectPrefabs[m_nIndex], new Vector3(Random.Range(0f - num, num), 0f, Random.Range(0f - num, num)), Quaternion.identity);
                gameObject.transform.localScale = gameObject.transform.localScale * m_fCreateScale;
                NcEffectBehaviour.PreloadTexture(gameObject);
                gameObject.transform.SetParent(GetInstanceRoot().transform, false);
                SetActiveRecursively(gameObject, true);
            }
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(GetButtonRect(0), "Next"))
        {
            if (m_nIndex < m_EffectPrefabs.Length - 1)
            {
                m_nIndex++;
            }
            else
            {
                m_nIndex = 0;
            }
            CreateEffect();
        }
        if (GUI.Button(GetButtonRect(1), "Recreate"))
        {
            CreateEffect();
        }
    }

    public GameObject GetInstanceRoot()
    {
        return NcEffectBehaviour.GetRootInstanceEffect();
    }

    public static Rect GetButtonRect()
    {
        int num = 2;
        return new Rect(Screen.width - Screen.width / 10 * num, Screen.height - Screen.height / 10, Screen.width / 10 * num, Screen.height / 10);
    }

    public static Rect GetButtonRect(int nIndex)
    {
        return new Rect(Screen.width - Screen.width / 10 * (nIndex + 1), Screen.height - Screen.height / 10, Screen.width / 10, Screen.height / 10);
    }

    public static void SetActiveRecursively(GameObject target, bool bActive)
    {
        for (int i = target.transform.childCount - 1; i >= 0; i--)
        {
            SetActiveRecursively(target.transform.GetChild(i).gameObject, bActive);
        }
        target.SetActive(bActive);
    }
}

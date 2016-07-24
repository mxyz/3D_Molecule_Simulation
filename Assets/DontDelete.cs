using UnityEngine;
using System.Collections;

public class DontDelete : MonoBehaviour {
    void Awake()
    {
        DontDestroyOnLoad(this);

    }
}

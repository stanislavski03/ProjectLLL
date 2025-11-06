using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EInteractable : MonoBehaviour
{
    [NonSerialized] public bool _isReady = false;
    [NonSerialized] public bool _canBeInteractedWith = true;
    protected Renderer _renderer;
    

    protected virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public virtual void MakeReady()
    {
        _isReady = true;
        _renderer.material.color += new Color(0.1f, 0.2f, 0);
    }

    public virtual void MakeNonReady()
    {
        if (_isReady)
        {
            _renderer.material.color -= new Color(0.1f, 0.2f, 0);
            _isReady = false;
        }
    }

    public virtual void Interact()
    {
    }

    public virtual void SetComplete()
    {
    }

}

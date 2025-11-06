using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    private List<EInteractable> _events = new List<EInteractable>();
    private EInteractable _activeEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EInteractable>())
        {
            if (other.GetComponent<EInteractable>()._canBeInteractedWith)
                _events.Add(other.GetComponent<EInteractable>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EInteractable>())
        {
            other.GetComponent<EInteractable>().MakeNonReady();
            _events.Remove(other.GetComponent<EInteractable>());
        }
    }
    private void Update()
    {

        if (_events.Count == 0 || _events == null) return;

        else if (_events.Count == 1)
        {
            if (_events[0]._canBeInteractedWith)
            {
                _activeEvent = _events[0];
            }
            else if (!_events[0]._canBeInteractedWith)
            {
                _events.Clear();
            }
        }

        else if (_events.Count > 1)
        {
            List<EInteractable> delete = new List<EInteractable>();
            float minDistance = ((CapsuleCollider)gameObject.GetComponent<Collider>()).radius; // �� ������, ���� �������� ����� � ���������� ������, ����� �� ������ � ���������� ������ ���������, ���� �����
            foreach (EInteractable e in _events)
            {
                if (!e._canBeInteractedWith || e == null)
                {
                    delete.Add(e);
                }
                else if (e._canBeInteractedWith)
                {
                    e.MakeNonReady();
                    if (Vector3.Distance(transform.position, e.transform.position) < minDistance)
                    {
                        _activeEvent = e;
                        minDistance = Vector3.Distance(transform.position, e.transform.position);
                    }
                }

            }
            foreach (EInteractable e in delete)
            {
                _events.Remove(e);
            }
        }

        if (!_activeEvent._isReady && _activeEvent._canBeInteractedWith)
            _activeEvent.MakeReady();

        if (Input.GetKeyDown(KeyCode.E))
        {
            _activeEvent.Interact();
        }
    }
}

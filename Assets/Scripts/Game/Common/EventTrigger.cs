using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    private List<QuestGiver> _events = new List<QuestGiver>();
    private QuestGiver _activeEvent;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<QuestGiver>())
        {
            if (other.GetComponent<QuestGiver>()._canBeInteractedWith)
                _events.Add(other.GetComponent<QuestGiver>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<QuestGiver>())
        {
            other.GetComponent<QuestGiver>().MakeNonReady();
            _events.Remove(other.GetComponent<QuestGiver>());
        }
    }
    private void Update()
    {

        if(_events.Count == 0 || _events == null)  return; 

        else if (_events.Count == 1)
        {
            if (_events[0]._canBeInteractedWith)
            {
                _activeEvent = _events[0];
            }
            else if(!_events[0]._canBeInteractedWith)
            {
                _events.Clear();
            }
        }

        else if (_events.Count >1)
        {
            List<QuestGiver> delete = new List<QuestGiver>();
            float minDistance= ((CapsuleCollider)gameObject.GetComponent<Collider>()).radius; // на случай, если колладер будем в инспекторе менять, можно на радиус в инспекторе просто поставить, норм будет
            foreach (QuestGiver e in _events)
            {
                if (e._canBeInteractedWith)
                {
                    e.MakeNonReady();
                    if (Vector3.Distance(transform.position, e.transform.position) < minDistance)
                    {
                        _activeEvent = e;
                        minDistance = Vector3.Distance(transform.position, e.transform.position);
                    }
                }
                else  if(!e._canBeInteractedWith)
                {
                    delete.Add(e);
                }
            }
            foreach (QuestGiver e in delete)
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

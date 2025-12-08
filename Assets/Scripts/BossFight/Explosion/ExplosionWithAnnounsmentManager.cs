using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ExplosionWithAnnounsmentManager : MonoBehaviour
{
    [SerializeField] private GameObject Announcement;
    private Vector3 AnnouncementScale;

    [SerializeField] GameObject Explosion;

    [SerializeField] public float _timeToReact = 1;
    [SerializeField] public float _lavaExistanceTime = 5;
    //private float _cooldownToReact = 0;
    //private float _cooldownForLava = 0;

    private float scale;

    private void OnEnable()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        Announcement.SetActive(true);
        AnnouncementScale = Announcement.transform.localScale;
        scale = Announcement.transform.localScale.x / _timeToReact * 0.01f;
        StartCoroutine(StartAnnouncement());
    }

    private async void Explode()
    {
        Explosion.SetActive(true);
        await UniTask.WaitForSeconds(_lavaExistanceTime);
        Explosion.SetActive(false);
    }

    private IEnumerator StartAnnouncement()
    {
        
        while (Announcement.transform.localScale.x > 0)
        {
            Announcement.transform.localScale -= new Vector3(scale, 0 , scale);
            yield return new WaitForSeconds(0.01f);
        }
        Announcement.transform.localScale = AnnouncementScale;
        Announcement.SetActive(false);

        Explode();
        
    }

}

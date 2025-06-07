using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Horror
{
    public class TimelineEventCreator : MonoBehaviour
    {
        [SerializeField]
        private List<PlayableDirector> _directors;
        private bool _isActive = false;
        

        public void Create(TimelineEventSettings settings)
        {
            StartCoroutine(ApplyTimelineEventRoutine(settings));
        }

        private IEnumerator ApplyTimelineEventRoutine(TimelineEventSettings settings)
        {
            if (settings.delay > 0)
                yield return new WaitForSeconds(settings.delay);

            foreach (var director in _directors)
            {
                _isActive = !_isActive;

                if (_isActive)
                    director.Play();
                else
                {
                    director.Play();
                    director.time = 22.1;
                }
            }
        }
    }
}

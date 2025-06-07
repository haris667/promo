using Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    public class LightingEventCreator : MonoBehaviour
    {
        [SerializeField]
        private List<Light> lights;
        private Dictionary<Light, float> _originalIntensities = new Dictionary<Light, float>();

        public void Create(LightingEventSettings settings)
        {
            StartCoroutine(ApplyLightingEffectRoutine(settings));
        }

        private IEnumerator ApplyLightingEffectRoutine(LightingEventSettings settings)
        {
            if (settings.delay > 0)
                yield return new WaitForSeconds(settings.delay);

            foreach (var light in lights)
            {
                if (light != null && !_originalIntensities.ContainsKey(light))
                {
                    _originalIntensities[light] = light.intensity;
                }
            }

            if (settings.isLerped)
            {
                yield return StartCoroutine(LerpLightIntensity(
                    settings.newIntensity,
                    settings.duration
                ));
            }
            else
            {
                SetIntensityForAll(settings.newIntensity);
            }

            if (settings.isRevertAfterDuration)
            {
                yield return new WaitForSeconds(settings.duration);

                if (settings.isLerped)
                {
                    yield return StartCoroutine(LerpLightIntensity(
                        GetOriginalIntensity(),
                        settings.duration
                    ));
                }
                else
                {
                    RestoreOriginalIntensity();
                }
            }
        }

        private void SetIntensityForAll(float intensity)
        {
            foreach (var light in lights)
            {
                if (light != null)
                {
                    light.intensity = intensity;
                }
            }
        }

        //плохо работает и ломается свет от этого. Вердикт - до лучших времен
        private IEnumerator LerpLightIntensity(float targetIntensity, float duration)
        {
            float elapsed = 0f;
            Dictionary<Light, float> startIntensities = new Dictionary<Light, float>();

            foreach (var light in lights)
            {
                if (light != null)
                {
                    startIntensities[light] = light.intensity;
                }
            }

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                foreach (var light in lights)
                {
                    if (light != null && startIntensities.ContainsKey(light))
                    {
                        light.intensity = Mathf.Lerp(
                            startIntensities[light],
                            targetIntensity,
                            t
                        );
                    }
                }
                yield return null;
            }

            SetIntensityForAll(targetIntensity);
        }

        private void RestoreOriginalIntensity()
        {
            foreach (var light in lights)
            {
                if (light != null && _originalIntensities.ContainsKey(light))
                {
                    light.intensity = _originalIntensities[light];
                }
            }
        }

        private float GetOriginalIntensity()
        {
            if (lights.Count > 0 && lights[0] != null && _originalIntensities.ContainsKey(lights[0]))
            {
                return _originalIntensities[lights[0]];
            }
            return 1.0f; 
        }

    }
}
